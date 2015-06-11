using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFT
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            byte[] ba1 = { 0x1, 0x2, 0x3 };
            byte[] salt1 = { 0x1 };
            byte[] hash1 = CUtils.GetHMAC(ba1, ba1);
            byte[] hash2 = CUtils.GetHMAC(salt1, ba1);

            bool IsEqual = true;

            if (hash1.Length != hash2.Length) IsEqual = false;

            if(IsEqual)
            {
                for (int i = 0; i < hash1.Length; i++)
                {
                    if (hash2[i] != hash1[i]) IsEqual = false;
                }
            }
            
            if(IsEqual)
            {
                int i = 1;
            }
            else
            {
                int i = 1;
            }
           
        }

        
    }
}
