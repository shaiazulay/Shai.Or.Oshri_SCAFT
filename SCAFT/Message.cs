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
        Unknown

    }

    public class Message
    {
        public User oUser { get; set; }

        public EMessageType eMessageType { get; set; }

        public string sStringContent { get; set; }

        public Message(IPAddress oUserIP, string sUserName, EMessageType eMsgType, string sContent)
        {
            oUser = new User(oUserIP, sUserName);
            eMessageType = eMsgType;
            sStringContent = sContent;
        }

        public Message(byte[] baEnc)
        {
            string sPlainText = CUtils.Decrypt(baEnc, CSession.baPasswordKey, CSession.lCurrentIV);

            string[] saBits = sPlainText.Split(' ');

            eMessageType = CUtils.GetMessageType(saBits[0]);

            IPAddress oIp = null;
            IPAddress.TryParse(saBits[2], out oIp);

            oUser = new User(oIp, saBits[1]);

            sStringContent = saBits[3];
        }

        public byte[] GetEncMessage()
        {
            string sPlainMsg = CUtils.GetEMessageTypeDesc(eMessageType) + " ";
            sPlainMsg += oUser.sUserName + " ";
            sPlainMsg += oUser.oIP.ToString() + " ";
            sPlainMsg += sStringContent;

            return CUtils.Encrypt(CSession.baPasswordKey, CSession.lCurrentIV, sPlainMsg);
        }

    } 
}
