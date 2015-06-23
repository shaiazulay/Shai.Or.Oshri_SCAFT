using System;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;

namespace SCAFTI

{
    static class ListeningUnicast
    {
        private static List<TcpClient> clientList;
        private static BackgroundWorker me;
        public static void ListenForPrivateSession(object sender, DoWorkEventArgs e)
        {
            object[] param = (object[])e.Argument;
            clientList = new List<TcpClient>();
            TcpListener listener = (TcpListener) param[0];
            SCAFTIForm scaftForm = (SCAFTIForm) param[1];
             me = (BackgroundWorker) sender;

            // read data
            try
            {
                while (!me.CancellationPending)
                {
                    TcpClient connectionSocket = null;
                    connectionSocket = listener.AcceptTcpClient();
                    if (connectionSocket != null)
                    {
            
                        BackgroundWorker bw = new BackgroundWorker();
                        clientList.Add(connectionSocket);
                        bw.DoWork += HandleClient.TcpSession;
                        bw.WorkerSupportsCancellation = true;

                        bw.WorkerReportsProgress = true;
                        bw.ProgressChanged += bw_ProgressChanged;
                        param[0] = connectionSocket;
                        param[1] = scaftForm;
                  
                        bw.RunWorkerAsync(param);

                    }
                  
                }
                cloaseAll();
                // read data
                // IPEndPoint messageCameFrom = new IPEndPoint(IPAddress.Any, 0);
                // byte[] packet = client.Receive(ref messageCameFrom);
                // convert to string
                // string fullMessage = UnicodeEncoding.UTF8.GetString(packet);
                // log it up to the screen
                //  object[] param = { DateTime.Now.ToLongTimeString(), messageCameFrom.Address.ToString(), packet};
             
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error in communication: " + ex.Message);
                cloaseAll();
            }
       

        }

        private static void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            me.ReportProgress(0, e.UserState);
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
