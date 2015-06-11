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
        private const char MESSAGE_DELIMITER_BETWEEN_FIELDS = ' ';
        public User oUser { get; set; }

        public EMessageType eMessageType { get; set; }

        public string sStringContent { get; set; }

        public byte[] baBytesContent { get; set; }

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
            
            byte[] baPlainBytes = CUtils.DecryptBytes(baEnc, CSession.baPasswordKey, out eMsgTypeTemp);
            eMessageType = eMsgTypeTemp;

            if (eMessageType == EMessageType.Bye || eMessageType == EMessageType.NO || 
                    eMessageType == EMessageType.Hellow || eMessageType == EMessageType.OK ||
                eMessageType == EMessageType.SENDFILE || eMessageType == EMessageType.Text)
            {
                string sPlainText = CSession.TextMessageContentEncoding.GetString(baPlainBytes);

                string[] saBits = sPlainText.Split(MESSAGE_DELIMITER_BETWEEN_FIELDS);

                IPAddress oIp = null;
                IPAddress.TryParse(saBits[1], out oIp);

                oUser = new User(oIp, saBits[0]);

                sStringContent = saBits[2];
                for (int i = 3; i < saBits.Length; i++)//if there are delimeter in content then concate them to be one contenct
                    sStringContent += MESSAGE_DELIMITER_BETWEEN_FIELDS + saBits[i];
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
            List<byte[]> lbaMessages = SpliteMultiMessagesIntoByteArrays(baEncrypted, CUtils.TCP_END_SINGLE_SIGN, CUtils.TCP_END_SIGN_NUM_OF_SIGNS);

            for (int i = 0; i < lbaMessages.Count; i++)
            {
                olMessages.Add(new Message(lbaMessages[i]));
            }

            return olMessages.ToArray();
        }
        //splits an array of bytes that is possible multi messages to arrays of bytes that each is one message encrypted. 
        public static List<byte[]> SpliteMultiMessagesIntoByteArrays(byte[] baEncrypted, byte bDelimiterSign, int iDelimiterNumOfTimes)
        {
            List<byte[]> lbaRes = new List<byte[]>();//holds result 
            int iNumOfSign = 0;
            int iFirstArrayIndex = 0;

            for(int i = 0; i < baEncrypted.Length; i++)//on all the message
            {
                if (baEncrypted[i] == bDelimiterSign && i < baEncrypted.Length - 1)//if delimiter sign, and not last byte in message
                {
                    iNumOfSign++;//count another sign spotted
                }
                else //if current byte is not a delimiter Or its last byte in Msg
                {
                    //if num of signs shows that its a new sequence that split and insert to Result List
                    if (iDelimiterNumOfTimes == iNumOfSign || (i == baEncrypted.Length - 1 && baEncrypted[i] == bDelimiterSign))
                    {
                        byte[] baTemp = new byte[i - iFirstArrayIndex];//length of current sequence
                        Array.Copy(baEncrypted, iFirstArrayIndex, baTemp, 0, baTemp.Length);
                        lbaRes.Add(baTemp);
                        iFirstArrayIndex = i;//set new sequence start index
                    }

                    iNumOfSign = 0; //start new count of delimiter signs
                }
            }

            return lbaRes;
        }

        public byte[] GetEncMessage()
        {
            if (eMessageType != EMessageType.FileContent_InBytes)
            {
                string sPlainMsg = "";
                sPlainMsg += oUser.sUserName + MESSAGE_DELIMITER_BETWEEN_FIELDS;
                sPlainMsg += oUser.oIP.ToString() + MESSAGE_DELIMITER_BETWEEN_FIELDS;
                sPlainMsg += sStringContent;
                 
                return CUtils.EncryptBytesAndInsertIV_AndMsgType(CSession.baPasswordKey, Encoding.UTF8.GetBytes(sPlainMsg), eMessageType);           
            }
            else
            {
                return CUtils.EncryptBytesAndInsertIV_AndMsgType(CSession.baPasswordKey, baBytesContent, eMessageType);
            }
        }
 
        public byte[] GetEncMessageWithHMAC()
        {
            byte[] baMsg = CUtils.InsertEndMsgDelimiter(CUtils.HASH_DELIMITER_SIGN, //get encripted msg with a end msg sign
                                            CUtils.HASH_DELIMITER_NUM_OF_TIMES,
                                            this.GetEncMessage());

            //insert Salt and Hash to Message Object
            CUtils.InsertHMAC_ToMessage(this);

            //add to Salt end 
            baMsg = CUtils.ConcatByteArrays(baMsg, this.baHashSalt);

            //add HMAC to end
            return CUtils.ConcatByteArrays(baMsg, this.baHash);
        }

        public static Message GetMessageFromHMAC_AndEncryptedMsg(byte[] baMsg)
        {
            Message oMessage = new Message();


            return oMessage;
        }
    } 
}
