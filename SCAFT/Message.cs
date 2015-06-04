using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCAFT
{
    public enum EMessageType
    {
        Hellow,
        Bye,
        SENDFILE,
        OK,
        NO,
        Text,
        FileTransfer,
        Unknown,
        FileContent_InBytes

    }

    public class Message
    {
        private const char DELIMITER = '{';
        public User oUser { get; set; }

        public EMessageType eMessageType { get; set; }

        public string sStringContent { get; set; }

        public byte[] baBytesContent { get; set; }

        public Message(IPAddress oUserIP, string sUserName, EMessageType eMsgType, string sContent)
        {
            oUser = new User(oUserIP, sUserName);
            eMessageType = eMsgType;
            sStringContent = sContent;
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
            
            byte[] baPlainBytes = CUtils.DecryptBytes(baEnc, CSession.baPasswordKey, out eMsgTypeTemp);
            eMessageType = eMsgTypeTemp;

            if (eMessageType == EMessageType.Bye || eMessageType == EMessageType.NO || 
                    eMessageType == EMessageType.Hellow || eMessageType == EMessageType.OK ||
                eMessageType == EMessageType.SENDFILE || eMessageType == EMessageType.Text)
            {
                string sPlainText = CSession.TextMessageContentEncoding.GetString(baPlainBytes);

                string[] saBits = sPlainText.Split(DELIMITER);

                IPAddress oIp = null;
                IPAddress.TryParse(saBits[1], out oIp);

                oUser = new User(oIp, saBits[0]);

                sStringContent = saBits[2];
                for (int i = 3; i < saBits.Length; i++)
                    sStringContent += DELIMITER + saBits[i];
            } 
            else
            {
                baBytesContent = baPlainBytes; 
            }
        }

        public static Message GetMessageFromTcpEncrypted(byte[] baEncrypted)
        {
            byte[] baMsg = CUtils.GetMessageWithoutTcpEndSignal(baEncrypted);
            return new Message(baMsg);
        }

        public static Message GetMsgFromTcpEncrypted(byte[] baEncrypted)
        {
            Message oResMsg = null;
            return oResMsg;
        }

        public byte[] GetEncMessage()
        {
            if (eMessageType != EMessageType.FileContent_InBytes)
            {
                string sPlainMsg = "";
                sPlainMsg += oUser.sUserName + DELIMITER;
                sPlainMsg += oUser.oIP.ToString() + DELIMITER;
                sPlainMsg += sStringContent;
                 
                return CUtils.EncryptBytesAndInsertIV_AndMsgType(CSession.baPasswordKey, Encoding.UTF8.GetBytes(sPlainMsg), eMessageType);           
            }
            else
            {
                return CUtils.EncryptBytesAndInsertIV_AndMsgType(CSession.baPasswordKey, baBytesContent, eMessageType);
            }
        }

        



    } 
}
