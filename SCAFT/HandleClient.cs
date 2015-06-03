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
using Microsoft.Win32;

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
            Message oCurrentMsg=null;
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
                            FileStream output = new FileStream(Path.GetFileName(oCurrentMsg.sStringContent), FileMode.OpenOrCreate, FileAccess.Write);
                            int defaultPacketSize = 1024;
                            TcpListener tcpServer = new TcpListener(scaftForm.oCurrentUser.oIP, randomePort);
                            tcpServer.Start();
                            connectionSocket = new TcpClient();
                            connectionSocket = tcpServer.AcceptTcpClient();
                            if (connectionSocket != null)
                            {

                                ns = connectionSocket.GetStream();
                                int totalRead = 0;
                                // read data while there is what to read
                                byte[] buffer = new byte[defaultPacketSize];
                                int read = 0;
                                int reportCount = 0;
                                while ((read = ns.Read(buffer, 0, defaultPacketSize)) > 0)
                                {
                                    totalRead += read;
                                    reportCount++;
                                    output.Write(buffer, 0, read);
                                }

                            }
                            MessageBox.Show("the file: " + Path.GetFileName(oCurrentMsg.sStringContent) +
                                            "was transferd from: "
                                            + oCurrentMsg.oUser.sUserName + " seccsesfuly");
                            
                            
                            tcpServer.Stop();
                            ns.Close();
                            output.Close();


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



            }
            catch (Exception e)
            {
                MessageBox.Show("the file: " + Path.GetFileName(oCurrentMsg.sStringContent) +
                                           "from: "
                                           + oCurrentMsg.oUser.sUserName + "has faild to arrive: " + e.Message);
            }


        }

      
    }
}
