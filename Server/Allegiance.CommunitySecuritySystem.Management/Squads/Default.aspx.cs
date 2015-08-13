using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Squads
{
	public partial class Default : UI.Page
	{
		protected bool UserIsAslOrBetter = false;

		//private Business.GroupRole? _userGroupRole = null;
		//protected Business.GroupRole UserGroupRole
		//{
		//    get
		//    {
		//        if (_userGroupRole == null)
		//        {
		//            using(DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
		//            {
		//                int groupID = Int32.Parse(ddlSquads.SelectedItem.Value);

		//                var login = db.Logins.FirstOrDefault(p => p.Username == User.Identity.Name);

		//                var gagrLogin = db.Group_Alias_GroupRoles.FirstOrDefault(p => p.Alias.LoginId == login.Id && p.GroupId == groupID);

		//                switch (gagrLogin.GroupRole.Name)
		//                {
		//                    case "Assistant Squad Leader":
		//                        _userGroupRole = Business.GroupRole.AssistantSquadLeader;
		//                        break;

		//                    case "Squad Leader":
		//                        _userGroupRole = Business.GroupRole.SquadLeader;
		//                        break;

		//                    default:
		//                        _userGroupRole = Business.GroupRole.Pilot;
		//                        break;
		//                }
		//            }
		//        }

		//        return _userGroupRole.Value;
		//    }
		//}

		private Business.GroupRole GetCurrentUserGroupRole(int groupID)
		{
			Business.GroupRole groupRole = Business.GroupRole.Pilot;

			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, User.Identity.Name);
				
				if (login != null)
				{
					var gagrLogin = db.Group_Alias_GroupRoles.FirstOrDefault(p => p.Alias.LoginId == login.Id && p.GroupId == groupID);

					groupRole = GetGroupRoleByRoleName(gagrLogin.GroupRole.Name);
				}
			}

			return groupRole;
		}

		private Business.GroupRole GetGroupRoleForCallsign(string callsign, int groupID)
		{
			Business.GroupRole groupRole;

			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				var gagrUser = db.Group_Alias_GroupRoles.FirstOrDefault(p => p.Alias.Callsign == callsign && p.GroupId == groupID);

				groupRole = GetGroupRoleByRoleName(gagrUser.GroupRole.Name);
			}

			return groupRole;
		}

		private Business.GroupRole GetGroupRoleByRoleName(string roleName)
		{
			Business.GroupRole groupRole;

			switch (roleName)
			{
				case "Assistant Squad Leader":
					groupRole = Business.GroupRole.AssistantSquadLeader;
					break;

				case "Squad Leader":
					groupRole = Business.GroupRole.SquadLeader;
					break;

				case "Zone Lead":
					groupRole = Business.GroupRole.ZoneLead;
					break;

				case "Help Desk":
					groupRole = Business.GroupRole.HelpDesk;
					break;

				default:
					groupRole = Business.GroupRole.Pilot;
					break;
			}

			return groupRole;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			int targetGroupID = 0;

			if (String.IsNullOrEmpty(Request.Params["action"]) == false)
			{
				if (Request.Params["action"].Equals("add", StringComparison.InvariantCultureIgnoreCase) == true)
				{
					if (String.IsNullOrEmpty(Request.Params["callsign"]) == false && String.IsNullOrEmpty(Request.Params["groupID"]) == false)
					{
						AddCallsignToGroup(Request.Params["callsign"], Int32.Parse(Request.Params["groupID"]));
					}

				}
				else if (Request.Params["action"].Equals("delete", StringComparison.InvariantCultureIgnoreCase) == true)
				{
					if (String.IsNullOrEmpty(Request.Params["callsign"]) == false && String.IsNullOrEmpty(Request.Params["groupID"]) == false)
					{
						RemoveCallsignFromGroup(Request.Params["callsign"], Int32.Parse(Request.Params["groupID"]));
					}
				}
			}
			else if (String.IsNullOrEmpty(Request.Params["groupID"]) == false)
			{
				targetGroupID = Int32.Parse(Request.Params["groupID"]);
			}


			if (this.IsPostBack == false)
				BindData(targetGroupID);
		}

		private void RemoveCallsignFromGroup(string callsign, int groupID)
		{
			Business.GroupRole currentUserGroupRole = GetCurrentUserGroupRole(groupID);

			if (currentUserGroupRole != Business.GroupRole.AssistantSquadLeader && currentUserGroupRole != Business.GroupRole.SquadLeader && currentUserGroupRole != Business.GroupRole.ZoneLead)
				throw new Exception("Access denied.");

			Business.GroupRole callsignGroupRole = GetGroupRoleForCallsign(callsign, groupID);

			if (callsignGroupRole == Business.GroupRole.SquadLeader && currentUserGroupRole != Business.GroupRole.SquadLeader && currentUserGroupRole != Business.GroupRole.ZoneLead)
				throw new Exception("Only squad leaders can perform this action.");

			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var alias = DataAccess.Alias.GetAliasByCallsign(db, callsign);
				var callsignAssignmentsToGroup = db.Group_Alias_GroupRoles.Where(p => p.GroupId == groupID && p.Alias.Id == alias.Id);
				
				db.Group_Alias_GroupRoles.DeleteAllOnSubmit(callsignAssignmentsToGroup);

				var group = db.Groups.FirstOrDefault(p => p.Id == groupID);
				if (group == null)
					throw new Exception("invalid group: " + groupID);

				// If the group name is the Moderators group, then add the Moderator role to the group member.
				if (group.Name.Equals("Moderators", StringComparison.InvariantCultureIgnoreCase) == true)
				{
					var moderatorRole = db.Roles.FirstOrDefault(p => p.Name == "Moderator");
					db.Login_Roles.DeleteAllOnSubmit(db.Login_Roles.Where(p => p.LoginId == alias.LoginId && p.RoleId == moderatorRole.Id));
				}

				//  If the group is ACS, then unbank the original hider.
				if (group.Tag.Equals("acs", StringComparison.InvariantCultureIgnoreCase) == true)
					UnbankAlias(db, alias);

				db.SubmitChanges();
			}

			Response.Redirect("~/Squads/Default.aspx?groupID=" + groupID, true);
		}

		

		private void AddCallsignToGroup(string callsign, int groupID)
		{
			Business.GroupRole currentUserGroupRole = GetCurrentUserGroupRole(groupID);

			if (currentUserGroupRole != Business.GroupRole.AssistantSquadLeader && currentUserGroupRole != Business.GroupRole.SquadLeader && currentUserGroupRole != Business.GroupRole.ZoneLead)
				throw new Exception("Access denied.");
				
			using (var db = new DataAccess.CSSDataContext())
			{
				var group = db.Groups.FirstOrDefault(p => p.Id == groupID);

				if (group == null)
					throw new Exception("Invalid groupID");

				var alias = db.Alias.FirstOrDefault(p => p.Callsign == callsign);

				if (alias == null)
					throw new Exception("Invalid callsign");

				var targetRole = db.GroupRoles.FirstOrDefault(p => p.Name == "Pilot");

				if (targetRole == null)
					throw new Exception("No pilot role.");

				DataAccess.Group_Alias_GroupRole gagrTarget = new DataAccess.Group_Alias_GroupRole()
				{
					AliasId = alias.Id,
					GroupId = group.Id,
					GroupRoleId = targetRole.Id
				};

				db.Group_Alias_GroupRoles.InsertOnSubmit(gagrTarget);

				// If the group name is the Moderators group, then add the Moderator role to the group member.
				if (group.Name.Equals("Moderators", StringComparison.InvariantCultureIgnoreCase) == true)
				{
					var moderatorRole = db.Roles.FirstOrDefault(p => p.Name == "Moderator");
					var loginRole = db.Login_Roles.FirstOrDefault(p => p.LoginId == alias.LoginId && p.RoleId == moderatorRole.Id);
					if (loginRole == null)
					{
						db.Login_Roles.InsertOnSubmit(new DataAccess.Login_Role()
						{
							LoginId = alias.LoginId,
							RoleId = moderatorRole.Id
						});
					}
				}

				// If the group is ACS, then bank the original alias, and swap the alias to an ACS_COM_XXX hider.
				if (group.Tag.Equals("acs", StringComparison.InvariantCultureIgnoreCase) == true)
				{
					BankAlias(db, alias);
					
				}

				db.SubmitChanges();
			}

			Response.Redirect("~/Squads/Default.aspx?groupID=" + groupID, true);
		}

		private void BankAlias(DataAccess.CSSDataContext db, DataAccess.Alias alias)
		{
			if (alias.AliasBanks.Count() == 0)
			{
				db.AliasBanks.InsertOnSubmit(new DataAccess.AliasBank()
				{
					AliasId = alias.Id,
					Callsign = alias.Callsign,
					DateCreated = DateTime.Now
				});

				alias.Callsign = "ACS_" + new Random().Next(100, 999);
			}
		}

		private void UnbankAlias(DataAccess.CSSDataContext db, DataAccess.Alias alias)
		{
			if (alias.AliasBanks.Count() > 0)
			{
				alias.Callsign = alias.AliasBanks.FirstOrDefault().Callsign;
				db.AliasBanks.DeleteAllOnSubmit(alias.AliasBanks);
			}
		}

		private void BindData()
		{
			BindData(0);
		}

		private void BindData(int selectedSquadID)
		{
			txtSendDate.Text = DateTime.Now.ToShortDateString();
			txtExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();

			using (var db = new DataAccess.CSSDataContext())
			{
				var currentLogin = DataAccess.Login.FindLoginByUsernameOrCallsign(db, User.Identity.Name);
				var aliases = db.Alias.Where(p => p.LoginId == currentLogin.Id);

				var availableSquads = DataAccess.Group.GetGroupsForLogin(db, currentLogin.Username, false);
				
				pNoSquadsAvailable.Visible = availableSquads.Count() == 0;
				pSquadList.Visible = availableSquads.Count() != 0;
				
				ddlSquads.Items.Clear();

				foreach (var availableSquad in availableSquads)
				{
					ddlSquads.Items.Add(new ListItem()
					{
						Text = availableSquad.Name,
						Value = availableSquad.Id.ToString(),
						Selected = (availableSquad.Id == selectedSquadID)
					});
				}

				int selectedGroupID;

				if (Int32.TryParse(ddlSquads.SelectedValue, out selectedGroupID) == true)
				{
					Business.GroupRole currentUserGroupRole = GetCurrentUserGroupRole(selectedGroupID);

					if (currentUserGroupRole == Business.GroupRole.AssistantSquadLeader 
						|| currentUserGroupRole == Business.GroupRole.SquadLeader
						|| currentUserGroupRole == Business.GroupRole.ZoneLead)
						UserIsAslOrBetter = true;

					pAslOptions.Visible = UserIsAslOrBetter;

					var gagrSquadMembers = db.Group_Alias_GroupRoles.Where(p => p.GroupId == selectedGroupID);

					List<Data.SquadMember> squadMembers = new List<Data.SquadMember>();
					foreach (var squadMember in gagrSquadMembers)
						squadMembers.Add(new Data.SquadMember()
						{
							Callsign = squadMember.Alias.Callsign,
							Token = squadMember.GroupRole.Token.GetValueOrDefault(),
							AliasID = squadMember.Alias.Id,
							SelectedRoleID = squadMember.GroupRole.Id,
							SelectedRoleName = squadMember.GroupRole.Name,
							GroupID = squadMember.GroupId,
						});

					squadMembers.Sort(delegate(Data.SquadMember left, Data.SquadMember right)
					{
						if (left.Token != right.Token)
						{
							if (left.Token == '*')
								return -1;

							if (right.Token == '*')
								return 1;

							if (left.Token == '^')
								return -1;

							if (right.Token == '^')
								return 1;
						}

						return left.Callsign.CompareTo(right.Callsign);
					});

					gvMembers.DataSource = squadMembers;
					gvMembers.DataBind();
				}
			}
		}

		protected void ddlSquads_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindData(Int32.Parse(ddlSquads.SelectedValue));
		}

		protected void gvMembers_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{

				Data.SquadMember squadMemeber = (Data.SquadMember)e.Row.DataItem;
				DropDownList ddlRoles = (DropDownList)e.Row.FindControl("ddlRoles");
				HiddenField txtGroupID = (HiddenField)e.Row.FindControl("txtGroupID");
				HiddenField txtAliasID = (HiddenField)e.Row.FindControl("txtAliasID");
				Label lblRoleName = (Label)e.Row.FindControl("lblRoleName");
				Panel pRemoveLink = (Panel)e.Row.FindControl("pRemoveLink");
				Panel pMessageLink = (Panel)e.Row.FindControl("pMessageLink");
				Panel pSelectMember = (Panel)e.Row.FindControl("pSelectMember");

				List<DataAccess.GroupRole> allRoles;

				Business.GroupRole groupRole = GetCurrentUserGroupRole(Int32.Parse(ddlSquads.SelectedValue));

				using (var db = new DataAccess.CSSDataContext())
				{
					var group = db.Groups.FirstOrDefault(p => p.Id == Int32.Parse(ddlSquads.SelectedValue));

					if (group == null)
						throw new Exception("Invalid groupID");

					switch (groupRole)
					{
							// Squad leaders can edit all roles on the squad.
						case Business.GroupRole.SquadLeader:
						case Business.GroupRole.ZoneLead:
							if (group.IsSquad == true)
								allRoles = db.GroupRoles.Where(p => p.Name == "Pilot" || p.Name == "Assistant Squad Leader" || p.Name == "Squad Leader").ToList();
							else
								allRoles = db.GroupRoles.ToList();
							break;

							// ASLs can only add/remove pilots.
						case Business.GroupRole.AssistantSquadLeader:
							if (group.IsSquad == true)
							{
								allRoles = db.GroupRoles.Where(p => p.Name == "Pilot" || p.Name == "Assistant Squad Leader").ToList();
							}
							else
							{
								//allRoles = db.GroupRoles.Where(p => p.Name != "Zone Lead" && p.Name != "Squad Leader" && p.Name != "Developer").ToList();

								// Only Zone Leaders can modify roles.
								allRoles = db.GroupRoles.Where(p => p.Name == "Pilot").ToList();
								pRemoveLink.Visible = false;
								ddlRoles.Visible = false;
								pMessageLink.Visible = false;
								pSelectMember.Visible = false;
							}

							if (squadMemeber.SelectedRoleName == "Squad Leader")
							{
								pRemoveLink.Visible = false;
								ddlRoles.Visible = false;
							}
							break;

							// Pilots can only view the list.
						default:
							allRoles = db.GroupRoles.Where(p => p.Name == "Pilot").ToList();
							pRemoveLink.Visible = false;
							ddlRoles.Visible = false;
							pMessageLink.Visible = false;
							pSelectMember.Visible = false;
							break;
					}

					// Prevent user from editing their own account.
					if(squadMemeber.Callsign.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase) == true)
						ddlRoles.Visible = false;					
				}

				if (ddlRoles.Visible == false)
				{
					lblRoleName.Visible = true;
					lblRoleName.Text = squadMemeber.SelectedRoleName;
				}

				ddlRoles.DataSource = allRoles;
				ddlRoles.DataTextField = "Name";
				ddlRoles.DataValueField = "Id";
				ddlRoles.DataBind();

				ddlRoles.SelectedValue = squadMemeber.SelectedRoleID.ToString();

				txtGroupID.Value = squadMemeber.GroupID.ToString();
				txtAliasID.Value = squadMemeber.AliasID.ToString();
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

			bool requiresSquadLeader = false;

			// Only a SL can grant SL to another user.
			if (ddlRoles.SelectedItem.Text == "Squad Leader")
				requiresSquadLeader = true;

			using (var db = new DataAccess.CSSDataContext())
			{
				var group = db.Groups.FirstOrDefault(p => p.Id == groupID);
				
				var groupRole = db.Group_Alias_GroupRoles.FirstOrDefault(p => p.AliasId == aliasID && p.GroupId == groupID);

				if (groupRole == null)
					throw new Exception("Couldn't set role for group. Group may have been deleted from alias, or role is no longer available.");



				var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, User.Identity.Name);

				var gagrLogin = groupRole.Group.Group_Alias_GroupRoles.FirstOrDefault(p => p.Alias.LoginId == login.Id);

				if (group.IsSquad == true)
				{
					// Only a SL can remove rights to another SL.
					if (groupRole.GroupRole.Name.Equals("Squad Leader", StringComparison.InvariantCultureIgnoreCase) == true)
						requiresSquadLeader = true;

					bool isSquadLeader = gagrLogin.GroupRole.Name.Equals("Squad Leader", StringComparison.InvariantCultureIgnoreCase);
					bool isAssistantSquadLeader = gagrLogin.GroupRole.Name.Equals("Assistant Squad Leader", StringComparison.InvariantCultureIgnoreCase);

					if ((isSquadLeader == false && isAssistantSquadLeader == false) || (requiresSquadLeader == true && isSquadLeader == false))
					{
						lblErrorMessage.Text = "You don't have rights to perform this action.";
						return;
					}
				}
				else
				{
					//var moderatorRole = db.Roles.FirstOrDefault(p => p.Name == "Moderator");

					//bool requiresZoneLeader = false;
					//if (groupRole.GroupRole.Name.Equals("Zone Lead", StringComparison.InvariantCultureIgnoreCase) == true)
					//    requiresZoneLeader = true;

					bool requiresZoneLeader = true;

					if (login.HasAnyRole(new Common.Enumerations.RoleType[] { Common.Enumerations.RoleType.ZoneLeader, Common.Enumerations.RoleType.Administrator, Common.Enumerations.RoleType.SuperAdministrator }) == false && requiresZoneLeader == true)
					{
						lblErrorMessage.Text = "You must be a Zone Leader or better to perform this action.";
						return;
					}

				}
			}

			// Can't use groupRole from above, some of the queries against it cause it to lock to a foreign key.
			using (var db = new DataAccess.CSSDataContext())
			{
				var groupRoleToUpdate = db.Group_Alias_GroupRoles.FirstOrDefault(p => p.AliasId == aliasID && p.GroupId == groupID);
				groupRoleToUpdate.GroupRoleId = roleID;

				// If the group is ACS, then bank the alias if the user is going into the pilot role, 
				// otherwise unbank it if they are going into a token'd role.
				if (groupRoleToUpdate.Group.Tag.Equals("acs", StringComparison.InvariantCultureIgnoreCase) == true)
				{
					if (groupRoleToUpdate.GroupRole.Token == null)
						BankAlias(db, groupRoleToUpdate.Alias);
					else
						UnbankAlias(db, groupRoleToUpdate.Alias);
				}

				db.SubmitChanges();
			}

			BindData(groupID);
		}
	}
}
