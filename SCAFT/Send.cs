using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCAFTI
{
    public static class Send
    {
       
        public static void SendUDPMessage(UdpClient udp,IPEndPoint multicastEP, string sMsg)
        {
            // Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            //         ProtocolType.Udp);

            // IPEndPoint endPoint = new IPEndPoint(sIP, PORT);
            udp.Send(CSession.TextMessageContentEncoding.GetBytes(sMsg),
                CSession.TextMessageContentEncoding.GetByteCount(sMsg), multicastEP);

            //string text = "Hello";
            // byte[] send_buffer = CUtils.Encrypt(CSession.baPasswordKey, CSession.)

            // sock.SendTo(send_buffer, endPoint);
        }

        public static void SendUDPMessage(UdpClient udp, IPEndPoint multicastEP, byte[] baMsg)
        {
            // Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            //         ProtocolType.Udp);

            // IPEndPoint endPoint = new IPEndPoint(sIP, PORT);
            udp.Send(baMsg,
                baMsg.Length, multicastEP);

            //string text = "Hello";
            // byte[] send_buffer = CUtils.Encrypt(CSession.baPasswordKey, CSession.)

            // sock.SendTo(send_buffer, endPoint);
        }
    }
    
}
