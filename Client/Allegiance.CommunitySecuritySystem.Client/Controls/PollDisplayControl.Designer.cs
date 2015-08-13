namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    partial class PollDisplayControl
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
			this._voteBtn = new System.Windows.Forms.Button();
			this._remindLaterBtn = new System.Windows.Forms.Button();
			this._questionText = new System.Windows.Forms.Label();
			this._dateLabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this._pollHeaderLabel = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this._optionList = new Allegiance.CommunitySecuritySystem.Client.Controls.Custom.RadioListBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// _voteBtn
			// 
			this._voteBtn.Location = new System.Drawing.Point(116, 6);
			this._voteBtn.Name = "_voteBtn";
			this._voteBtn.Size = new System.Drawing.Size(75, 23);
			this._voteBtn.TabIndex = 10;
			this._voteBtn.Text = "Vote";
			this._voteBtn.UseVisualStyleBackColor = true;
			this._voteBtn.Click += new System.EventHandler(this._voteBtn_Click);
			// 
			// _remindLaterBtn
			// 
			this._remindLaterBtn.Location = new System.Drawing.Point(3, 6);
			this._remindLaterBtn.Name = "_remindLaterBtn";
			this._remindLaterBtn.Size = new System.Drawing.Size(107, 23);
			this._remindLaterBtn.TabIndex = 11;
			this._remindLaterBtn.Text = "Remind Me Later";
			this._remindLaterBtn.UseVisualStyleBackColor = true;
			this._remindLaterBtn.Click += new System.EventHandler(this._remindLaterBtn_Click);
			// 
			// _questionText
			// 
			this._questionText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._questionText.Dock = System.Windows.Forms.DockStyle.Top;
			this._questionText.Location = new System.Drawing.Point(0, 25);
			this._questionText.Name = "_questionText";
			this._questionText.Size = new System.Drawing.Size(308, 77);
			this._questionText.TabIndex = 13;
			this._questionText.Text = "_questionText";
			// 
			// _dateLabel
			// 
			this._dateLabel.AutoSize = true;
			this._dateLabel.Dock = System.Windows.Forms.DockStyle.Right;
			this._dateLabel.Location = new System.Drawing.Point(238, 0);
			this._dateLabel.Name = "_dateLabel";
			this._dateLabel.Size = new System.Drawing.Size(60, 13);
			this._dateLabel.TabIndex = 14;
			this._dateLabel.Text = "_dateLabel";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._pollHeaderLabel);
			this.panel1.Controls.Add(this._dateLabel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this.panel1.Size = new System.Drawing.Size(308, 25);
			this.panel1.TabIndex = 15;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 229);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(308, 34);
			this.panel2.TabIndex = 16;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this._remindLaterBtn);
			this.panel3.Controls.Add(this._voteBtn);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel3.Location = new System.Drawing.Point(113, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(195, 34);
			this.panel3.TabIndex = 0;
			// 
			// _pollHeaderLabel
			// 
			this._pollHeaderLabel.AutoSize = true;
			this._pollHeaderLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this._pollHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._pollHeaderLabel.Location = new System.Drawing.Point(0, 0);
			this._pollHeaderLabel.Name = "_pollHeaderLabel";
			this._pollHeaderLabel.Size = new System.Drawing.Size(106, 13);
			this._pollHeaderLabel.TabIndex = 15;
			this._pollHeaderLabel.Text = "_pollHeaderLabel";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 102);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(308, 3);
			this.splitter1.TabIndex = 17;
			this.splitter1.TabStop = false;
			// 
			// _optionList
			// 
			this._optionList.BackColor = System.Drawing.SystemColors.Window;
			this._optionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._optionList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this._optionList.FormattingEnabled = true;
			this._optionList.Location = new System.Drawing.Point(0, 105);
			this._optionList.Name = "_optionList";
			this._optionList.Size = new System.Drawing.Size(308, 124);
			this._optionList.TabIndex = 12;
			// 
			// PollDisplayControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._optionList);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this._questionText);
			this.Controls.Add(this.panel1);
			this.Name = "PollDisplayControl";
			this.Size = new System.Drawing.Size(308, 263);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _voteBtn;
        private System.Windows.Forms.Button _remindLaterBtn;
        private Allegiance.CommunitySecuritySystem.Client.Controls.Custom.RadioListBox _optionList;
        private System.Windows.Forms.Label _questionText;
        private System.Windows.Forms.Label _dateLabel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label _pollHeaderLabel;
		private System.Windows.Forms.Splitter splitter1;
    }
}
