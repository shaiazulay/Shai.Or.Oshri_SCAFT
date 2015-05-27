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
            string clientInfo =
              (connectionSocket.Client.RemoteEndPoint as IPEndPoint).
                  Address.ToString()
              + ":" +
              (connectionSocket.Client.RemoteEndPoint as IPEndPoint).Port;
            try
            {
                while (!me.CancellationPending)
                {
                    byte[] packet = new byte[64];
                    //netStream.Read(packet, 0, (int)connectionSocket.ReceiveBufferSize);
                    ns.Read(packet, 0, 64);
                    Message oCurrentMsg = new Message(packet);
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
                                   //TODO recive file here!
                               }
                               else
                               {
                                   byte[] noMessage = new Message(scaftForm.oCurrentUser.oIP,
                                       scaftForm.oCurrentUser.sUserName, EMessageType.NO, "").GetEncMessage();
                                   ns.Write(noMessage, 0, noMessage.Length);
                               }
                               break;
                           }
                       case EMessageType.OK:
                           {

                               User oUser = scaftForm.GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                               if (oUser != null && oUser.sIWantToSendThisFileNameToThisUser == oCurrentMsg.sStringContent) //only if the user is a friend send the file.
                               {

                                   //TODO CHECK ABOUT CONFLICTS WITH SENDING TO MORE THEN ONE USER BEFORE ACCEPTING
                                   //SendFileToUser(File.ReadAllBytes(txtFilePath.Text), oUser);
                               }
                               break;
                           }
                       case EMessageType.NO:
                           MessageBox.Show("The user did not accept the file transfer", "", MessageBoxButtons.OK);
                           break;
                         
                   }

                  //  object[] package = { DateTime.Now.ToLongTimeString(),   
                  //                (connectionSocket.Client.RemoteEndPoint as IPEndPoint).Address.ToString(),
                 //                   packet };
                   // me.ReportProgress(0, package); 
                    //todo hundle client requests here!

                }
            }
            catch(Exception e)
            {
                
            }


        }
    }
}
