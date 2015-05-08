﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCAFT
{
    public static class Send
    {
       
        public static void SendUDPMessage(UdpClient udp,IPEndPoint multicastEP, string sMsg, IPAddress sIP)
        {
            // Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            //         ProtocolType.Udp);

            // IPEndPoint endPoint = new IPEndPoint(sIP, PORT);
            udp.Send(UnicodeEncoding.UTF8.GetBytes(sMsg),
                UnicodeEncoding.UTF8.GetByteCount(sMsg), multicastEP);

            //string text = "Hello";
            // byte[] send_buffer = CUtils.Encrypt(CSession.baPasswordKey, CSession.)

            // sock.SendTo(send_buffer, endPoint);
        }
    }
    
}
