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
        private static byte KEY_AND_IV_PADDING = 1;

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

        public static byte[] Encrypt(byte[] key, byte[] iv, string sPlainText)
        {
            key = PaddingOrTrimming(key);
            iv = PaddingOrTrimming(iv);

            byte[] cipherText = null;
            try
            { 
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
                aes.BlockSize = BLOCK_AND_KEY_SIZE;
                aes.Key = key; 
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                
                

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(),
                                              CryptoStreamMode.Write);
                StreamWriter swOut = new StreamWriter(cs);

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

        public static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            key = PaddingOrTrimming(key);
            iv = PaddingOrTrimming(iv);

            string sResult = "Error Decrypting";
            try
            {
                MemoryStream ms = new MemoryStream(cipherText);

                /*DESCryptoServiceProvider des = new DESCryptoServiceProvider();*/
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();//
                aes.BlockSize = BLOCK_AND_KEY_SIZE;
                aes.Key = key;
                aes.IV = iv;
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

        public static byte[] PaddingOrTrimming(byte[] baInput)
        {
            int iBytNum = BLOCK_AND_KEY_SIZE/8;
            byte[] baOutput = new byte[iBytNum];

            for (int i = 0; i < iBytNum; i++)
            {
                baOutput[i] = (i >= baInput.Length) ? KEY_AND_IV_PADDING : baInput[i];
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
    }

     
    
}
