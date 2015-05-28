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
{
     class SendFileSession
    {

        public static void SendFileTcpSession(object sender, DoWorkEventArgs doe)
        {
            BackgroundWorker me = (BackgroundWorker)sender;
            object[] param = (object[])doe.Argument;
            TcpClient client = new TcpClient();
            User selectedUser = (User) param[1];
            User oCurrentUser = (User)param[0];
            string filePath = (string) param[2];
            SCAFTForm scaftForm = (SCAFTForm)param[3];
            client.Connect(selectedUser.oIP, 5000); //TODO, change the port.
            byte[] msg  =
                new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.SENDFILE, filePath).GetEncMessage();
            NetworkStream ns = client.GetStream();
            Message oCurrentMsg;
            ns.Write(msg, 0, msg.Length);
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
                        msg = messageStream.ToArray();
                        oCurrentMsg = new Message(msg);
                        switch(oCurrentMsg.eMessageType)
                        {
                            case EMessageType.OK:
                                {

                                    User oUser = scaftForm.GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                                   // if (oUser != null && oUser.sIWantToSendThisFileNameToThisUser == oCurrentMsg.sStringContent) //only if the user is a friend send the file.
                                 //   {
                                        
                                        byte[] fileArray = File.ReadAllBytes(filePath);
                                        //TODO CHECK ABOUT CONFLICTS WITH SENDING TO MORE THEN ONE USER BEFORE ACCEPTING
                                        // SendFileToUser(, oUser);
                                        FileStream fsIn = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                        byte[] buf = new byte[1024];
                                        int read = 0;
                                        int tatalRead = 0;
                                       
                                        while ((read = fsIn.Read(buf, 0, buf.Length)) > 0 && !me.CancellationPending)
                                        {
                                            ns.Write(buf, 0, read);
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
             }
                 catch(Exception e)
             {

             }

          
            
            
        }

    }
}
