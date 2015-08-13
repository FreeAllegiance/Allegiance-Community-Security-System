using System;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    public partial class MessageSingleControl : UserControl
    {
        #region Event Declarations

        public event EventHandler CloseClick
        {
            add
            {
                _closeButton.Click += value;
                _deleteButton.Click += value;
            }
            remove
            {
                _closeButton.Click -= value;
                _deleteButton.Click -= value;
            }
        }

        public event EventHandler DeleteClick
        {
            add { _deleteButton.Click += value; }
            remove { _deleteButton.Click -= value; }
        }

        public event EventHandler NextClick
        {
            add { _nextButton.Click += value; }
            remove { _nextButton.Click -= value; }
        }

        public event EventHandler PrevClick
        {
            add { _previousButton.Click += value; }
            remove { _previousButton.Click -= value; }
        }

        #endregion

        #region Constructors

        public MessageSingleControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        internal void FillForm(BaseMessage message)
        {
            Tag                         = message;
            _titleLabel.Text            = message.Subject;
            _fromLabel.Text             = string.Concat("From: ", message.Sender);

			string messageDateTime;
			if(message.DateToSend < message.DateCreated)
				messageDateTime = string.Format("{0:MMM dd, yyyy} at {0:h:mm tt}", message.DateCreated);
			else
				messageDateTime = string.Format("{0:MMM dd, yyyy}", message.DateToSend);

			_dateSentLabel.Text = messageDateTime;
            _messageBodyTextBox.Text    = message.Message;
        }

        #endregion
    }
}