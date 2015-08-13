namespace Allegiance.CommunitySecuritySystem.Client
{
	partial class VirtualMachineInfo
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
			this.label1 = new System.Windows.Forms.Label();
			this.btnShowInfo = new System.Windows.Forms.LinkLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(10);
			this.label1.Size = new System.Drawing.Size(536, 56);
			this.label1.TabIndex = 0;
			this.label1.Text = "ACSS detected that you are trying to launch Allegiance under a Virtual Machine. Y" +
    "ou will need to first launch Allegiance from a physical machine before you can r" +
    "un Allegiance under a virtual instance.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnShowInfo
			// 
			this.btnShowInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnShowInfo.AutoSize = true;
			this.btnShowInfo.Location = new System.Drawing.Point(163, 91);
			this.btnShowInfo.Name = "btnShowInfo";
			this.btnShowInfo.Size = new System.Drawing.Size(178, 13);
			this.btnShowInfo.TabIndex = 1;
			this.btnShowInfo.TabStop = true;
			this.btnShowInfo.Text = "Get More Information From The Wiki";
			this.btnShowInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnShowInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnShowInfo_LinkClicked);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 168);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(536, 32);
			this.panel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnClose);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(448, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(88, 32);
			this.panel2.TabIndex = 0;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(4, 4);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// VirtualMachineInfo
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(536, 200);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnShowInfo);
			this.Controls.Add(this.label1);
			this.Name = "VirtualMachineInfo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Virtual Machine Detected";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel btnShowInfo;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnClose;
	}
}