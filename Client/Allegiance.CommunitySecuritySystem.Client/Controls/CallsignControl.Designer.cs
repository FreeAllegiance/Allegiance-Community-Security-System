namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    partial class CallsignControl
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
			this._callsignListView = new System.Windows.Forms.ListView();
			this._defaultColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._callsignColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._setDefaultButton = new System.Windows.Forms.Button();
			this._createCallsignButton = new System.Windows.Forms.Button();
			this._aliasesRemainingLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _callsignListView
			// 
			this._callsignListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._defaultColumnHeader,
            this._callsignColumnHeader});
			this._callsignListView.Enabled = false;
			this._callsignListView.FullRowSelect = true;
			this._callsignListView.Location = new System.Drawing.Point(0, 0);
			this._callsignListView.MultiSelect = false;
			this._callsignListView.Name = "_callsignListView";
			this._callsignListView.Size = new System.Drawing.Size(397, 268);
			this._callsignListView.TabIndex = 12;
			this._callsignListView.UseCompatibleStateImageBehavior = false;
			this._callsignListView.View = System.Windows.Forms.View.Details;
			// 
			// _defaultColumnHeader
			// 
			this._defaultColumnHeader.Text = "";
			// 
			// _callsignColumnHeader
			// 
			this._callsignColumnHeader.Text = "Callsign";
			this._callsignColumnHeader.Width = 300;
			// 
			// _setDefaultButton
			// 
			this._setDefaultButton.Enabled = false;
			this._setDefaultButton.Location = new System.Drawing.Point(305, 274);
			this._setDefaultButton.Name = "_setDefaultButton";
			this._setDefaultButton.Size = new System.Drawing.Size(92, 23);
			this._setDefaultButton.TabIndex = 11;
			this._setDefaultButton.Text = "Set as Default";
			this._setDefaultButton.UseVisualStyleBackColor = true;
			this._setDefaultButton.Click += new System.EventHandler(this._setDefaultButton_Click);
			// 
			// _createCallsignButton
			// 
			this._createCallsignButton.Enabled = false;
			this._createCallsignButton.Location = new System.Drawing.Point(207, 274);
			this._createCallsignButton.Name = "_createCallsignButton";
			this._createCallsignButton.Size = new System.Drawing.Size(92, 23);
			this._createCallsignButton.TabIndex = 10;
			this._createCallsignButton.Text = "Create Callsign";
			this._createCallsignButton.UseVisualStyleBackColor = true;
			// 
			// _aliasesRemainingLabel
			// 
			this._aliasesRemainingLabel.AutoSize = true;
			this._aliasesRemainingLabel.Location = new System.Drawing.Point(3, 279);
			this._aliasesRemainingLabel.Name = "_aliasesRemainingLabel";
			this._aliasesRemainingLabel.Size = new System.Drawing.Size(128, 13);
			this._aliasesRemainingLabel.TabIndex = 13;
			this._aliasesRemainingLabel.Text = "_aliasesRemainingLabel";
			// 
			// CallsignControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this._aliasesRemainingLabel);
			this.Controls.Add(this._setDefaultButton);
			this.Controls.Add(this._createCallsignButton);
			this.Controls.Add(this._callsignListView);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "CallsignControl";
			this.Size = new System.Drawing.Size(400, 300);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView _callsignListView;
        private System.Windows.Forms.ColumnHeader _defaultColumnHeader;
		private System.Windows.Forms.ColumnHeader _callsignColumnHeader;
        private System.Windows.Forms.Button _setDefaultButton;
        private System.Windows.Forms.Button _createCallsignButton;
		private System.Windows.Forms.Label _aliasesRemainingLabel;
    }
}
