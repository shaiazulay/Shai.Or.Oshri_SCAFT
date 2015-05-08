using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static byte[] Encrypt(byte[] key, byte[] iv, byte[] baPlainText)
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

                swOut.Write(baPlainText);
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

        private static byte[] PaddingOrTrimming(byte[] baInput)
        {
            int iBytNum = BLOCK_AND_KEY_SIZE/8;
            byte[] baOutput = new byte[iBytNum];

            for (int i = 0; i < iBytNum; i++)
            {
                baOutput[i] = (i >= baInput.Length) ? KEY_AND_IV_PADDING : baInput[i];
            } 

            return baOutput;
        }
        public static string getRequestType(string sentence)
        {
            return sentence.Split(' ').FirstOrDefault();
        }
        public static string getOnlyString(string sentence)
        {
            return Remove(sentence, getRequestType(sentence), 1).TrimStart(' ');
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
    }

}
