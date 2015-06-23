using System;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Net;

namespace SCAFTI

{
    static class ListeningBroadcast
    {
        public static void ListenForMessages(object sender, DoWorkEventArgs e)
        {
            UdpClient client = (UdpClient)e.Argument;

            BackgroundWorker me = (BackgroundWorker)sender;

            // read data
            try
            {
                while (!me.CancellationPending)
                {
                    // read data
                    IPEndPoint messageCameFrom = new IPEndPoint(IPAddress.Any, 0);
                    byte[] packet = client.Receive(ref messageCameFrom);
                    // convert to string
                   // string fullMessage = UnicodeEncoding.UTF8.GetString(packet);
                    // log it up to the screen
                    object[] param = { DateTime.Now.ToLongTimeString(), messageCameFrom.Address.ToString(), packet};
                    me.ReportProgress(0,param); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in communication: " + ex.Message);
            }

            return;
        }
 
    }

}
