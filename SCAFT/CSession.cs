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
        public static Encoding TextMessageContentEncoding { get { return Encoding.UTF8; } }

        public static string sUserName
        {
            get { return "shai"; }
            
        }

        private static int _iPort;

        public static int iPort { get{return 5000;}
            set { _iPort = value; } 
        }

        private static IPAddress _oMulticastIP;
        public static IPAddress oMulticastIP
        {
            get { return IPAddress.Parse("224.1.1.77"); }
            set { _oMulticastIP = value; }
        }

        private static byte[] _baPasswordKey;
        public static byte[] baPasswordKey 
        {
            get
            {
                return CUtils.Trimming(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
                                    0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20});
            }
            set { _baPasswordKey = value; } 
        }

        private static List<Form> _olForms;

        private static byte[] _baCurrentIV;

        public static byte[] baCurrentIV { get { return _baCurrentIV; } 
            set{_baCurrentIV = value;}
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
