using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;


namespace SCAFT
{
    public partial class SCAFTForm : Form//
    {
        Timer oHellowTimer;
        private const int iHellowMsgMiliSecInterval = 10000;
        internal User oCurrentUser;
        private static UdpClient udp;
        private BackgroundWorker bwUDP;
        private BackgroundWorker bwTCP;
        private static TcpListener tcpListener;
        private List<BackgroundWorker> list;
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
            try
            {
                InitializeComponent();

                oHellowTimer = new Timer();
                oHellowTimer.Interval = iHellowMsgMiliSecInterval;
                oHellowTimer.Tick += new EventHandler(OnHellowTickEvent);

                MoveToChatOrConfigurationTabs(false);


                CSession.olForms.Add(this);
            }
            catch { }
        }



        private void SendHellow()
        {
            try
            {
                Send.SendUDPMessage(udp, multicastEP, (new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.Hellow, "")).GetEncMessage());
            }
            catch { }
        }

        private void btnExitSCAFT_Click(object sender, EventArgs e)
        {
            try
            {
                CloseScaft();
            }
            catch { }
        }

        private void CloseScaft()
        {
            try
            {
                LogOut();
                CSession.OrderedExit();
            }
            catch { }
        }

        private void LogOut()
        {
            try
            {
                Send.SendUDPMessage(udp, multicastEP, (new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.Bye, "")).GetEncMessage());
                oHellowTimer.Stop();
                if (bwTCP.IsBusy)
                {
                    bwTCP.CancelAsync();
                    tcpListener.Stop();
                }
                if (bwUDP.IsBusy)
                {
                    bwUDP.CancelAsync();
                    udp.Close();
                }
            }
            catch { }
         
        }
        private void OnHellowTickEvent(Object source, EventArgs e)
        {
            try
            {
                if (udp != null && multicastEP != null) //if udp is running.
                    SendHellow();
            }
            catch { }
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                Send.SendUDPMessage(udp, multicastEP,
                    (new Message(oCurrentUser.oIP, oCurrentUser.sUserName, EMessageType.Text,
                        txtMessageToSend.Text).GetEncMessage()));
            }
            catch
            { }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    txtFilePath.Text = openFileDialog1.FileName;
                }
            }
            catch { }
        }

        internal void SendFileToUser(byte[] baFileBytes, User oUserToSendTo)
        {
            //TODO SETUP  connection and send the file with tcp
            MessageBox.Show("send the file now!!!");
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            try { 
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
                 
            BackgroundWorker sendFileTcpWorker = new BackgroundWorker();
            sendFileTcpWorker.DoWork += SendFileSession.SendFileTcpSession;
            User selectedUser = (User)listBoxConnectedUsers.SelectedItem;
            object[] param = { oCurrentUser, selectedUser, txtFilePath.Text, this };
            sendFileTcpWorker.RunWorkerAsync(param);

            }
            catch 
            {
                MessageBox.Show("The File Wasn`t Sent because of an Error");
                return;
            }
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!LogIn())
                    return;
                else
                {
                    MoveToChatOrConfigurationTabs(true);
                }

                try
                {
                    // close the old one if there is one
                    if (udp != null)
                    {
                        udp.Close();
                    }
                    // create a new one on the listed port

                    udp = new UdpClient(CSession.iPort, AddressFamily.InterNetwork);
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

                // start to listen for incoming messages udp too
                bwUDP = new BackgroundWorker();
                bwUDP.WorkerReportsProgress = true;
                bwUDP.WorkerSupportsCancellation = true;
                bwUDP.ProgressChanged += ReceivedUDPMessage;
                bwUDP.DoWork += ListeningBroadcast.ListenForMessages;
                bwUDP.RunWorkerAsync(udp);

                tcpListener = new TcpListener(oCurrentUser.oIP, CSession.iPort);
                tcpListener.Start();
                ListenTCP();
            }
            catch { }
        }

        private void ListenTCP()
        {
            try
            {
                bwTCP = new BackgroundWorker();
                bwTCP.WorkerReportsProgress = true;
                bwTCP.WorkerSupportsCancellation = true;
                bwTCP.ProgressChanged += ReceivedUDPMessage;
                bwTCP.DoWork += ListeningUnicast.ListenForPrivateSession;
                object[] param = { tcpListener, this };
                bwTCP.RunWorkerAsync(param);
            }
            catch { }
        }
        private void ReceivedUDPMessage(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                // show the new message in the textbox
                //tbLog.Text = e.UserState.ToString() + Environment.NewLine + tbLog.Text;
                object[] param = (object[])e.UserState;
                string time = param[0].ToString();
                IPAddress sourceIp = IPAddress.Parse(param[1].ToString());

                byte[] bafullMessage = (byte[])param[2];
                Message oCurrentMsg = new Message(bafullMessage);
                if (oCurrentMsg.eMessageType != EMessageType.Hellow)
                {
                    tbLog.Text = oCurrentMsg.oUser.sUserName + " " + time + " -> \"" + oCurrentMsg.sStringContent + "\"" + Environment.NewLine + tbLog.Text;
                }
                //TODO print only msg from peaple in the group, add group password, add AES AND CBC for all the commands, etc.

               
                switch (oCurrentMsg.eMessageType)
                {
                    case EMessageType.Hellow:
                        {
                            User oUser = GetConnectedUserByName(oCurrentMsg.oUser.sUserName);

                            if (oUser == null)// && !oCurrentMsg.oUser.Equals(oCurrentUser))
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
                    default:
                        break;
                }

            }
            catch { }
        }
        internal User GetConnectedUserByName(string _sUserName)
        {
            try
            {
                foreach (User oUser in listBoxConnectedUsers.Items)
                {
                    if (oUser.sUserName == _sUserName)
                        return oUser;
                }
                return null;
            }
            catch { return null; }
        }

        public bool ProcessSendFileMessage(Message oMsg)
        {
            try
            {
                User oUser = GetConnectedUserByName(oMsg.oUser.sUserName);
                if (oUser != null)
                {
                    string sFileName = oMsg.sStringContent;
                    try
                    {
                        sFileName = Path.GetFileName(oMsg.sStringContent);
                    }
                    catch { }
                    var result =
                        MessageBox.Show(
                            "Hello " + CSession.sUserName + " do you want to accept the file \"" + sFileName +
                            "\" from user name: \"" + oMsg.oUser.sUserName + "\"", "New File Is Waiting for approval",
                            MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    { 
                        return true;
                    }


                }
                return false;
            }
            catch { return false; }
        }
        
        private void SCAFTForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                LogOut();
            }
            catch { }
        }
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                LogOut();
                MoveToChatOrConfigurationTabs(false);
            }
            catch { }
        }
        public static void EnableTab(TabPage page, bool enable)
        {
            try
            {
                foreach (Control ctl in page.Controls) ctl.Enabled = enable;
            }
            catch { }
        }
        private bool LogIn()
        {
            try
            {
                int iPort = -1;
                if (txtUserName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("No User Name Inserted!!");
                    return false;
                }

                if (txtUserName.Text.Contains('@') || txtUserName.Text.Contains(' '))
                {
                    MessageBox.Show("user name may not contain whitespace characters or the ‘@’ character!!");
                    return false;
                }


                if (int.TryParse(txtPort.Text, out iPort))
                {
                    if (iPort > IPEndPoint.MaxPort || iPort <= 0)
                    {
                        MessageBox.Show("the port Needs to be between 1 to " + IPEndPoint.MaxPort);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("insert a whole number for the port!!");
                    return false;
                }

                IPAddress oMulticastIP;
                if (!IPAddress.TryParse(txtMulticastIP.Text, out oMulticastIP))
                {
                    MessageBox.Show("insert valid ip for Multicast!!");
                    return false;
                }
                else
                {
                    string[] saBits = txtMulticastIP.Text.Split('.');
                    int firstIpNum = int.Parse(saBits[0]);
                    if (firstIpNum >= 240 || firstIpNum < 224)
                    {
                        MessageBox.Show("Multicast Range is 224.0.0.0 to 239.255.255.255!!");
                        return false;
                    }
                }

                byte[] baKey = Encoding.UTF8.GetBytes(txtKey.Text.Trim());

                if (baKey.Length < 16)
                {
                    MessageBox.Show("the Key is less then 16 byte (after converted from UTF8 Encoding to bytes)!!");
                    return false;
                }

                byte[] baMacKey = Encoding.UTF8.GetBytes(txtMacKey.Text.Trim());

                if (baMacKey.Length < 64)
                {
                    MessageBox.Show("the MAC Key is less then 64 byte (after converted from UTF8 Encoding to bytes)!!");
                    return false;
                }

                CSession.sUserName = txtUserName.Text.Trim();
                CSession.iPort = iPort;
                CSession.oMulticastIP = oMulticastIP;
                CSession.baPasswordKey = CUtils.Trimming(baKey);
                CSession.baPassworMacdKey = CUtils.Trimming(baMacKey, 64);

                oCurrentUser = new User(CUtils.GetMyLocalIPAddress(), CSession.sUserName);
                oHellowTimer.Start();

                lblUserName.Text = CSession.sUserName;

                return true;
            }
            catch (Exception e){ return false; }
        }

        private void MoveToChatOrConfigurationTabs(bool IsChat)
        {
            try
            {
                if (IsChat)
                {
                    EnableTab(tabChate, true);
                    EnableTab(tabConfiguration, false);
                    tabControl.SelectedTab = tabChate;
                }
                else
                {
                    EnableTab(tabChate, false);
                    EnableTab(tabConfiguration, true);
                    tabControl.SelectedTab = tabConfiguration;
                }
            }
            catch { }
        }
    }
}    
