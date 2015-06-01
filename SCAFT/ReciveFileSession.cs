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
    class ReciveFileSession
    {
        internal static void ReciveFileTcpSession(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker me = (BackgroundWorker)sender;
           
            object[] param = (object[])e.Argument;
            //NetworkStream ns = (NetworkStream)param[0];
            int randomePort = (int)param[0];
            string fileName = (string)param[1];
            User oCorrentUser = (User)param[2];
            //FileStream output = File.Create(fileName);
            FileStream output = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            NetworkStream ns = null;
            try
            {
                int defaultPacketSize = 1024;
                TcpListener tcpServer = new TcpListener(oCorrentUser.oIP, randomePort);
                tcpServer.Start();
                TcpClient connectionSocket = null;
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
               
                ns.Close();
                output.Close();
                
            }

            catch (IOException iox)
            {  
                //      me.ReportProgress(NetworkTimingGui.TCP_PRINT_LOG, "Error reading from " + clientInfo + " on TCP: " + iox.Message);
            }
        }
    }
}
