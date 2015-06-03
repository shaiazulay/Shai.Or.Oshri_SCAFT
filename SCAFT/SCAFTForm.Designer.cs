namespace SCAFT
{
    partial class SCAFTForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBoxConnectedUsers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnExitSCAFT = new System.Windows.Forms.Button();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.txtMessageToSend = new System.Windows.Forms.TextBox();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabChate = new System.Windows.Forms.TabPage();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.tabConfiguration = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtMulticastIP = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabChate.SuspendLayout();
            this.tabConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxConnectedUsers
            // 
            this.listBoxConnectedUsers.FormattingEnabled = true;
            this.listBoxConnectedUsers.Location = new System.Drawing.Point(6, 57);
            this.listBoxConnectedUsers.Name = "listBoxConnectedUsers";
            this.listBoxConnectedUsers.Size = new System.Drawing.Size(125, 264);
            this.listBoxConnectedUsers.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("David", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "connected users";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User name:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(82, 9);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(35, 13);
            this.lblUserName.TabIndex = 3;
            this.lblUserName.Text = "label3";
            // 
            // btnExitSCAFT
            // 
            this.btnExitSCAFT.Location = new System.Drawing.Point(631, 5);
            this.btnExitSCAFT.Name = "btnExitSCAFT";
            this.btnExitSCAFT.Size = new System.Drawing.Size(84, 20);
            this.btnExitSCAFT.TabIndex = 4;
            this.btnExitSCAFT.Text = "Exit SCAFT";
            this.btnExitSCAFT.UseVisualStyleBackColor = true;
            this.btnExitSCAFT.Click += new System.EventHandler(this.btnExitSCAFT_Click);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(133, 62);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(74, 20);
            this.btnSendMessage.TabIndex = 5;
            this.btnSendMessage.Text = "Send";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // txtMessageToSend
            // 
            this.txtMessageToSend.Location = new System.Drawing.Point(232, 62);
            this.txtMessageToSend.Name = "txtMessageToSend";
            this.txtMessageToSend.Size = new System.Drawing.Size(337, 20);
            this.txtMessageToSend.TabIndex = 6;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(133, 88);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(74, 23);
            this.btnSendFile.TabIndex = 7;
            this.btnSendFile.Text = "Send File";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(232, 88);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(52, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(290, 88);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(279, 20);
            this.txtFilePath.TabIndex = 9;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(137, 117);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(562, 190);
            this.tbLog.TabIndex = 10;
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(611, 6);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(88, 32);
            this.btnJoin.TabIndex = 11;
            this.btnJoin.Text = "Join ";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabChate);
            this.tabControl.Controls.Add(this.tabConfiguration);
            this.tabControl.Location = new System.Drawing.Point(12, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(713, 353);
            this.tabControl.TabIndex = 12;
            // 
            // tabChate
            // 
            this.tabChate.Controls.Add(this.btnLogOut);
            this.tabChate.Controls.Add(this.tbLog);
            this.tabChate.Controls.Add(this.txtFilePath);
            this.tabChate.Controls.Add(this.txtMessageToSend);
            this.tabChate.Controls.Add(this.btnSendMessage);
            this.tabChate.Controls.Add(this.btnSendFile);
            this.tabChate.Controls.Add(this.label1);
            this.tabChate.Controls.Add(this.btnBrowse);
            this.tabChate.Controls.Add(this.listBoxConnectedUsers);
            this.tabChate.Location = new System.Drawing.Point(4, 22);
            this.tabChate.Name = "tabChate";
            this.tabChate.Padding = new System.Windows.Forms.Padding(3);
            this.tabChate.Size = new System.Drawing.Size(705, 327);
            this.tabChate.TabIndex = 0;
            this.tabChate.Text = "Chate";
            this.tabChate.UseVisualStyleBackColor = true;
            // 
            // btnLogOut
            // 
            this.btnLogOut.Location = new System.Drawing.Point(624, 6);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(75, 23);
            this.btnLogOut.TabIndex = 11;
            this.btnLogOut.Text = "LogOut";
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // tabConfiguration
            // 
            this.tabConfiguration.Controls.Add(this.txtKey);
            this.tabConfiguration.Controls.Add(this.txtMulticastIP);
            this.tabConfiguration.Controls.Add(this.txtPort);
            this.tabConfiguration.Controls.Add(this.label6);
            this.tabConfiguration.Controls.Add(this.label5);
            this.tabConfiguration.Controls.Add(this.label4);
            this.tabConfiguration.Controls.Add(this.txtUserName);
            this.tabConfiguration.Controls.Add(this.label3);
            this.tabConfiguration.Controls.Add(this.btnJoin);
            this.tabConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfiguration.Size = new System.Drawing.Size(705, 327);
            this.tabConfiguration.TabIndex = 1;
            this.tabConfiguration.Text = "Configuration";
            this.tabConfiguration.ToolTipText = "only when offline";
            this.tabConfiguration.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "User Name:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(75, 22);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(181, 20);
            this.txtUserName.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Port:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Multicast Ip:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Key:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(75, 57);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(181, 20);
            this.txtPort.TabIndex = 17;
            // 
            // txtMulticastIP
            // 
            this.txtMulticastIP.Location = new System.Drawing.Point(76, 86);
            this.txtMulticastIP.Name = "txtMulticastIP";
            this.txtMulticastIP.Size = new System.Drawing.Size(181, 20);
            this.txtMulticastIP.TabIndex = 18;
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(76, 121);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKey.Size = new System.Drawing.Size(435, 55);
            this.txtKey.TabIndex = 21;
            // 
            // SCAFTForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 418);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnExitSCAFT);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.label2);
            this.Name = "SCAFTForm";
            this.Text = "SCAFTForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SCAFTForm_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabChate.ResumeLayout(false);
            this.tabChate.PerformLayout();
            this.tabConfiguration.ResumeLayout(false);
            this.tabConfiguration.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxConnectedUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Button btnExitSCAFT;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox txtMessageToSend;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabChate;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.TabPage tabConfiguration;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMulticastIP;
        private System.Windows.Forms.TextBox txtKey;
    }
}