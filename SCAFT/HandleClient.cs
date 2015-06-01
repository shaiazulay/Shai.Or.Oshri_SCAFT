using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAFT
{
    internal class HandleClient
    {

        internal static void TcpSession(object sender, DoWorkEventArgs doe)
        {
            BackgroundWorker me = (BackgroundWorker) sender;
            object[] param = (object[]) doe.Argument;

            TcpClient connectionSocket = (TcpClient) param[0];
            SCAFTForm scaftForm = (SCAFTForm) param[1];
            NetworkStream ns = connectionSocket.GetStream();
            Message oCurrentMsg;
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
                        } while (ns.DataAvailable);
                    }

                    /* msg is the final byte array from the stream */
                    byte[] msg = messageStream.ToArray();
                    oCurrentMsg = new Message(msg);
                }
                switch (oCurrentMsg.eMessageType)
                {
                    case EMessageType.SENDFILE:
                    {
                        bool accept = scaftForm.ProcessSendFileMessage(oCurrentMsg);
                        if (accept)
                        {
                            Random rand = new Random();
                            int randomePort = rand.Next()%3000 + 1000;
                            byte[] okMessage = new Message(scaftForm.oCurrentUser.oIP,
                                scaftForm.oCurrentUser.sUserName,
                                EMessageType.OK, randomePort.ToString()).GetEncMessage();
                            ns.Write(okMessage, 0, okMessage.Length);

                            //after sending ok, wait for the file in onther thread.
                            BackgroundWorker reciveFileTcpWorker = new BackgroundWorker();
                            reciveFileTcpWorker.DoWork += ReciveFileSession.ReciveFileTcpSession;
                            object[] par = { randomePort, Path.GetFileName(oCurrentMsg.sStringContent), scaftForm.oCurrentUser };
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



            }
            catch (Exception e)
            {
                MessageBox.Show("File Transfer Error" + e.Message);
            }


        }

      
    }
}
