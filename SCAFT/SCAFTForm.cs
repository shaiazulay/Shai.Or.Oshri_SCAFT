using System;
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
        private static UdpClient udp;
        private BackgroundWorker bwListener;
        private string sExpectedFileName;
        private Dictionary<IPAddress, string> connectedUsers; 
        // TODO ADD PORT FROM GUI
        private const int PORT = 5000;
        private const String IP = "224.1.1.1";

        private static IPEndPoint multicastEP
        {
            get
            {
                // get the address
                IPAddress address = IPAddress.Parse(IP);
                // put it in a new end point
                return new IPEndPoint(address, PORT);
            }
        }

        public SCAFTForm()
        {
            InitializeComponent();
            connectedUsers = new Dictionary<IPAddress, string>();
            CSession.olForms.Add(this);

            

           
            oHellowTimer = new Timer();
            oHellowTimer.Interval = 1000;
            oHellowTimer.Tick += new EventHandler(OnHellowTickEvent);
            oHellowTimer.Start(); 

            lblUserName.Text = CSession.sUserName;

            //listBoxConnectedUsers.Items.Add("user1");
           // listBoxConnectedUsers.Items.Add("user2");
                  

        }

        private static void SendHellow(string sUserName)
        {
            Send.SendUDPMessage(udp, multicastEP, "HELLO " + sUserName, IPAddress.Parse(IP));
        }

        private void btnExitSCAFT_Click(object sender, EventArgs e)
        {
            Send.SendUDPMessage(udp, multicastEP, "BYE", IPAddress.Parse(IP));
            CSession.OrderedExit();
        }
        private static void OnHellowTickEvent(Object source, EventArgs e)
        {
            if (udp != null && multicastEP != null) //if udp is running.
                SCAFTForm.SendHellow(CSession.sUserName);
        }

        private void ProcessHelloMessage(string sUserNameThatSayedHELLO, IPAddress sourceIp)
        {
            if (sUserNameThatSayedHELLO.Trim() != "" && sUserNameThatSayedHELLO != CSession.sUserName)
            {
                //bool IsUserListed = false;
                //foreach(object oItem in listBoxConnectedUsers.Items)
                //{
                //    if (oItem.ToString() == sUserThatSentHellow)
                //    {
                //        IsUserListed = true;
                //    }
                //}
                //if (!IsUserListed)
                //{
                if (!connectedUsers.ContainsKey(sourceIp))
                {
                    connectedUsers.Add(sourceIp, sUserNameThatSayedHELLO);
                    listBoxConnectedUsers.Items.Add(sUserNameThatSayedHELLO);
                }


            }
            
        }

       // private void SendUDPMessage(string sMsg, IPAddress sIP)
       // {
           // Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
           //         ProtocolType.Udp);
             
           // IPEndPoint endPoint = new IPEndPoint(sIP, PORT);
      ////      udp.Send(UnicodeEncoding.UTF8.GetBytes(sMsg),
       //         UnicodeEncoding.UTF8.GetByteCount(sMsg), multicastEP);

            //string text = "Hello";
           // byte[] send_buffer = CUtils.Encrypt(CSession.baPasswordKey, CSession.)

           // sock.SendTo(send_buffer, endPoint);
      //  }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            Send.SendUDPMessage(udp, multicastEP, txtMessageToSend.Text, IPAddress.Parse(IP));


        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                txtFilePath.Text = openFileDialog1.FileName;
            }

        }

        private void SendFileToSelectedUser(byte[] baFileBytes , IPAddress destinationAddress)
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
                Send.SendUDPMessage(udp, multicastEP, "SENDFILE ", 
                    GetIPFromUserName(listBoxConnectedUsers.SelectedItem.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Error: " + ex.Message);
                return;
            }
        }

        private void SendRecieveFileOK(CUserItem sUserName, string sFileName)
        {
            sExpectedFileName = sFileName;

            string sMsg = "";
            Send.SendUDPMessage(udp, multicastEP, sMsg, sUserName.oUserIP);
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
                udp.JoinMulticastGroup(IPAddress.Parse("224.1.1.1"));
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
            
            string fullMessage = param[2].ToString();
            tbLog.Text = time + " - " + sourceIp + ": " + fullMessage + Environment.NewLine + tbLog.Text;
            //TODO print only msg from peaple in the group, add group password, add AES AND CBC for all the commands, etc.
            string cmd = CUtils.getRequestType(fullMessage);
            string msg = CUtils.getOnlyString(fullMessage);
            switch (cmd)
            {
                case "HELLO":
                    ProcessHelloMessage(msg, sourceIp);
                  
                    return;
                case "BYE":
                    listBoxConnectedUsers.Items.Remove(GetUserNameFromIp(sourceIp)); //first clear list box
                    connectedUsers.Remove(sourceIp); //remove the object from the list dicionary
                   
                    return;
                case "SENDFILE":
                    if (connectedUsers.ContainsKey(sourceIp)) //only if the user is a friend pop the quastion
                    {
                        ProcessSendFileMessage(msg, new CUserItem(GetUserNameFromIp(sourceIp), sourceIp));
                    }
                    return;
                case "OK":
                    if (connectedUsers.ContainsKey(sourceIp)) //only if the user is a friend send the file.
                    {
                        //TODO CHECK ABOUT CONFLICTS WITH SENDING TO MORE THEN ONE USER BEFORE ACCEPTING
                        SendFileToSelectedUser(File.ReadAllBytes(txtFilePath.Text),
                            GetIPFromUserName(listBoxConnectedUsers.SelectedItem.ToString()));
                    }
                    return;
                case "NO":
                    if (connectedUsers.ContainsKey(sourceIp)) //only if the user is a friend
                    {
                        MessageBox.Show("The user did not accept the file transfer", "", MessageBoxButtons.OK);
                    }
                    return;

            }
            
        }

        private bool ProcessSendFileMessage(string msg, CUserItem userItem)
        {
            var result = MessageBox.Show("Hello " + CSession.sUserName + " do you want to recive the file: " + msg +
                            " from user name: " + userItem.sUserName, "New File Is Waiting for approval",
                            MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // the user want to recive the file return ok to the spacifiec ip address
                Send.SendUDPMessage(udp, multicastEP, "OK", userItem.oUserIP);
            
            }
            else
            {
                // the user dosnt approve the file transfer and return no to the specifiec address
                Send.SendUDPMessage(udp, multicastEP, "NO", userItem.oUserIP);
            }
            return true;
        }

        private string GetUserNameFromIp(IPAddress ip)
        {
            string name = null;
            connectedUsers.TryGetValue(ip, out name);   
            return name;
        }
        private IPAddress GetIPFromUserName(string userName)
        {
            var ip = connectedUsers.FirstOrDefault(x => x.Value == userName).Key;
            if (ip != null) return (IPAddress) ip;
            else return IPAddress.None;
        }
    }

}

 
    
