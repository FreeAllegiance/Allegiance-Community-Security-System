namespace Allegiance.CommunitySecuritySystem.Client.Install
{
	partial class CopyProgress
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
			this.label1 = new System.Windows.Forms.Label();
			this.lblCurrentFile = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.bwtBackgroundWorkerThread = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Copying: ";
			// 
			// lblCurrentFile
			// 
			this.lblCurrentFile.AutoSize = true;
			this.lblCurrentFile.Location = new System.Drawing.Point(59, 13);
			this.lblCurrentFile.Name = "lblCurrentFile";
			this.lblCurrentFile.Size = new System.Drawing.Size(0, 13);
			this.lblCurrentFile.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(269, 75);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pbProgress
			// 
			this.pbProgress.Location = new System.Drawing.Point(12, 41);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(594, 23);
			this.pbProgress.TabIndex = 3;
			// 
			// bwtBackgroundWorkerThread
			// 
			this.bwtBackgroundWorkerThread.WorkerReportsProgress = true;
			this.bwtBackgroundWorkerThread.WorkerSupportsCancellation = true;
			this.bwtBackgroundWorkerThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwtBackgroundWorkerThread_DoWork);
			this.bwtBackgroundWorkerThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwtBackgroundWorkerThread_ProgressChanged);
			this.bwtBackgroundWorkerThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwtBackgroundWorkerThread_RunWorkerCompleted);
			// 
			// CopyProgress
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(618, 110);
			this.ControlBox = false;
			this.Controls.Add(this.pbProgress);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblCurrentFile);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CopyProgress";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Copy Files";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.CopyProgress_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblCurrentFile;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ProgressBar pbProgress;
		private System.ComponentModel.BackgroundWorker bwtBackgroundWorkerThread;
	}
}