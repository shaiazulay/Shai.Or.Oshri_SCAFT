using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFTI
{
    public partial class InsertUserNameForm : Form
    {
        public InsertUserNameForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                MoveToSCAFT();
            }
        }

        private bool IsValidated()
        {
            return true;
        }

        private void MoveToSCAFT()
        {

        }
    }
}
