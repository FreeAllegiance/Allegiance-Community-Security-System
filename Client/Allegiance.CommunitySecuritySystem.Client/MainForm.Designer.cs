namespace Allegiance.CommunitySecuritySystem.Client
{
    partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._mainStatusStrip = new System.Windows.Forms.StatusStrip();
			this._mainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._mainToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this._mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this._fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._launchWindowedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._logChatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._autoLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._safeModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._debugLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._noMoviesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._systemInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._dividerToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
			this._squadRostersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._cssDiagnosticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._leaderboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._banlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._troubleshootingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._newToAllegToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._newToCSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._mainTabControl = new System.Windows.Forms.TabControl();
			this._playTabPage = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this._messagesTabPage = new System.Windows.Forms.TabPage();
			this._messageListControl = new Allegiance.CommunitySecuritySystem.Client.Controls.MessageListControl();
			this._callsignsTabPage = new System.Windows.Forms.TabPage();
			this._callsignControl = new Allegiance.CommunitySecuritySystem.Client.Controls.CallsignControl();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this._notificationIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this._notificationMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._mainStatusStrip.SuspendLayout();
			this._mainMenuStrip.SuspendLayout();
			this._mainTabControl.SuspendLayout();
			this._playTabPage.SuspendLayout();
			this._messagesTabPage.SuspendLayout();
			this._callsignsTabPage.SuspendLayout();
			this._notificationMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// _mainStatusStrip
			// 
			this._mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mainToolStripStatusLabel,
            this._mainToolStripProgressBar});
			this._mainStatusStrip.Location = new System.Drawing.Point(0, 360);
			this._mainStatusStrip.Name = "_mainStatusStrip";
			this._mainStatusStrip.Size = new System.Drawing.Size(406, 22);
			this._mainStatusStrip.SizingGrip = false;
			this._mainStatusStrip.TabIndex = 0;
			// 
			// _mainToolStripStatusLabel
			// 
			this._mainToolStripStatusLabel.Name = "_mainToolStripStatusLabel";
			this._mainToolStripStatusLabel.Size = new System.Drawing.Size(391, 17);
			this._mainToolStripStatusLabel.Spring = true;
			this._mainToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _mainToolStripProgressBar
			// 
			this._mainToolStripProgressBar.Name = "_mainToolStripProgressBar";
			this._mainToolStripProgressBar.Size = new System.Drawing.Size(125, 16);
			this._mainToolStripProgressBar.Visible = false;
			// 
			// _mainMenuStrip
			// 
			this._mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileToolStripMenuItem,
            this._preferencesToolStripMenuItem,
            this._toolsToolStripMenuItem,
            this._helpToolStripMenuItem});
			this._mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this._mainMenuStrip.Name = "_mainMenuStrip";
			this._mainMenuStrip.Size = new System.Drawing.Size(406, 28);
			this._mainMenuStrip.TabIndex = 1;
			this._mainMenuStrip.Text = "menuStrip1";
			// 
			// _fileToolStripMenuItem
			// 
			this._fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._logoutToolStripMenuItem,
            this._exitToolStripMenuItem});
			this._fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
			this._fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this._fileToolStripMenuItem.Text = "File";
			// 
			// _logoutToolStripMenuItem
			// 
			this._logoutToolStripMenuItem.Name = "_logoutToolStripMenuItem";
			this._logoutToolStripMenuItem.Size = new System.Drawing.Size(125, 24);
			this._logoutToolStripMenuItem.Text = "Logout";
			this._logoutToolStripMenuItem.Click += new System.EventHandler(this._logoutToolStripMenuItem_Click);
			// 
			// _exitToolStripMenuItem
			// 
			this._exitToolStripMenuItem.Name = "_exitToolStripMenuItem";
			this._exitToolStripMenuItem.Size = new System.Drawing.Size(125, 24);
			this._exitToolStripMenuItem.Text = "Exit";
			this._exitToolStripMenuItem.Click += new System.EventHandler(this._exitToolStripMenuItem_Click);
			// 
			// _preferencesToolStripMenuItem
			// 
			this._preferencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._launchWindowedToolStripMenuItem,
            this._logChatToolStripMenuItem,
            this._autoLoginToolStripMenuItem,
            this._safeModeToolStripMenuItem,
            this._debugLogToolStripMenuItem,
            this._noMoviesToolStripMenuItem});
			this._preferencesToolStripMenuItem.Name = "_preferencesToolStripMenuItem";
			this._preferencesToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
			this._preferencesToolStripMenuItem.Text = "Preferences";
			// 
			// _launchWindowedToolStripMenuItem
			// 
			this._launchWindowedToolStripMenuItem.Name = "_launchWindowedToolStripMenuItem";
			this._launchWindowedToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
			this._launchWindowedToolStripMenuItem.Text = "Launch Windowed";
			this._launchWindowedToolStripMenuItem.Click += new System.EventHandler(this._launchWindowedToolStripMenuItem_Click);
			// 
			// _logChatToolStripMenuItem
			// 
			this._logChatToolStripMenuItem.Checked = true;
			this._logChatToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this._logChatToolStripMenuItem.Name = "_logChatToolStripMenuItem";
			this._logChatToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
			this._logChatToolStripMenuItem.Text = "Log Chat";
			this._logChatToolStripMenuItem.Click += new System.EventHandler(this._logChatToolStripMenuItem_Click);
			// 
			// _autoLoginToolStripMenuItem
			// 
			this._autoLoginToolStripMenuItem.Name = "_autoLoginToolStripMenuItem";
			this._autoLoginToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
			this._autoLoginToolStripMenuItem.Text = "Auto Login";
			this._autoLoginToolStripMenuItem.Click += new System.EventHandler(this._autoLoginToolStripMenuItem_Click);
			// 
			// _safeModeToolStripMenuItem
			// 
			this._safeModeToolStripMenuItem.Name = "_safeModeToolStripMenuItem";
			this._safeModeToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
			this._safeModeToolStripMenuItem.Text = "Safe Mode";
			this._safeModeToolStripMenuItem.Click += new System.EventHandler(this._safeModeToolStripMenuItem_Click);
			// 
			// _debugLogToolStripMenuItem
			// 
			this._debugLogToolStripMenuItem.Name = "_debugLogToolStripMenuItem";
			this._debugLogToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
			this._debugLogToolStripMenuItem.Text = "Debug Log";
			this._debugLogToolStripMenuItem.Click += new System.EventHandler(this._debugLogToolStripMenuItem_Click);
			// 
			// _noMoviesToolStripMenuItem
			// 
			this._noMoviesToolStripMenuItem.Name = "_noMoviesToolStripMenuItem";
			this._noMoviesToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
			this._noMoviesToolStripMenuItem.Text = "No Movies";
			this._noMoviesToolStripMenuItem.Click += new System.EventHandler(this._noMoviesToolStripMenuItem_Click);
			// 
			// _toolsToolStripMenuItem
			// 
			this._toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._systemInfoToolStripMenuItem,
            this._dividerToolStripMenuItem,
            this._squadRostersToolStripMenuItem,
            this._cssDiagnosticsToolStripMenuItem,
            this._leaderboardToolStripMenuItem,
            this._banlistToolStripMenuItem});
			this._toolsToolStripMenuItem.Name = "_toolsToolStripMenuItem";
			this._toolsToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
			this._toolsToolStripMenuItem.Text = "Tools";
			// 
			// _systemInfoToolStripMenuItem
			// 
			this._systemInfoToolStripMenuItem.Name = "_systemInfoToolStripMenuItem";
			this._systemInfoToolStripMenuItem.Size = new System.Drawing.Size(207, 24);
			this._systemInfoToolStripMenuItem.Text = "System Information";
			this._systemInfoToolStripMenuItem.Click += new System.EventHandler(this._systemInfoToolStripMenuItem_Click);
			// 
			// _dividerToolStripMenuItem
			// 
			this._dividerToolStripMenuItem.Name = "_dividerToolStripMenuItem";
			this._dividerToolStripMenuItem.Size = new System.Drawing.Size(204, 6);
			// 
			// _squadRostersToolStripMenuItem
			// 
			this._squadRostersToolStripMenuItem.Name = "_squadRostersToolStripMenuItem";
			this._squadRostersToolStripMenuItem.Size = new System.Drawing.Size(207, 24);
			this._squadRostersToolStripMenuItem.Text = "Squad Rosters";
			this._squadRostersToolStripMenuItem.Click += new System.EventHandler(this._squadRostersToolStripMenuItem_Click);
			// 
			// _cssDiagnosticsToolStripMenuItem
			// 
			this._cssDiagnosticsToolStripMenuItem.Name = "_cssDiagnosticsToolStripMenuItem";
			this._cssDiagnosticsToolStripMenuItem.Size = new System.Drawing.Size(207, 24);
			this._cssDiagnosticsToolStripMenuItem.Text = "ACSS Diagnostics";
			this._cssDiagnosticsToolStripMenuItem.Click += new System.EventHandler(this._cssDiagnosticsToolStripMenuItem_Click);
			// 
			// _leaderboardToolStripMenuItem
			// 
			this._leaderboardToolStripMenuItem.Name = "_leaderboardToolStripMenuItem";
			this._leaderboardToolStripMenuItem.Size = new System.Drawing.Size(207, 24);
			this._leaderboardToolStripMenuItem.Text = "Leaderboard";
			this._leaderboardToolStripMenuItem.Click += new System.EventHandler(this._leaderboardToolStripMenuItem_Click);
			// 
			// _banlistToolStripMenuItem
			// 
			this._banlistToolStripMenuItem.Name = "_banlistToolStripMenuItem";
			this._banlistToolStripMenuItem.Size = new System.Drawing.Size(207, 24);
			this._banlistToolStripMenuItem.Text = "Banlist";
			this._banlistToolStripMenuItem.Click += new System.EventHandler(this._banlistToolStripMenuItem_Click);
			// 
			// _helpToolStripMenuItem
			// 
			this._helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._troubleshootingToolStripMenuItem,
            this._newToAllegToolStripMenuItem,
            this._newToCSSToolStripMenuItem,
            this._aboutToolStripMenuItem});
			this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
			this._helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
			this._helpToolStripMenuItem.Text = "Help";
			// 
			// _troubleshootingToolStripMenuItem
			// 
			this._troubleshootingToolStripMenuItem.Name = "_troubleshootingToolStripMenuItem";
			this._troubleshootingToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this._troubleshootingToolStripMenuItem.Text = "Troubleshooting";
			this._troubleshootingToolStripMenuItem.Click += new System.EventHandler(this._troubleshootingToolStripMenuItem_Click);
			// 
			// _newToAllegToolStripMenuItem
			// 
			this._newToAllegToolStripMenuItem.Name = "_newToAllegToolStripMenuItem";
			this._newToAllegToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this._newToAllegToolStripMenuItem.Text = "New to Alleg?";
			this._newToAllegToolStripMenuItem.Click += new System.EventHandler(this._newToAllegToolStripMenuItem_Click);
			// 
			// _newToCSSToolStripMenuItem
			// 
			this._newToCSSToolStripMenuItem.Name = "_newToCSSToolStripMenuItem";
			this._newToCSSToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this._newToCSSToolStripMenuItem.Text = "New to CSS?";
			this._newToCSSToolStripMenuItem.Click += new System.EventHandler(this._newToCSSToolStripMenuItem_Click);
			// 
			// _aboutToolStripMenuItem
			// 
			this._aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
			this._aboutToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this._aboutToolStripMenuItem.Text = "About";
			this._aboutToolStripMenuItem.Click += new System.EventHandler(this._aboutToolStripMenuItem_Click);
			// 
			// _mainTabControl
			// 
			this._mainTabControl.Controls.Add(this._playTabPage);
			this._mainTabControl.Controls.Add(this._messagesTabPage);
			this._mainTabControl.Controls.Add(this._callsignsTabPage);
			this._mainTabControl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._mainTabControl.Location = new System.Drawing.Point(0, 39);
			this._mainTabControl.Margin = new System.Windows.Forms.Padding(0);
			this._mainTabControl.Name = "_mainTabControl";
			this._mainTabControl.SelectedIndex = 0;
			this._mainTabControl.Size = new System.Drawing.Size(408, 326);
			this._mainTabControl.TabIndex = 1;
			// 
			// _playTabPage
			// 
			this._playTabPage.BackColor = System.Drawing.SystemColors.Control;
			this._playTabPage.Controls.Add(this.label1);
			this._playTabPage.Location = new System.Drawing.Point(4, 28);
			this._playTabPage.Margin = new System.Windows.Forms.Padding(0);
			this._playTabPage.Name = "_playTabPage";
			this._playTabPage.Padding = new System.Windows.Forms.Padding(3);
			this._playTabPage.Size = new System.Drawing.Size(400, 294);
			this._playTabPage.TabIndex = 0;
			this._playTabPage.Text = "Play";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(41, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(309, 68);
			this.label1.TabIndex = 1;
			this.label1.Text = "Due to VS2010 bug, the playControl is loaded in the MainForm_Load event. Check th" +
    "ere to make changes.";
			// 
			// _messagesTabPage
			// 
			this._messagesTabPage.Controls.Add(this._messageListControl);
			this._messagesTabPage.Location = new System.Drawing.Point(4, 28);
			this._messagesTabPage.Margin = new System.Windows.Forms.Padding(0);
			this._messagesTabPage.Name = "_messagesTabPage";
			this._messagesTabPage.Size = new System.Drawing.Size(400, 294);
			this._messagesTabPage.TabIndex = 1;
			this._messagesTabPage.Text = "Messages";
			this._messagesTabPage.UseVisualStyleBackColor = true;
			// 
			// _messageListControl
			// 
			this._messageListControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._messageListControl.Location = new System.Drawing.Point(0, 0);
			this._messageListControl.Margin = new System.Windows.Forms.Padding(0);
			this._messageListControl.Name = "_messageListControl";
			this._messageListControl.Size = new System.Drawing.Size(400, 294);
			this._messageListControl.TabIndex = 0;
			// 
			// _callsignsTabPage
			// 
			this._callsignsTabPage.Controls.Add(this._callsignControl);
			this._callsignsTabPage.Location = new System.Drawing.Point(4, 28);
			this._callsignsTabPage.Margin = new System.Windows.Forms.Padding(0);
			this._callsignsTabPage.Name = "_callsignsTabPage";
			this._callsignsTabPage.Size = new System.Drawing.Size(400, 294);
			this._callsignsTabPage.TabIndex = 2;
			this._callsignsTabPage.Text = "Callsigns";
			this._callsignsTabPage.UseVisualStyleBackColor = true;
			// 
			// _callsignControl
			// 
			this._callsignControl.BackColor = System.Drawing.SystemColors.Control;
			this._callsignControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._callsignControl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._callsignControl.Location = new System.Drawing.Point(0, 0);
			this._callsignControl.Margin = new System.Windows.Forms.Padding(0);
			this._callsignControl.Name = "_callsignControl";
			this._callsignControl.Size = new System.Drawing.Size(400, 294);
			this._callsignControl.TabIndex = 0;
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _notificationIcon
			// 
			this._notificationIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this._notificationIcon.BalloonTipTitle = "Allegiance Community Security System";
			this._notificationIcon.ContextMenuStrip = this._notificationMenu;
			this._notificationIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("_notificationIcon.Icon")));
			this._notificationIcon.Text = "Allegiance Community Security System";
			this._notificationIcon.Visible = true;
			this._notificationIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._notificationIcon_MouseDoubleClick);
			// 
			// _notificationMenu
			// 
			this._notificationMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem});
			this._notificationMenu.Name = "_notificationMenu";
			this._notificationMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._notificationMenu.ShowImageMargin = false;
			this._notificationMenu.Size = new System.Drawing.Size(86, 52);
			// 
			// showToolStripMenuItem
			// 
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			this.showToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
			this.showToolStripMenuItem.Text = "Hide";
			this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this._exitToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(406, 382);
			this.Controls.Add(this._mainStatusStrip);
			this.Controls.Add(this._mainMenuStrip);
			this.Controls.Add(this._mainTabControl);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this._mainMenuStrip;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Community Security System";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this._mainStatusStrip.ResumeLayout(false);
			this._mainStatusStrip.PerformLayout();
			this._mainMenuStrip.ResumeLayout(false);
			this._mainMenuStrip.PerformLayout();
			this._mainTabControl.ResumeLayout(false);
			this._playTabPage.ResumeLayout(false);
			this._messagesTabPage.ResumeLayout(false);
			this._callsignsTabPage.ResumeLayout(false);
			this._notificationMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip _mainStatusStrip;
        private System.Windows.Forms.MenuStrip _mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _launchWindowedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _logChatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _safeModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _debugLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _systemInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _dividerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _squadRostersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _cssDiagnosticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _leaderboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _banlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _troubleshootingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _newToAllegToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _newToCSSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _aboutToolStripMenuItem;
        private System.Windows.Forms.TabControl _mainTabControl;
        private System.Windows.Forms.TabPage _playTabPage;
        private System.Windows.Forms.TabPage _messagesTabPage;
        private System.Windows.Forms.TabPage _callsignsTabPage;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem _autoLoginToolStripMenuItem;
        //private PlayControl _playControl;
        private Allegiance.CommunitySecuritySystem.Client.Controls.MessageListControl _messageListControl;
        private Allegiance.CommunitySecuritySystem.Client.Controls.CallsignControl _callsignControl;
        private System.Windows.Forms.ToolStripStatusLabel _mainToolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar _mainToolStripProgressBar;
        private System.Windows.Forms.NotifyIcon _notificationIcon;
        private System.Windows.Forms.ContextMenuStrip _notificationMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _noMoviesToolStripMenuItem;
		private System.Windows.Forms.Label label1;
    }
}