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
            get { return "shai2"; }
            
        }

        private static int _iPort;

        public static int iPort { get{return 5000;}
            set { _iPort = value; } 
        }

        private static IPAddress _oMulticastIP;
        public static IPAddress oMulticastIP
        {
            get { return IPAddress.Parse("224.1.1.1"); }
            set { _oMulticastIP = value; }
        }

        private static byte[] _baPasswordKey;
        public static byte[] baPasswordKey 
        {
            get
            {
                return CUtils.PaddingOrTrimming(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
                                    0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20});
            }
            set { _baPasswordKey = value; } 
        }

        private static List<Form> _olForms;

        public static byte[] lCurrentIV { get { return CUtils.PaddingOrTrimming(new byte[] {}); } 
        //    set;
        }

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
