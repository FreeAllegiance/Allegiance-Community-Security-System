using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Security.Principal;

namespace Allegiance.CommunitySecuritySystem.Management.Users
{
	public partial class GlobalMessage : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Global Message Manager";

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				var globalMessages = db.GroupMessages.Where(p => p.GroupId == null).OrderBy(p => p.DateToSend);

				gvGlobalMessages.DataSource = globalMessages;
				gvGlobalMessages.DataBind();
			}

			txtSendDate.Text = DateTime.Now.ToShortDateString();
			txtExpirationDate.Text = DateTime.Now.AddMonths(1).ToShortDateString();
		}

		protected void btnSaveMessage_Click(object sender, EventArgs e)
		{
			int globalMessageID;
			DataAccess.GroupMessage message;

			DateTime sendDate = DateTime.Parse(txtSendDate.Text);
			DateTime expirationDate = DateTime.Parse(txtExpirationDate.Text);

			using(CSSDataContext db = new CSSDataContext())
			{
				if (Int32.TryParse(txtGlobalMessageID.Value, out globalMessageID) == false)
				{
					IPrincipal principal = HttpContext.Current.User;
					
					message = new GroupMessage()
					{
						Alias = Alias.GetAliasByCallsign(db, principal.Identity.Name)
					};

					db.GroupMessages.InsertOnSubmit(message);
				}
				else
				{
					message = db.GroupMessages.FirstOrDefault(p => p.Id == globalMessageID);

					if (message == null)
						throw new Exception("Couldn't get group message for id: " + globalMessageID);
				}

				message.DateExpires = expirationDate;
				message.DateToSend = sendDate;
				message.Message = txtMessage.Text;
				message.Subject = txtSubject.Text;
				message.DateCreated = DateTime.Now;

				db.SubmitChanges();
			}

			BindData();
		}

		protected void btnDeleteMessage_Click(object sender, EventArgs e)
		{
			int globalMessageID;
			if(Int32.TryParse(txtGlobalMessageID.Value, out globalMessageID) == true)
			{
				using (CSSDataContext db = new CSSDataContext())
				{
					db.GroupMessages.DeleteAllOnSubmit(db.GroupMessages.Where(p => p.Id == globalMessageID));
					db.SubmitChanges();
				}
			}

			BindData();
		}
	}
}