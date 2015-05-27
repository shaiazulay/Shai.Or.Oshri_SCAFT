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
            StreamReader srIn = new StreamReader(connectionSocket.GetStream());
            StreamWriter swOut = new StreamWriter(connectionSocket.GetStream());
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
                    byte[] packet = new byte[connectionSocket.ReceiveBufferSize];
                    //netStream.Read(packet, 0, (int)connectionSocket.ReceiveBufferSize);
                    ns.Read(packet, 0, (int) connectionSocket.ReceiveBufferSize);
                    Message oCurrentMsg = new Message(packet);
                    string s = oCurrentMsg.ToString();
                    switch (oCurrentMsg.eMessageType)
                   {
                       case EMessageType.SENDFILE:
                           {
                               bool accept = scaftForm.ProcessSendFileMessage(oCurrentMsg);
                               if (!accept) swOut.WriteLine(new Message(scaftForm.oCurrentUser.oIP, scaftForm.oCurrentUser.sUserName,EMessageType.NO, "").GetEncMessage());
                               else swOut.WriteLine(new Message(scaftForm.oCurrentUser.oIP, scaftForm.oCurrentUser.sUserName, EMessageType.OK, "").GetEncMessage());
                               //TODO recive file here! 
                               break;
                           }
                       //case EMessageType.OK:
                       //    {
                            
                       //        User oUser = GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                       //        if (oUser != null && oUser.sIWantToSendThisFileNameToThisUser == oCurrentMsg.sStringContent) //only if the user is a friend send the file.
                       //        {

                       //            //TODO CHECK ABOUT CONFLICTS WITH SENDING TO MORE THEN ONE USER BEFORE ACCEPTING
                       //            SendFileToUser(File.ReadAllBytes(txtFilePath.Text), oUser);
                       //        }
                       //        break;
                       //    }
                       //case EMessageType.NO:
                       //    MessageBox.Show("The user did not accept the file transfer", "", MessageBoxButtons.OK);
                       //    break;
                         
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
