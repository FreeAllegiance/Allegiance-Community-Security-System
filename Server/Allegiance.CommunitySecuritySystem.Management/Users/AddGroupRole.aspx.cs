using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security;

namespace Allegiance.CommunitySecuritySystem.Management.Users
{
	public partial class AddGroupRole : UI.Page
	{
		private int AliasID
		{
			get
			{
				if (String.IsNullOrEmpty(Request["aliasID"]) == true)
					throw new Exception("Must specify aliasID");

				return Int32.Parse(Request["aliasID"]);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Business.Authorization.IsAdminOrSuperAdmin(User) == false)
				throw new SecurityException("Access denied");

			using (var db = new DataAccess.CSSDataContext())
			{
				//var unassignedGroups = db.Groups.Where(
				//    p => db.Group_Alias_GroupRoles.Where(
				//        q => q.GroupId == p.Id && q.AliasId == AliasID).Select(
				//            r => r.GroupId).Contains(p.Id) == false);

				ddlGroup.DataSource = db.Groups;
				ddlGroup.DataTextField = "Name";
				ddlGroup.DataValueField = "Id";
				ddlGroup.DataBind();

				ddlRole.DataSource = db.GroupRoles;
				ddlRole.DataTextField = "Name";
				ddlRole.DataValueField = "Id";
				ddlRole.DataBind();

				lblCallsign.Text = db.Alias.FirstOrDefault(p => p.Id == AliasID).Callsign;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				int groupID = Int32.Parse(ddlGroup.SelectedValue);
				int roleID = Int32.Parse(ddlRole.SelectedValue);

				if (db.Group_Alias_GroupRoles.FirstOrDefault(p => p.GroupId == groupID && p.GroupRoleId == roleID && p.AliasId == AliasID) != null)
				{
					lblErrorMessage.Text = "User is already assigned to this group/role combination.";
					return;
				}

				DataAccess.Group_Alias_GroupRole newGroupRole = new Allegiance.CommunitySecuritySystem.DataAccess.Group_Alias_GroupRole()
				{
					AliasId = AliasID,
					GroupId = groupID,
					GroupRoleId = roleID				
				};

				db.Group_Alias_GroupRoles.InsertOnSubmit(newGroupRole);
				db.SubmitChanges();

				int loginID = db.Alias.FirstOrDefault(p => p.Id == AliasID).LoginId;

				Response.Redirect(String.Format("~/User/EditUser.aspx?LoginID={0}&AliasID={1}", loginID, AliasID), true);
			}
		}
	}
}
