namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    partial class CreateCallsignControl
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
			this._cancelButton = new System.Windows.Forms.Button();
			this._createButton = new System.Windows.Forms.Button();
			this._newCallsignTextBox = new System.Windows.Forms.TextBox();
			this._newCallsignabel = new System.Windows.Forms.Label();
			this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this._legacyPasswordPanel = new System.Windows.Forms.Panel();
			this._asgsPasswordTextbox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this._legacyPasswordPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._cancelButton.Location = new System.Drawing.Point(87, 3);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 7;
			this._cancelButton.Text = "&Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _createButton
			// 
			this._createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._createButton.Enabled = false;
			this._createButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._createButton.Location = new System.Drawing.Point(8, 3);
			this._createButton.Name = "_createButton";
			this._createButton.Size = new System.Drawing.Size(75, 23);
			this._createButton.TabIndex = 6;
			this._createButton.Text = "C&reate";
			this._createButton.UseVisualStyleBackColor = true;
			this._createButton.Click += new System.EventHandler(this._createButton_Click);
			// 
			// _newCallsignTextBox
			// 
			this._newCallsignTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._newCallsignTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._newCallsignTextBox.Location = new System.Drawing.Point(113, 20);
			this._newCallsignTextBox.MaxLength = 17;
			this._newCallsignTextBox.Name = "_newCallsignTextBox";
			this._newCallsignTextBox.Size = new System.Drawing.Size(201, 22);
			this._newCallsignTextBox.TabIndex = 5;
			this._newCallsignTextBox.TextChanged += new System.EventHandler(this._newCallsignTextBox_TextChanged);
			// 
			// _newCallsignabel
			// 
			this._newCallsignabel.AutoSize = true;
			this._newCallsignabel.Dock = System.Windows.Forms.DockStyle.Left;
			this._newCallsignabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._newCallsignabel.Location = new System.Drawing.Point(20, 20);
			this._newCallsignabel.Name = "_newCallsignabel";
			this._newCallsignabel.Padding = new System.Windows.Forms.Padding(4, 4, 15, 4);
			this._newCallsignabel.Size = new System.Drawing.Size(93, 21);
			this._newCallsignabel.TabIndex = 4;
			this._newCallsignabel.Text = "New Callsign";
			// 
			// _errorProvider
			// 
			this._errorProvider.ContainerControl = this;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._legacyPasswordPanel);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.panel3);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(30, 30);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(340, 240);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Create Callsign";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 208);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(334, 29);
			this.panel1.TabIndex = 9;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this._createButton);
			this.panel2.Controls.Add(this._cancelButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(169, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(165, 29);
			this.panel2.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this._newCallsignTextBox);
			this.panel3.Controls.Add(this._newCallsignabel);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 16);
			this.panel3.Name = "panel3";
			this.panel3.Padding = new System.Windows.Forms.Padding(20);
			this.panel3.Size = new System.Drawing.Size(334, 60);
			this.panel3.TabIndex = 10;
			// 
			// _legacyPasswordPanel
			// 
			this._legacyPasswordPanel.Controls.Add(this._asgsPasswordTextbox);
			this._legacyPasswordPanel.Controls.Add(this.label1);
			this._legacyPasswordPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this._legacyPasswordPanel.Location = new System.Drawing.Point(3, 76);
			this._legacyPasswordPanel.Name = "_legacyPasswordPanel";
			this._legacyPasswordPanel.Padding = new System.Windows.Forms.Padding(20);
			this._legacyPasswordPanel.Size = new System.Drawing.Size(334, 60);
			this._legacyPasswordPanel.TabIndex = 11;
			this._legacyPasswordPanel.Visible = false;
			// 
			// _asgsPasswordTextbox
			// 
			this._asgsPasswordTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._asgsPasswordTextbox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._asgsPasswordTextbox.Location = new System.Drawing.Point(114, 20);
			this._asgsPasswordTextbox.MaxLength = 17;
			this._asgsPasswordTextbox.Name = "_asgsPasswordTextbox";
			this._asgsPasswordTextbox.Size = new System.Drawing.Size(200, 22);
			this._asgsPasswordTextbox.TabIndex = 5;
			this._asgsPasswordTextbox.UseSystemPasswordChar = true;
			this._asgsPasswordTextbox.TextChanged += new System.EventHandler(this._asgsPasswordTextbox_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(20, 20);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(4);
			this.label1.Size = new System.Drawing.Size(94, 21);
			this.label1.TabIndex = 4;
			this.label1.Text = "ASGS Password";
			// 
			// CreateCallsignControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.groupBox1);
			this.Name = "CreateCallsignControl";
			this.Padding = new System.Windows.Forms.Padding(30);
			this.Size = new System.Drawing.Size(400, 300);
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this._legacyPasswordPanel.ResumeLayout(false);
			this._legacyPasswordPanel.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _createButton;
        private System.Windows.Forms.TextBox _newCallsignTextBox;
		private System.Windows.Forms.Label _newCallsignabel;
		private System.Windows.Forms.ErrorProvider _errorProvider;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel _legacyPasswordPanel;
		private System.Windows.Forms.TextBox _asgsPasswordTextbox;
		private System.Windows.Forms.Label label1;
    }
}
