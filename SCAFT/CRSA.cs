using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFTI
{
    public static class CRSA
    {
        private static string RSA_SIGN_ALG = "SHA256";
        private static int SIGN_LENGTH_SIZE = 8;
        public static RSACryptoServiceProvider rsa;
        private static string OTHER_USERS_KEYS_FILE_PATH = "SCAFTIDS_Key_Store.txt"; 
        private static char OTHER_USERS_DELIMITER = ' '; 

        public static void GenerateWithPrivateKey(int iKeySizeInBits)
        {
            rsa = new RSACryptoServiceProvider(iKeySizeInBits); 
        }

        public static byte[] RSASign(byte[] baMsg)
        {
            object oHalg = CryptoConfig.CreateFromName(RSA_SIGN_ALG);
            return rsa.SignData(baMsg, oHalg);
        }

        public static byte[] AddSignatureToMsg(byte[] baMsg)
        {
            byte[] baSign = RSASign(baMsg);
            byte[] baSignLength = CUtils.InsertIntValueToByteArray(baSign.Length, CRSA.SIGN_LENGTH_SIZE);

            byte[] baTemp = CUtils.ConcatByteArrays(baSignLength, baSign);

            baTemp = CUtils.ConcatByteArrays(baTemp, baMsg); 

            return baTemp; 
        }

        public static List<byte[]> GetSignBytesAndMsgBytes(byte[] baMsgWithSign)
        {
            return CUtils.SplitByLength(baMsgWithSign, CRSA.SIGN_LENGTH_SIZE); 
        }

        public static bool? IsSignatureValid(Message oMessage, byte[] baSignature, byte[] baDataSigned)
        {
            RSACryptoServiceProvider UserRsa = new RSACryptoServiceProvider();
            object oHalg = CryptoConfig.CreateFromName(RSA_SIGN_ALG);

            if(oMessage.oUser.sUserName == CUtils.oCurrentUser.sUserName)
            {
                return CRSA.rsa.VerifyData(baDataSigned, oHalg, baSignature);
            }

            
            string sXmlPublicKey = CRSA.GetUserPublicKeyFromOtherUsersFile(oMessage.oUser.sUserName);

            if (sXmlPublicKey == null)
            {
                return false;
            }

            UserRsa.FromXmlString(sXmlPublicKey);

            

            return UserRsa.VerifyData(baDataSigned, oHalg, baSignature);
        }

        public static void AddOtherUserKeyToPublicKeys(string sUserName, string sUserPublicKey)
        { 
            try
            {
                if (GetUserPublicKeyFromOtherUsersFile(sUserName) == null)
                { 
                    using (StreamWriter sw = File.AppendText(OTHER_USERS_KEYS_FILE_PATH))
                    {


                        sw.WriteLine(" UserName:" + sUserName + "@UserKey:" + sUserPublicKey + "");
                    }
                }
                else
                {
                    MessageBox.Show("user Name public key already exists");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("exporting fail  check that a key was generated");
            } 
        }

        public static string GetUserPublicKeyFromOtherUsersFile(string sUserName)
        {
            using(FileStream fsIn = new FileStream(OTHER_USERS_KEYS_FILE_PATH, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using(StreamReader srIn = new StreamReader(fsIn))
                {
                    string sUsers = srIn.ReadToEnd();
                    string[] saUsers = sUsers.Split(OTHER_USERS_DELIMITER);
 
                    foreach(string sUserData in saUsers)
                    {
                        int iTempIndex = sUserData.IndexOf(':');//getNameStartIndex
                        if (iTempIndex > 0 && iTempIndex < sUserData.Length)
                        {
                            string sTemp = sUserData.Substring(iTempIndex + 1); //now sTemp start from UserName
                            iTempIndex = sTemp.IndexOf('@');//getNameEndIndex
                            string sCurrentUserName = sTemp.Substring(0, iTempIndex);

                            if (sCurrentUserName == sUserName)
                            {
                                sTemp = sTemp.Substring(iTempIndex + 1);//now sTemp start from UserKeyTitle
                                iTempIndex = sTemp.IndexOf(':');
                                sTemp = sTemp.Substring(iTempIndex + 1);//now sTemp start from UserKeyData
                                return Regex.Replace(sTemp, @"\t|\n|\r", "");
                            }
                        }
                    }
                }
            }

            return null;
            
        }

    }
}
