namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
	partial class UpdateCheckControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._updateCheckLabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this._exitWithoutUpdatingButton = new System.Windows.Forms.Button();
			this._mainStatusPanel = new System.Windows.Forms.Panel();
			this._updateProgressPanel = new System.Windows.Forms.Panel();
			this._updateProgressBar = new System.Windows.Forms.ProgressBar();
			this._updateStatusLabel = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this._mainStatusPanel.SuspendLayout();
			this._updateProgressPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _updateCheckLabel
			// 
			this._updateCheckLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this._updateCheckLabel.Location = new System.Drawing.Point(0, 0);
			this._updateCheckLabel.Name = "_updateCheckLabel";
			this._updateCheckLabel.Size = new System.Drawing.Size(291, 43);
			this._updateCheckLabel.TabIndex = 0;
			this._updateCheckLabel.Text = "Just a moment while we check for updates.";
			this._updateCheckLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 212);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(291, 36);
			this.panel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this._exitWithoutUpdatingButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(166, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(125, 36);
			this.panel2.TabIndex = 0;
			// 
			// _exitWithoutUpdatingButton
			// 
			this._exitWithoutUpdatingButton.Location = new System.Drawing.Point(3, 4);
			this._exitWithoutUpdatingButton.Name = "_exitWithoutUpdatingButton";
			this._exitWithoutUpdatingButton.Size = new System.Drawing.Size(111, 23);
			this._exitWithoutUpdatingButton.TabIndex = 0;
			this._exitWithoutUpdatingButton.Text = "&Cancel Update";
			this._exitWithoutUpdatingButton.UseVisualStyleBackColor = true;
			this._exitWithoutUpdatingButton.Click += new System.EventHandler(this._exitWithoutUpdatingButton_Click);
			// 
			// _mainStatusPanel
			// 
			this._mainStatusPanel.Controls.Add(this._updateProgressPanel);
			this._mainStatusPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this._mainStatusPanel.Location = new System.Drawing.Point(0, 43);
			this._mainStatusPanel.Name = "_mainStatusPanel";
			this._mainStatusPanel.Size = new System.Drawing.Size(291, 100);
			this._mainStatusPanel.TabIndex = 3;
			// 
			// _updateProgressPanel
			// 
			this._updateProgressPanel.Controls.Add(this._updateProgressBar);
			this._updateProgressPanel.Controls.Add(this._updateStatusLabel);
			this._updateProgressPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._updateProgressPanel.Location = new System.Drawing.Point(0, 0);
			this._updateProgressPanel.Name = "_updateProgressPanel";
			this._updateProgressPanel.Padding = new System.Windows.Forms.Padding(10);
			this._updateProgressPanel.Size = new System.Drawing.Size(291, 100);
			this._updateProgressPanel.TabIndex = 5;
			// 
			// _updateProgressBar
			// 
			this._updateProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
			this._updateProgressBar.Location = new System.Drawing.Point(10, 28);
			this._updateProgressBar.Name = "_updateProgressBar";
			this._updateProgressBar.Size = new System.Drawing.Size(271, 23);
			this._updateProgressBar.TabIndex = 1;
			// 
			// _updateStatusLabel
			// 
			this._updateStatusLabel.AutoSize = true;
			this._updateStatusLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this._updateStatusLabel.Location = new System.Drawing.Point(10, 10);
			this._updateStatusLabel.Name = "_updateStatusLabel";
			this._updateStatusLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 5);
			this._updateStatusLabel.Size = new System.Drawing.Size(112, 18);
			this._updateStatusLabel.TabIndex = 0;
			this._updateStatusLabel.Text = "_updateStatusLabel";
			// 
			// UpdateCheckControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this._mainStatusPanel);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._updateCheckLabel);
			this.Name = "UpdateCheckControl";
			this.Size = new System.Drawing.Size(291, 248);
			this.Load += new System.EventHandler(this.UpdateCheck_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this._mainStatusPanel.ResumeLayout(false);
			this._updateProgressPanel.ResumeLayout(false);
			this._updateProgressPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label _updateCheckLabel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button _exitWithoutUpdatingButton;
		private System.Windows.Forms.Panel _mainStatusPanel;
		private System.Windows.Forms.Panel _updateProgressPanel;
		private System.Windows.Forms.ProgressBar _updateProgressBar;
		private System.Windows.Forms.Label _updateStatusLabel;
	}
}
