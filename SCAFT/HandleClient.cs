﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
//needs to fix recieving files 
//needs to open file on location.
namespace SCAFT
{
    internal class HandleClient
    {

        internal static void TcpSession(object sender, DoWorkEventArgs doe)
        {
            BackgroundWorker me = (BackgroundWorker) sender;
            object[] param = (object[]) doe.Argument;

            TcpClient connectionSocket = (TcpClient) param[0];
            SCAFTForm scaftForm = (SCAFTForm) param[1];
            NetworkStream ns = connectionSocket.GetStream();
            Message oCurrentMsg=null;
            try
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
                        } while (ns.DataAvailable);
                    }

                    /* msg is the final byte array from the stream */ 
                    oCurrentMsg = Message.GetMessageFromTcpEncrypted(messageStream.ToArray());
                }
                switch (oCurrentMsg.eMessageType)
                {
                    case EMessageType.SENDFILE:
                    {
                        bool accept = scaftForm.ProcessSendFileMessage(oCurrentMsg);
                        if (accept)
                        {
                            Random rand = new Random();
                            int randomePort = rand.Next()%3000 + 1000;
                            byte[] okMessage = new Message(scaftForm.oCurrentUser.oIP,
                                scaftForm.oCurrentUser.sUserName,
                                EMessageType.OK, randomePort.ToString()).GetEncMessage();
                            ns.Write(okMessage, 0, okMessage.Length);
                            FileStream output = new FileStream(Path.GetFileName(oCurrentMsg.sStringContent), FileMode.OpenOrCreate, FileAccess.Write);
                            int defaultPacketSize = 1056;
                            TcpListener tcpServer = new TcpListener(scaftForm.oCurrentUser.oIP, randomePort);
                            tcpServer.Start();
                            connectionSocket = new TcpClient();
                            connectionSocket = tcpServer.AcceptTcpClient();
                            Message fileChunkMsg = null;
                            if (connectionSocket != null)
                            {
                                //TODO WRITE THIS AGAIN WITH SIZE FROM SENDER. 
                                bool IsLast = false;
                                ns = connectionSocket.GetStream();
                                int totalRead = 0;
                                // read data while there is what to read
                                byte[] buffer = new byte[defaultPacketSize];
                                int read = 0;
                                int reportCount = 0;

                                byte[] baTemp = new byte[0];
                                using (MemoryStream messageStream = new MemoryStream())
                                {
                                    byte[] inbuffer = new byte[65535];
                                    if (ns.CanRead)
                                    {
                                        int iOffset = 0;
                                        do
                                        {
                                            int bytesRead = ns.Read(inbuffer, 0, inbuffer.Length);
                                            if (bytesRead == 0) break;
                                            inbuffer = CUtils.ByteArrayRemoveTrailing0(inbuffer);
                                            if (inbuffer.Length < 1024) IsLast = true;
                                            inbuffer = Message.GetMessageFromTcpEncrypted(inbuffer).baBytesContent;
                                            output.Write(inbuffer, 0, inbuffer.Length);
                                            iOffset += inbuffer.Length;
                                            inbuffer = new byte[65535];
                                        } while (ns.DataAvailable);
                                    }

                                    /* msg is the final byte array from the stream */

                                    //if (baTemp.Length > (CUtils.TCP_END_SIGN_NUM_OF_SIGNS + CUtils.iKeyIvSizeInBytes))
                                    //{
                                    //    fileChunkMsg = Message.GetMessageFromTcpEncrypted(baTemp);
                                    //}

                                    output.Flush();
                                    
                                    if (IsLast)
                                    {
                                        tcpServer.Stop();
                                        ns.Close();
                                        output.Close();
                                    }
                                }
                            }

                                //Message recivedMsg = fileChunkMsg;//

                                //using (MemoryStream messageStream = new MemoryStream())
                                //{
                                //    //byte[] inbuffer = new byte[100000];
                                //    //byte[] baTemp = new byte[0];
                                //    //if (ns.CanRead)
                                //    //{
                                //    //    do
                                //    //    {
                                //    //        int bytesRead = ns.Read(inbuffer, 0, inbuffer.Length);
                                //    //        baTemp = new byte[bytesRead];
                                //    //        messageStream.Write(inbuffer, 0, bytesRead);
                                //    //        Array.Copy(inbuffer, baTemp, baTemp.Length);
                                //    //    } while (ns.DataAvailable);
                                //    //}

                                //    ///* msg is the final byte array from the stream */
                                //    //recivedMsg = Message.GetMessageFromTcpEncrypted(baTemp);  
                                //    if (recivedMsg.eMessageType != EMessageType.FileContent_InBytes)
                                //    {
                                //        HandleError(ns, fileChunkMsg, new Exception("wrong type of msg"));
                                //        return;
                                //    }
                                //    totalRead += read;
                                //    reportCount++;
                                //    output.Write(recivedMsg.baBytesContent, 0, recivedMsg.baBytesContent.Length);
                                   
                            //    }

                            //}
                            //DialogResult drslt = MessageBox.Show("the file: " + Path.GetFileName(oCurrentMsg.sStringContent) +
                            //                "was transferd from: "
                            //                + oCurrentMsg.oUser.sUserName + " seccsesfuly, Would you like to open it? ", "New File Recived", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            //tcpServer.Stop();
                            //ns.Close();
                            //output.Close();
                            //if (drslt == DialogResult.Yes) System.Diagnostics.Process.Start(Path.GetFileName(oCurrentMsg.sStringContent));


                        }
                        else
                        {
                            byte[] noMessage = new Message(scaftForm.oCurrentUser.oIP,
                                scaftForm.oCurrentUser.sUserName, EMessageType.NO, "").GetEncMessage();
                            ns.Write(noMessage, 0, noMessage.Length);
                            ns.Close();
                        }
                        break;
                    }
                } 
            }
            catch (Exception e)
            {
                HandleError(ns, oCurrentMsg, e);

            }


        }

        private static void HandleError(NetworkStream ns, Message oCurrentMsg, Exception e)
        {
            try
            {
                MessageBox.Show("the file: " + Path.GetFileName(oCurrentMsg.sStringContent) +
                                           "from: "
                                           + oCurrentMsg.oUser.sUserName + "has faild to arrive: " + e.Message);
            }
            catch
            {

            }
            ns.Close();
        }

        
    }
}
