namespace Allegiance.CommunitySecuritySystem.Client
{
	partial class OfflineLaunch
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfflineLaunch));
			this._promptLabel = new System.Windows.Forms.Label();
			this._helpLinkLinkLabel = new System.Windows.Forms.LinkLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this._retryLoginButton = new System.Windows.Forms.Button();
			this._playOfflineButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// _promptLabel
			// 
			this._promptLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this._promptLabel.Location = new System.Drawing.Point(0, 0);
			this._promptLabel.Name = "_promptLabel";
			this._promptLabel.Padding = new System.Windows.Forms.Padding(10);
			this._promptLabel.Size = new System.Drawing.Size(468, 74);
			this._promptLabel.TabIndex = 0;
			this._promptLabel.Text = "_promptLabel";
			// 
			// _helpLinkLinkLabel
			// 
			this._helpLinkLinkLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this._helpLinkLinkLabel.Location = new System.Drawing.Point(0, 74);
			this._helpLinkLinkLabel.Name = "_helpLinkLinkLabel";
			this._helpLinkLinkLabel.Size = new System.Drawing.Size(468, 38);
			this._helpLinkLinkLabel.TabIndex = 1;
			this._helpLinkLinkLabel.TabStop = true;
			this._helpLinkLinkLabel.Text = "_helpLinkLinkLabel";
			this._helpLinkLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this._helpLinkLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._helpLinkLinkLabel_LinkClicked);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 161);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(468, 40);
			this.panel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this._retryLoginButton);
			this.panel2.Controls.Add(this._playOfflineButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(290, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(178, 40);
			this.panel2.TabIndex = 0;
			// 
			// _retryLoginButton
			// 
			this._retryLoginButton.Location = new System.Drawing.Point(85, 2);
			this._retryLoginButton.Name = "_retryLoginButton";
			this._retryLoginButton.Size = new System.Drawing.Size(75, 23);
			this._retryLoginButton.TabIndex = 1;
			this._retryLoginButton.Text = "&Retry Login";
			this._retryLoginButton.UseVisualStyleBackColor = true;
			this._retryLoginButton.Click += new System.EventHandler(this._retryLoginButton_Click);
			// 
			// _playOfflineButton
			// 
			this._playOfflineButton.Location = new System.Drawing.Point(3, 3);
			this._playOfflineButton.Name = "_playOfflineButton";
			this._playOfflineButton.Size = new System.Drawing.Size(75, 23);
			this._playOfflineButton.TabIndex = 0;
			this._playOfflineButton.Text = "&Play Offline";
			this._playOfflineButton.UseVisualStyleBackColor = true;
			this._playOfflineButton.Click += new System.EventHandler(this._playOfflineButton_Click);
			// 
			// OfflineLaunch
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(468, 201);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._helpLinkLinkLabel);
			this.Controls.Add(this._promptLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OfflineLaunch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Offline Launch";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label _promptLabel;
		private System.Windows.Forms.LinkLabel _helpLinkLinkLabel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button _retryLoginButton;
		private System.Windows.Forms.Button _playOfflineButton;
	}
}