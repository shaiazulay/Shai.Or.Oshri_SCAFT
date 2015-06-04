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
        public static byte TCP_END_SINGLE_SIGN = 0x4;
        public static byte TCP_END_SIGN_NUM_OF_SIGNS = 40;
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

        private static int iKeyIvSizeInBytes { get { return BLOCK_AND_KEY_SIZE / 8; } }

        public static byte[] ConvertUTF8_toBytes(string baInput)
        {
            return Encoding.UTF8.GetBytes(baInput);
        }

        public static string ConvertBytesToUTF8(byte[] baInput)
        {
            return Encoding.UTF8.GetString(baInput);
        }

        public static string CovertUtf8ToAsci(string sUtf8String)
        {
            byte[] baUtf8bytes = Encoding.UTF8.GetBytes(sUtf8String);
            byte[] baWin1252Bytes = Encoding.Convert(
                            Encoding.UTF8, Encoding.GetEncoding("windows-1252"), baUtf8bytes);
            return Encoding.ASCII.GetString(baWin1252Bytes);
        }

        //public static byte[] Encrypt(byte[] key, string sPlainText)
        //{
        //    key = Trimming(key);
        //    byte[] baIV;

        //    byte[] cipherText = null;
        //    try
        //    { 
        //        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
        //        aes.BlockSize = BLOCK_AND_KEY_SIZE;
        //        aes.Key = key;
        //        aes.GenerateIV();
        //        baIV = aes.IV; 
        //        aes.Mode = CipherMode.CBC;
                 
        //        MemoryStream ms = new MemoryStream();
        //        CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(),
        //                                      CryptoStreamMode.Write);
        //        StreamWriter swOut = new StreamWriter(cs, CSession.TextMessageContentEncoding);

        //        swOut.Write(sPlainText);
        //        swOut.Close();
        //        cs.Close(); 
        //        cipherText = ms.ToArray();
        //        cipherText = CUtils.ConcatByteArrays(baIV, cipherText);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("encrypt Error!!!:" + ex.Message);
        //    }

        //    return cipherText;
        //}

        //public static string Decrypt(byte[] _encryptedText, byte[] key)
        //{
        //    byte[] encryptedText = new byte[0];
        //    int iEncryptedLength = _encryptedText.Length - iKeyIvSizeInBytes;
        //    if (iEncryptedLength > 0)
        //    {
        //        encryptedText = new byte[iEncryptedLength];

        //        for (int i = iKeyIvSizeInBytes; i < _encryptedText.Length; i++)
        //        {
        //            encryptedText[i - iKeyIvSizeInBytes] = _encryptedText[i];
        //        }
        //    }

        //    key = Trimming(key); 

        //    string sResult = "Error Decrypting";
        //    try
        //    {
        //        MemoryStream ms = new MemoryStream(encryptedText);

        //        /*DESCryptoServiceProvider des = new DESCryptoServiceProvider();*/
        //        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
        //        aes.BlockSize = BLOCK_AND_KEY_SIZE;
        //        aes.Key = key;
        //        byte[] baIV = new byte[iKeyIvSizeInBytes]; 
        //        for (int i = 0; i < iKeyIvSizeInBytes; i++)
        //        {
        //            baIV[i] = _encryptedText[i];
        //        }
        //        aes.IV = baIV;
        //        aes.Mode = CipherMode.CBC;

                  
            
                 
        //        CryptoStream cs = new CryptoStream(ms,
        //            aes.CreateDecryptor(), CryptoStreamMode.Read);
        //        StreamReader srIn = new StreamReader(cs);

        //        sResult = srIn.ReadToEnd();
        //        srIn.Close();
        //        cs.Close();
        //    }
        //    catch (Exception ex)
        //    { 
        //        throw new Exception("encrypt dycrypting!!!:" + ex.Message);
        //    }

        //    return sResult;
        //}
         
        public static byte[] Trimming(byte[] baInput)
        {
            int iByteNum = iKeyIvSizeInBytes;
            byte[] baOutput = new byte[iByteNum];

            for (int i = 0; i < iByteNum; i++)
            {
                baOutput[i] = baInput[i];
            } 

            return baOutput;
        }
         
        // metod to cut first substring.
         private static string Remove(this string source, string remove, int firstN)
        {
            if (firstN <= 0 || string.IsNullOrEmpty(source) || string.IsNullOrEmpty(remove))
            {
                return source;
            }
            int index = source.IndexOf(remove);
            return index < 0 ? source : source.Remove(index, remove.Length).Remove(remove, --firstN);
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
                case EMessageType.FileTransfer:
                    bRes = 6;
                    break;
                case EMessageType.Hellow:
                    bRes = 7;
                    break;
                case EMessageType.Text:
                    bRes = 8;
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
                    return EMessageType.FileTransfer;
                case 7:
                    return EMessageType.Hellow;
                case 8:
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
            ///////////////////////////////////////////////////////////////////////
            List<long> olLocationSignalIn = new List<long>();
            int iNumOfTimesPerSign = 0;
            bool IsAddSignal = false;
            for (int i = 0; i < encryptedText.Length; i++)
            {
                if (encryptedText[i] == TCP_END_SINGLE_SIGN)
                {
                    iNumOfTimesPerSign++;
                    if (iNumOfTimesPerSign >= TCP_END_SIGN_NUM_OF_SIGNS)
                        IsAddSignal = true;
                }
                else
                {
                    iNumOfTimesPerSign = 0;
                    if (IsAddSignal)
                    {
                        IsAddSignal = false;
                        olLocationSignalIn.Add(i-1);
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////

            baRes = CUtils.ConcatByteArrays(baIV, encryptedText);
            baRes = CUtils.ConcatByteArrays(baRes, TCP_END_MESSAGE_SIGN);
            //lMsgLength += baRes.Length + 40;

           // byte[] baMsgLength40;

           // byte[] baMsgLength = BitConverter.GetBytes(lMsgLength); 

           // byte[] baPadding = new byte[40 - baMsgLength.Length];
           // baMsgLength40 = CUtils.ConcatByteArrats(baMsgLength, baPadding);

           // baRes = CUtils.ConcatByteArrats(baMsgLength40, baRes);

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
            _encryptedText = CUtils.GetMessageWithoutTcpEndSignal(_encryptedText);
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

                /*DESCryptoServiceProvider des = new DESCryptoServiceProvider();*/
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
    }
     
    
}
