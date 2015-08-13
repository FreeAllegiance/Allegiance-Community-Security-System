namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
	partial class NewAccount
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this._forgotPasswordButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._createAccountButton = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.panel4 = new System.Windows.Forms.Panel();
			this._captchaPanel = new System.Windows.Forms.Panel();
			this._animatedThrobber = new Allegiance.CommunitySecuritySystem.Client.Controls.AnimatedThrobberControl.AnimatedThrobber();
			this._verificationCodePictureBox = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this._usernameTextBox = new System.Windows.Forms.TextBox();
			this._verificationCodeTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this._passwordTextBox = new System.Windows.Forms.TextBox();
			this._verifyPasswordTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this._emailTextBox = new System.Windows.Forms.TextBox();
			this._loginBlurb = new System.Windows.Forms.Label();
			this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this._captchaPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._verificationCodePictureBox)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Controls.Add(this.panel3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.tableLayoutPanel3);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.tableLayoutPanel1);
			this.groupBox1.Controls.Add(this._loginBlurb);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(585, 449);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Create New Account";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 321);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(579, 73);
			this.tableLayoutPanel2.TabIndex = 45;
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.Controls.Add(this._forgotPasswordButton);
			this.panel2.Controls.Add(this._cancelButton);
			this.panel2.Controls.Add(this._createAccountButton);
			this.panel2.Location = new System.Drawing.Point(82, 3);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(20);
			this.panel2.Size = new System.Drawing.Size(415, 66);
			this.panel2.TabIndex = 46;
			// 
			// _forgotPasswordButton
			// 
			this._forgotPasswordButton.AutoSize = true;
			this._forgotPasswordButton.Location = new System.Drawing.Point(146, 20);
			this._forgotPasswordButton.MinimumSize = new System.Drawing.Size(120, 0);
			this._forgotPasswordButton.Name = "_forgotPasswordButton";
			this._forgotPasswordButton.Size = new System.Drawing.Size(120, 23);
			this._forgotPasswordButton.TabIndex = 7;
			this._forgotPasswordButton.Text = "&Forgot Password";
			this._forgotPasswordButton.UseVisualStyleBackColor = true;
			this._forgotPasswordButton.Click += new System.EventHandler(this._forgotPasswordButton_Click);
			// 
			// _cancelButton
			// 
			this._cancelButton.AutoSize = true;
			this._cancelButton.CausesValidation = false;
			this._cancelButton.Location = new System.Drawing.Point(272, 20);
			this._cancelButton.MinimumSize = new System.Drawing.Size(120, 0);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(120, 23);
			this._cancelButton.TabIndex = 6;
			this._cancelButton.Text = "&Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
			// 
			// _createAccountButton
			// 
			this._createAccountButton.AutoSize = true;
			this._createAccountButton.Enabled = false;
			this._createAccountButton.Location = new System.Drawing.Point(20, 20);
			this._createAccountButton.MinimumSize = new System.Drawing.Size(120, 0);
			this._createAccountButton.Name = "_createAccountButton";
			this._createAccountButton.Size = new System.Drawing.Size(120, 23);
			this._createAccountButton.TabIndex = 5;
			this._createAccountButton.Text = "Create &Account";
			this._createAccountButton.UseVisualStyleBackColor = true;
			this._createAccountButton.Click += new System.EventHandler(this._createAccountButton_Click);
			// 
			// panel3
			// 
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 311);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(579, 10);
			this.panel3.TabIndex = 46;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Location = new System.Drawing.Point(3, 240);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(579, 71);
			this.label4.TabIndex = 40;
			this.label4.Text = "Enter this verification code to create a new account.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Controls.Add(this.panel4, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 178);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(579, 62);
			this.tableLayoutPanel3.TabIndex = 47;
			// 
			// panel4
			// 
			this.panel4.AutoSize = true;
			this.panel4.Controls.Add(this._captchaPanel);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(189, 3);
			this.panel4.MinimumSize = new System.Drawing.Size(200, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(200, 56);
			this.panel4.TabIndex = 46;
			// 
			// _captchaPanel
			// 
			this._captchaPanel.Controls.Add(this._animatedThrobber);
			this._captchaPanel.Controls.Add(this._verificationCodePictureBox);
			this._captchaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._captchaPanel.Location = new System.Drawing.Point(0, 0);
			this._captchaPanel.Name = "_captchaPanel";
			this._captchaPanel.Size = new System.Drawing.Size(200, 56);
			this._captchaPanel.TabIndex = 42;
			// 
			// _animatedThrobber
			// 
			this._animatedThrobber.Dock = System.Windows.Forms.DockStyle.Fill;
			this._animatedThrobber.InnerCircleRadius = 8;
			this._animatedThrobber.Location = new System.Drawing.Point(0, 0);
			this._animatedThrobber.Name = "_animatedThrobber";
			this._animatedThrobber.NumberOfSpoke = 10;
			this._animatedThrobber.OuterCircleRadius = 10;
			this._animatedThrobber.Size = new System.Drawing.Size(200, 56);
			this._animatedThrobber.SpokeThickness = 4;
			this._animatedThrobber.TabIndex = 41;
			this._animatedThrobber.TabStop = true;
			// 
			// _verificationCodePictureBox
			// 
			this._verificationCodePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._verificationCodePictureBox.Location = new System.Drawing.Point(0, 0);
			this._verificationCodePictureBox.Name = "_verificationCodePictureBox";
			this._verificationCodePictureBox.Size = new System.Drawing.Size(200, 56);
			this._verificationCodePictureBox.TabIndex = 39;
			this._verificationCodePictureBox.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 168);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(579, 10);
			this.panel1.TabIndex = 44;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.80656F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.19344F));
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label6, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._usernameTextBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._verificationCodeTextBox, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this._passwordTextBox, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._verifyPasswordTextBox, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this._emailTextBox, 1, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 38);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(579, 130);
			this.tableLayoutPanel1.TabIndex = 43;
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(114, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 13);
			this.label5.TabIndex = 22;
			this.label5.Text = "Callsign";
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(104, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 13);
			this.label6.TabIndex = 23;
			this.label6.Text = "Password";
			// 
			// _usernameTextBox
			// 
			this._usernameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._usernameTextBox.Location = new System.Drawing.Point(163, 3);
			this._usernameTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
			this._usernameTextBox.MaxLength = 17;
			this._usernameTextBox.Name = "_usernameTextBox";
			this._usernameTextBox.Size = new System.Drawing.Size(376, 20);
			this._usernameTextBox.TabIndex = 0;
			this._usernameTextBox.TextChanged += new System.EventHandler(this._usernameTextBox_TextChanged);
			this._usernameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			// 
			// _verificationCodeTextBox
			// 
			this._verificationCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._verificationCodeTextBox.Location = new System.Drawing.Point(163, 107);
			this._verificationCodeTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
			this._verificationCodeTextBox.Name = "_verificationCodeTextBox";
			this._verificationCodeTextBox.Size = new System.Drawing.Size(376, 20);
			this._verificationCodeTextBox.TabIndex = 4;
			this._verificationCodeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(154, 26);
			this.label3.TabIndex = 38;
			this.label3.Text = "Verification Code";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _passwordTextBox
			// 
			this._passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._passwordTextBox.Location = new System.Drawing.Point(163, 29);
			this._passwordTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
			this._passwordTextBox.Name = "_passwordTextBox";
			this._passwordTextBox.Size = new System.Drawing.Size(376, 20);
			this._passwordTextBox.TabIndex = 1;
			this._passwordTextBox.UseSystemPasswordChar = true;
			this._passwordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			// 
			// _verifyPasswordTextBox
			// 
			this._verifyPasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._verifyPasswordTextBox.Location = new System.Drawing.Point(163, 55);
			this._verifyPasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
			this._verifyPasswordTextBox.Name = "_verifyPasswordTextBox";
			this._verifyPasswordTextBox.Size = new System.Drawing.Size(376, 20);
			this._verifyPasswordTextBox.TabIndex = 2;
			this._verifyPasswordTextBox.UseSystemPasswordChar = true;
			this._verifyPasswordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 26);
			this.label1.TabIndex = 35;
			this.label1.Text = "Password (Verify)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(154, 26);
			this.label2.TabIndex = 36;
			this.label2.Text = "Email";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _emailTextBox
			// 
			this._emailTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._emailTextBox.Location = new System.Drawing.Point(163, 81);
			this._emailTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
			this._emailTextBox.Name = "_emailTextBox";
			this._emailTextBox.Size = new System.Drawing.Size(376, 20);
			this._emailTextBox.TabIndex = 3;
			this._emailTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			// 
			// _loginBlurb
			// 
			this._loginBlurb.Dock = System.Windows.Forms.DockStyle.Top;
			this._loginBlurb.Location = new System.Drawing.Point(3, 16);
			this._loginBlurb.Name = "_loginBlurb";
			this._loginBlurb.Size = new System.Drawing.Size(579, 22);
			this._loginBlurb.TabIndex = 32;
			this._loginBlurb.Text = "Please enter your account details to create a new Allegiance account.";
			// 
			// _errorProvider
			// 
			this._errorProvider.ContainerControl = this;
			// 
			// NewAccount
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.groupBox1);
			this.Name = "NewAccount";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.Size = new System.Drawing.Size(591, 455);
			this.Load += new System.EventHandler(this.NewAccount_Load);
			this.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.panel4.ResumeLayout(false);
			this._captchaPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._verificationCodePictureBox)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label _loginBlurb;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _createAccountButton;
		private System.Windows.Forms.TextBox _passwordTextBox;
		private System.Windows.Forms.TextBox _usernameTextBox;
		private System.Windows.Forms.TextBox _verifyPasswordTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox _emailTextBox;
		private System.Windows.Forms.ErrorProvider _errorProvider;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox _verificationCodeTextBox;
		private AnimatedThrobberControl.AnimatedThrobber _animatedThrobber;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button _forgotPasswordButton;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel _captchaPanel;
		private System.Windows.Forms.PictureBox _verificationCodePictureBox;
	}
}
