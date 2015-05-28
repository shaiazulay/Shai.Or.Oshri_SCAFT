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
        internal static void SendFileTcpSession(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker me = (BackgroundWorker)sender;
            int defaultPacketSize = 1024;
            object[] param = (object[])e.Argument;
            NetworkStream ns = (NetworkStream)param[0];
            string fileName = (string)param[1];
            //FileStream output = File.Create(fileName);
            FileStream output = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                int totalRead = 0;
                // read data while there is what to read
                byte[] buffer = new byte[defaultPacketSize];
                int read = 0;
                int reportCount = 0;
                while ((read = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    totalRead += read;
                    reportCount++;
                    output.Write(buffer, 0, read);
                }
                output.Close();
                ns.Close();
            }

            catch (IOException iox)
            {
                //      me.ReportProgress(NetworkTimingGui.TCP_PRINT_LOG, "Error reading from " + clientInfo + " on TCP: " + iox.Message);
            }
        }
    }
}
