namespace Allegiance.CommunitySecuritySystem.Client
{
    partial class DiagnosticsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagnosticsForm));
			this._winVerLabel = new System.Windows.Forms.Label();
			this._winVerTextBox = new System.Windows.Forms.TextBox();
			this._procTextBox = new System.Windows.Forms.TextBox();
			this._procLabel = new System.Windows.Forms.Label();
			this._ramTextBox = new System.Windows.Forms.TextBox();
			this._ramLabel = new System.Windows.Forms.Label();
			this._connectionLabel = new System.Windows.Forms.Label();
			this._modemTextBox = new System.Windows.Forms.TextBox();
			this._modemLabel = new System.Windows.Forms.Label();
			this._routerTextBox = new System.Windows.Forms.TextBox();
			this._routerLabel = new System.Windows.Forms.Label();
			this._ispTextBox = new System.Windows.Forms.TextBox();
			this._ispLabel = new System.Windows.Forms.Label();
			this._antiVirusTextBox = new System.Windows.Forms.TextBox();
			this._antiVirusLabel = new System.Windows.Forms.Label();
			this._dxDiagTextBox = new System.Windows.Forms.TextBox();
			this._dxDiagLabel = new System.Windows.Forms.Label();
			this._copyToClipboardButton = new System.Windows.Forms.Button();
			this._connectionComboBox = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this._dxDiagThrobber = new Allegiance.CommunitySecuritySystem.Client.Controls.AnimatedThrobberControl.AnimatedThrobber();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _winVerLabel
			// 
			this._winVerLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._winVerLabel.Location = new System.Drawing.Point(12, 91);
			this._winVerLabel.Name = "_winVerLabel";
			this._winVerLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._winVerLabel.Size = new System.Drawing.Size(140, 13);
			this._winVerLabel.TabIndex = 0;
			this._winVerLabel.Text = "Windows Version";
			// 
			// _winVerTextBox
			// 
			this._winVerTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._winVerTextBox.Location = new System.Drawing.Point(155, 88);
			this._winVerTextBox.Name = "_winVerTextBox";
			this._winVerTextBox.ReadOnly = true;
			this._winVerTextBox.Size = new System.Drawing.Size(383, 22);
			this._winVerTextBox.TabIndex = 1;
			// 
			// _procTextBox
			// 
			this._procTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._procTextBox.Location = new System.Drawing.Point(155, 113);
			this._procTextBox.Name = "_procTextBox";
			this._procTextBox.ReadOnly = true;
			this._procTextBox.Size = new System.Drawing.Size(383, 22);
			this._procTextBox.TabIndex = 3;
			// 
			// _procLabel
			// 
			this._procLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._procLabel.Location = new System.Drawing.Point(12, 116);
			this._procLabel.Name = "_procLabel";
			this._procLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._procLabel.Size = new System.Drawing.Size(140, 13);
			this._procLabel.TabIndex = 2;
			this._procLabel.Text = "Processor";
			// 
			// _ramTextBox
			// 
			this._ramTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._ramTextBox.Location = new System.Drawing.Point(155, 139);
			this._ramTextBox.Name = "_ramTextBox";
			this._ramTextBox.ReadOnly = true;
			this._ramTextBox.Size = new System.Drawing.Size(383, 22);
			this._ramTextBox.TabIndex = 5;
			// 
			// _ramLabel
			// 
			this._ramLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._ramLabel.Location = new System.Drawing.Point(12, 142);
			this._ramLabel.Name = "_ramLabel";
			this._ramLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._ramLabel.Size = new System.Drawing.Size(140, 13);
			this._ramLabel.TabIndex = 4;
			this._ramLabel.Text = "RAM";
			// 
			// _connectionLabel
			// 
			this._connectionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._connectionLabel.Location = new System.Drawing.Point(12, 171);
			this._connectionLabel.Name = "_connectionLabel";
			this._connectionLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._connectionLabel.Size = new System.Drawing.Size(140, 13);
			this._connectionLabel.TabIndex = 10;
			this._connectionLabel.Text = "Connection Type";
			// 
			// _modemTextBox
			// 
			this._modemTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._modemTextBox.Location = new System.Drawing.Point(155, 194);
			this._modemTextBox.Name = "_modemTextBox";
			this._modemTextBox.Size = new System.Drawing.Size(383, 22);
			this._modemTextBox.TabIndex = 13;
			this._modemTextBox.Text = "Modem make and model number";
			// 
			// _modemLabel
			// 
			this._modemLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._modemLabel.Location = new System.Drawing.Point(12, 197);
			this._modemLabel.Name = "_modemLabel";
			this._modemLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._modemLabel.Size = new System.Drawing.Size(140, 13);
			this._modemLabel.TabIndex = 12;
			this._modemLabel.Text = "Modem";
			// 
			// _routerTextBox
			// 
			this._routerTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._routerTextBox.Location = new System.Drawing.Point(155, 220);
			this._routerTextBox.Name = "_routerTextBox";
			this._routerTextBox.Size = new System.Drawing.Size(383, 22);
			this._routerTextBox.TabIndex = 15;
			this._routerTextBox.Text = "Router make and model number";
			// 
			// _routerLabel
			// 
			this._routerLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._routerLabel.Location = new System.Drawing.Point(12, 223);
			this._routerLabel.Name = "_routerLabel";
			this._routerLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._routerLabel.Size = new System.Drawing.Size(140, 13);
			this._routerLabel.TabIndex = 14;
			this._routerLabel.Text = "Router";
			// 
			// _ispTextBox
			// 
			this._ispTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._ispTextBox.Location = new System.Drawing.Point(155, 246);
			this._ispTextBox.Name = "_ispTextBox";
			this._ispTextBox.Size = new System.Drawing.Size(383, 22);
			this._ispTextBox.TabIndex = 17;
			this._ispTextBox.Text = "Country and ISP";
			// 
			// _ispLabel
			// 
			this._ispLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._ispLabel.Location = new System.Drawing.Point(12, 249);
			this._ispLabel.Name = "_ispLabel";
			this._ispLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._ispLabel.Size = new System.Drawing.Size(140, 13);
			this._ispLabel.TabIndex = 16;
			this._ispLabel.Text = "Internet Service Provider";
			// 
			// _antiVirusTextBox
			// 
			this._antiVirusTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._antiVirusTextBox.Location = new System.Drawing.Point(155, 272);
			this._antiVirusTextBox.Name = "_antiVirusTextBox";
			this._antiVirusTextBox.Size = new System.Drawing.Size(383, 22);
			this._antiVirusTextBox.TabIndex = 19;
			// 
			// _antiVirusLabel
			// 
			this._antiVirusLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._antiVirusLabel.Location = new System.Drawing.Point(12, 275);
			this._antiVirusLabel.Name = "_antiVirusLabel";
			this._antiVirusLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._antiVirusLabel.Size = new System.Drawing.Size(140, 13);
			this._antiVirusLabel.TabIndex = 18;
			this._antiVirusLabel.Text = "AntiVirus";
			// 
			// _dxDiagTextBox
			// 
			this._dxDiagTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._dxDiagTextBox.Location = new System.Drawing.Point(155, 300);
			this._dxDiagTextBox.Multiline = true;
			this._dxDiagTextBox.Name = "_dxDiagTextBox";
			this._dxDiagTextBox.ReadOnly = true;
			this._dxDiagTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._dxDiagTextBox.Size = new System.Drawing.Size(383, 139);
			this._dxDiagTextBox.TabIndex = 21;
			// 
			// _dxDiagLabel
			// 
			this._dxDiagLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._dxDiagLabel.Location = new System.Drawing.Point(12, 303);
			this._dxDiagLabel.Name = "_dxDiagLabel";
			this._dxDiagLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._dxDiagLabel.Size = new System.Drawing.Size(140, 13);
			this._dxDiagLabel.TabIndex = 20;
			this._dxDiagLabel.Text = "DxDiag";
			// 
			// _copyToClipboardButton
			// 
			this._copyToClipboardButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._copyToClipboardButton.Location = new System.Drawing.Point(460, 12);
			this._copyToClipboardButton.Name = "_copyToClipboardButton";
			this._copyToClipboardButton.Size = new System.Drawing.Size(112, 51);
			this._copyToClipboardButton.TabIndex = 22;
			this._copyToClipboardButton.Text = "Copy to Clipboard";
			this._copyToClipboardButton.UseVisualStyleBackColor = true;
			this._copyToClipboardButton.Click += new System.EventHandler(this._copyToClipboardButton_Click);
			// 
			// _connectionComboBox
			// 
			this._connectionComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._connectionComboBox.FormattingEnabled = true;
			this._connectionComboBox.Items.AddRange(new object[] {
            "Dialup",
            "Cable",
            "DSL",
            "Satellite",
            "Other",
            "Unknown"});
			this._connectionComboBox.Location = new System.Drawing.Point(155, 167);
			this._connectionComboBox.Name = "_connectionComboBox";
			this._connectionComboBox.Size = new System.Drawing.Size(383, 21);
			this._connectionComboBox.TabIndex = 23;
			this._connectionComboBox.Text = "Please select your connection type";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.linkLabel1);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(16, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(443, 76);
			this.panel1.TabIndex = 24;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(4, 55);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(109, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "to paste this report.";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(183, 38);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(247, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Describe your problem clearly and don\'t forget";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkLabel1.Location = new System.Drawing.Point(4, 38);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(183, 13);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "start a new thread in the Helpline.";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(4, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(410, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "If you are experiencing trouble, please use the \"Copy to clipboard\" button and";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(345, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "This is system information that can help you troubleshoot issues.";
			// 
			// _dxDiagThrobber
			// 
			this._dxDiagThrobber.InnerCircleRadius = 8;
			this._dxDiagThrobber.Location = new System.Drawing.Point(239, 321);
			this._dxDiagThrobber.Name = "_dxDiagThrobber";
			this._dxDiagThrobber.NumberOfSpoke = 10;
			this._dxDiagThrobber.OuterCircleRadius = 10;
			this._dxDiagThrobber.Size = new System.Drawing.Size(207, 91);
			this._dxDiagThrobber.SpokeThickness = 4;
			this._dxDiagThrobber.TabIndex = 25;
			this._dxDiagThrobber.TabStop = true;
			// 
			// DiagnosticsForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(584, 451);
			this.Controls.Add(this._dxDiagThrobber);
			this.Controls.Add(this._dxDiagTextBox);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._connectionComboBox);
			this.Controls.Add(this._copyToClipboardButton);
			this.Controls.Add(this._dxDiagLabel);
			this.Controls.Add(this._antiVirusTextBox);
			this.Controls.Add(this._antiVirusLabel);
			this.Controls.Add(this._ispTextBox);
			this.Controls.Add(this._ispLabel);
			this.Controls.Add(this._routerTextBox);
			this.Controls.Add(this._routerLabel);
			this.Controls.Add(this._modemTextBox);
			this.Controls.Add(this._modemLabel);
			this.Controls.Add(this._connectionLabel);
			this.Controls.Add(this._ramTextBox);
			this.Controls.Add(this._ramLabel);
			this.Controls.Add(this._procTextBox);
			this.Controls.Add(this._procLabel);
			this.Controls.Add(this._winVerTextBox);
			this.Controls.Add(this._winVerLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "DiagnosticsForm";
			this.Text = "System Information";
			this.Load += new System.EventHandler(this.DiagnosticsForm_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _winVerLabel;
        private System.Windows.Forms.TextBox _winVerTextBox;
        private System.Windows.Forms.TextBox _procTextBox;
        private System.Windows.Forms.Label _procLabel;
        private System.Windows.Forms.TextBox _ramTextBox;
		private System.Windows.Forms.Label _ramLabel;
        private System.Windows.Forms.Label _connectionLabel;
        private System.Windows.Forms.TextBox _modemTextBox;
        private System.Windows.Forms.Label _modemLabel;
        private System.Windows.Forms.TextBox _routerTextBox;
        private System.Windows.Forms.Label _routerLabel;
        private System.Windows.Forms.TextBox _ispTextBox;
        private System.Windows.Forms.Label _ispLabel;
        private System.Windows.Forms.TextBox _antiVirusTextBox;
        private System.Windows.Forms.Label _antiVirusLabel;
        private System.Windows.Forms.TextBox _dxDiagTextBox;
        private System.Windows.Forms.Label _dxDiagLabel;
        private System.Windows.Forms.Button _copyToClipboardButton;
        private System.Windows.Forms.ComboBox _connectionComboBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label4;
		private Controls.AnimatedThrobberControl.AnimatedThrobber _dxDiagThrobber;
    }
}