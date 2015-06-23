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
    public partial class ConfigurationDetailsForm : Form
    {
        public ConfigurationDetailsForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                InsertConfiguration();

                MoveToInsertUserName();
            }
        }

        private bool IsValidated()
        {
            return true;
        }

        private void InsertConfiguration()
        {

        }

        private void MoveToInsertUserName()
        {

        }
    }
}
