using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Groups
{
	public partial class Default : UI.Page
	{
		protected string ValidationMessage;

		protected void Page_Load(object sender, EventArgs e)
		{
			ValidationMessage = String.Empty;
			lblErrorText.Text = String.Empty;
		}

		protected void btnSaveNewGroup_Click(object sender, EventArgs e)
		{
			using(CSSDataContext db = new CSSDataContext())
			{
				if (db.Groups.FirstOrDefault(p => p.Name == txtName.Text.Trim()) != null)
				{
					lblErrorText.Text = "The group or squad name you specified: " + txtName.Text + " already exists.";
					return;
				}

				if (db.Groups.FirstOrDefault(p => p.Tag == txtTag.Text.Trim()) != null)
				{
					lblErrorText.Text = "The tag name you specified: " + txtTag.Text + " already exists.";
					return;
				}

				db.Groups.InsertOnSubmit(new Group()
				{
					Name = txtName.Text.Trim(),
					DateCreated = DateTime.Now,
					IsSquad = chkIsSquad.Checked,
					Tag = txtTag.Text
				});

				db.SubmitChanges();

				this.DataBind();
			}
		}

		protected void ldsGroups_Updating(object sender, LinqDataSourceUpdateEventArgs e)
		{
			((Group)e.NewObject).Name = ((Group)e.NewObject).Name.Trim();
			((Group)e.NewObject).Tag = ((Group)e.NewObject).Tag.Trim();

			//Group group = (Group) e.NewObject;

			//using (CSSDataContext db = new CSSDataContext())
			//{
			//    if (String.IsNullOrEmpty(group.Name) == true)
			//    {
			//        ValidationMessage = "Name cannot be empty.";
			//        e.Cancel = true;
			//    }
			//    else if (db.Groups.FirstOrDefault(p => p.Name == group.Name.Trim() && p.Id != group.Id) != null)
			//    {
			//        ValidationMessage = "Name is already used .";
			//        e.Cancel = true;
			//    }

			//}
		}

		protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
		{
			CustomValidator cvName = (CustomValidator)source;

			Label lblId = (Label)cvName.Parent.FindControl("lblId");

			int groupID = Int32.Parse(lblId.Text);

			using (CSSDataContext db = new CSSDataContext())
			{
				if (db.Groups.FirstOrDefault(p => p.Name == args.Value.Trim() && p.Id != groupID) != null)
					args.IsValid = false;
			}	
		}

		protected void cvTag_ServerValidate(object source, ServerValidateEventArgs args)
		{
			CustomValidator cvTag = (CustomValidator)source;

			Label lblId = (Label)cvTag.Parent.FindControl("lblId");

			int groupID = Int32.Parse(lblId.Text);

			using (CSSDataContext db = new CSSDataContext())
			{
				if (db.Groups.FirstOrDefault(p => p.Tag == args.Value.Trim() && p.Id != groupID) != null)
					args.IsValid = false;
			}
		}
	}
}