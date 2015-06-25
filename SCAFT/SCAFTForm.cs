using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;


namespace SCAFTI
{
    public partial class SCAFTIForm : Form//
    {  
        Timer oHellowTimer;
        private const int iHellowMsgMiliSecInterval = 1000; 
        private static UdpClient udp;
        private BackgroundWorker bwUDP;
        private BackgroundWorker bwTCP;
        private static TcpListener tcpListener; 
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

        public SCAFTIForm()
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
                Send.SendUDPMessage(udp, multicastEP, (new Message(CUtils.oCurrentUser.oIP, CUtils.oCurrentUser.sUserName, EMessageType.Hellow, "")).GetEncMessage(true));
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
                Send.SendUDPMessage(udp, multicastEP, (new Message(CUtils.oCurrentUser.oIP, CUtils.oCurrentUser.sUserName, EMessageType.Bye, "")).GetEncMessage(true));
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
                    (new Message(CUtils.oCurrentUser.oIP, CUtils.oCurrentUser.sUserName, EMessageType.Text,
                        txtMessageToSend.Text).GetEncMessage(true)));
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
            object[] param = { CUtils.oCurrentUser, selectedUser, txtFilePath.Text, this };
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

                tcpListener = new TcpListener(CUtils.oCurrentUser.oIP, CSession.iPort);
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

                Message oCurrentMsg = CUtils.CheckMacWriteToLog_AndReturnMessages(bafullMessage, CSession.iPort, true);

                if (oCurrentMsg != null)
                {
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

                                if (oUser == null )//&& !oCurrentMsg.oUser.Equals(CUtils.oCurrentUser))
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

            }
            catch (Exception e2) { }
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
                if(CRSA.rsa == null)
                {
                    MessageBox.Show("No RSA Key was generated!!");
                    return false;
                }
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

                CUtils.oCurrentUser = new User(CUtils.GetMyLocalIPAddress(), CSession.sUserName);
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

        private void btnGenerateWithPrivateKey_Click(object sender, EventArgs e)
        {
            try
            {
                CRSA.GenerateWithPrivateKey(int.Parse(txtRSAKeySize.Text));

                ShowRSAParams(true);
                MessageBox.Show("Your Rsa public/private keys are Ready!");
            }
            catch
            {
                MessageBox.Show("invalid rsa key size!!");
            }
        }

        private void ShowRSAParams(bool IsWithPrivate)
        {
            // show the parameters in the window
            RSAParameters par = CRSA.rsa.ExportParameters(IsWithPrivate);

            this.txtP.Text = CUtils.ByteArrayToHexString(par.P);
            this.txtQ.Text = CUtils.ByteArrayToHexString(par.Q);
            this.txtModulus.Text = CUtils.ByteArrayToHexString(par.Modulus);
            this.txtExponent.Text = CUtils.ByteArrayToHexString(par.Exponent);
            this.txtD.Text = CUtils.ByteArrayToHexString(par.D);
            this.txtDmodP1.Text = CUtils.ByteArrayToHexString(par.DP);
            this.txtDmodQ1.Text = CUtils.ByteArrayToHexString(par.DQ);
        }
  
        private void btnExportWithPrivate_Click(object sender, EventArgs e)
        {
            ExportRsaToFile(true);
        }

        private void btnExportWithoutPrivateKey_Click(object sender, EventArgs e)
        {
            ExportRsaToFile(false);
        }

        private void ExportRsaToFile(bool IsWithPrivateKey)
        {
            FileStream fsOut = null;
            StreamWriter swOut = null;
            try
            {
                fdExport.FileName = "";

                if (fdExport.ShowDialog() ==
                    System.Windows.Forms.DialogResult.OK)
                {
                    fsOut = new FileStream(fdExport.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                    swOut = new StreamWriter(fsOut);

                    //covert public private key to KML

                    swOut.Write(CRSA.rsa.ToXmlString(IsWithPrivateKey));
                    swOut.Close();
                    fsOut.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("exporting fail  check that a key was generated");
            }
            finally
            {
                if (swOut != null) swOut.Close();
                if (fsOut != null) fsOut.Close();
            }
        }

        private void btnImportWithPrivateKey_Click(object sender, EventArgs e)
        {
            FileStream fsIn = null;
            StreamReader srIn = null;
            try
            {
                fdImport.FileName = "";

                if (fdImport.ShowDialog() ==
                    System.Windows.Forms.DialogResult.OK)
                {
                    fsIn = new FileStream(fdImport.FileName, FileMode.Open, FileAccess.Read);
                    srIn = new StreamReader(fsIn);
                    string key = srIn.ReadToEnd();

                    CRSA.rsa = new RSACryptoServiceProvider();

                    CRSA.rsa.FromXmlString(key);

                    ShowRSAParams(true);
                    MessageBox.Show("your public/private keys are ready (import went well)");
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show("import failed check if file is valid and has private key");
            }
            finally
            {
                if (srIn != null) srIn.Close();
                if (fsIn != null) fsIn.Close();
            }
        }

        private void btnLoadOtherUserPublicKey_Click(object sender, EventArgs e)
        {
            if(txtOtherUserName.Text.Trim().Length == 0)
            {
                MessageBox.Show("No User Name was entered for this public key");
                return;
            }

            if(txtOtherUserName.Text.Contains(' ') || txtOtherUserName.Text.Contains('@'))
            {
                MessageBox.Show("User Name Can`t contain '@' Or whilte spaces");
                return;
            }

            FileStream fsIn = null;
            StreamReader srIn = null;
            try
            {
                fdImport.FileName = "";

                if (fdLoadUserPublicKey.ShowDialog() ==
                    System.Windows.Forms.DialogResult.OK)
                {
                    fsIn = new FileStream(fdLoadUserPublicKey.FileName, FileMode.Open, FileAccess.Read);
                    srIn = new StreamReader(fsIn);
                    string key = srIn.ReadToEnd();

                    RSACryptoServiceProvider oTempRsa = new RSACryptoServiceProvider();

                    oTempRsa.FromXmlString(key);

                    oTempRsa.ExportParameters(false);//check that is valid

                    CRSA.AddOtherUserKeyToPublicKeys(txtOtherUserName.Text, oTempRsa.ToXmlString(false));

                    MessageBox.Show("User \"" + txtOtherUserName.Text + "\" public key was added");
                    txtOtherUserName.Text = "";
                }
            }
            catch (Exception e3)
            {
                MessageBox.Show("user public key file not loaded. check that the file is valid");
            }
            finally
            {
                if (srIn != null) srIn.Close();
                if (fsIn != null) fsIn.Close();
            }
        } 
    }
}    
