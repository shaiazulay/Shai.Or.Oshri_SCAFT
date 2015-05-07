using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace SCAFT
{
    public partial class SCAFTForm : Form
    {
        Timer oHellowTimer;

        private string sExpectedFileName;

        public SCAFTForm()
        {
            InitializeComponent();

            CSession.olForms.Add(this);

            oHellowTimer = new Timer();
            oHellowTimer.Interval = 1000;
            oHellowTimer.Tick += new EventHandler(OnHellowTickEvent);
            oHellowTimer.Start();

            lblUserName.Text = CSession.sUserName;

            listBoxConnectedUsers.Items.Add("user1");
            listBoxConnectedUsers.Items.Add("user2");
        }

        private static void SendHellow(string sUserName)
        {
            
        }

        private void btnExitSCAFT_Click(object sender, EventArgs e)
        {
            CSession.OrderedExit();
        }


        private static void OnHellowTickEvent(Object source, EventArgs e)
        {
            SCAFTForm.SendHellow(CSession.sUserName);
        }

        private void ProcessMessage()
        {
            string sUserThatSentHellow = GetUserNameFromHellowMessage();
            if (sUserThatSentHellow.Trim() != "")
            {
                bool IsUserListed = false;
                foreach(object oItem in listBoxConnectedUsers.Items)
                {
                    if (oItem.ToString() == sUserThatSentHellow)
                    {
                        IsUserListed = true;
                    }
                }
                if (!IsUserListed)
                {
                    listBoxConnectedUsers.Items.Add(sUserThatSentHellow);
                }
            }
        }
         
        private string GetUserNameFromHellowMessage()
        {
            return "";
        }

        private void SendUDPMessage(string sMsg, IPAddress sIP)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                    ProtocolType.Udp);
             
            IPEndPoint endPoint = new IPEndPoint(sIP, 11000);


            //string text = "Hello";
           // byte[] send_buffer = CUtils.Encrypt(CSession.baPasswordKey, CSession.)

           // sock.SendTo(send_buffer, endPoint);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SendUDPMessage(txtMessageToSend.Text, IPAddress.Parse("212.150.112.56"));
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                txtFilePath.Text = openFileDialog1.FileName;
            }

        }

        private void SendFileToSelectedUser(byte[] baFileBytes)
        {
            
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
            {
                SendFileToSelectedUser(File.ReadAllBytes(txtFilePath.Text));
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
            SendUDPMessage(sMsg, sUserName.oUserIP);
        }
    }

    public class CUserItem
    {
        public string sUserName {get;set;}
        public IPAddress oUserIP {get;set;}
        
        public string ToString()
        {
            return sUserName;
        }
       
        public CUserItem(string _sUserName)
        {
            sUserName = _sUserName;
        }
    
        public CUserItem(string _sUserName, IPAddress _oUserIP)
        {
            sUserName = _sUserName;
            oUserIP = _oUserIP;
        }
    }
}

 
    
