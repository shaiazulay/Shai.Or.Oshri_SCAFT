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
            string sPlainText = CUtils.Decrypt(baEnc, CSession.baPasswordKey);

            string[] saBits = sPlainText.Split(' ');

            eMessageType = CUtils.GetMessageType(saBits[0]);
            if (eMessageType != EMessageType.Unknown)
            {
                IPAddress oIp = null;
                IPAddress.TryParse(saBits[2], out oIp);

                oUser = new User(oIp, saBits[1]);

                sStringContent = saBits[3];
                for (int i = 4; i < saBits.Length; i++)
                    sStringContent += " " + saBits[i];
            }
            else
            {
                baBytesContent = CUtils.DecryptBytesWithIV(baEnc, CSession.baPasswordKey);
                eMessageType = EMessageType.FileContent_InBytes;
            }
        }

        public byte[] GetEncMessage()
        {
            if (eMessageType != EMessageType.FileContent_InBytes)
            {
                string sPlainMsg = CUtils.GetEMessageTypeDesc(eMessageType) + " ";
                sPlainMsg += oUser.sUserName + " ";
                sPlainMsg += oUser.oIP.ToString() + " ";
                sPlainMsg += sStringContent;

                byte[] baEnc = CUtils.Encrypt(CSession.baPasswordKey, sPlainMsg);
                return CUtils.ConcatByteArrats(CSession.baCurrentTxtMsgIV, baEnc);
            }
            else
            {
                return CUtils.EncryptBytesAndInsertIV(CSession.baPasswordKey, baBytesContent);
            }
        }



    } 
}
