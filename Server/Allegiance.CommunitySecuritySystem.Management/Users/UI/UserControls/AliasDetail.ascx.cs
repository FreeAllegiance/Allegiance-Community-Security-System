using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Management.Users.UI.UserControls
{
	public partial class AliasDetail : System.Web.UI.UserControl, ITemplate
	{
		public int AliasID;
		public int LoginID;
		public bool CanShowDeleteButton = false;
		public bool ForcePageReload = false;

		public delegate void OnRequiresDataBindDelegate();
		public event OnRequiresDataBindDelegate OnRequiresDataBind;


		protected void Page_Load(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(Request.Params[txtDeleteFlag.UniqueID]) == false)
			{
				int aliasID = Int32.Parse(Request.Params[txtAliasID.UniqueID]);
				int groupID = Int32.Parse(Request.Params[txtGroupID.UniqueID]);

				DeleteGroupRole(aliasID, groupID);

				txtDeleteFlag.Value = String.Empty;
				txtAliasID.Value = String.Empty;
				txtGroupID.Value = String.Empty;
			}

			if (String.IsNullOrEmpty(Request.Params[txtDeleteAliasFlag.UniqueID]) == false)
			{
				int aliasID = Int32.Parse(Request.Params[txtAliasID.UniqueID]);

				DeleteAlias(aliasID);

				txtDeleteAliasFlag.Value = String.Empty;
				txtAliasID.Value = String.Empty;
			}

			using (var db = new DataAccess.CSSDataContext())
			{
				var alias = db.Alias.FirstOrDefault(p => p.Id == AliasID);

				if (alias != null)
					CanShowDeleteButton = alias.Login.Aliases.OrderBy(p => p.DateCreated).First().Id != AliasID;
			}

			//if (this.IsPostBack == false)
			//    BindData();
		}

		private void DeleteAlias(int aliasID)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var aliasToDelete = db.Alias.FirstOrDefault(p => p.Id == aliasID);
				if (aliasToDelete != null)
				{
					db.PersonalMessages.DeleteAllOnSubmit(aliasToDelete.PersonalMessages);
					db.GroupMessages.DeleteAllOnSubmit(aliasToDelete.GroupMessages);
					db.GroupRequests.DeleteAllOnSubmit(aliasToDelete.GroupRequests);
					db.Group_Alias_GroupRoles.DeleteAllOnSubmit(aliasToDelete.Group_Alias_GroupRoles);
					db.GroupMessage_Alias.DeleteAllOnSubmit(aliasToDelete.GroupMessage_Alias);
					db.AliasBanks.DeleteAllOnSubmit(aliasToDelete.AliasBanks);
				
					db.Alias.DeleteOnSubmit(aliasToDelete);
					db.SubmitChanges();
				}
				
			}

			if (OnRequiresDataBind != null)
				OnRequiresDataBind();

			ForcePageReload = true;
		}

		protected override void OnPreRender(EventArgs e)
		{
			BindData();

			base.OnPreRender(e);
		}

		private void DeleteGroupRole(int aliasID, int groupID)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var groupRole = db.Group_Alias_GroupRoles.FirstOrDefault(p => p.AliasId == aliasID && p.GroupId == groupID);

				if (groupRole == null)
					return; 

				db.Group_Alias_GroupRoles.DeleteOnSubmit(groupRole);
				db.SubmitChanges();
			}

			if(OnRequiresDataBind != null)
				OnRequiresDataBind();
		}


		public void BindData()
		{
			lblErrorMessage.Text = String.Empty;

			using (var db = new DataAccess.CSSDataContext())
			{
				var alias = db.Alias.FirstOrDefault(p => p.Id == AliasID);

				if (alias == null)
					return;

				List<Data.EditableGroupRole> groupRoles = new List<Data.EditableGroupRole>();
				foreach (var groupRole in alias.Group_Alias_GroupRoles)
				{
				    groupRoles.Add(new Data.EditableGroupRole()
				    {
						GroupID = groupRole.GroupId,
						AliasID = groupRole.AliasId,
				        GroupName = groupRole.Group.Name,
				        SelectedRoleID = groupRole.GroupRole.Id,
				        Tag = groupRole.Group.Tag,
				        Token = groupRole.GroupRole.Token
				    });
				}

				gvGroups.DataSource = groupRoles;
				gvGroups.DataBind();

				//pGroups.Visible = (groupRoles.Count > 0);
				pNoGroupRolesAssigned.Visible = (groupRoles.Count == 0);

				var availableGroups = db.Groups.Where(p => db.Group_Alias_GroupRoles.Where(r => r.AliasId == AliasID).Select(s => s.GroupId).Contains(p.Id) == false);

				if (availableGroups.Count() > 0)
				{
					pAddGroup.Visible = true;

					ddlGroup.DataSource = availableGroups;
					ddlGroup.DataTextField = "Name";
					ddlGroup.DataValueField = "Id";
					ddlGroup.DataBind();

					ddlRole.DataSource = db.GroupRoles;
					ddlRole.DataTextField = "Name";
					ddlRole.DataValueField = "Id";
					ddlRole.DataBind();
				}
				else
				{
					pAddGroup.Visible = false;
				}
			}
		}

		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
			container.Controls.Add(this);
		}

		#endregion

		protected void ddlRoles_DataBinding(object sender, EventArgs e)
		{
			//DropDownList dataTarget = (DropDownList)sender;

		}

		protected void gvGroups_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{

				Data.EditableGroupRole groupRole = (Data.EditableGroupRole)e.Row.DataItem;
				DropDownList ddlRoles = (DropDownList)e.Row.FindControl("ddlRoles");
				HiddenField txtGroupID = (HiddenField)e.Row.FindControl("txtGroupID");
				HiddenField txtAliasID = (HiddenField)e.Row.FindControl("txtAliasID");

				List<Data.EditableRole> allRoles = new List<Allegiance.CommunitySecuritySystem.Management.Users.Data.EditableRole>();

				using (var db = new DataAccess.CSSDataContext())
				{
					foreach (var currentGroupRole in db.GroupRoles)
					{
						allRoles.Add(new Allegiance.CommunitySecuritySystem.Management.Users.Data.EditableRole()
						{
							Id = currentGroupRole.Id,
							Name = currentGroupRole.Name
						});
					}
				}

				ddlRoles.DataSource = allRoles;
				ddlRoles.DataTextField = "Name";
				ddlRoles.DataValueField = "Id";
				ddlRoles.DataBind();

				ddlRoles.SelectedValue = groupRole.SelectedRoleID.ToString();

				txtGroupID.Value = groupRole.GroupID.ToString();
				txtAliasID.Value = groupRole.AliasID.ToString();
			}
		}

	
		protected void btnAddGroupRole_Click(object sender, EventArgs e)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				int groupID = Int32.Parse(ddlGroup.SelectedValue);
				int roleID = Int32.Parse(ddlRole.SelectedValue);

				if (db.Group_Alias_GroupRoles.FirstOrDefault(p => p.GroupId == groupID && p.AliasId == AliasID) != null)
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

				if(OnRequiresDataBind != null)
					OnRequiresDataBind();

				//int loginID = db.Alias.FirstOrDefault(p => p.Id == AliasID).LoginId;

				//Response.Redirect(String.Format("~/User/EditUser.aspx?LoginID={0}&AliasID={1}", loginID, AliasID), true);
			}
		}

		// When the role dropdown changes in the datagrid.
		protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
		{
			DropDownList ddlRoles = (DropDownList)sender;
			GridViewRow row = (GridViewRow)((DataControlFieldCell)((DropDownList)sender).Parent).Parent;
			HiddenField txtGroupID = (HiddenField)row.FindControl("txtGroupID");
			HiddenField txtAliasID = (HiddenField)row.FindControl("txtAliasID");

			int groupID = Int32.Parse(txtGroupID.Value);
			int aliasID = Int32.Parse(txtAliasID.Value);
			int roleID = Int32.Parse(ddlRoles.SelectedValue);

			//((GridViewRow) ((DataControlFieldCell) ((DropDownList)sender).Parent).Parent).Cells[0].Text

			using (var db = new DataAccess.CSSDataContext())
			{
				var groupRole = db.Group_Alias_GroupRoles.FirstOrDefault(p => p.AliasId == aliasID && p.GroupId == groupID);

				if (groupRole == null)
					throw new Exception("Couldn't set role for group. Group may have been deleted from alias, or role is no longer available.");

				groupRole.GroupRoleId = roleID;
		
				db.SubmitChanges();
			}

			if(OnRequiresDataBind != null)
				OnRequiresDataBind();
		}
	}
}