namespace Allegiance.CommunitySecuritySystem.Client
{
	partial class CssDiagnosticsForm
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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this._diagnosticsOutputTextbox = new System.Windows.Forms.TextBox();
			this.pButtons = new System.Windows.Forms.Panel();
			this._copyToClipboardButton = new System.Windows.Forms.Button();
			this.pFloatRight = new System.Windows.Forms.Panel();
			this._closeButton = new System.Windows.Forms.Button();
			this.pButtons.SuspendLayout();
			this.pFloatRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 402);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(860, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// _diagnosticsOutputTextbox
			// 
			this._diagnosticsOutputTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._diagnosticsOutputTextbox.Location = new System.Drawing.Point(0, 0);
			this._diagnosticsOutputTextbox.Multiline = true;
			this._diagnosticsOutputTextbox.Name = "_diagnosticsOutputTextbox";
			this._diagnosticsOutputTextbox.ReadOnly = true;
			this._diagnosticsOutputTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._diagnosticsOutputTextbox.Size = new System.Drawing.Size(860, 374);
			this._diagnosticsOutputTextbox.TabIndex = 1;
			// 
			// pButtons
			// 
			this.pButtons.Controls.Add(this.pFloatRight);
			this.pButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pButtons.Location = new System.Drawing.Point(0, 374);
			this.pButtons.Name = "pButtons";
			this.pButtons.Size = new System.Drawing.Size(860, 28);
			this.pButtons.TabIndex = 2;
			// 
			// _copyToClipboardButton
			// 
			this._copyToClipboardButton.Location = new System.Drawing.Point(3, 3);
			this._copyToClipboardButton.Name = "_copyToClipboardButton";
			this._copyToClipboardButton.Size = new System.Drawing.Size(108, 23);
			this._copyToClipboardButton.TabIndex = 0;
			this._copyToClipboardButton.Text = "C&opy to Clipboard";
			this._copyToClipboardButton.UseVisualStyleBackColor = true;
			this._copyToClipboardButton.Click += new System.EventHandler(this._copyToClipboardButton_Click);
			// 
			// pFloatRight
			// 
			this.pFloatRight.Controls.Add(this._closeButton);
			this.pFloatRight.Controls.Add(this._copyToClipboardButton);
			this.pFloatRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pFloatRight.Location = new System.Drawing.Point(663, 0);
			this.pFloatRight.Name = "pFloatRight";
			this.pFloatRight.Size = new System.Drawing.Size(197, 28);
			this.pFloatRight.TabIndex = 0;
			// 
			// _closeButton
			// 
			this._closeButton.Location = new System.Drawing.Point(117, 3);
			this._closeButton.Name = "_closeButton";
			this._closeButton.Size = new System.Drawing.Size(75, 23);
			this._closeButton.TabIndex = 1;
			this._closeButton.Text = "&Close";
			this._closeButton.UseVisualStyleBackColor = true;
			this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
			// 
			// CssDiagnosticsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(860, 424);
			this.Controls.Add(this._diagnosticsOutputTextbox);
			this.Controls.Add(this.pButtons);
			this.Controls.Add(this.statusStrip1);
			this.Name = "CssDiagnosticsForm";
			this.Text = "ACSS Diagnostics Form";
			this.Load += new System.EventHandler(this.CssDiagnosticsForm_Load);
			this.pButtons.ResumeLayout(false);
			this.pFloatRight.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TextBox _diagnosticsOutputTextbox;
		private System.Windows.Forms.Panel pButtons;
		private System.Windows.Forms.Panel pFloatRight;
		private System.Windows.Forms.Button _closeButton;
		private System.Windows.Forms.Button _copyToClipboardButton;
	}
}