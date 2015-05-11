﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;


namespace SCAFT
{
    public partial class SCAFTForm : Form
    {
        Timer oHellowTimer;
        private User oCurrentUser;
        private static UdpClient udp;
        private BackgroundWorker bwListener; 
          
        private static IPEndPoint multicastEP
        {
            get
            {
                // get the address
                IPAddress address = CSession.oMulticastIP;
                // put it in a new end point
                return new IPEndPoint(address, CSession.iPort);
            }
        }

        public SCAFTForm()
        {
            InitializeComponent();

            oCurrentUser = new User(CUtils.GetMyLocalIPAddress(), CSession.sUserName); 
            CSession.olForms.Add(this);

            oHellowTimer = new Timer();
            oHellowTimer.Interval = 1000;
            oHellowTimer.Tick += new EventHandler(OnHellowTickEvent);
            oHellowTimer.Start();

            lblUserName.Text = CSession.sUserName; 
        }



        private void SendHellow()
        {
            Send.SendUDPMessage(udp, multicastEP, (new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.Hellow, "")).GetEncMessage());
        }

        private void btnExitSCAFT_Click(object sender, EventArgs e)
        {
            Send.SendUDPMessage(udp, multicastEP, (new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.Bye, "")).GetEncMessage());
            CSession.OrderedExit();
        }

        private void OnHellowTickEvent(Object source, EventArgs e)
        {
            if (udp != null && multicastEP != null) //if udp is running.
                SendHellow();
        }
         
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            Send.SendUDPMessage(udp, multicastEP, 
                (new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.Text, 
                    txtMessageToSend.Text).GetEncMessage()));
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                txtFilePath.Text = openFileDialog1.FileName;
            } 
        }

        private void SendFileToUser(byte[] baFileBytes, User oUserToSendTo)
        {
            //TODO SETUP  connection and send the file with tcp
            MessageBox.Show("send the file now!!!");
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("File Path Not Valid!!! You need to type a File Path, Or select a file using Browse button");
                return;
            }

            if (listBoxConnectedUsers.SelectedIndex < 0)
            {
                MessageBox.Show("You Need To Select a User to Send a File to!!!");
                return;
            }

            try
            { // send file to a user by using his ip address
                Send.SendUDPMessage(udp, multicastEP,
                    (new Message(CUtils.GetMyLocalIPAddress(), oCurrentUser.sUserName, 
                        EMessageType.SENDFILE, Path.GetFileName(txtFilePath.Text)).GetEncMessage()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Error: " + ex.Message);
                return;
            }
        }
         
        private void btnJoin_Click(object sender, EventArgs e)
        {

            try
            {
                // close the old one if there is one
                if (udp != null)
                {
                    udp.Close();
                }
                // create a new one on the listed port

                udp = new UdpClient(5000, AddressFamily.InterNetwork);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening UDP client : " + ex.Message);
            }

            try
            {
                udp.JoinMulticastGroup(CSession.oMulticastIP);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error joining multicast group " + ex.Message);
                return;
            }

            // start to listen for incoming messages too
            bwListener = new BackgroundWorker();
            bwListener.WorkerReportsProgress = true;
            bwListener.WorkerSupportsCancellation = true;
            bwListener.ProgressChanged += new ProgressChangedEventHandler(ReceivedMessage);
            bwListener.DoWork += Listening.ListenForMessages;
            bwListener.RunWorkerAsync(udp);
        }

        private void ReceivedMessage(object sender, ProgressChangedEventArgs e)
        {
            // show the new message in the textbox
            //tbLog.Text = e.UserState.ToString() + Environment.NewLine + tbLog.Text;
            object[] param = (object[])e.UserState;
            string time = param[0].ToString();
            IPAddress sourceIp = IPAddress.Parse(param[1].ToString());

            byte[] bafullMessage = (byte[])param[2];
            Message oCurrentMsg = new Message(bafullMessage);
            tbLog.Text = time + " - " + sourceIp + ": " + oCurrentMsg.sStringContent + Environment.NewLine + tbLog.Text;
            //TODO print only msg from peaple in the group, add group password, add AES AND CBC for all the commands, etc.
             
         //   string msg = CUtils.getOnlyString(oCurrentMsg.sStringContent);
            switch (oCurrentMsg.eMessageType)
            {
                case EMessageType.Hellow:
                    {
                        User oUser = GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                        if (oUser == null && oCurrentMsg.oUser.sUserName != oCurrentUser.sUserName)
                            listBoxConnectedUsers.Items.Add(oCurrentMsg.oUser);

                        if (oUser != null)
                            oUser.oIP = oCurrentMsg.oUser.oIP;

                        break;
                    }
                case EMessageType.Bye:
                    {
                        User oUser = GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                        if (oUser != null)
                            listBoxConnectedUsers.Items.Remove(oUser);
                        break;
                    }
                case EMessageType.SENDFILE:
                    {
                        ProcessSendFileMessage(oCurrentMsg);
                        break;
                    }
                case EMessageType.OK:
                    {
                        User oUser = GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                        if (oUser != null && oUser.sIWantToSendThisFileNameToThisUser == oCurrentMsg.sStringContent) //only if the user is a friend send the file.
                        {

                            //TODO CHECK ABOUT CONFLICTS WITH SENDING TO MORE THEN ONE USER BEFORE ACCEPTING
                            SendFileToUser(File.ReadAllBytes(txtFilePath.Text), oUser);
                        }
                        break;
                    }
                case EMessageType.NO:
                    MessageBox.Show("The user did not accept the file transfer", "", MessageBoxButtons.OK);
                    break;
                default:
                    break;
            }

        }

        private User GetConnectedUserByName(string _sUserName)
        {
            foreach (User oUser in listBoxConnectedUsers.Items)
            {
                if (oUser.sUserName == _sUserName)
                    return oUser;
            }
            return null;
        }

        private void ProcessSendFileMessage(Message oMsg)
        {
            User oUser = GetConnectedUserByName(oMsg.oUser.sUserName);
            if (oUser != null)
            {
                var result = MessageBox.Show("Hello " + CSession.sUserName + " do you want to recive the file: " + oMsg.sStringContent +
                                " from user name: " + oMsg.oUser.sUserName, "New File Is Waiting for approval",
                                MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    oUser.sIApprovedThisFileNameToSendMe = oMsg.sStringContent;
                    // the user want to recive the file return ok to the spacifiec ip address
                    Send.SendUDPMessage(udp, multicastEP,
                        (new Message(CUtils.GetMyLocalIPAddress(), oCurrentUser.sUserName,
                            EMessageType.OK, oMsg.sStringContent).GetEncMessage())); 
                }
                else
                {
                    // the user dosnt approve the file transfer and return no to the specifiec address
                    Send.SendUDPMessage(udp, multicastEP,
                        (new Message(CUtils.GetMyLocalIPAddress(), oCurrentUser.sUserName,
                            EMessageType.NO, oMsg.sStringContent).GetEncMessage()));
                } 
            }
        } 
    }


    
}    
