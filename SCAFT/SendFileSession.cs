using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            string fileName = (string) param[2];
            client.Connect(selectedUser.oIP, 5000); //TODO, change the port.
            byte[] msg  =
                new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.SENDFILE, fileName).GetEncMessage();
            NetworkStream ns = client.GetStream();
            ns.Write(msg, 0, msg.Length);
           
            
            
        }

    }
}
