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
        FileContent_InBytes,
        Unknown
    }

    public class Message
    {
        private const char DELIMITER = ' ';
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

        //gets an encrypted byte[] (with end message byte sequence) that has possible few messages and return the messages
        public static Message[] GetMsgFromTcpEncrypted(byte[] baEncrypted)
        { 
            List<Message> olMessages= new List<Message>();
            List<byte[]> lbaMessages = SpliteMultiMessagesIntoByeArrays(baEncrypted);

            for (int i = 0; i < lbaMessages.Count; i++)
            {
                olMessages.Add(new Message(lbaMessages[i]));
            }

            return olMessages.ToArray();
        }
        //splits an array of bytes that is possible multi messages to arrays of bytes that each is one message encrypted. 
        private static List<byte[]> SpliteMultiMessagesIntoByeArrays(byte[] baEncrypted)
        {
            List<byte[]> lbaRes = new List<byte[]>();
            int iNumOfSign = 0;
            int iFirstArrayIndex = 0;

            for(int i = 0; i < baEncrypted.Length; i++)
            {
                if (baEncrypted[i] == CUtils.TCP_END_SINGLE_SIGN && i < baEncrypted.Length - 1)
                {
                    iNumOfSign++;
                }
                else
                {
                    if (CUtils.TCP_END_SIGN_NUM_OF_SIGNS == iNumOfSign || i == baEncrypted.Length - 1)
                    {
                        byte[] baTemp = new byte[i - iFirstArrayIndex];
                        Array.Copy(baEncrypted, iFirstArrayIndex, baTemp, 0, baTemp.Length);
                        lbaRes.Add(baTemp);
                        iFirstArrayIndex = i;
                    }

                    iNumOfSign = 0; 
                }
            }

            return lbaRes;
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
