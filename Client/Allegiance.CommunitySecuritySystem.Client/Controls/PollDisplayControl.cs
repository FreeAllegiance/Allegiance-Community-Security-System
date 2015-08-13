using System;
using System.Windows.Forms;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    public partial class PollDisplayControl : UserControl
    {
        #region Event Declarations

        public event EventHandler PollComplete;

        #endregion

        #region Constructors

        public PollDisplayControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void _voteBtn_Click(object sender, EventArgs e)
        {
			var option = _optionList.SelectedItem as ClientService.PollOption;

            if (option != null)
                option.ApplyVote();
            else
                return;

            if (PollComplete != null)
                PollComplete(this, EventArgs.Empty);
        }

        private void _remindLaterBtn_Click(object sender, EventArgs e)
        {
            if (PollComplete != null)
                PollComplete(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

		internal void FillForm(ClientService.Poll poll)
        {
            this.Tag            = poll;
            _questionText.Text  = poll.Question;
            _dateLabel.Text     = string.Format("Sent {0:M/d/yy} at {0:h:mm tt}", poll.DateCreated);
			_pollHeaderLabel.Text = "Poll #" + poll.Id;

            _optionList.Items.Clear();

            foreach (var option in poll.PollOptions)
                _optionList.Items.Add(option);
        }

        #endregion
    }
}