namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    partial class MessageSingleControl
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
			this._closeButton = new System.Windows.Forms.Button();
			this._previousButton = new System.Windows.Forms.Button();
			this._nextButton = new System.Windows.Forms.Button();
			this._deleteButton = new System.Windows.Forms.Button();
			this._dateSentLabel = new System.Windows.Forms.Label();
			this._fromLabel = new System.Windows.Forms.Label();
			this._messageBodyTextBox = new System.Windows.Forms.TextBox();
			this._titleLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _closeButton
			// 
			this._closeButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._closeButton.Location = new System.Drawing.Point(99, 274);
			this._closeButton.Name = "_closeButton";
			this._closeButton.Size = new System.Drawing.Size(63, 23);
			this._closeButton.TabIndex = 15;
			this._closeButton.Text = "Close";
			this._closeButton.UseVisualStyleBackColor = true;
			// 
			// _previousButton
			// 
			this._previousButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._previousButton.Location = new System.Drawing.Point(291, 274);
			this._previousButton.Name = "_previousButton";
			this._previousButton.Size = new System.Drawing.Size(50, 23);
			this._previousButton.TabIndex = 14;
			this._previousButton.Text = "Prev";
			this._previousButton.UseVisualStyleBackColor = true;
			// 
			// _nextButton
			// 
			this._nextButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._nextButton.Location = new System.Drawing.Point(347, 274);
			this._nextButton.Name = "_nextButton";
			this._nextButton.Size = new System.Drawing.Size(50, 23);
			this._nextButton.TabIndex = 13;
			this._nextButton.Text = "Next";
			this._nextButton.UseVisualStyleBackColor = true;
			// 
			// _deleteButton
			// 
			this._deleteButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._deleteButton.Location = new System.Drawing.Point(3, 274);
			this._deleteButton.Name = "_deleteButton";
			this._deleteButton.Size = new System.Drawing.Size(75, 23);
			this._deleteButton.TabIndex = 12;
			this._deleteButton.Text = "Delete";
			this._deleteButton.UseVisualStyleBackColor = true;
			// 
			// _dateSentLabel
			// 
			this._dateSentLabel.AutoSize = true;
			this._dateSentLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._dateSentLabel.Location = new System.Drawing.Point(249, 33);
			this._dateSentLabel.Name = "_dateSentLabel";
			this._dateSentLabel.Size = new System.Drawing.Size(57, 13);
			this._dateSentLabel.TabIndex = 11;
			this._dateSentLabel.Text = "Date Sent";
			// 
			// _fromLabel
			// 
			this._fromLabel.AutoSize = true;
			this._fromLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._fromLabel.Location = new System.Drawing.Point(2, 33);
			this._fromLabel.Name = "_fromLabel";
			this._fromLabel.Size = new System.Drawing.Size(63, 13);
			this._fromLabel.TabIndex = 10;
			this._fromLabel.Text = "From Label";
			// 
			// _messageBodyTextBox
			// 
			this._messageBodyTextBox.Location = new System.Drawing.Point(3, 49);
			this._messageBodyTextBox.Multiline = true;
			this._messageBodyTextBox.Name = "_messageBodyTextBox";
			this._messageBodyTextBox.ReadOnly = true;
			this._messageBodyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._messageBodyTextBox.Size = new System.Drawing.Size(394, 219);
			this._messageBodyTextBox.TabIndex = 9;
			// 
			// _titleLabel
			// 
			this._titleLabel.AutoSize = true;
			this._titleLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._titleLabel.Location = new System.Drawing.Point(1, 7);
			this._titleLabel.Name = "_titleLabel";
			this._titleLabel.Size = new System.Drawing.Size(50, 25);
			this._titleLabel.TabIndex = 8;
			this._titleLabel.Text = "Title";
			// 
			// MessageSingleControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this._closeButton);
			this.Controls.Add(this._previousButton);
			this.Controls.Add(this._nextButton);
			this.Controls.Add(this._deleteButton);
			this.Controls.Add(this._dateSentLabel);
			this.Controls.Add(this._fromLabel);
			this.Controls.Add(this._messageBodyTextBox);
			this.Controls.Add(this._titleLabel);
			this.Name = "MessageSingleControl";
			this.Size = new System.Drawing.Size(400, 300);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _previousButton;
        private System.Windows.Forms.Button _nextButton;
        private System.Windows.Forms.Button _deleteButton;
        private System.Windows.Forms.Label _dateSentLabel;
        private System.Windows.Forms.Label _fromLabel;
        private System.Windows.Forms.TextBox _messageBodyTextBox;
        private System.Windows.Forms.Label _titleLabel;
    }
}
