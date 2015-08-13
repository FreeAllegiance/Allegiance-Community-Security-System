namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    partial class MessageListControl
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "New Message",
            "Sender",
            "Date"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "New Poll",
            "Sender",
            "Date"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Old Message",
            "Sender",
            "Date"}, -1);
            this._messagesListView = new System.Windows.Forms.ListView();
            this._subjectColumnHeader = new System.Windows.Forms.ColumnHeader();
            this._senderColumnHeader = new System.Windows.Forms.ColumnHeader();
            this._dateColumnHeader = new System.Windows.Forms.ColumnHeader();
            this._openButton = new System.Windows.Forms.Button();
            this._deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _messagesListView
            // 
            this._messagesListView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this._messagesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._subjectColumnHeader,
            this._senderColumnHeader,
            this._dateColumnHeader});
            this._messagesListView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._messagesListView.FullRowSelect = true;
            this._messagesListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this._messagesListView.Location = new System.Drawing.Point(0, 2);
            this._messagesListView.Margin = new System.Windows.Forms.Padding(0);
            this._messagesListView.MultiSelect = false;
            this._messagesListView.Name = "_messagesListView";
            this._messagesListView.Size = new System.Drawing.Size(398, 265);
            this._messagesListView.TabIndex = 11;
            this._messagesListView.UseCompatibleStateImageBehavior = false;
            this._messagesListView.View = System.Windows.Forms.View.Details;
            this._messagesListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._messagesListView_MouseDoubleClick);
            // 
            // _subjectColumnHeader
            // 
            this._subjectColumnHeader.Text = "Subject";
            this._subjectColumnHeader.Width = 217;
            // 
            // _senderColumnHeader
            // 
            this._senderColumnHeader.Text = "Sender";
            this._senderColumnHeader.Width = 100;
            // 
            // _dateColumnHeader
            // 
            this._dateColumnHeader.Text = "Date";
            this._dateColumnHeader.Width = 73;
            // 
            // _openButton
            // 
            this._openButton.Enabled = false;
            this._openButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._openButton.Location = new System.Drawing.Point(318, 272);
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(75, 23);
            this._openButton.TabIndex = 10;
            this._openButton.Text = "Open";
            this._openButton.UseVisualStyleBackColor = true;
            this._openButton.Click += new System.EventHandler(this._openButton_Click);
            // 
            // _deleteButton
            // 
            this._deleteButton.Enabled = false;
            this._deleteButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._deleteButton.Location = new System.Drawing.Point(237, 272);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(75, 23);
            this._deleteButton.TabIndex = 9;
            this._deleteButton.Text = "Delete";
            this._deleteButton.UseVisualStyleBackColor = true;
            this._deleteButton.Click += new System.EventHandler(this._deleteButton_Click);
            // 
            // MessageListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._messagesListView);
            this.Controls.Add(this._openButton);
            this.Controls.Add(this._deleteButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MessageListControl";
            this.Size = new System.Drawing.Size(400, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _messagesListView;
        private System.Windows.Forms.ColumnHeader _dateColumnHeader;
        private System.Windows.Forms.ColumnHeader _subjectColumnHeader;
        private System.Windows.Forms.ColumnHeader _senderColumnHeader;
        private System.Windows.Forms.Button _openButton;
        private System.Windows.Forms.Button _deleteButton;
    }
}
