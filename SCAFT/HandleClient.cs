using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFT
{
    class HandleClient
    {

        internal static void TcpSession(object sender, DoWorkEventArgs doe)
        {
            BackgroundWorker me = (BackgroundWorker)sender;
            object[] param = (object[]) doe.Argument;

            TcpClient connectionSocket = (TcpClient)param[0];
            SCAFTForm scaftForm = (SCAFTForm) param[1];
            NetworkStream ns = connectionSocket.GetStream();
            Message oCurrentMsg;
            try
            {
                while (!me.CancellationPending)
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
                        byte[] msg = messageStream.ToArray();
                        oCurrentMsg = new Message(msg);
                    }

                    //byte[] packet = new byte[64];
                    //netStream.Read(packet, 0, (int)connectionSocket.ReceiveBufferSize);
                    //ns.Read(packet, 0, 64);

                    switch (oCurrentMsg.eMessageType)
                    {
                        case EMessageType.SENDFILE:
                            {
                                bool accept = scaftForm.ProcessSendFileMessage(oCurrentMsg);
                                if (accept)
                                {
                                    byte[] okMessage = new Message(scaftForm.oCurrentUser.oIP,
                                                                          scaftForm.oCurrentUser.sUserName,
                                                                          EMessageType.OK, "").GetEncMessage();
                                    ns.Write(okMessage, 0, okMessage.Length);
                                    //after sending ok, wait for the file in onther thread.
                                    BackgroundWorker reciveFileTcpWorker = new BackgroundWorker();
                                    reciveFileTcpWorker.DoWork += ReciveFileSession.SendFileTcpSession;
                                    object[] par = {ns, Path.GetFileName(oCurrentMsg.sStringContent)};
                                    reciveFileTcpWorker.RunWorkerAsync(par);
                                  
                                }
                                else
                                {
                                    byte[] noMessage = new Message(scaftForm.oCurrentUser.oIP,
                                        scaftForm.oCurrentUser.sUserName, EMessageType.NO, "").GetEncMessage();
                                    ns.Write(noMessage, 0, noMessage.Length);
                                }
                                break;
                            }
                    }

                    //  object[] package = { DateTime.Now.ToLongTimeString(),   
                    //                (connectionSocket.Client.RemoteEndPoint as IPEndPoint).Address.ToString(),
                    //                   packet };
                    // me.ReportProgress(0, package); 
                    //todo hundle client requests here!

                }
            }
            catch (Exception e)
            {

            }


        }
    }
}
