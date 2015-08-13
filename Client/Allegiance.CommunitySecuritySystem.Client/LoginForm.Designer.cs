namespace Allegiance.CommunitySecuritySystem.Client
{
    partial class LoginForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			this._welcomeLabel = new System.Windows.Forms.Label();
			this._tooltipProvider = new System.Windows.Forms.ToolTip(this.components);
			this._loginStatusStrip = new System.Windows.Forms.StatusStrip();
			this._loginToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._loginToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this._statusText = new System.Windows.Forms.ToolStripStatusLabel();
			this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
			this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._accountControlsPanel = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this._loginStatusStrip.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// _welcomeLabel
			// 
			this._welcomeLabel.BackColor = System.Drawing.Color.Transparent;
			this._welcomeLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._welcomeLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._welcomeLabel.ForeColor = System.Drawing.Color.White;
			this._welcomeLabel.Location = new System.Drawing.Point(0, 105);
			this._welcomeLabel.Name = "_welcomeLabel";
			this._welcomeLabel.Size = new System.Drawing.Size(417, 26);
			this._welcomeLabel.TabIndex = 0;
			this._welcomeLabel.Text = "Welcome to the best game you have never played.";
			this._welcomeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// _tooltipProvider
			// 
			this._tooltipProvider.AutoPopDelay = 5000;
			this._tooltipProvider.InitialDelay = 500;
			this._tooltipProvider.ReshowDelay = 100;
			// 
			// _loginStatusStrip
			// 
			this._loginStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._loginToolStripStatusLabel,
            this._loginToolStripProgressBar});
			this._loginStatusStrip.Location = new System.Drawing.Point(0, 564);
			this._loginStatusStrip.Name = "_loginStatusStrip";
			this._loginStatusStrip.Size = new System.Drawing.Size(446, 22);
			this._loginStatusStrip.TabIndex = 35;
			// 
			// _loginToolStripStatusLabel
			// 
			this._loginToolStripStatusLabel.Name = "_loginToolStripStatusLabel";
			this._loginToolStripStatusLabel.Size = new System.Drawing.Size(279, 17);
			this._loginToolStripStatusLabel.Spring = true;
			this._loginToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _loginToolStripProgressBar
			// 
			this._loginToolStripProgressBar.AutoSize = false;
			this._loginToolStripProgressBar.Name = "_loginToolStripProgressBar";
			this._loginToolStripProgressBar.Size = new System.Drawing.Size(150, 16);
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
			this.toolStripProgressBar1.Step = 5;
			this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// _statusText
			// 
			this._statusText.Name = "_statusText";
			this._statusText.Size = new System.Drawing.Size(134, 17);
			this._statusText.Text = "Checking connectivity...";
			// 
			// _progressBar
			// 
			this._progressBar.Name = "_progressBar";
			this._progressBar.Size = new System.Drawing.Size(100, 16);
			this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// _statusLabel
			// 
			this._statusLabel.Name = "_statusLabel";
			this._statusLabel.Size = new System.Drawing.Size(134, 17);
			this._statusLabel.Text = "Checking connectivity...";
			// 
			// _accountControlsPanel
			// 
			this._accountControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._accountControlsPanel.Location = new System.Drawing.Point(0, 131);
			this._accountControlsPanel.Name = "_accountControlsPanel";
			this._accountControlsPanel.Size = new System.Drawing.Size(446, 433);
			this._accountControlsPanel.TabIndex = 36;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 417F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(446, 131);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Black;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(14, 131);
			this.panel1.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Black;
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(431, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(15, 131);
			this.panel2.TabIndex = 2;
			// 
			// panel3
			// 
			this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
			this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.panel3.Controls.Add(this._welcomeLabel);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(14, 0);
			this.panel3.Margin = new System.Windows.Forms.Padding(0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(417, 131);
			this.panel3.TabIndex = 3;
			// 
			// LoginForm
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(446, 586);
			this.Controls.Add(this._accountControlsPanel);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this._loginStatusStrip);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(462, 624);
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Welcome to Allegiance";
			this.Load += new System.EventHandler(this.LoginForm_Load);
			this._loginStatusStrip.ResumeLayout(false);
			this._loginStatusStrip.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _welcomeLabel;
		private System.Windows.Forms.ToolTip _tooltipProvider;
        private System.Windows.Forms.StatusStrip _loginStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
        private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel _statusText;
        private System.Windows.Forms.ToolStripStatusLabel _loginToolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar _loginToolStripProgressBar;
		private System.Windows.Forms.Panel _accountControlsPanel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;

    }
}