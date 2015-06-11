using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SCAFT
{
    public static class CUtils
    {
        public static int HASH_SALT_BYTE_NUM = 16;
        public static byte TCP_END_SINGLE_SIGN = 0x4;
        public static int TCP_END_SIGN_NUM_OF_SIGNS = 40;
        public static byte HASH_DELIMITER_SIGN = 0x2;
        public static int HASH_DELIMITER_NUM_OF_TIMES = 40;

        private static byte[] TCP_END_MESSAGE_SIGN
        {
            get 
            {
                byte[] baTCP_EndSignal = new byte[TCP_END_SIGN_NUM_OF_SIGNS];
                for (int i = 0; i < TCP_END_SIGN_NUM_OF_SIGNS; i++)
                {
                    baTCP_EndSignal[i] = TCP_END_SINGLE_SIGN;
                }
                return baTCP_EndSignal;
            }
        }

        private static int BLOCK_AND_KEY_SIZE = 128;

        public static int iKeyIvSizeInBytes { get { return BLOCK_AND_KEY_SIZE / 8; } }
         
        public static byte[] Trimming(byte[] baInput, int? iTrimTo = null)
        {
            int iByteNum = (iTrimTo == null) ? iKeyIvSizeInBytes : (int)iTrimTo;
            byte[] baOutput = new byte[iByteNum];

            for (int i = 0; i < iByteNum; i++)
            {
                baOutput[i] = baInput[i];
            } 

            return baOutput;
        }
          
         public static byte GetTcpEMessageTypeDesc(EMessageType eMsgType)
        {
            byte bRes;

            switch(eMsgType)
            {
                case EMessageType.FileContent_InBytes:
                    bRes = 1;
                    break;
                case EMessageType.OK:
                    bRes = 2;
                    break;
                case EMessageType.NO:
                    bRes = 3;
                    break;
                case EMessageType.SENDFILE:
                    bRes = 4;
                    break;
                case EMessageType.Bye:
                    bRes = 5;
                    break;  
                case EMessageType.Hellow:
                    bRes = 6;
                    break;
                case EMessageType.Text:
                    bRes = 7;
                    break; 
                default:
                    bRes = 0;
                    break;
            }

            return bRes;
        }

        public static EMessageType GetTcpEMessageTypeDesc(byte bMessageTypeByte)
        { 
            switch(bMessageTypeByte)
            {
                case 1:
                    return EMessageType.FileContent_InBytes;
                case 2:
                    return EMessageType.OK;
                case 3:
                    return EMessageType.NO;
                case 4:
                    return EMessageType.SENDFILE;
                case 5:
                    return EMessageType.Bye;
                case 6: 
                    return EMessageType.Hellow;
                case 7:
                    return EMessageType.Text;
                default:
                    return EMessageType.Unknown; 
            }
        }

        public static IPAddress GetMyLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return IPAddress.Parse(localIP);
        }

        public static byte[] ConcatByteArrays(byte[] baFirst, byte[] baSecond)
        {
            byte[] baNew = new byte[baFirst.Length + baSecond.Length];
             
            for(int i = 0; i < baFirst.Length; i++)
            {
                baNew[i] = baFirst[i];
            }

            for(int i = baFirst.Length; i < baFirst.Length+baSecond.Length; i++)
            {
                baNew[i] = baSecond[i - baFirst.Length];
            }

            return baNew;
        }
         
        public static byte[] EncryptBytesAndInsertIV_AndMsgType(byte[] key, byte[] baPlainText, EMessageType eMsgType)
        {
            byte[] baRes;
            byte[] encryptedText = null;
            byte[] baIV = new byte[iKeyIvSizeInBytes];

            baPlainText = InsertMessageTypeCode(baPlainText, eMsgType); 
            key = Trimming(key);
             
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
                aes.BlockSize = BLOCK_AND_KEY_SIZE;
                aes.Key = key;
                aes.GenerateIV(); 
                 Array.Copy(aes.IV, baIV,iKeyIvSizeInBytes);
                aes.Mode = CipherMode.CBC;
                 
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(),
                                              CryptoStreamMode.Write); 

                cs.Write(baPlainText, 0, baPlainText.Length); 
                cs.Close();
                encryptedText = ms.ToArray();
                ms.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("encrypt Error!!!:" + ex.Message);
            }
             
            baRes = new byte[encryptedText.Length + iKeyIvSizeInBytes]; 
            

            baRes = CUtils.ConcatByteArrays(baIV, encryptedText);
            baRes = InsertEndMsgDelimiter(TCP_END_SINGLE_SIGN, TCP_END_SIGN_NUM_OF_SIGNS, baRes);
           

            return baRes;
        }

        private static byte[] InsertMessageTypeCode(byte[] baMsg, EMessageType eMsgType)
        {
            byte[] baMsgCode = new byte[1];

            byte b = CUtils.GetTcpEMessageTypeDesc(eMsgType);

            baMsgCode[0] = b;

            return CUtils.ConcatByteArrays(baMsgCode, baMsg);
        }

        public static byte[] DecryptBytes(byte[] _encryptedText, byte[] key, out EMessageType eMsgType)
        {
            _encryptedText = CUtils.RemoveSignalFromMsg(TCP_END_SINGLE_SIGN, TCP_END_SIGN_NUM_OF_SIGNS,_encryptedText);

            byte[] baResult = new byte[0];
            byte[] encryptedText = new byte[0];
            byte bMsgType = 0;
            int iCyperLength = _encryptedText.Length - iKeyIvSizeInBytes;
            if (iCyperLength > 0)
            {
                encryptedText = new byte[iCyperLength]; 
                for (int i = iKeyIvSizeInBytes; i < _encryptedText.Length; i++)
                {
                    encryptedText[i - iKeyIvSizeInBytes] = _encryptedText[i];
                }
            }

            key = Trimming(key);

            byte[] baDecrypted = new byte[0];
            try
            {
                MemoryStream ms = new MemoryStream();
                 
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
                aes.BlockSize = BLOCK_AND_KEY_SIZE;
                aes.Key = key;
                byte[] baIV = new byte[iKeyIvSizeInBytes];
                for (int i = 0; i < iKeyIvSizeInBytes; i++)
                {
                    baIV[i] = _encryptedText[i];
                }
                aes.IV = baIV;
                aes.Mode = CipherMode.CBC;
                 
                CryptoStream cs = new CryptoStream(ms,
                    aes.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(encryptedText, 0, encryptedText.Length);
                cs.FlushFinalBlock();
                  
                baDecrypted = ms.ToArray(); 
                bMsgType = baDecrypted[0];

                baResult = new byte[baDecrypted.Length - 1];
                Array.Copy(baDecrypted, 1, baResult, 0, baResult.Length); 
            }
            catch (Exception ex)
            {
                throw new Exception("encrypt dycrypting!!!:" + ex.Message);
            }

            eMsgType = CUtils.GetTcpEMessageTypeDesc(bMsgType);
            return baResult;
        }
         
        public static byte[] GetMessageWithoutTcpEndSignal(byte[] msgWithTcpSignal)
        { 
            long lNumOfMsgContentBytes = msgWithTcpSignal.Length;
            int iSignCount = 0;
            for (long i = 0; i < msgWithTcpSignal.Length; i++)
            {
                if (msgWithTcpSignal[i] == CUtils.TCP_END_SINGLE_SIGN)
                {
                    iSignCount++;
                    if (iSignCount == CUtils.TCP_END_SIGN_NUM_OF_SIGNS)
                    {
                        lNumOfMsgContentBytes = i - CUtils.TCP_END_SIGN_NUM_OF_SIGNS + 1;
                        break;
                    }
                }
                else
                {
                    iSignCount = 0;
                }
            }

            byte[] baRes = new byte[lNumOfMsgContentBytes];

            Array.Copy(msgWithTcpSignal, 0, baRes, 0, lNumOfMsgContentBytes);

            return baRes;
        }

        //insert end message sign and duplicate sign when found in message
        public static byte[] InsertEndMsgDelimiter(byte bSign, int iSignNumOfTimes, byte[] baMsg)
        {
            int iFindDelCounter = 0;
            List<byte> olByte = new List<byte>();

            for (int i = 0; i < baMsg.Length; i++)
            {
                olByte.Add(baMsg[i]);
                if (baMsg[i] == bSign)
                {
                    iFindDelCounter++;
                    if (iSignNumOfTimes == iFindDelCounter)
                    {
                        iFindDelCounter = 0;
                        for (int z = 0; z < iSignNumOfTimes; z++)
                        {
                            olByte.Add(bSign);
                        }
                    }
                }
                else
                {
                    iFindDelCounter = 0;
                }
            }

            for (int z = 0; z < iSignNumOfTimes; z++)
                olByte.Add(bSign);

            return olByte.ToArray();
        }

        //takes message with endSignal (and signal duplicated in message) and return original message
        public static byte[] RemoveSignalFromMsg(byte bSign, int iSignNumOfTimes, byte[] baMsg)
        {
            List<byte> olByte = new List<byte>();
            int iFindDelCounter = 0;

            for (int i = 0; i < baMsg.Length; i++)
            {
                if (baMsg[i] == bSign)
                {
                    iFindDelCounter++; //count delimiter sign
                    if (iSignNumOfTimes * 2 == iFindDelCounter)//if sign spotted 2 times then its should be 1 time on Original Msg
                    {
                        iFindDelCounter = 0;
                        for (int z = 0; z < iSignNumOfTimes; z++)//put 1 time the delimiter sequence on msg
                        {
                            olByte.Add(bSign);
                        }
                    } 
                }

                if (baMsg[i] != bSign || i == baMsg.Length - 1)//if last byte Or not a delimiter sequence Then copy all delimiter signs skipped
                {
                    if (i == baMsg.Length - 1 && baMsg[i] == bSign)//if end of squence dont copt it
                        iFindDelCounter -= iSignNumOfTimes;

                    for (int z = 0; z < iFindDelCounter; z++)//insert all sign because we new see its not a full delimiter sequence
                    {
                        olByte.Add(bSign);
                    }

                    if (baMsg[i] != bSign)
                    {
                        olByte.Add(baMsg[i]);
                    }

                    iFindDelCounter = 0;
                }
            }

            return olByte.ToArray();
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                if (b > 15)
                    hex.AppendFormat("{0:x2},", b);
                else
                    hex.AppendFormat("{0:x1},", b);
            }
            return hex.ToString();
        }

        public static byte[] CheckMacAndReturnMsgByteArray(byte[] baMsgWithMac, out bool IsMacOK, out byte[] baHMAcInMsg)
        {
            List<byte[]> lbaRes = Message.SpliteMultiMessagesIntoByteArrays(baMsgWithMac, CUtils.HASH_DELIMITER_SIGN, CUtils.HASH_DELIMITER_NUM_OF_TIMES);
            List<byte> lbHash = new List<byte>();

            if (lbaRes.Count != 2 || lbaRes[1].Length < CUtils.HASH_SALT_BYTE_NUM) throw new Exception("Msg wasn`t recieved like send");

            byte[] baSalt = new byte[CUtils.HASH_SALT_BYTE_NUM];
            
            for(int i=0; i < baSalt.Length; i++)
            {
                baSalt[i] = lbaRes[1][i];
            }


            for(int i = baSalt.Length; i < lbaRes[1].Length; i++)
            {
                lbHash.Add(lbaRes[1][i]);
            }

            baHMAcInMsg = lbHash.ToArray();

            IsMacOK = ByteArrayCompare(CUtils.GetHMAC(baSalt, lbaRes[0]), baHMAcInMsg);

            return lbaRes[0]; 
        }

        public static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        public static void InsertHMAC_ToMessage(Message oMsg)
        { 
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            oMsg.baHashSalt = new byte[HASH_SALT_BYTE_NUM];

            rand.GetBytes(oMsg.baHashSalt);

            oMsg.baHash = CUtils.GetHMAC(oMsg.baHashSalt, oMsg.GetEncMessage()); 
        }

        


        public static byte[] GetHMAC(byte[] baSalt, byte[] baMsg)
        { 
            HMACSHA512 oHMACSHA512 = new HMACSHA512(CSession.baPassworMacdKey);
            byte[] baTemp = CUtils.ConcatByteArrays(baSalt, baMsg);
            baTemp = CUtils.ConcatByteArrays(baTemp, baSalt);

            return oHMACSHA512.ComputeHash(baTemp);
        }
    } 
}
