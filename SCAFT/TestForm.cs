using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFT
{
    public partial class TestForm : Form
    {
        byte[] _bytes;

        byte[] encrypted;
        public TestForm()
        {
            InitializeComponent();
        }

        private byte[] InsertSignalToMsg(byte bSign, int iSignNumOfTimes, byte[] baMsg)
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

        private byte[] RemoveSignalFromMsg(byte bSign, int iSignNumOfTimes, byte[] baMsg)
        {
            List<byte> olByte = new List<byte>();
            int iFindDelCounter = 0;

            for (int i = 0; i < baMsg.Length; i++)
            {
                if (baMsg[i] == bSign)
                {
                    iFindDelCounter++;
                    if (iSignNumOfTimes * 2 == iFindDelCounter)
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
                    if (i == baMsg.Length - 1)
                        iFindDelCounter -= iSignNumOfTimes;

                    for (int z = 0; z < iFindDelCounter; z++)
                    {
                        olByte.Add(bSign);
                    }
                    iFindDelCounter = 0;
                    olByte.Add(baMsg[i]);
                }
            }

            return olByte.ToArray();
        }

        private void btnString1Tobytes_Click(object sender, EventArgs e)
        {
            long lMsgLength = 240000;
            byte[] del = { 0x4 };
            byte[] a = { 0x0, 0x1, 0x4, 0x2, 0x3, 0x4, 0x4, 0x4, 0x5, 0x6, 0x7 };
            byte[] b = { 0x9, 0x9, 0x9, 0x9, 0x9, 0x9, 0x9, 0x9 };

            int icounter = 0;

            byte[] After = InsertSignalToMsg(0x4, 2, a);

            byte[] After2 = RemoveSignalFromMsg(0x4, 2, After);
            
            
           bool IsEqual2 = true;
           if (a.Length == After2.Length)
            {
                for (int i = 0; i < After2.Length; i++)
                {
                    if (a[i] != After2[i])
                        IsEqual2 = false;
                }
            }
            else
            {
                IsEqual2 = false;
            }


            if (IsEqual2)
            {
                int i = 9;
            }
            else
            {
                int i = 9;
            }
            
            
             

            byte[] c =CUtils.InsertArrayInMiddleOfArray(a, b, 3);
            byte[] baMsgLength = BitConverter.GetBytes(lMsgLength);

            byte[] baMsgLength40;

            byte[] baPadding = new byte[40 - baMsgLength.Length];
            baMsgLength40 = CUtils.ConcatByteArrays(baMsgLength,baPadding);

            long res = BitConverter.ToInt64(baMsgLength40,0);

            if (res == lMsgLength)
            {
                int i = 2;
            }
            else
            {
                int i = 2;
            }
            //CUtils.GetMyLocalIPAddress();
            //byte[] key = CUtils.ConvertUTF8_toBytes(txtKey.Text);
           

            // byte[] plaintext1 = CUtils.ConvertUTF8_toBytes(txtPlainText1.Text);

            //byte[] iv =  BitConverter.GetBytes(int.Parse(txtIV.Text));

            //encrypted = CUtils.Encrypt(key, txtPlainText1.Text);
            //txtCyperText.Text = CUtils.ConvertBytesToUTF8(encrypted); 
        }

        private string WriteBytes(byte[] array)
        {
            string spaced = "";

            for (int i = 0; i < array.Length; i++)
            {
                spaced += array[i] + " ";
            }

            // drop the last space
            spaced = spaced.Substring(0, spaced.Length - 1);

            return spaced;
        }

        private void btnBytesToString_Click(object sender, EventArgs e)
        {
            byte[] key = CUtils.ConvertUTF8_toBytes(txtKey.Text);

            byte[] iv = BitConverter.GetBytes(int.Parse(txtIV.Text));

            byte[] cypertext1 = CUtils.ConvertUTF8_toBytes(txtCyperText.Text);

             
        }


    }
}
