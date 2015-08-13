using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using System.Drawing;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    public partial class MessageListControl : UserControl
    {
		private int _newMessageCount = 0;

        #region Event Declarations

        public event EventHandler OpenClick
        {
            add
            {
                _openButton.Click               += value;
                _messagesListView.DoubleClick   += value;
            }
            remove
            {
                _openButton.Click               -= value;
                _messagesListView.DoubleClick   -= value;
            }
        }

		public event EventHandler MessageReceived;

        #endregion

        #region Constructors

        public MessageListControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void _deleteButton_Click(object sender, EventArgs e)
        {
            if (_messagesListView.SelectedItems.Count > 0)
            {
                var li      = _messagesListView.SelectedItems[0];

				if (li.Index >= _messagesListView.Items.Count - 1 && li.Index > 0)
					_messagesListView.Items[li.Index - 1].Selected = true;

				else if(_messagesListView.Items.Count > 1)
					_messagesListView.Items[li.Index + 1].Selected = true;

				if (li.Font.Bold == true)
				{
					_newMessageCount--;
					UpdateMessageCountTab();
				}

                var message = li.Tag as BaseMessage;

                //Remove message from datastore
                BaseMessage.Messages.Remove(message);
                BaseMessage.MessageStore.Save();

                //Remove item from list
                _messagesListView.Items.Remove(li);

				_messagesListView.Focus();
            }
        }

        private void _openButton_Click(object sender, EventArgs e)
        {
            if (_messagesListView.SelectedItems.Count > 0)
            {
                var li      = _messagesListView.SelectedItems[0];
                var message = li.Tag as BaseMessage;

				if (li.Font.Bold == true)
				{
					li.Font = new System.Drawing.Font(li.Font, FontStyle.Regular);
					_newMessageCount--;
					UpdateMessageCountTab();
				}

                //Display message in full form
                MainForm.ViewMessageControl.FillForm(message);
            }
        }

        private void _messagesListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _openButton_Click(this, EventArgs.Empty);
        }

        void ViewMessageControl_PrevClick(object sender, EventArgs e)
        {
            var message = MainForm.ViewMessageControl.Tag as BaseMessage;
            var item    = FindElement(message);

            if (item.Index > 0)
            {
                var prevItem = _messagesListView.Items[item.Index - 1];
                prevItem.Selected = true;

				if (prevItem.Font.Bold == true)
				{
					prevItem.Font = new System.Drawing.Font(prevItem.Font, FontStyle.Regular);
					_newMessageCount--;
					UpdateMessageCountTab();
				}

                MainForm.ViewMessageControl.FillForm(prevItem.Tag as BaseMessage);
            }
        }

        void ViewMessageControl_NextClick(object sender, EventArgs e)
        {
            var message = MainForm.ViewMessageControl.Tag as BaseMessage;
            var item    = FindElement(message);

            if (item.Index < _messagesListView.Items.Count - 1)
            {
                var nextItem = _messagesListView.Items[item.Index + 1];
                nextItem.Selected = true;

				if (nextItem.Font.Bold == true)
				{
					nextItem.Font = new System.Drawing.Font(nextItem.Font, FontStyle.Regular);
					_newMessageCount--;
					UpdateMessageCountTab();
				}

                MainForm.ViewMessageControl.FillForm(nextItem.Tag as BaseMessage);
            }
        }

        void ViewMessageControl_DeleteClick(object sender, EventArgs e)
        {
            var message     = MainForm.ViewMessageControl.Tag as BaseMessage;
            var toRemove    = FindElement(message);

            if (toRemove != null)
            {
                toRemove.Selected = true;
                _deleteButton_Click(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Methods

        private ListViewItem FindElement(BaseMessage message)
        {
            ListViewItem toRemove = null;
            foreach (ListViewItem li in _messagesListView.Items)
            {
                if (li.Tag == message)
                {
                    toRemove = li;
                    break;
                }
            }

            return toRemove;
        }

        internal void LoadMessages()
        {
            TaskDelegate signal = delegate(object data)
            {
                if (data == null)
                {
                    Log.Write(new Exception("Failed to load messages."));
                    return;
                }

                var parameters = data as object[];
                LoadMessages(parameters[0] as List<BaseMessage>, (int)parameters[1]);
            };

            BaseMessage.RetrieveMessages(delegate(object data)
            {
                //Check if it is safe to interact with the form
                if (this.InvokeRequired)
                    this.Invoke(signal, data);
                else
                    signal(data);
            });
        }

        internal void LoadMessages(List<BaseMessage> messages, int numberNew)
        {
            _messagesListView.Items.Clear();

			_newMessageCount = numberNew;
			int newCount = 0;

            foreach (var message in messages)
            {
                var fromToday   = message.DateToSend.Date == DateTime.Today;
                var format      = fromToday ? "{0:h:mm tt}" : "{0:M/d/yy}";

                var li = new ListViewItem(new string[]
                {
                    message.Subject,
                    message.Sender,
                    string.Format(format, message.DateToSend.ToShortDateString())
                });
                li.Tag = message;
                li.Group = _messagesListView.Groups["_newMsgListViewGroup"];

				if (newCount++ < numberNew)
					li.SubItems[0].Font = new System.Drawing.Font(li.Font, System.Drawing.FontStyle.Bold);

                _messagesListView.Items.Add(li);
            }

            //Alert the user there is a new message
            if (numberNew > 0)
            {
				MessageBox.Show("You have " + numberNew + " messages.");

				if (MessageReceived != null)
					MessageReceived(this, EventArgs.Empty);
            }

			UpdateMessageCountTab();

            MainForm.ViewMessageControl.DeleteClick += new EventHandler(ViewMessageControl_DeleteClick);
            MainForm.ViewMessageControl.NextClick   += new EventHandler(ViewMessageControl_NextClick);
            MainForm.ViewMessageControl.PrevClick   += new EventHandler(ViewMessageControl_PrevClick);

            SetEnabled(true);
        }

		private void UpdateMessageCountTab()
		{
			if(_newMessageCount > 0)
				MainForm.MessagesTabPage.Text = string.Format("Messages ({0})", _newMessageCount);
			else
				MainForm.MessagesTabPage.Text = "Messages";
		}

        private void SetEnabled(bool enabled)
        {
            _messagesListView.Enabled   = enabled;

			if (_messagesListView.Items.Count > 0)
			{
				_deleteButton.Enabled = enabled;
				_openButton.Enabled = enabled;
			}
			else
			{
				_deleteButton.Enabled = false;
				_openButton.Enabled = false;
			}
        }

        #endregion
    }
}