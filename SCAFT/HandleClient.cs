using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCAFT
{
    class HandleClient
    {

        internal static void TcpSession(object sender, DoWorkEventArgs doe)
        {
            BackgroundWorker me = (BackgroundWorker)sender;
            TcpClient connectionSocket = (TcpClient)doe.Argument;
            StreamReader srIn = new StreamReader(connectionSocket.GetStream());
            StreamWriter swOut = new StreamWriter(connectionSocket.GetStream());
            string clientInfo =
              (connectionSocket.Client.RemoteEndPoint as IPEndPoint).
                  Address.ToString()
              + ":" +
              (connectionSocket.Client.RemoteEndPoint as IPEndPoint).Port;
            try
            {
                while (me.CancellationPending)
                {
                    string message = srIn.ReadLine();
                    //todo hundle client requests here!

                }
            }
            catch
            {
                
            }


        }
    }
}
