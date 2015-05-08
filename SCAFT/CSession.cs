using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;

namespace SCAFT
{
    public static class CSession
    {
        public static string sUserName
        {
            get { return "ALICE"; }
            
        }

        public static int iPort { get; set; }

        public static IPAddress oMulticastFromIP { get; set; }

        public static IPAddress oMulticastToIP { get; set; }

        public static byte[] baPasswordKey { get; set; }

        private static List<Form> _olForms;

        public static long lCurrentIV { get; set; }

        public static List<Form> olForms 
        { 
            get
            {
                if (_olForms == null)
                    _olForms = new List<Form>();

                return _olForms;
             } 
        }

        public static void OrderedExit()
        {
            foreach(Form f in olForms)
            {
                f.Dispose();
            }
        }
    }
}
