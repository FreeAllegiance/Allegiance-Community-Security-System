namespace Allegiance.CommunitySecuritySystem.Client
{
    partial class PlayControl
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
			this.components = new System.ComponentModel.Container();
			this._playOfflineButton = new System.Windows.Forms.Button();
			this._playBetaButton = new System.Windows.Forms.Button();
			this._playOnlineButton = new System.Windows.Forms.Button();
			this._loginComboBox = new System.Windows.Forms.ComboBox();
			this._loginAsLabel = new System.Windows.Forms.Label();
			this._callsignGroupbox = new System.Windows.Forms.GroupBox();
			this._learnMoreLinkLabel = new System.Windows.Forms.LinkLabel();
			this.manageCallsignsButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this._playmodeGroupbox = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.playControlErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this._allegianceRunningGroupBox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._exitAllegianceButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this._callsignGroupbox.SuspendLayout();
			this._playmodeGroupbox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.playControlErrorProvider)).BeginInit();
			this._allegianceRunningGroupBox.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _playOfflineButton
			// 
			this._playOfflineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._playOfflineButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._playOfflineButton.Location = new System.Drawing.Point(13, 105);
			this._playOfflineButton.Name = "_playOfflineButton";
			this._playOfflineButton.Size = new System.Drawing.Size(135, 35);
			this._playOfflineButton.TabIndex = 15;
			this._playOfflineButton.Text = "Play Offline";
			this._playOfflineButton.UseVisualStyleBackColor = true;
			this._playOfflineButton.Click += new System.EventHandler(this._playOfflineButton_Click);
			// 
			// _playBetaButton
			// 
			this._playBetaButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._playBetaButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._playBetaButton.Location = new System.Drawing.Point(13, 64);
			this._playBetaButton.Name = "_playBetaButton";
			this._playBetaButton.Size = new System.Drawing.Size(135, 35);
			this._playBetaButton.TabIndex = 14;
			this._playBetaButton.Text = "Play Beta";
			this._playBetaButton.UseVisualStyleBackColor = true;
			this._playBetaButton.Click += new System.EventHandler(this._playBetaButton_Click);
			// 
			// _playOnlineButton
			// 
			this._playOnlineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._playOnlineButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._playOnlineButton.Location = new System.Drawing.Point(13, 23);
			this._playOnlineButton.Name = "_playOnlineButton";
			this._playOnlineButton.Size = new System.Drawing.Size(135, 35);
			this._playOnlineButton.TabIndex = 13;
			this._playOnlineButton.Text = "Play Online";
			this._playOnlineButton.UseVisualStyleBackColor = true;
			this._playOnlineButton.Click += new System.EventHandler(this._playOnlineButton_Click);
			// 
			// _loginComboBox
			// 
			this._loginComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._loginComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this._loginComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._loginComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._loginComboBox.FormattingEnabled = true;
			this._loginComboBox.Location = new System.Drawing.Point(76, 90);
			this._loginComboBox.Name = "_loginComboBox";
			this._loginComboBox.Size = new System.Drawing.Size(150, 21);
			this._loginComboBox.Sorted = true;
			this._loginComboBox.TabIndex = 12;
			this._loginComboBox.Validating += new System.ComponentModel.CancelEventHandler(this._loginComboBox_Validating);
			// 
			// _loginAsLabel
			// 
			this._loginAsLabel.AutoSize = true;
			this._loginAsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._loginAsLabel.Location = new System.Drawing.Point(11, 93);
			this._loginAsLabel.Name = "_loginAsLabel";
			this._loginAsLabel.Size = new System.Drawing.Size(60, 13);
			this._loginAsLabel.TabIndex = 11;
			this._loginAsLabel.Text = "Sign in as:";
			// 
			// _callsignGroupbox
			// 
			this._callsignGroupbox.BackColor = System.Drawing.SystemColors.Control;
			this._callsignGroupbox.Controls.Add(this._learnMoreLinkLabel);
			this._callsignGroupbox.Controls.Add(this.manageCallsignsButton);
			this._callsignGroupbox.Controls.Add(this.label4);
			this._callsignGroupbox.Controls.Add(this.label3);
			this._callsignGroupbox.Controls.Add(this._loginAsLabel);
			this._callsignGroupbox.Controls.Add(this._loginComboBox);
			this._callsignGroupbox.Controls.Add(this.label9);
			this._callsignGroupbox.Dock = System.Windows.Forms.DockStyle.Top;
			this._callsignGroupbox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._callsignGroupbox.Location = new System.Drawing.Point(5, 10);
			this._callsignGroupbox.Name = "_callsignGroupbox";
			this._callsignGroupbox.Size = new System.Drawing.Size(390, 124);
			this._callsignGroupbox.TabIndex = 16;
			this._callsignGroupbox.TabStop = false;
			this._callsignGroupbox.Text = "Callsign";
			// 
			// _learnMoreLinkLabel
			// 
			this._learnMoreLinkLabel.AutoSize = true;
			this._learnMoreLinkLabel.Location = new System.Drawing.Point(141, 55);
			this._learnMoreLinkLabel.Name = "_learnMoreLinkLabel";
			this._learnMoreLinkLabel.Size = new System.Drawing.Size(67, 13);
			this._learnMoreLinkLabel.TabIndex = 22;
			this._learnMoreLinkLabel.TabStop = true;
			this._learnMoreLinkLabel.Text = "Learn more.";
			this._learnMoreLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.learnMoreLinkLabel_LinkClicked);
			// 
			// manageCallsignsButton
			// 
			this.manageCallsignsButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.manageCallsignsButton.Location = new System.Drawing.Point(232, 89);
			this.manageCallsignsButton.Name = "manageCallsignsButton";
			this.manageCallsignsButton.Size = new System.Drawing.Size(130, 23);
			this.manageCallsignsButton.TabIndex = 21;
			this.manageCallsignsButton.Text = "Manage Callsigns";
			this.manageCallsignsButton.UseVisualStyleBackColor = true;
			this.manageCallsignsButton.Click += new System.EventHandler(this.manageCallsignsButton_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(11, 35);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(0, 13);
			this.label4.TabIndex = 19;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 18);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(191, 13);
			this.label3.TabIndex = 18;
			this.label3.Text = "Choose your callsign to log in with.";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 31);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(370, 50);
			this.label9.TabIndex = 23;
			this.label9.Text = "You can choose between a number of callsigns you own to login with.";
			// 
			// _playmodeGroupbox
			// 
			this._playmodeGroupbox.Controls.Add(this.label8);
			this._playmodeGroupbox.Controls.Add(this.label6);
			this._playmodeGroupbox.Controls.Add(this.label1);
			this._playmodeGroupbox.Controls.Add(this._playOnlineButton);
			this._playmodeGroupbox.Controls.Add(this._playBetaButton);
			this._playmodeGroupbox.Controls.Add(this._playOfflineButton);
			this._playmodeGroupbox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._playmodeGroupbox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._playmodeGroupbox.Location = new System.Drawing.Point(5, 284);
			this._playmodeGroupbox.Name = "_playmodeGroupbox";
			this._playmodeGroupbox.Size = new System.Drawing.Size(390, 146);
			this._playmodeGroupbox.TabIndex = 17;
			this._playmodeGroupbox.TabStop = false;
			this._playmodeGroupbox.Text = "Play Mode";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(160, 118);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(191, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "Play tutorial missions or LAN games.";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(160, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(211, 13);
			this.label6.TabIndex = 18;
			this.label6.Text = "Help test the next version of Allegiance.";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(160, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(153, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Fly with other pilots on-line!";
			// 
			// playControlErrorProvider
			// 
			this.playControlErrorProvider.ContainerControl = this;
			// 
			// _allegianceRunningGroupBox
			// 
			this._allegianceRunningGroupBox.Controls.Add(this.tableLayoutPanel1);
			this._allegianceRunningGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this._allegianceRunningGroupBox.Location = new System.Drawing.Point(5, 134);
			this._allegianceRunningGroupBox.Name = "_allegianceRunningGroupBox";
			this._allegianceRunningGroupBox.Size = new System.Drawing.Size(390, 146);
			this._allegianceRunningGroupBox.TabIndex = 18;
			this._allegianceRunningGroupBox.TabStop = false;
			this._allegianceRunningGroupBox.Text = "Allegiance Status";
			this._allegianceRunningGroupBox.Visible = false;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
			this.tableLayoutPanel1.Controls.Add(this._exitAllegianceButton, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 127);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _exitAllegianceButton
			// 
			this._exitAllegianceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._exitAllegianceButton.Location = new System.Drawing.Point(124, 14);
			this._exitAllegianceButton.Margin = new System.Windows.Forms.Padding(0);
			this._exitAllegianceButton.MaximumSize = new System.Drawing.Size(135, 35);
			this._exitAllegianceButton.MinimumSize = new System.Drawing.Size(135, 35);
			this._exitAllegianceButton.Name = "_exitAllegianceButton";
			this._exitAllegianceButton.Size = new System.Drawing.Size(135, 35);
			this._exitAllegianceButton.TabIndex = 1;
			this._exitAllegianceButton.Text = "&Logout";
			this._exitAllegianceButton.UseVisualStyleBackColor = true;
			this._exitAllegianceButton.Click += new System.EventHandler(this._exitAllegianceButton_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(124, 83);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.label2.Size = new System.Drawing.Size(135, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "Allegiance Status: Active";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// PlayControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this._allegianceRunningGroupBox);
			this.Controls.Add(this._playmodeGroupbox);
			this.Controls.Add(this._callsignGroupbox);
			this.Name = "PlayControl";
			this.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
			this.Size = new System.Drawing.Size(400, 440);
			this._callsignGroupbox.ResumeLayout(false);
			this._callsignGroupbox.PerformLayout();
			this._playmodeGroupbox.ResumeLayout(false);
			this._playmodeGroupbox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.playControlErrorProvider)).EndInit();
			this._allegianceRunningGroupBox.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _playOfflineButton;
        private System.Windows.Forms.Button _playBetaButton;
        private System.Windows.Forms.Button _playOnlineButton;
        private System.Windows.Forms.ComboBox _loginComboBox;
        private System.Windows.Forms.Label _loginAsLabel;
        private System.Windows.Forms.GroupBox _callsignGroupbox;
		private System.Windows.Forms.GroupBox _playmodeGroupbox;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button manageCallsignsButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.LinkLabel _learnMoreLinkLabel;
		private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ErrorProvider playControlErrorProvider;
		private System.Windows.Forms.GroupBox _allegianceRunningGroupBox;
		private System.Windows.Forms.Button _exitAllegianceButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
