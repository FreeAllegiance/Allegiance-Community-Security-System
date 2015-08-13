using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Users
{
	public partial class EditUser : UI.Page
	{
		private int LoginID 
		{ 
			get
			{
				if (String.IsNullOrEmpty(Request.Params["loginID"]) == true)
					throw new Exception("Must specify loginID");

				return Int32.Parse(Request.Params["loginID"]);
			}
		}


		protected void Page_Load(object sender, EventArgs e)
		{
			lblSaveMessage.Text = String.Empty;

			if(this.IsPostBack == false)
				BindData();
		}

		
		private void BindData()
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Id == LoginID);

				if(login == null)
					Response.Redirect("Default.aspx?errorMessage=" + Server.UrlEncode("The user account could not be found"), true);

				txtUsername.Text = login.Username;
				txtEmail.Text = login.Email;
				chkAllowVirtualMachine.Checked = login.AllowVirtualMachineLogin;

				// Left join type query.
				var assignedRoles = db.Roles
				   //.Where( p => p.Name != "SuperAdministrator" && p.Name != "Administrator")
				   .Select 
				   (
					  r => 
						 new  
						 {
							Id = r.Id, 
							Name = r.Name, 
							Assigned = (r.Login_Roles.Where (p => (p.RoleId == r.Id && p.LoginId == LoginID)).Count () > 0)
						 }
				   );

				cblLoginRoles.DataSource = assignedRoles;
				cblLoginRoles.DataTextField = "Name";
				cblLoginRoles.DataValueField = "Id";
				cblLoginRoles.DataBind();

				// After 7 years, still no way to pre-check a checkbox list without a seperate loop. 
				foreach(var assignedRole in assignedRoles)
					cblLoginRoles.Items.FindByValue(assignedRole.Id.ToString()).Selected = assignedRole.Assigned;
				

				if(Business.Authorization.IsAdminOrSuperAdmin(User) == true)
				{
					tcAliases.Visible = true;

					tcAliases.Tabs.Clear();

					foreach(var alias in login.Aliases)
					{
						AjaxControlToolkit.TabPanel aliasTab = new AjaxControlToolkit.TabPanel();
						aliasTab.ID = "tpAlias_" + alias.Id;
						aliasTab.HeaderText = alias.Callsign;

						// TODO: Move the user control directly into the dynamic tabs again,
						// once the issues with postback events not attaching correctly is resolved.
						// Working around the post back events not attaching correctly to the 
						// dynamic tabs, will revist this one later.
						LiteralControl iframeHtml = new LiteralControl(String.Format(@"
							<iframe src=""{0}"" class=""aliasManagementIframe""></iframe>
						", ResolveUrl("~/Users/AliasIframe.aspx?aliasID=" + alias.Id)));

						aliasTab.Controls.Add(iframeHtml);

						/*

						UI.UserControls.AliasDetail aliasDetail = (UI.UserControls.AliasDetail) LoadControl("~/Users/UI/UserControls/AliasDetail.ascx");
						aliasDetail.AliasID = alias.Id;
						aliasDetail.LoginID = LoginID;
						aliasDetail.OnRequiresDataBind += new Allegiance.CommunitySecuritySystem.Management.Users.UI.UserControls.AliasDetail.OnRequiresDataBindDelegate(aliasDetail_OnRequiresDataBind);

						//aliasDetail.BindData();

						aliasTab.Controls.Add(aliasDetail);
						//aliasTab.ContentTemplate = aliasDetail;
						 */

						tcAliases.Tabs.Add(aliasTab);
					}
				}
				else
				{
					tcAliases.Visible = false;
				}
			}
		}

		private bool _hasBoundTabData = false;
		void aliasDetail_OnRequiresDataBind()
		{
			if (_hasBoundTabData == false)
			{
				_hasBoundTabData = true;

				BindData();
			}
		}

		protected void OnDataChanged(object sender, EventArgs e)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var assignedRoles = db.Roles
					   //.Where(p => p.Name != "SuperAdministrator" && p.Name != "Administrator")
					   .Select
					   (
						  r =>
							 new
							 {
								 Id = r.Id,
								 Name = r.Name,
								 Assigned = (r.Login_Roles.Where(p => (p.RoleId == r.Id && p.LoginId == LoginID)).Count() > 0),
								 Login_Role = r.Login_Roles.FirstOrDefault(p => (p.RoleId == r.Id && p.LoginId == LoginID))
							 }
					   );

				foreach (var assignedRole in assignedRoles)
				{
					if (cblLoginRoles.Items.FindByValue(assignedRole.Id.ToString()).Selected != assignedRole.Assigned)
					{
						if (assignedRole.Assigned == true)
							db.Login_Roles.DeleteOnSubmit(assignedRole.Login_Role);
						else
							db.Login_Roles.InsertOnSubmit(new Allegiance.CommunitySecuritySystem.DataAccess.Login_Role()
							{
								LoginId = LoginID,
								RoleId = assignedRole.Id
							});
					}
				}


				var login = db.Logins.FirstOrDefault(p => p.Id == LoginID);
				if (login == null)
					throw new Exception("Couldn't find login for loginID: " + LoginID);

				login.Email = txtEmail.Text.Trim();
				login.Username = txtUsername.Text.Trim();
				login.AllowVirtualMachineLogin = chkAllowVirtualMachine.Checked;

				// Keep the first alias the same as the user's login name.
				login.Aliases.OrderBy(p => p.DateCreated).First().Callsign = txtUsername.Text.Trim();

				db.SubmitChanges();

				lblSaveMessage.Text = "Data saved.";

				BindData();
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Users/Default.aspx?searchText=" + Server.UrlEncode(Request.Params["searchText"]));
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			BindData();
		}
	}
}
