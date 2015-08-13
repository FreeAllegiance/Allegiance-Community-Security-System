using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Polls
{
	public partial class EditPoll : UI.Page
	{
		private Poll _poll = null;
		protected Poll Poll
		{
			get
			{
				if (_poll == null)
				{
					using (CSSDataContext db = new CSSDataContext())
					{
						_poll = db.Polls.FirstOrDefault(p => p.Id == PollID);
					}
				}

				return _poll;
			}
		}

		protected int? PollID
		{
			get 
			{
				int pollID;
				if (Int32.TryParse(Request.Params["pollID"], out pollID) == true)
					return pollID;
				else
					return null; 
			}
		}

		private int? DeletePollOptionID
		{
			get
			{
				int deletePollOptionID;
				if (Int32.TryParse(Request.Params["deletePollOptionID"], out deletePollOptionID) == true)
					return deletePollOptionID;
				else
					return null; 
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (DeletePollOptionID != null)
			{
				DeletePollOption(DeletePollOptionID.Value);
				BindData();
			}

			if(this.IsPostBack == false)
				BindData();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			ClientScript.RegisterForEventValidation(btnSavePollOption.UniqueID, "OnClick");

			base.Render(writer);
		}

		private void BindData()
		{
			if (PollID != null)
			{
				using (CSSDataContext db = new CSSDataContext())
				{
					var poll = db.Polls.FirstOrDefault(p => p.Id == PollID);

					txtPollCreationDate.Text = poll.DateCreated.ToShortDateString();
					txtPollExpirationDate.Text = poll.DateExpires.ToShortDateString();
					txtQuestion.Text = poll.Question;

					gvPollOptions.DataSource = poll.PollOptions;
					gvPollOptions.DataBind();
				}

				pPollOptions.Visible = true;
			}

			else
			{
				txtPollExpirationDate.Text = DateTime.Now.AddMonths(1).ToShortDateString();
				txtPollCreationDate.Text = DateTime.Now.ToShortDateString();
			}

			if (String.IsNullOrEmpty(Request.Params["pollCreated"]) == false)
				lblErrorMessage.Text = "Poll created.";
		}

		private void DeletePollOption(int pollOptionID)
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				db.PollOptions.DeleteOnSubmit(db.PollOptions.FirstOrDefault(p => p.Id == pollOptionID));
				db.SubmitChanges();
			}

			Response.Redirect("EditPoll.aspx?pollID=" + PollID);
		}

		protected void btnSavePoll_Click(object sender, EventArgs e)
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				if (PollID == null)
				{
					Poll poll = new Poll()
					{
						DateCreated = DateTime.Now,
						DateExpires = DateTime.Parse(txtPollExpirationDate.Text),
						LastRecalculation = DateTime.Now,
						Question = txtQuestion.Text
					};

					db.Polls.InsertOnSubmit(poll);
					db.SubmitChanges();

					Response.Redirect("EditPoll.aspx?pollCreated=1&pollID=" + poll.Id, true);
				}
				else
				{
					Poll poll = db.Polls.FirstOrDefault(p => p.Id == PollID);

					if (poll == null)
						throw new Exception("Couldn't find poll: " + PollID);

					poll.DateExpires = DateTime.Parse(txtPollExpirationDate.Text);
					poll.Question = txtQuestion.Text;

					db.SubmitChanges();

					lblErrorMessage.Text = "Poll updated.";

					pPollOptions.Visible = true;
				}
			}
		}

		protected void cvExpirationDate_ServerValidate(object source, ServerValidateEventArgs args)
		{
			DateTime expirationDate;
			DateTime pollCreationDate = DateTime.Parse(txtPollCreationDate.Text);

			if (DateTime.TryParse(txtPollExpirationDate.Text, out expirationDate) == false)
				args.IsValid = false;
			else if (expirationDate < pollCreationDate)
				args.IsValid = false;
			else
				args.IsValid = true;
		}

		protected void btnSavePollOption_Click(object sender, EventArgs e)
		{
			//string pollOptionText = txtPollOption.Value;

			using (CSSDataContext db = new CSSDataContext())
			{
				int editingPollOptionID = Int32.Parse(txtEditingPollOptionID.Value);
				PollOption pollOption;

				var poll = db.Polls.FirstOrDefault(p => p.Id == PollID);

				if (poll == null)
					throw new Exception("Couldn't find poll: " + PollID);

				if (editingPollOptionID == 0)
				{
					pollOption = new PollOption();
					pollOption.VoteCount = 0;
					poll.PollOptions.Add(pollOption);
				}
				else
				{
					pollOption = db.PollOptions.FirstOrDefault(p => p.Id == editingPollOptionID);
					if (pollOption == null)
						throw new Exception("Couldn't get poll option: " + editingPollOptionID);
				}

				if (txtPollOptionValue.Value.Length > 70)
					pollOption.Option = txtPollOptionValue.Value.Substring(0, 70);
				else
					pollOption.Option = txtPollOptionValue.Value;

				db.SubmitChanges();
			}

			BindData();
		}

		protected void btnRecalculatePoll_Click(object sender, EventArgs e)
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				var poll = db.Polls.FirstOrDefault(p => p.Id == PollID);
				poll.Recalculate();

				db.SubmitChanges();
			}

			BindData();
		}



	}
}