
namespace Detyra2_TCPserver
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtReceiver = new System.Windows.Forms.TextBox();
            this.serverStart_bttn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtReceiver
            // 
            this.txtReceiver.AllowDrop = true;
            this.txtReceiver.Location = new System.Drawing.Point(14, 64);
            this.txtReceiver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReceiver.Multiline = true;
            this.txtReceiver.Name = "txtReceiver";
            this.txtReceiver.ReadOnly = true;
            this.txtReceiver.Size = new System.Drawing.Size(882, 519);
            this.txtReceiver.TabIndex = 0;
            this.txtReceiver.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // serverStart_bttn
            // 
            this.serverStart_bttn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.serverStart_bttn.Location = new System.Drawing.Point(14, 10);
            this.serverStart_bttn.Name = "serverStart_bttn";
            this.serverStart_bttn.Size = new System.Drawing.Size(245, 45);
            this.serverStart_bttn.TabIndex = 1;
            this.serverStart_bttn.TabStop = false;
            this.serverStart_bttn.Text = "Start Server";
            this.serverStart_bttn.UseVisualStyleBackColor = true;
            this.serverStart_bttn.Click += new System.EventHandler(this.serverStart_bttn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 600);
            this.Controls.Add(this.serverStart_bttn);
            this.Controls.Add(this.txtReceiver);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReceiver;
        private System.Windows.Forms.Button serverStart_bttn;
    }
}

