using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCAFTI
{
    public enum EMessageType
    {
        Hellow,
        Bye,
        SENDFILE,
        OK,
        NO,
        Text, 
        FileContent_InBytes,
        Unknown
    }

    public class Message
    {
        private const char MESSAGE_DELIMITER_BETWEEN_FIELDS = ' ';
        public User oUser { get; set; }

        public EMessageType eMessageType { get; set; }

        public string sStringContent { get; set; }

        public byte[] baBytesContent { get; set; }

        public byte[] baRecievedIV { get; set; }

        public byte[] baHashSalt { get; set; }

        public byte[] baHash { get; set; }

        public Message(IPAddress oUserIP, string sUserName, EMessageType eMsgType, string sContent)
        {
            oUser = new User(oUserIP, sUserName);
            eMessageType = eMsgType;
            sStringContent = sContent;
        }

        public Message()
        {
            oUser = new User(IPAddress.None, "");
            eMessageType = EMessageType.Unknown;
            sStringContent = "";
        }

        public Message(IPAddress oUserIP, string sUserName, byte[] _baBytesContent)
        {
            eMessageType = EMessageType.FileContent_InBytes;
            oUser = new User(oUserIP, sUserName); 
            baBytesContent = _baBytesContent;
        }

        public Message(byte[] baEnc)
        {
            EMessageType eMsgTypeTemp;

            byte[] _baRecievedIV;

            byte[] baPlainBytes = CUtils.DecryptBytes(baEnc, CSession.baPasswordKey, out eMsgTypeTemp, out _baRecievedIV);

            baRecievedIV = _baRecievedIV;

            eMessageType = eMsgTypeTemp;

            List<byte[]> lbaMsg = CUtils.SplitByLength(baPlainBytes, CUtils.REGULAR_MESSAGE_HEADER_LENGTH_FIELD_SIZE);

            string sPlainText = CSession.TextMessageContentEncoding.GetString(lbaMsg[0]);

            string[] saBits = sPlainText.Split(MESSAGE_DELIMITER_BETWEEN_FIELDS);

            IPAddress oIp = null;
            IPAddress.TryParse(saBits[1], out oIp);

            oUser = new User(oIp, saBits[0]);

            if (lbaMsg.Count > 1)//Msg without content is possible (Hellow,Bye...)
            {
                if (eMessageType == EMessageType.FileContent_InBytes)
                {
                    this.baBytesContent = lbaMsg[1];
                }
                else
                {
                    this.sStringContent = CSession.TextMessageContentEncoding.GetString(lbaMsg[1]);
                }
            }
        }



        public byte[] GetEncMessage(bool withSign)
        {
            byte[] baEnc;

            string sPlainMsg = "";
            sPlainMsg += oUser.sUserName + MESSAGE_DELIMITER_BETWEEN_FIELDS;
            sPlainMsg += oUser.oIP.ToString() + MESSAGE_DELIMITER_BETWEEN_FIELDS;

            byte[] baHeaderContent = Encoding.UTF8.GetBytes(sPlainMsg);

            byte[] baHeaderLength = CUtils.InsertIntValueToByteArray(baHeaderContent.Length, CUtils.REGULAR_MESSAGE_HEADER_LENGTH_FIELD_SIZE);

            byte[] baMessage = CUtils.ConcatByteArrays(baHeaderLength, baHeaderContent);

            byte[] baMessageContent = (eMessageType != EMessageType.FileContent_InBytes) ?
                                CSession.TextMessageContentEncoding.GetBytes(this.sStringContent) :
                                this.baBytesContent;

            baMessage = CUtils.ConcatByteArrays(baMessage, baMessageContent);

            baEnc = CUtils.EncryptBytesAndInsertIV_AndMsgType(CSession.baPasswordKey, baMessage, eMessageType);

            baEnc = CUtils.GetMsgWithHMacBytes(this, baEnc);//Msg encrypted with HMAC
            if (!withSign)
                return baEnc;

            return  CRSA.AddSignatureToMsg(baEnc); 
        }
    } 
}
