namespace Allegiance.CommunitySecuritySystem.Client.Install
{
	partial class VC2010Install
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
			this.lblMessage = new System.Windows.Forms.Label();
			this.lnkDownloadLink = new System.Windows.Forms.LinkLabel();
			this.rbInstallVc = new System.Windows.Forms.RadioButton();
			this.rbSkipInstall = new System.Windows.Forms.RadioButton();
			this.rbAbortInstallation = new System.Windows.Forms.RadioButton();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnContinue = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblMessage.Location = new System.Drawing.Point(0, 0);
			this.lblMessage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Padding = new System.Windows.Forms.Padding(10);
			this.lblMessage.Size = new System.Drawing.Size(693, 33);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "Visual C++ 2010 x86 Runtime is not installed or needs to be updated. Please selec" +
    "t OK to download and install the VC++ 2010 Runtimes from:";
			// 
			// lnkDownloadLink
			// 
			this.lnkDownloadLink.AutoSize = true;
			this.lnkDownloadLink.Dock = System.Windows.Forms.DockStyle.Top;
			this.lnkDownloadLink.Location = new System.Drawing.Point(0, 33);
			this.lnkDownloadLink.Name = "lnkDownloadLink";
			this.lnkDownloadLink.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.lnkDownloadLink.Size = new System.Drawing.Size(571, 13);
			this.lnkDownloadLink.TabIndex = 1;
			this.lnkDownloadLink.TabStop = true;
			this.lnkDownloadLink.Text = "http://download.microsoft.com/download/5/B/C/5BC5DBB3-652D-4DCE-B14A-475AB85EEF6E" +
    "/vcredist_x86.exe";
			this.lnkDownloadLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDownloadLink_LinkClicked);
			// 
			// rbInstallVc
			// 
			this.rbInstallVc.AutoSize = true;
			this.rbInstallVc.Checked = true;
			this.rbInstallVc.Dock = System.Windows.Forms.DockStyle.Top;
			this.rbInstallVc.Location = new System.Drawing.Point(0, 46);
			this.rbInstallVc.Name = "rbInstallVc";
			this.rbInstallVc.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
			this.rbInstallVc.Size = new System.Drawing.Size(734, 47);
			this.rbInstallVc.TabIndex = 2;
			this.rbInstallVc.TabStop = true;
			this.rbInstallVc.Text = "&Download and Install VC++ 2010 x86 Redistributable.";
			this.rbInstallVc.UseVisualStyleBackColor = true;
			// 
			// rbSkipInstall
			// 
			this.rbSkipInstall.AutoSize = true;
			this.rbSkipInstall.Dock = System.Windows.Forms.DockStyle.Top;
			this.rbSkipInstall.Location = new System.Drawing.Point(0, 93);
			this.rbSkipInstall.Name = "rbSkipInstall";
			this.rbSkipInstall.Padding = new System.Windows.Forms.Padding(10);
			this.rbSkipInstall.Size = new System.Drawing.Size(734, 37);
			this.rbSkipInstall.TabIndex = 3;
			this.rbSkipInstall.Text = "&Skip VC++ 2010 x86 Redistributable and Continue (Not Recommended).";
			this.rbSkipInstall.UseVisualStyleBackColor = true;
			// 
			// rbAbortInstallation
			// 
			this.rbAbortInstallation.AutoSize = true;
			this.rbAbortInstallation.Dock = System.Windows.Forms.DockStyle.Top;
			this.rbAbortInstallation.Location = new System.Drawing.Point(0, 130);
			this.rbAbortInstallation.Name = "rbAbortInstallation";
			this.rbAbortInstallation.Padding = new System.Windows.Forms.Padding(10);
			this.rbAbortInstallation.Size = new System.Drawing.Size(734, 37);
			this.rbAbortInstallation.TabIndex = 4;
			this.rbAbortInstallation.Text = "&Abort ACSS Installation.";
			this.rbAbortInstallation.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.btnContinue, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 194);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 29);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// btnContinue
			// 
			this.btnContinue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnContinue.Location = new System.Drawing.Point(329, 3);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(75, 23);
			this.btnContinue.TabIndex = 0;
			this.btnContinue.Text = "&Continue";
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			// 
			// VC2010Install
			// 
			this.AcceptButton = this.btnContinue;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(734, 223);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.rbAbortInstallation);
			this.Controls.Add(this.rbSkipInstall);
			this.Controls.Add(this.rbInstallVc);
			this.Controls.Add(this.lnkDownloadLink);
			this.Controls.Add(this.lblMessage);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "VC2010Install";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "VC++ 2010 x86 Runtime Install";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.VC2010Install_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.LinkLabel lnkDownloadLink;
		private System.Windows.Forms.RadioButton rbInstallVc;
		private System.Windows.Forms.RadioButton rbSkipInstall;
		private System.Windows.Forms.RadioButton rbAbortInstallation;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnContinue;
	}
}