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

        public static byte[] Encrypt(byte[] key, string sPlainText)
        {
            key = Trimming(key);
             

            byte[] cipherText = null;
            try
            { 
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
                aes.BlockSize = BLOCK_AND_KEY_SIZE;
                aes.Key = key;
                aes.GenerateIV();
                CSession.baCurrentIV = aes.IV;
                aes.Mode = CipherMode.CBC;
                
                

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(),
                                              CryptoStreamMode.Write);
                StreamWriter swOut = new StreamWriter(cs, CSession.TextMessageContentEncoding);

                swOut.Write(sPlainText);
                swOut.Close();
                cs.Close(); 
                cipherText = ms.ToArray(); 
            }
            catch (Exception ex)
            {
                throw new Exception("encrypt Error!!!:" + ex.Message);
            }

            return cipherText;
        }

        public static string Decrypt(byte[] _cipherText, byte[] key)
        {
            byte[] cipherText = new byte[0];
            int iCyperLength = _cipherText.Length - iKeyIvSizeInBytes;
            if (iCyperLength > 0)
            {
                cipherText = new byte[iCyperLength];

                for(int i = iKeyIvSizeInBytes; i < _cipherText.Length; i++)
                {
                    cipherText[i - iKeyIvSizeInBytes] = _cipherText[i];
                }
            }

            key = Trimming(key); 

            string sResult = "Error Decrypting";
            try
            {
                MemoryStream ms = new MemoryStream(cipherText);

                /*DESCryptoServiceProvider des = new DESCryptoServiceProvider();*/
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
                aes.BlockSize = BLOCK_AND_KEY_SIZE;
                aes.Key = key;
                byte[] baIV = new byte[iKeyIvSizeInBytes]; 
                for (int i = 0; i < iKeyIvSizeInBytes; i++)
                {
                    baIV[i] = _cipherText[i];
                }
                aes.IV = baIV;
                aes.Mode = CipherMode.CBC;

                  
            
                 
                CryptoStream cs = new CryptoStream(ms,
                    aes.CreateDecryptor(), CryptoStreamMode.Read);
                StreamReader srIn = new StreamReader(cs);

                sResult = srIn.ReadToEnd();
                srIn.Close();
                cs.Close();
            }
            catch (Exception ex)
            { 
                throw new Exception("encrypt dycrypting!!!:" + ex.Message);
            }

            return sResult;
        }

        //public bool Test(byte[] key, byte[] iv, byte[] baPlainText)
        //{
        //    byte[] enc = Encrypt( key, iv,  baPlainText);
        //    Decrypt(enc, key, iv);
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

        public static string GetEMessageTypeDesc(EMessageType eMsgType)
         {
            switch (eMsgType)
            {
                case EMessageType.Hellow:
                    return "HELLO";
                case EMessageType.Bye:
                    return "BYE";
                case EMessageType.SENDFILE:
                    return "SENDFILE";
                case EMessageType.OK:
                    return "OK";
                case EMessageType.NO:
                    return "NO";
                case EMessageType.Text:
                    return "Text";
                case EMessageType.FileTransfer:
                    return "FileTransfer";
                default:
                    return "Unknown";
            }
         }

        public static EMessageType GetMessageType(string sMsgType)
        {
            switch (sMsgType)
            {
                case "HELLO":
                    return EMessageType.Hellow;
                case "BYE":
                    return EMessageType.Bye;
                case "SENDFILE":
                    return EMessageType.SENDFILE;
                case "OK":
                    return EMessageType.OK;
                case "NO":
                    return EMessageType.NO;
                case "Text":
                    return EMessageType.Text;
                case "FileTransfer":
                    return EMessageType.FileTransfer;
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

        public static byte[] ConcatByteArrats(byte[] baFirst, byte[] baSecond)
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
    }

    

     
    
}
