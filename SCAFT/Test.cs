using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFTI
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
         //   CRSA.AddOtherUserKeyToPublicKeys("Or2", "1234");

          //  CRSA.AddOtherUserKeyToPublicKeys("Or1", "12345");


      //      string s = CRSA.GetUserPublicKeyFromOtherUsersFile("yosef");

            CRSA.rsa = new RSACryptoServiceProvider();

            byte[] baMsg = { 0x24, 0x25 };

            byte[] baSign = CRSA.RSASign(baMsg);


          //  bool IsValid = CRSA.IsSignatureValid(CRSA.rsa.ToXmlString(false), baMsg, baSign);
         //   rsa.FromXmlString(s);

            //if (IsValid)
            //{
            //    int i = 2;
            ////    s = s;
            //}
        }

        
    }
}
