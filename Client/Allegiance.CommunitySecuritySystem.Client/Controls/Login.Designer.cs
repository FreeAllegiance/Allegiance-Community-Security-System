namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
	partial class Login
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._usernameLabel = new System.Windows.Forms.Label();
			this._usernameTextBox = new System.Windows.Forms.TextBox();
			this._passwordLabel = new System.Windows.Forms.Label();
			this._passwordTextBox = new System.Windows.Forms.TextBox();
			this._loginBlurb = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this._playOfflineButton = new System.Windows.Forms.Button();
			this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnForgotPassword = new System.Windows.Forms.Button();
			this._loginButton = new System.Windows.Forms.Button();
			this._registerButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
			this.panel3.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.tableLayoutPanel1);
			this.groupBox1.Controls.Add(this._loginBlurb);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(605, 91);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Security System Login";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.02479F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.97521F));
			this.tableLayoutPanel1.Controls.Add(this._usernameLabel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._usernameTextBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._passwordLabel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._passwordTextBox, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 36);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(599, 52);
			this.tableLayoutPanel1.TabIndex = 33;
			// 
			// _usernameLabel
			// 
			this._usernameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._usernameLabel.AutoSize = true;
			this._usernameLabel.Location = new System.Drawing.Point(55, 6);
			this._usernameLabel.Name = "_usernameLabel";
			this._usernameLabel.Size = new System.Drawing.Size(43, 13);
			this._usernameLabel.TabIndex = 22;
			this._usernameLabel.Text = "Callsign";
			// 
			// _usernameTextBox
			// 
			this._usernameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._usernameTextBox.Location = new System.Drawing.Point(104, 3);
			this._usernameTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
			this._usernameTextBox.Name = "_usernameTextBox";
			this._usernameTextBox.Size = new System.Drawing.Size(455, 20);
			this._usernameTextBox.TabIndex = 1;
			this._usernameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			// 
			// _passwordLabel
			// 
			this._passwordLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._passwordLabel.AutoSize = true;
			this._passwordLabel.Location = new System.Drawing.Point(45, 32);
			this._passwordLabel.Name = "_passwordLabel";
			this._passwordLabel.Size = new System.Drawing.Size(53, 13);
			this._passwordLabel.TabIndex = 23;
			this._passwordLabel.Text = "Password";
			// 
			// _passwordTextBox
			// 
			this._passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._passwordTextBox.Location = new System.Drawing.Point(104, 29);
			this._passwordTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
			this._passwordTextBox.Name = "_passwordTextBox";
			this._passwordTextBox.Size = new System.Drawing.Size(455, 20);
			this._passwordTextBox.TabIndex = 2;
			this._passwordTextBox.UseSystemPasswordChar = true;
			this._passwordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
			// 
			// _loginBlurb
			// 
			this._loginBlurb.Dock = System.Windows.Forms.DockStyle.Top;
			this._loginBlurb.Location = new System.Drawing.Point(3, 16);
			this._loginBlurb.Name = "_loginBlurb";
			this._loginBlurb.Size = new System.Drawing.Size(599, 20);
			this._loginBlurb.TabIndex = 32;
			this._loginBlurb.Text = "Please Login or create a New Account to play online.";
			this._loginBlurb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.Controls.Add(this.tableLayoutPanel2);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(3, 139);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(605, 49);
			this.groupBox2.TabIndex = 35;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Training missions and LAN games";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this._playOfflineButton, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(599, 30);
			this.tableLayoutPanel2.TabIndex = 37;
			// 
			// _playOfflineButton
			// 
			this._playOfflineButton.AutoSize = true;
			this._playOfflineButton.CausesValidation = false;
			this._playOfflineButton.Location = new System.Drawing.Point(235, 3);
			this._playOfflineButton.MinimumSize = new System.Drawing.Size(128, 0);
			this._playOfflineButton.Name = "_playOfflineButton";
			this._playOfflineButton.Size = new System.Drawing.Size(128, 23);
			this._playOfflineButton.TabIndex = 7;
			this._playOfflineButton.Text = "Play Offline";
			this._playOfflineButton.UseVisualStyleBackColor = true;
			this._playOfflineButton.Click += new System.EventHandler(this._playOfflineButton_Click);
			// 
			// _errorProvider
			// 
			this._errorProvider.ContainerControl = this;
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 124);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(605, 15);
			this.panel1.TabIndex = 36;
			// 
			// panel3
			// 
			this.panel3.AutoSize = true;
			this.panel3.Controls.Add(this.tableLayoutPanel3);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 94);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(605, 30);
			this.panel3.TabIndex = 37;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Controls.Add(this.panel2, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(605, 30);
			this.tableLayoutPanel3.TabIndex = 38;
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.Controls.Add(this.btnForgotPassword);
			this.panel2.Controls.Add(this._loginButton);
			this.panel2.Controls.Add(this._registerButton);
			this.panel2.Location = new System.Drawing.Point(111, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(383, 29);
			this.panel2.TabIndex = 24;
			// 
			// btnForgotPassword
			// 
			this.btnForgotPassword.AutoSize = true;
			this.btnForgotPassword.CausesValidation = false;
			this.btnForgotPassword.Location = new System.Drawing.Point(260, 3);
			this.btnForgotPassword.MinimumSize = new System.Drawing.Size(120, 0);
			this.btnForgotPassword.Name = "btnForgotPassword";
			this.btnForgotPassword.Size = new System.Drawing.Size(120, 23);
			this.btnForgotPassword.TabIndex = 7;
			this.btnForgotPassword.Text = "&Forgot Password";
			this.btnForgotPassword.UseVisualStyleBackColor = true;
			this.btnForgotPassword.Click += new System.EventHandler(this.btnForgotPassword_Click);
			// 
			// _loginButton
			// 
			this._loginButton.AutoSize = true;
			this._loginButton.Location = new System.Drawing.Point(0, 3);
			this._loginButton.MinimumSize = new System.Drawing.Size(120, 0);
			this._loginButton.Name = "_loginButton";
			this._loginButton.Size = new System.Drawing.Size(128, 23);
			this._loginButton.TabIndex = 3;
			this._loginButton.Text = "&Login";
			this._loginButton.UseVisualStyleBackColor = true;
			this._loginButton.Click += new System.EventHandler(this._loginButton_Click);
			// 
			// _registerButton
			// 
			this._registerButton.AutoSize = true;
			this._registerButton.CausesValidation = false;
			this._registerButton.Location = new System.Drawing.Point(134, 3);
			this._registerButton.MinimumSize = new System.Drawing.Size(120, 0);
			this._registerButton.Name = "_registerButton";
			this._registerButton.Size = new System.Drawing.Size(120, 23);
			this._registerButton.TabIndex = 6;
			this._registerButton.Text = "&New Account";
			this._registerButton.UseVisualStyleBackColor = true;
			this._registerButton.Click += new System.EventHandler(this._registerButton_Click);
			// 
			// Login
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.groupBox1);
			this.Name = "Login";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.Size = new System.Drawing.Size(611, 404);
			this.Load += new System.EventHandler(this.Login_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
			this.panel3.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label _loginBlurb;
		private System.Windows.Forms.Label _passwordLabel;
		private System.Windows.Forms.Label _usernameLabel;
		private System.Windows.Forms.TextBox _passwordTextBox;
		private System.Windows.Forms.TextBox _usernameTextBox;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button _playOfflineButton;
		private System.Windows.Forms.ErrorProvider _errorProvider;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnForgotPassword;
		private System.Windows.Forms.Button _loginButton;
		private System.Windows.Forms.Button _registerButton;
	}
}
