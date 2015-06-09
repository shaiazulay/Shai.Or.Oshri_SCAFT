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
        private static Message oCurrentMsg;
        private static FileStream output;
        private static TcpListener tcpServer;
        internal static void TcpSession(object sender, DoWorkEventArgs doe)
        {
            const int defaultPacketSize = 1024;
            BackgroundWorker me = (BackgroundWorker) sender;
            object[] param = (object[]) doe.Argument;

            TcpClient connectionSocket = (TcpClient) param[0];
            SCAFTForm scaftForm = (SCAFTForm) param[1];
            NetworkStream ns = connectionSocket.GetStream();
         
            try
            {
                using (MemoryStream messageStream = new MemoryStream())
                {
                    byte[] inbuffer = new byte[65535];//the size of buffer doesn`t have to be this

                    if (ns.CanRead)
                    {
                        do
                        {
                            int bytesRead = ns.Read(inbuffer, 0, inbuffer.Length);
                            messageStream.Write(inbuffer, 0, bytesRead);
                        } while (ns.DataAvailable);
                    }

                    /* msg is the final byte array from the stream */ 
                    oCurrentMsg = Message.GetMessageFromTcpEncrypted(messageStream.ToArray());
                }

               
                switch (oCurrentMsg.eMessageType)
                {
                    case EMessageType.SENDFILE:
                    {
                        bool accept = scaftForm.ProcessSendFileMessage(oCurrentMsg);
                        if (accept)
                        {
                            Random rand = new Random();
                            int randomePort = rand.Next()%3000 + 1000;//random port 1000-4000
                            byte[] okMessage = new Message(scaftForm.oCurrentUser.oIP,
                                scaftForm.oCurrentUser.sUserName,
                                EMessageType.OK, randomePort.ToString()).GetEncMessage();
                            ns.Write(okMessage, 0, okMessage.Length);
                            output = new FileStream(Path.GetFileName(oCurrentMsg.sStringContent), FileMode.OpenOrCreate, FileAccess.Write);
                            
                            tcpServer = new TcpListener(scaftForm.oCurrentUser.oIP, randomePort);
                            tcpServer.Start();
                            connectionSocket = new TcpClient();
                            connectionSocket = tcpServer.AcceptTcpClient();

                            if (connectionSocket != null && !me.CancellationPending)
                            { 
                                ns = connectionSocket.GetStream(); 
                                // read data while there is what to read
                                byte[] buffer = new byte[defaultPacketSize]; 

                                byte[] baTemp = new byte[0];
                                using (MemoryStream messageStream = new MemoryStream())
                                {
                                    byte[] inbuffer = new byte[defaultPacketSize];
                                    if (ns.CanRead)
                                    {
                                        int bytesRead = 0;
                                        do
                                        {
                                            bytesRead = ns.Read(inbuffer, 0, inbuffer.Length);
                                            messageStream.Write(inbuffer, 0, bytesRead);
                                            inbuffer = new byte[65535];
                                        } while (bytesRead > 0 && !me.CancellationPending);
                                    }

                                    Message[] oaMessage = Message.GetMsgFromTcpEncrypted(messageStream.ToArray());

                                     foreach(Message oMsg in oaMessage)
                                     {
                                         output.Write(oMsg.baBytesContent, 0, oMsg.baBytesContent.Length);
                                     }


                                }
                                output.Flush();
                                output.Close();
                                tcpServer.Stop();
                                ns.Close();
                                DialogResult drslt = MessageBox.Show("the file \"" + Path.GetFileName(oCurrentMsg.sStringContent) +
                                            "\" was transferd from \""
                                            + oCurrentMsg.oUser.sUserName + "\" seccsesfuly, Would you like to open it? ", "New File Recived", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                
                                 
                                if (drslt == DialogResult.Yes) System.Diagnostics.Process.Start(Path.GetFileName(oCurrentMsg.sStringContent));
                            } 
                        }
                        else
                        {
                            byte[] noMessage = new Message(scaftForm.oCurrentUser.oIP,
                                scaftForm.oCurrentUser.sUserName, EMessageType.NO, "").GetEncMessage();
                            ns.Write(noMessage, 0, noMessage.Length);
                            ns.Close();
                        }
                        break;
                    }
                } 
            }
            catch (Exception e)
            {
                HandleError(ns, oCurrentMsg, e);

            } 
        }

        private static void HandleError(NetworkStream ns, Message oCurrentMsg, Exception e)
        {

            if (output != null) output.Close();
            if (tcpServer != null) tcpServer.Stop();
            if (ns != null) ns.Close();
            if (oCurrentMsg != null)
            {
                MessageBox.Show("the file: " + Path.GetFileName(oCurrentMsg.sStringContent) +
                                           "from: "
                                           + oCurrentMsg.oUser.sUserName + "has faild to arrive: " + e.Message);
            }
            else MessageBox.Show("Error: " + e.Message);

        }

        
    }
}
