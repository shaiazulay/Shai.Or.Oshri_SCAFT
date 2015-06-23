using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SCAFTI
{
    public static class MyHMAC
    {
        private static SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider();
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private static int SALT_BYTE_NUM = 16;
        private static string IPAD_HEX_BYTE = "36";
        private static string OPAD_HEX_BYTE = "5c";
        
          
        public static byte[] GetMacKeyXorHexByte(string sHex)
        {
            byte[] _baRes = new byte[CSession.baPassworMacdKey.Length];

            for (int i = 0; i < _baRes.Length; i++)
            {
                _baRes[i] = MyHMAC.XOR_byteWithHex(CSession.baPassworMacdKey[i], sHex);
            }
            return _baRes;
        }
    
         
        public static void PutHashInThisMessage(Message oMsg)
        {
            byte[] pi = GetMacKeyXorHexByte(IPAD_HEX_BYTE);
            byte[] keyXorOpad = GetMacKeyXorHexByte(OPAD_HEX_BYTE);
            byte[] baMsg = oMsg.GetEncMessage();
            byte[] baTemp;
            byte[] temp1;
            byte[] mcode;

            baTemp = CUtils.ConcatByteArrays(pi, baMsg);
            temp1 = sha512.ComputeHash(sha512.ComputeHash(baTemp));


            baTemp = CUtils.ConcatByteArrays(keyXorOpad, temp1);

            mcode = sha512.ComputeHash(sha512.ComputeHash(baTemp));

            oMsg.baHash = mcode;
        } 

        //return bInput XOR sHex
        private static byte XOR_byteWithHex(byte bInput, string sHex)
        {
            byte bHex = byte.Parse(sHex, System.Globalization.NumberStyles.HexNumber);

            return (byte)(bInput ^ bHex);
        }
    }
}
