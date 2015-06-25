using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCAFTI
{
    public enum ELOG_MESSAGE_TYPE
    {
        BadMac,
        BadSign,
        BadMacAndSign,
        None
    }

    public enum EBadSignType
    {
        UncheckableSignature,
        FailedSignature,
        None
    }

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
        public EBadSignType eBadSignType;
        public ELOG_MESSAGE_TYPE eLOG_MESSAGE_TYPE;

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
            byte[] _baExpectedMacValue, string _sRecievedFileNameOrMessageContect, bool IsFile, ELOG_MESSAGE_TYPE _eLOG_MESSAGE_TYPE, EBadSignType _eBadSignType)
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
            eLOG_MESSAGE_TYPE = _eLOG_MESSAGE_TYPE;

            eBadSignType = _eBadSignType;
        }

        public string GetLogLine()
        {
            string sLine = "";
            string sMainData = "";
            
            
            sMainData += "Date- (" + dEventDateTime.ToString() + ")| ";

            sMainData += "IP:Port- (" + oRecievedFromIP.ToString() + ":" + iRecievedFromPort.ToString() + ")| ";

            sMainData += "UserName-(\"" + sRecievedFromUserName + "\")| ";

            sMainData += "RecievedIV(bytesInHex)- (" + CUtils.ByteArrayToHexString(baRecievedIV) + ")| ";

            sMainData += "RecievedMacValue(bytesInHex)- (" + CUtils.ByteArrayToHexString(baRecievedMacValue) + ")| ";

            sMainData += "ExpectedMacValue(bytesInHex)- (" + CUtils.ByteArrayToHexString(baExpectedMacValue) + ")| ";


            sMainData += (IsFileMessage) ? " FileName-" : " MessageContent-";

            sMainData += " (\"" + sRecievedFileNameOrMessageContect + "\")";

            sMainData += Environment.NewLine + Environment.NewLine;

            if (eLOG_MESSAGE_TYPE == ELOG_MESSAGE_TYPE.BadMac || eLOG_MESSAGE_TYPE == ELOG_MESSAGE_TYPE.BadMacAndSign)
            {
                sLine += (IsFileMessage) ? "BAD_HMAC_FILE_MESSAGE:: " : "BAD_HMAC_TEXT_MESSAGE:: ";
                sLine += sMainData;

            }

            if (eLOG_MESSAGE_TYPE == ELOG_MESSAGE_TYPE.BadSign || eLOG_MESSAGE_TYPE == ELOG_MESSAGE_TYPE.BadMacAndSign)
            {
                sLine += "BAD_DIGITAL_SIGNATURE";

                sLine +=  (eBadSignType == EBadSignType.FailedSignature) ?  "(Failed Signature)":
                    (eBadSignType == EBadSignType.UncheckableSignature)? "(Uncheckable Signature)":""; 

                sLine +=":: "+ sMainData;
            }
            

            return sLine;

        }
    }
}
