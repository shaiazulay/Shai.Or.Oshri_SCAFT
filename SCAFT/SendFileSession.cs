using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFT
{//needs to tell user if file is sent or is not sent
     class SendFileSession
    {

        public static void SendFileTcpSession(object sender, DoWorkEventArgs doe)
        {
            BackgroundWorker me = (BackgroundWorker)sender;
            object[] param = (object[])doe.Argument;
            TcpClient client = new TcpClient();
            User selectedUser = (User)param[1];
            User oCurrentUser = (User)param[0];
            string filePath = (string) param[2];
            SCAFTForm scaftForm = (SCAFTForm)param[3];
            client.Connect(selectedUser.oIP, CSession.iPort); //TODO, change the port.
            byte[] msg  =
                new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.SENDFILE, filePath).GetEncMessage();
            NetworkStream ns = client.GetStream();
            Message oCurrentMsg= null;
            ns.Write(msg, 0, msg.Length);
            try
            {
                using (MemoryStream messageStream = new MemoryStream())
                {
                    byte[] inbuffer = new byte[65535];

                    if (ns.CanRead)
                    {
                        do
                        {
                            int bytesRead = ns.Read(inbuffer, 0, inbuffer.Length);
                            messageStream.Write(inbuffer, 0, bytesRead);
                        }
                        while (ns.DataAvailable);
                    }

                    /* msg is the final byte array from the stream */ 
                    oCurrentMsg = Message.GetMessageFromTcpEncrypted(messageStream.ToArray());
                    switch (oCurrentMsg.eMessageType)
                    {
                        case EMessageType.OK:
                            {

                                User oUser = scaftForm.GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                                // if (oUser != null && oUser.sIWantToSendThisFileNameToThisUser == oCurrentMsg.sStringContent) //only if the user is a friend send the file.
                                //   {
                                client = new TcpClient();
                                int defaultPacketSize = 1024;
                                int recivedRandomePort = int.Parse(oCurrentMsg.sStringContent);
                                client.Connect(selectedUser.oIP, recivedRandomePort);
                                ns = client.GetStream();
                         
                                FileStream fsIn = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                byte[] buf = new byte[defaultPacketSize];
                                int read = 0;
                                int tatalRead = 0;

                                while ((read = fsIn.Read(buf, 0, defaultPacketSize)) > 0 && !me.CancellationPending)
                                {
                                    Message sendBufEncMessage;
                                    if (buf.Length > read)
                                    {
                                        byte[] baTamp = new byte[read];
                                        Array.Copy(buf, baTamp, baTamp.Length);

                                        sendBufEncMessage = new Message(oCurrentUser.oIP,
                                        selectedUser.sUserName, baTamp);
                                    }
                                    else
                                    {
                                        sendBufEncMessage = new Message(oCurrentUser.oIP,
                                        selectedUser.sUserName, buf);
                                    }
                                    
                                    byte[] encMsgBytes = sendBufEncMessage.GetEncMessage();
                                    ns.Write(encMsgBytes, 0, encMsgBytes.Length);
                                    tatalRead += read;
                                    ns.Flush();
                                }
                                ns.Close();
                                fsIn.Close();




                                //  }
                                break;
                            }
                        case EMessageType.NO:
                            MessageBox.Show("The user did not accept the file transfer", "", MessageBoxButtons.OK);
                            break;
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("the file: " + Path.GetFileName(oCurrentMsg.sStringContent) +
                                          "was not sended to: "
                                          + oCurrentMsg.oUser.sUserName + "becouse of an error: " + e.Message);

            }

          
            
            
        }

    }
}
