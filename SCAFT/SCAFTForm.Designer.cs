namespace SCAFTI
{
    partial class SCAFTIForm
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
            this.btnLoadOtherUserPublicKey = new System.Windows.Forms.Button();
            this.txtOtherUserName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtRSAKeySize = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDmodQ1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDmodP1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtD = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtExponent = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtModulus = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtQ = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtP = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnGenerateWithPrivateKey = new System.Windows.Forms.Button();
            this.btnImportWithPrivateKey = new System.Windows.Forms.Button();
            this.btnExportWithoutPrivateKey = new System.Windows.Forms.Button();
            this.btnExportWithPrivate = new System.Windows.Forms.Button();
            this.txtMacKey = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtMulticastIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fdExport = new System.Windows.Forms.SaveFileDialog();
            this.fdImport = new System.Windows.Forms.OpenFileDialog();
            this.fdLoadUserPublicKey = new System.Windows.Forms.OpenFileDialog();
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
            this.listBoxConnectedUsers.Size = new System.Drawing.Size(241, 264);
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
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User name:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(70, 11);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(0, 13);
            this.lblUserName.TabIndex = 3;
            // 
            // btnExitSCAFT
            // 
            this.btnExitSCAFT.Location = new System.Drawing.Point(631, 5);
            this.btnExitSCAFT.Name = "btnExitSCAFT";
            this.btnExitSCAFT.Size = new System.Drawing.Size(84, 20);
            this.btnExitSCAFT.TabIndex = 4;
            this.btnExitSCAFT.Text = "Exit SCAFTI";
            this.btnExitSCAFT.UseVisualStyleBackColor = true;
            this.btnExitSCAFT.Click += new System.EventHandler(this.btnExitSCAFT_Click);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(253, 64);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(89, 20);
            this.btnSendMessage.TabIndex = 5;
            this.btnSendMessage.Text = "Send Message";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // txtMessageToSend
            // 
            this.txtMessageToSend.Location = new System.Drawing.Point(348, 65);
            this.txtMessageToSend.Name = "txtMessageToSend";
            this.txtMessageToSend.Size = new System.Drawing.Size(351, 20);
            this.txtMessageToSend.TabIndex = 6;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(253, 89);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(89, 23);
            this.btnSendFile.TabIndex = 7;
            this.btnSendFile.Text = "Send File";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(348, 91);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(52, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(406, 91);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(293, 20);
            this.txtFilePath.TabIndex = 9;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(253, 117);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(446, 204);
            this.tbLog.TabIndex = 10;
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(793, 3);
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
            this.tabControl.Size = new System.Drawing.Size(895, 353);
            this.tabControl.TabIndex = 12;
            // 
            // tabChate
            // 
            this.tabChate.Controls.Add(this.btnLogOut);
            this.tabChate.Controls.Add(this.tbLog);
            this.tabChate.Controls.Add(this.lblUserName);
            this.tabChate.Controls.Add(this.txtFilePath);
            this.tabChate.Controls.Add(this.label2);
            this.tabChate.Controls.Add(this.txtMessageToSend);
            this.tabChate.Controls.Add(this.btnSendMessage);
            this.tabChate.Controls.Add(this.btnSendFile);
            this.tabChate.Controls.Add(this.label1);
            this.tabChate.Controls.Add(this.btnBrowse);
            this.tabChate.Controls.Add(this.listBoxConnectedUsers);
            this.tabChate.Location = new System.Drawing.Point(4, 22);
            this.tabChate.Name = "tabChate";
            this.tabChate.Padding = new System.Windows.Forms.Padding(3);
            this.tabChate.Size = new System.Drawing.Size(887, 327);
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
            this.btnLogOut.Text = "disconnect";
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // tabConfiguration
            // 
            this.tabConfiguration.Controls.Add(this.btnLoadOtherUserPublicKey);
            this.tabConfiguration.Controls.Add(this.txtOtherUserName);
            this.tabConfiguration.Controls.Add(this.label19);
            this.tabConfiguration.Controls.Add(this.label18);
            this.tabConfiguration.Controls.Add(this.txtRSAKeySize);
            this.tabConfiguration.Controls.Add(this.label17);
            this.tabConfiguration.Controls.Add(this.label15);
            this.tabConfiguration.Controls.Add(this.txtDmodQ1);
            this.tabConfiguration.Controls.Add(this.label8);
            this.tabConfiguration.Controls.Add(this.txtDmodP1);
            this.tabConfiguration.Controls.Add(this.label9);
            this.tabConfiguration.Controls.Add(this.txtD);
            this.tabConfiguration.Controls.Add(this.label10);
            this.tabConfiguration.Controls.Add(this.txtExponent);
            this.tabConfiguration.Controls.Add(this.label11);
            this.tabConfiguration.Controls.Add(this.txtModulus);
            this.tabConfiguration.Controls.Add(this.label12);
            this.tabConfiguration.Controls.Add(this.txtQ);
            this.tabConfiguration.Controls.Add(this.label13);
            this.tabConfiguration.Controls.Add(this.txtP);
            this.tabConfiguration.Controls.Add(this.label14);
            this.tabConfiguration.Controls.Add(this.label16);
            this.tabConfiguration.Controls.Add(this.btnGenerateWithPrivateKey);
            this.tabConfiguration.Controls.Add(this.btnImportWithPrivateKey);
            this.tabConfiguration.Controls.Add(this.btnExportWithoutPrivateKey);
            this.tabConfiguration.Controls.Add(this.btnExportWithPrivate);
            this.tabConfiguration.Controls.Add(this.txtMacKey);
            this.tabConfiguration.Controls.Add(this.label7);
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
            this.tabConfiguration.Size = new System.Drawing.Size(887, 327);
            this.tabConfiguration.TabIndex = 1;
            this.tabConfiguration.Text = "Configuration";
            this.tabConfiguration.ToolTipText = "only when offline";
            this.tabConfiguration.UseVisualStyleBackColor = true;
            // 
            // btnLoadOtherUserPublicKey
            // 
            this.btnLoadOtherUserPublicKey.Location = new System.Drawing.Point(671, 177);
            this.btnLoadOtherUserPublicKey.Name = "btnLoadOtherUserPublicKey";
            this.btnLoadOtherUserPublicKey.Size = new System.Drawing.Size(198, 23);
            this.btnLoadOtherUserPublicKey.TabIndex = 50;
            this.btnLoadOtherUserPublicKey.Text = "Load other user public Key";
            this.btnLoadOtherUserPublicKey.UseVisualStyleBackColor = true;
            this.btnLoadOtherUserPublicKey.Click += new System.EventHandler(this.btnLoadOtherUserPublicKey_Click);
            // 
            // txtOtherUserName
            // 
            this.txtOtherUserName.Location = new System.Drawing.Point(733, 149);
            this.txtOtherUserName.Name = "txtOtherUserName";
            this.txtOtherUserName.Size = new System.Drawing.Size(136, 20);
            this.txtOtherUserName.TabIndex = 49;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(668, 151);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 13);
            this.label19.TabIndex = 48;
            this.label19.Text = "user name:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label18.Location = new System.Drawing.Point(667, 121);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(202, 20);
            this.label18.TabIndex = 47;
            this.label18.Text = "Load Other User public Key";
            // 
            // txtRSAKeySize
            // 
            this.txtRSAKeySize.Enabled = false;
            this.txtRSAKeySize.Location = new System.Drawing.Point(745, 67);
            this.txtRSAKeySize.Name = "txtRSAKeySize";
            this.txtRSAKeySize.Size = new System.Drawing.Size(64, 20);
            this.txtRSAKeySize.TabIndex = 46;
            this.txtRSAKeySize.Text = "4096";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label17.Location = new System.Drawing.Point(37, 10);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(218, 25);
            this.label17.TabIndex = 45;
            this.label17.Text = "Encryption And Integrity";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label15.Location = new System.Drawing.Point(465, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(200, 25);
            this.label15.TabIndex = 44;
            this.label15.Text = "Digital RSA Signature";
            // 
            // txtDmodQ1
            // 
            this.txtDmodQ1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDmodQ1.Enabled = false;
            this.txtDmodQ1.Location = new System.Drawing.Point(383, 277);
            this.txtDmodQ1.Name = "txtDmodQ1";
            this.txtDmodQ1.Size = new System.Drawing.Size(247, 20);
            this.txtDmodQ1.TabIndex = 43;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(314, 277);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 42;
            this.label8.Text = "D mod (Q-1)";
            // 
            // txtDmodP1
            // 
            this.txtDmodP1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDmodP1.Enabled = false;
            this.txtDmodP1.Location = new System.Drawing.Point(383, 243);
            this.txtDmodP1.Name = "txtDmodP1";
            this.txtDmodP1.Size = new System.Drawing.Size(247, 20);
            this.txtDmodP1.TabIndex = 41;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(314, 243);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "D mod (P-1)";
            // 
            // txtD
            // 
            this.txtD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtD.Enabled = false;
            this.txtD.Location = new System.Drawing.Point(383, 213);
            this.txtD.Name = "txtD";
            this.txtD.Size = new System.Drawing.Size(247, 20);
            this.txtD.TabIndex = 39;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(310, 213);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Private Exp (d)";
            // 
            // txtExponent
            // 
            this.txtExponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExponent.Enabled = false;
            this.txtExponent.Location = new System.Drawing.Point(383, 177);
            this.txtExponent.Name = "txtExponent";
            this.txtExponent.Size = new System.Drawing.Size(247, 20);
            this.txtExponent.TabIndex = 37;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(310, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 36;
            this.label11.Text = "Exponent (e)";
            // 
            // txtModulus
            // 
            this.txtModulus.Enabled = false;
            this.txtModulus.Location = new System.Drawing.Point(330, 144);
            this.txtModulus.Name = "txtModulus";
            this.txtModulus.Size = new System.Drawing.Size(300, 20);
            this.txtModulus.TabIndex = 35;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(312, 151);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "n";
            // 
            // txtQ
            // 
            this.txtQ.Enabled = false;
            this.txtQ.Location = new System.Drawing.Point(330, 118);
            this.txtQ.Name = "txtQ";
            this.txtQ.Size = new System.Drawing.Size(300, 20);
            this.txtQ.TabIndex = 33;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(310, 121);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Q";
            // 
            // txtP
            // 
            this.txtP.Enabled = false;
            this.txtP.Location = new System.Drawing.Point(330, 93);
            this.txtP.Name = "txtP";
            this.txtP.Size = new System.Drawing.Size(300, 20);
            this.txtP.TabIndex = 31;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(310, 93);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 13);
            this.label14.TabIndex = 30;
            this.label14.Text = "P";
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(742, 49);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 13);
            this.label16.TabIndex = 28;
            this.label16.Text = "RSA Key Size:";
            // 
            // btnGenerateWithPrivateKey
            // 
            this.btnGenerateWithPrivateKey.Location = new System.Drawing.Point(635, 46);
            this.btnGenerateWithPrivateKey.Name = "btnGenerateWithPrivateKey";
            this.btnGenerateWithPrivateKey.Size = new System.Drawing.Size(102, 41);
            this.btnGenerateWithPrivateKey.TabIndex = 27;
            this.btnGenerateWithPrivateKey.Text = "Generate With Private Key";
            this.btnGenerateWithPrivateKey.UseVisualStyleBackColor = true;
            this.btnGenerateWithPrivateKey.Click += new System.EventHandler(this.btnGenerateWithPrivateKey_Click);
            // 
            // btnImportWithPrivateKey
            // 
            this.btnImportWithPrivateKey.Location = new System.Drawing.Point(527, 46);
            this.btnImportWithPrivateKey.Name = "btnImportWithPrivateKey";
            this.btnImportWithPrivateKey.Size = new System.Drawing.Size(102, 41);
            this.btnImportWithPrivateKey.TabIndex = 26;
            this.btnImportWithPrivateKey.Text = "Import with Private Key";
            this.btnImportWithPrivateKey.UseVisualStyleBackColor = true;
            this.btnImportWithPrivateKey.Click += new System.EventHandler(this.btnImportWithPrivateKey_Click);
            // 
            // btnExportWithoutPrivateKey
            // 
            this.btnExportWithoutPrivateKey.Location = new System.Drawing.Point(419, 46);
            this.btnExportWithoutPrivateKey.Name = "btnExportWithoutPrivateKey";
            this.btnExportWithoutPrivateKey.Size = new System.Drawing.Size(102, 41);
            this.btnExportWithoutPrivateKey.TabIndex = 25;
            this.btnExportWithoutPrivateKey.Text = "Export without Private Key";
            this.btnExportWithoutPrivateKey.UseVisualStyleBackColor = true;
            this.btnExportWithoutPrivateKey.Click += new System.EventHandler(this.btnExportWithoutPrivateKey_Click);
            // 
            // btnExportWithPrivate
            // 
            this.btnExportWithPrivate.Location = new System.Drawing.Point(311, 46);
            this.btnExportWithPrivate.Name = "btnExportWithPrivate";
            this.btnExportWithPrivate.Size = new System.Drawing.Size(102, 41);
            this.btnExportWithPrivate.TabIndex = 24;
            this.btnExportWithPrivate.Text = "Export with Private Key";
            this.btnExportWithPrivate.UseVisualStyleBackColor = true;
            this.btnExportWithPrivate.Click += new System.EventHandler(this.btnExportWithPrivate_Click);
            // 
            // txtMacKey
            // 
            this.txtMacKey.Location = new System.Drawing.Point(73, 228);
            this.txtMacKey.Multiline = true;
            this.txtMacKey.Name = "txtMacKey";
            this.txtMacKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMacKey.Size = new System.Drawing.Size(182, 55);
            this.txtMacKey.TabIndex = 23;
            this.txtMacKey.Text = "\';pg,orj\'gjo,\'radgkoj,\'rg\'sdtgjtsd\'j\'j.\'klf\'gfksdl\'gfsdlg\'.\'sfgfsdgsdfgsfdgsfdgdg" +
    "fd";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "MAC Key:";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(74, 156);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKey.Size = new System.Drawing.Size(181, 55);
            this.txtKey.TabIndex = 21;
            this.txtKey.Text = "\';pg,orj\'gjo,\'radgkoj,\'rg\'sdtgjtsd\'j\'j.\'klf\'gfksdl\'gfsdlg\'.\'sfgfsdgsdfgsfdgsfdgdg" +
    "fd";
            // 
            // txtMulticastIP
            // 
            this.txtMulticastIP.Location = new System.Drawing.Point(74, 121);
            this.txtMulticastIP.Name = "txtMulticastIP";
            this.txtMulticastIP.Size = new System.Drawing.Size(181, 20);
            this.txtMulticastIP.TabIndex = 18;
            this.txtMulticastIP.Text = "238.0.0.0";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(73, 92);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(181, 20);
            this.txtPort.TabIndex = 17;
            this.txtPort.Text = "300";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Key:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Multicast Ip:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Port:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(73, 57);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(181, 20);
            this.txtUserName.TabIndex = 13;
            this.txtUserName.Text = "Or";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "User Name:";
            // 
            // fdExport
            // 
            this.fdExport.Filter = "XML Files(*.xml)|*.xml";
            // 
            // fdImport
            // 
            this.fdImport.FileName = "openFileDialog2";
            this.fdImport.Filter = "XML Files(*.xml)|*.xml";
            // 
            // fdLoadUserPublicKey
            // 
            this.fdLoadUserPublicKey.FileName = "openFileDialog2";
            this.fdLoadUserPublicKey.Filter = "XML Files(*.xml)|*.xml";
            // 
            // SCAFTIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 418);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnExitSCAFT);
            this.Name = "SCAFTIForm";
            this.Text = "SCAFTForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SCAFTForm_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabChate.ResumeLayout(false);
            this.tabChate.PerformLayout();
            this.tabConfiguration.ResumeLayout(false);
            this.tabConfiguration.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TextBox txtMacKey;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnGenerateWithPrivateKey;
        private System.Windows.Forms.Button btnImportWithPrivateKey;
        private System.Windows.Forms.Button btnExportWithoutPrivateKey;
        private System.Windows.Forms.Button btnExportWithPrivate;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDmodQ1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDmodP1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtD;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtExponent;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtModulus;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtQ;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.SaveFileDialog fdExport;
        private System.Windows.Forms.OpenFileDialog fdImport;
        private System.Windows.Forms.TextBox txtRSAKeySize;
        private System.Windows.Forms.Button btnLoadOtherUserPublicKey;
        private System.Windows.Forms.TextBox txtOtherUserName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.OpenFileDialog fdLoadUserPublicKey;
    }
}