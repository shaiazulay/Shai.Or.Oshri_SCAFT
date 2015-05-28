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

        private void btnString1Tobytes_Click(object sender, EventArgs e)
        {
            CUtils.GetMyLocalIPAddress();
            byte[] key = CUtils.ConvertUTF8_toBytes(txtKey.Text);
           

             byte[] plaintext1 = CUtils.ConvertUTF8_toBytes(txtPlainText1.Text);

            byte[] iv =  BitConverter.GetBytes(int.Parse(txtIV.Text));

            encrypted = CUtils.Encrypt(key, iv, txtPlainText1.Text);
            txtCyperText.Text = CUtils.ConvertBytesToUTF8(encrypted); 
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

            txtPlainText2.Text = CUtils.Decrypt(encrypted, key, iv);
        }


    }
}
