namespace SCAFT
{
    partial class TestForm
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
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtIV = new System.Windows.Forms.TextBox();
            this.txtPlainText1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtCyperText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPlainText2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(239, 95);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(166, 23);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnString1Tobytes_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(12, 67);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(190, 69);
            this.txtKey.TabIndex = 1;
            // 
            // txtIV
            // 
            this.txtIV.Location = new System.Drawing.Point(12, 175);
            this.txtIV.Multiline = true;
            this.txtIV.Name = "txtIV";
            this.txtIV.Size = new System.Drawing.Size(190, 69);
            this.txtIV.TabIndex = 2;
            // 
            // txtPlainText1
            // 
            this.txtPlainText1.Location = new System.Drawing.Point(12, 284);
            this.txtPlainText1.Multiline = true;
            this.txtPlainText1.Name = "txtPlainText1";
            this.txtPlainText1.Size = new System.Drawing.Size(190, 69);
            this.txtPlainText1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "string1 = key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "bytes iv";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 268);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "string2 plain text";
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(239, 258);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(166, 23);
            this.btnDecrypt.TabIndex = 7;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnBytesToString_Click);
            // 
            // txtCyperText
            // 
            this.txtCyperText.Location = new System.Drawing.Point(12, 399);
            this.txtCyperText.Multiline = true;
            this.txtCyperText.Name = "txtCyperText";
            this.txtCyperText.Size = new System.Drawing.Size(190, 69);
            this.txtCyperText.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 372);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = " cyper text";
            // 
            // txtPlainText2
            // 
            this.txtPlainText2.Location = new System.Drawing.Point(239, 399);
            this.txtPlainText2.Multiline = true;
            this.txtPlainText2.Name = "txtPlainText2";
            this.txtPlainText2.Size = new System.Drawing.Size(190, 69);
            this.txtPlainText2.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(302, 372);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "plain text 2";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 492);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPlainText2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCyperText);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPlainText1);
            this.Controls.Add(this.txtIV);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.btnEncrypt);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtIV;
        private System.Windows.Forms.TextBox txtPlainText1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtCyperText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPlainText2;
        private System.Windows.Forms.Label label5;
    }
}