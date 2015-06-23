using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCAFTI
{
    public class CLog
    {
        public static string LOG_FILE_NAME = "log.txt";

        private static Object thisLock = new Object();

        public static void WriteLineToLoge(LogMessage oLogMessage)
        {
            string sLine = oLogMessage.GetLogLine();

            lock (thisLock)
            {
                using (TextWriter myWriter = new StreamWriter(LOG_FILE_NAME, true))
                {
                    TextWriter.Synchronized(myWriter).Write(sLine);
                }
            }
        }
    }

    public class LogMessage
    {
        public bool IsFileMessage;

        public DateTime dEventDateTime;

        public int iRecievedFromPort;

        public IPAddress oRecievedFromIP;

        public string sRecievedFromUserName;
         
        public byte[] baRecievedIV;

        public byte[] baRecievedMacValue;

        public byte[] baExpectedMacValue;

        public string sRecievedFileNameOrMessageContect;
         
        public LogMessage(DateTime _dEventDateTime, int _iRecievedFromPort, IPAddress _oRecievedFromIP,
            string _sRecievedFromUserName, byte[] _baRecievedIV, byte[] _baRecievedMacValue,
            byte[] _baExpectedMacValue, string _sRecievedFileNameOrMessageContect, bool IsFile)
        {
            dEventDateTime = _dEventDateTime;
            iRecievedFromPort = _iRecievedFromPort;
            oRecievedFromIP = _oRecievedFromIP;
            sRecievedFromUserName = _sRecievedFromUserName;
            baRecievedIV = _baRecievedIV;
            baRecievedMacValue = _baRecievedMacValue;
            baExpectedMacValue= _baExpectedMacValue;
            sRecievedFileNameOrMessageContect = _sRecievedFileNameOrMessageContect;

            IsFileMessage = IsFile;
        }

        public string GetLogLine()
        {
            string sLine = (IsFileMessage) ? "BAD_HMAC_FILE_MESSAGE:: " : "BAD_HMAC_TEXT_MESSAGE::";

            sLine += "Date- (" + dEventDateTime.ToString() + ")| ";

            sLine += "IP:Port- (" + oRecievedFromIP.ToString() + ":" + iRecievedFromPort.ToString() + ")| ";

            sLine += "UserName-(\"" + sRecievedFromUserName + "\")| ";

            sLine += "RecievedIV(bytesInHex)- (" + CUtils.ByteArrayToHexString(baRecievedIV) + ")| ";

            sLine += "RecievedMacValue(bytesInHex)- (" + CUtils.ByteArrayToHexString(baRecievedMacValue) + ")| ";

            sLine += "ExpectedMacValue(bytesInHex)- (" + CUtils.ByteArrayToHexString(baExpectedMacValue) + ")| ";


            sLine += (IsFileMessage) ? " FileName-" : " MessageContent-";
            
            sLine += " (\"" + sRecievedFileNameOrMessageContect + "\")";
            
            sLine += Environment.NewLine + Environment.NewLine;
             
            return sLine;

        }
    }
}
