using System;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;

namespace SCAFT

{
    static class ListeningUnicast
    {
        private static List<TcpClient> clientList;
        public static void ListenForPrivateSession(object sender, DoWorkEventArgs e)
        {
            clientList = new List<TcpClient>();
            TcpClient client = (TcpClient)e.Argument;

            BackgroundWorker me = (BackgroundWorker)sender;

            // read data
            try
            {
                while (!me.CancellationPending)
                {
                    TcpClient connectionSocket = null;
                    try
                    {
                        if (connectionSocket != null)
                        {
                            me.ReportProgress(0,
                           "Received connection from " +
                          (connectionSocket.Client.RemoteEndPoint as
                          IPEndPoint).Address.ToString()
                          + ":" +
                          (connectionSocket.Client.RemoteEndPoint as
                           IPEndPoint).Port);
                            BackgroundWorker bw = new BackgroundWorker();
                            clientList.Add(connectionSocket);
                            bw.DoWork += HandleClient.TcpSession;
                            bw.WorkerSupportsCancellation = true;

                            bw.WorkerReportsProgress = true;
                           // bw.ProgressChanged += bw_ProgressChanged;
                            bw.RunWorkerAsync(connectionSocket);

                        }
                    }
                    catch
                    {

                    }
                    // read data
                   // IPEndPoint messageCameFrom = new IPEndPoint(IPAddress.Any, 0);
                   // byte[] packet = client.Receive(ref messageCameFrom);
                    // convert to string
                   // string fullMessage = UnicodeEncoding.UTF8.GetString(packet);
                    // log it up to the screen
                  //  object[] param = { DateTime.Now.ToLongTimeString(), messageCameFrom.Address.ToString(), packet};
                    me.ReportProgress(0,""); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in communication: " + ex.Message);
            }

            return;
        }
        public static void cloaseAll()
        {
            foreach (TcpClient c in clientList)
            {
                c.Close();
            }
        }
 
    }

}
