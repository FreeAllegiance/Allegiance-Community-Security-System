namespace Allegiance.CommunitySecuritySystem.Client
{
	partial class LauncherStartupProgress
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherStartupProgress));
			this._startupProgressBar = new System.Windows.Forms.ProgressBar();
			this._progressLabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _startupProgressBar
			// 
			this._startupProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
			this._startupProgressBar.Location = new System.Drawing.Point(10, 10);
			this._startupProgressBar.Name = "_startupProgressBar";
			this._startupProgressBar.Size = new System.Drawing.Size(643, 23);
			this._startupProgressBar.TabIndex = 0;
			this._startupProgressBar.UseWaitCursor = true;
			// 
			// _progressLabel
			// 
			this._progressLabel.AutoSize = true;
			this._progressLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this._progressLabel.Location = new System.Drawing.Point(0, 0);
			this._progressLabel.Name = "_progressLabel";
			this._progressLabel.Padding = new System.Windows.Forms.Padding(20, 10, 0, 0);
			this._progressLabel.Size = new System.Drawing.Size(89, 23);
			this._progressLabel.TabIndex = 1;
			this._progressLabel.Text = "Starting Up...";
			this._progressLabel.UseWaitCursor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._startupProgressBar);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 23);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10);
			this.panel1.Size = new System.Drawing.Size(663, 118);
			this.panel1.TabIndex = 2;
			this.panel1.UseWaitCursor = true;
			// 
			// LauncherStartupProgress
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(663, 141);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._progressLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "LauncherStartupProgress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Starting Allegiance";
			this.UseWaitCursor = true;
			this.Load += new System.EventHandler(this.LauncherStartupProgress_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar _startupProgressBar;
		private System.Windows.Forms.Label _progressLabel;
		private System.Windows.Forms.Panel panel1;
	}
}