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
            int d=1000000000;

            byte[] baMsgLength = BitConverter.GetBytes(d);

            byte[] baTemp = new byte[CUtils.TCP_MESSAGE_LENGTH_FIELD_SIZE];

            for (int i = 0; i < baTemp.Length;i++ )
            {
                baTemp[i] = (i < baMsgLength.Length) ? baMsgLength[i] : (byte)0;
            }


            long c =BitConverter.ToInt64(baTemp, 0);

            if (d == c)
                d++;
            bool IsEqual = false;
            
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
