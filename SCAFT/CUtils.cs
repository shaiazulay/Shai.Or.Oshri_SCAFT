using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFTI
{
    public static class CUtils
    {
        public static char HEX_STRING_DELIMITER = ',';
        public static int HASH_SALT_BYTE_NUM = 0; 
        public static int TCP_MESSAGE_LENGTH_FIELD_SIZE = 40;
        public static int REGULAR_MESSAGE_HEADER_LENGTH_FIELD_SIZE = 10;
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
           

            return baRes;
        }

        private static byte[] InsertMessageTypeCode(byte[] baMsg, EMessageType eMsgType)
        {
            byte[] baMsgCode = new byte[1];

            byte b = CUtils.GetTcpEMessageTypeDesc(eMsgType);

            baMsgCode[0] = b;

            return CUtils.ConcatByteArrays(baMsgCode, baMsg);
        }

        public static byte[] DecryptBytes(byte[] _encryptedText, byte[] key, out EMessageType eMsgType, out byte[] baIVRecieved)
        { 
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
                baIVRecieved = new byte[iKeyIvSizeInBytes];
                for (int i = 0; i < iKeyIvSizeInBytes; i++)
                {
                    baIVRecieved[i] = _encryptedText[i];
                }
                aes.IV = baIVRecieved;
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
           
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            for (int i = 0; i <ba.Length; i++)
            {
                hex.AppendFormat("{0:x2}", ba[i]);
                
                if(i < ba.Length - 1)//dint add ',' to after last byte
                {
                    hex.AppendFormat(HEX_STRING_DELIMITER.ToString());
                }
            }
            return hex.ToString();
        }

        public static byte[] HexStringToByeArray(string s)
        {
            try
            {
                string[] saBits = s.Split(HEX_STRING_DELIMITER);

                List<byte> lb = new List<byte>();

                for (int i = 0; i < saBits.Length; i++)
                {
                    lb.Add(Convert.ToByte(saBits[i], 16));
                }

                return lb.ToArray();
            }
            catch
            {
                MessageBox.Show("the hex string wasn`t in correct format");
                return new byte[0];
            }
        }

        public static Message CheckMacWriteToLog_AndReturnMessages(byte[] baMsgWithMac, int iPort, bool IsSignVerify, string sFileName = null)
        {
            try
            {
                bool DontWriteToLogNotVerified = true;

                if(IsSignVerify)
                {
                    DontWriteToLogNotVerified = CRSA.GetMessageWithoutSignatueIfVerify(out baMsgWithMac);
                }

                List<byte[]> lbaRes = SplitByLength(baMsgWithMac, CUtils.TCP_MESSAGE_LENGTH_FIELD_SIZE);
                List<byte> lbHash = new List<byte>();

                if (lbaRes.Count != 2 || lbaRes[1].Length < CUtils.HASH_SALT_BYTE_NUM) throw new Exception("Msg wasn`t recieved like send");

                byte[] baSalt = new byte[CUtils.HASH_SALT_BYTE_NUM];

                for (int i = 0; i < baSalt.Length; i++)
                {
                    baSalt[i] = lbaRes[1][i];
                }


                for (int i = baSalt.Length; i < lbaRes[1].Length; i++)
                {
                    lbHash.Add(lbaRes[1][i]);
                }

                byte[] baHMAcInMsg = lbHash.ToArray();
                byte[] baExpectedHMAC = CUtils.GetHMAC(baSalt, lbaRes[0]);
                bool IsMacOK = (lbaRes[0].Length == 0) ? false : ByteArrayCompare(baExpectedHMAC, baHMAcInMsg);

                Message oMessage = new Message(lbaRes[0]);

                if (!IsMacOK)
                {

                    LogMessage oLogMessage;
                    if (sFileName != null)
                    {
                        oLogMessage = new LogMessage(DateTime.Now, iPort, oMessage.oUser.oIP, oMessage.oUser.sUserName,
                                                   oMessage.baRecievedIV, baHMAcInMsg, baExpectedHMAC, sFileName, true);
                        MessageBox.Show("Warning!!!  File with a bad MAC digest Received (See \"" + CLog.LOG_FILE_NAME + "\" file).");
                    }
                    else
                    {
                        string sMsgContent;
                        switch(oMessage.eMessageType)
                        {
                            case EMessageType.Bye:
                                sMsgContent = "MessageType:Bye";
                                break;
                            case EMessageType.Hellow:
                                sMsgContent = "MessageType:Hellow";
                                break;
                            case EMessageType.NO:
                                sMsgContent = "MessageType:NO(about file sending)";
                                break;
                            case EMessageType.OK:
                                sMsgContent = "MessageType:OK(about file sending)";
                                break;
                            case EMessageType.SENDFILE:
                                sMsgContent = "MessageType:SENDFILE(about file sending)";
                                break;
                            case EMessageType.Text:
                                sMsgContent = "MessageType:Text MessageContent:\""+ oMessage.sStringContent+"\"";
                                break;
                            default:
                                sMsgContent = "MessageType:Unknown";
                                break;
                        }
                        oLogMessage = new LogMessage(DateTime.Now, iPort, oMessage.oUser.oIP, oMessage.oUser.sUserName,
                                                   oMessage.baRecievedIV, baHMAcInMsg, baExpectedHMAC, sMsgContent, false);
                        MessageBox.Show("Warning!!!  Message with a bad MAC digest Received (See \"" + CLog.LOG_FILE_NAME + "\" file).");
                    }
                    CLog.WriteLineToLoge(oLogMessage);
                    if (!DontWriteToLogNotVerified)
                    {
                        //tbd
                    }
                    return null;
                }
                else
                {
                    if (!DontWriteToLogNotVerified)
                    {
                        //tbd
                    }
                    return oMessage;
                }
            }
            catch { MessageBox.Show("Warning!!!  Altered Transmit Received."); return null; }
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

        public static void InsertHMAC_ToMessage(Message oMsg, byte[] baThisMsgEncyptedContent)
        { 
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            oMsg.baHashSalt = new byte[HASH_SALT_BYTE_NUM];

            rand.GetBytes(oMsg.baHashSalt);

            oMsg.baHash = CUtils.GetHMAC(oMsg.baHashSalt, baThisMsgEncyptedContent); 
        }
         
        public static byte[] GetHMAC(byte[] baSalt, byte[] baMsg)
        {
            HMACSHA256 oHMACSHA256 = new HMACSHA256(CSession.baPassworMacdKey);
            byte[] baTemp = CUtils.ConcatByteArrays(baSalt, baMsg);
            baTemp = CUtils.ConcatByteArrays(baTemp, baSalt);

            return oHMACSHA256.ComputeHash(baTemp);
        }

        //splits an array of bytes that is possible multi messages to arrays of bytes that each is one message encrypted. 
        public static List<byte[]> SplitByLength(byte[] baMsg, int iLengthFieldByteNum)
        { 
            byte[] baLength = new byte[iLengthFieldByteNum];

            for (int i = 0; i < baLength.Length; i++)
            {
                baLength[i] = baMsg[i];
            }

            long lMsgLength = BitConverter.ToInt64(baLength, 0);
            List<byte> lbTemp = new List<byte>();
            List<byte[]> lRes = new List<byte[]>();

            for (long i = baLength.Length; i < baMsg.Length; i++)
            {
                if (i == lMsgLength + baLength.Length)
                {
                    lRes.Add(lbTemp.ToArray());
                    lbTemp = new List<byte>();
                }

                lbTemp.Add(baMsg[i]);
            }

            lRes.Add(lbTemp.ToArray());

            return lRes;
        }

        //return byte[] with first CUtils.TCP_MESSAGE_LENGTH_FIELD_SIZE is msgContentLength, then MsgContent, then HASH_SALT_BYTE_NUM bytes with salt, then Hash
        public static byte[] GetMsgWithHMacBytes(Message oMsg, byte[] baMsgContent)
        {
            CUtils.InsertHMAC_ToMessage(oMsg, baMsgContent);
             
            byte[] baMsgLength = CUtils.InsertIntValueToByteArray(baMsgContent.Length, CUtils.TCP_MESSAGE_LENGTH_FIELD_SIZE);
              
            byte[] baTemp = CUtils.ConcatByteArrays(baMsgLength, baMsgContent);

            baTemp = CUtils.ConcatByteArrays(baTemp, oMsg.baHashSalt);

            baTemp = CUtils.ConcatByteArrays(baTemp, oMsg.baHash);

            return baTemp;
        }

        public static byte[] InsertIntValueToByteArray(int iValue, int iArraySize)
        {
            byte[] baTemp = BitConverter.GetBytes(iValue);

            byte[] baResult = new byte[iArraySize];

            for (int i = 0; i < baResult.Length; i++)
            {
                baResult[i] = (i < baTemp.Length) ? baTemp[i] : (byte)0;
            }

            return baResult;
        }
        
            


            
    } 
}
