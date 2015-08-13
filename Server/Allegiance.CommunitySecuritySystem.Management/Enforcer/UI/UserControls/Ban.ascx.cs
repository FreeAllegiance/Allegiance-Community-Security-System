using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using System.Data.Linq.SqlClient;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer.UI.UserControls
{
	public partial class Ban : UI.UserControl
	{
		public bool ActivePlayersOnly = false;
		public string SearchText = String.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
				BindData();
		}

		public void BindData()
		{
			BindPlayerData();

			BindBanReasons();

			SetUiVisibility();
		}


		private void SetUiVisibility()
		{
			tpAuto.Visible = false;
			tpManual.Visible = false;

			if (Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.Administrator.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.ZoneLeader.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.Moderator.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.SuperAdministrator.ToString()) == true)
			{
				tpAuto.Visible = true;

				if (Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.ZoneLeader.ToString()) == true
					|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.SuperAdministrator.ToString()) == true
					|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.Moderator.ToString()) == true)
				{
					tpManual.Visible = true;
				}
			}

			if(tpAuto.Visible == true)
				tcBanTypes.ActiveTab = tpAuto;

		}

		private void BindBanReasons()
		{
			ddlAutoBanReason.Items.Clear();
			ddlAutoBanReason.Items.Add(new ListItem("-- Rules of Conduct --", "0"));

			using (var db = new DataAccess.CSSDataContext())
			{
				//var banTypes = db.BanTypes.OrderBy(p => p, new BanTypeComparer());
				var banTypes = db.BanTypes.OrderBy(p => p.RocNumber.GetValueOrDefault(9999)).ThenBy(p => p.SrNumber.GetValueOrDefault(9999));

				bool addedSrHeader = false;
				bool addedOtherHeader = false;
				bool defaultItemSelected = false;

				foreach (var banType in banTypes)
				{
					if (banType.RocNumber != null)
					{
						ddlAutoBanReason.Items.Add(new ListItem("    RoC #" + banType.RocNumber.ToString() + ": " + banType.Description, banType.Id.ToString()));

						if (defaultItemSelected == false)
						{
							ddlAutoBanReason.SelectedIndex = ddlAutoBanReason.Items.Count - 1;
							defaultItemSelected = true;
						}
					}
					else if (banType.SrNumber != null)
					{
						if (addedSrHeader == false)
						{
							ddlAutoBanReason.Items.Add(new ListItem("", "0"));
							ddlAutoBanReason.Items.Add(new ListItem("-- Supplementary Rules --", "0"));
							addedSrHeader = true;
						}

						ddlAutoBanReason.Items.Add(new ListItem("    SR #" + banType.SrNumber.ToString() + ": " + banType.Description, banType.Id.ToString()));
					}
					else
					{
						if (addedOtherHeader == false)
						{
							ddlAutoBanReason.Items.Add(new ListItem("", "0"));
							ddlAutoBanReason.Items.Add(new ListItem("-- Other --", "0"));
							addedOtherHeader = true;
						}

						ddlAutoBanReason.Items.Add(new ListItem("    " + banType.Description, banType.Id.ToString()));
					}
				}
			}
		}

		private void BindPlayerData()
		{
			bool canBeUnbanned = Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(HttpContext.Current.User);
			
			List<Data.Player> players = new List<Data.Player>();

			if (ActivePlayersOnly == true)
				players = GetActivePlayers(canBeUnbanned);

			else if(String.IsNullOrEmpty(SearchText) == false)
				players = SearchPlayers(canBeUnbanned);

			int IPColumnIndex = 5;
			gvPlayers.Columns[IPColumnIndex].Visible = Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(HttpContext.Current.User);

			gvPlayers.DataSource = players;
			gvPlayers.DataBind();
		}

		private List<Allegiance.CommunitySecuritySystem.Management.Enforcer.Data.Player> SearchPlayers(bool canBeUnbanned)
		{
			string searchText = SearchText;
			if (searchText.Contains("%") == false)
				searchText += "%";

			List<Data.Player> players = new List<Data.Player>();

			using (var db = new DataAccess.CSSDataContext())
			{
				//var aliases = db.Alias.Where(p => SqlMethods.Like(p.Callsign, searchText) || SqlMethods.Like(p.Login.Username, searchText)).OrderBy(p => p.Callsign).Take(100);
				var aliases = db.Alias.Where(p => SqlMethods.Like(p.Callsign, searchText)).OrderBy(p => p.Callsign).Take(100);

				foreach (var alias in aliases)
				{
					players.Add(new Data.Player()
					{
						LoginId = alias.LoginId,
						Callsign = alias.Callsign,
						IsBanned = alias.Login.IsBanned,
						LastContact = DateTime.MinValue,
						BanImage = (alias.Login.IsBanned == true) ? "~/images/dg_banned.png" : "~/images/dg_notbanned.png",
						CanBeUnbanned = canBeUnbanned
					});
				}
			}

			return players;
		}

		private List<Allegiance.CommunitySecuritySystem.Management.Enforcer.Data.Player> GetActivePlayers(bool canBeUnbanned)
		{
			List<Data.Player> players = new List<Data.Player>();

			using (var db = new DataAccess.CSSDataContext())
			{
				var activeSessions = db.Sessions.Where(p => p.DateLastCheckIn > DateTime.Now.AddMinutes(-3)).OrderBy(p => p.Alias.Callsign);

				foreach (var activeSession in activeSessions)
				{
					players.Add(new Data.Player()
					{
						LoginId = activeSession.LoginId,
						Callsign = activeSession.Alias.Callsign,
						IsBanned = activeSession.Login.IsBanned,
						LastContact = activeSession.DateLastCheckIn,
						BanImage = (activeSession.Login.IsBanned == true) ? "~/images/dg_banned.png" : "~/images/dg_notbanned.png",
						CanBeUnbanned = canBeUnbanned
					});
				}
			}

			return players;
		}

		protected void btnApplyAutoBan_Click(object sender, EventArgs e)
		{
			if (Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.Moderator.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.Administrator.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.ZoneLeader.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.SuperAdministrator.ToString()) == true)
			{
				Allegiance.CommunitySecuritySystem.Server.Administration administration = new Allegiance.CommunitySecuritySystem.Server.Administration();

				using (var db = new DataAccess.CSSDataContext())
				{
					var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, Page.User.Identity.Name);
					if (login != null)
					{
						administration.SetBan(new BanData()
						{
							Alias = txtCallsign.Value,
							BanTypeId = Convert.ToInt32(Request.Form[ddlAutoBanReason.UniqueID]), // For some reason, the post back doesn't get this value back into the control.
							BanMode = Allegiance.CommunitySecuritySystem.Common.Enumerations.BanMode.Auto,
							Password = login.Password,
							Username = login.Username
						});
					}
				}
			}

			BindData();
		}

		protected void btnUnban_Click(object sender, EventArgs e)
		{
			if (Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(Page.User) == true)
			{
				Allegiance.CommunitySecuritySystem.Server.Administration administration = new Allegiance.CommunitySecuritySystem.Server.Administration();

				using (var db = new DataAccess.CSSDataContext())
				{
					var adminLogin = DataAccess.Login.FindLoginByUsernameOrCallsign(db, Page.User.Identity.Name);
					var userLogin = DataAccess.Login.FindLoginByUsernameOrCallsign(db, txtCallsign.Value);

					if (userLogin != null && adminLogin != null)
					{
						foreach (DataAccess.Ban ban in userLogin.Bans.Where(p => p.InEffect == true && p.DateExpires > DateTime.Now))
						{
							administration.RemoveBan(new BanData()
							{
								Alias = txtCallsign.Value,
								BanId = ban.Id,
								Password = adminLogin.Password,
								Username = adminLogin.Username
							});
						}
					}
				}
			}

			BindData();
		}

		protected void btnApplyManualBan_Click(object sender, EventArgs e)
		{
			if (Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.Moderator.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.ZoneLeader.ToString()) == true
				|| Page.User.IsInRole(Allegiance.CommunitySecuritySystem.Common.Enumerations.RoleType.SuperAdministrator.ToString()) == true)
			{
				Allegiance.CommunitySecuritySystem.Server.Administration administration = new Allegiance.CommunitySecuritySystem.Server.Administration();

				DateTime banDate = DateTime.MinValue;

				banDate = banDate.AddYears(Int32.Parse(ddlManualBanYears.SelectedValue));
				banDate = banDate.AddMonths(Int32.Parse(ddlManualBanMonths.SelectedValue));
				banDate = banDate.AddDays(Int32.Parse(ddlManualBanDays.SelectedValue));
				banDate = banDate.AddHours(Int32.Parse(ddlManualBanHours.SelectedValue));
				banDate = banDate.AddMinutes(Int32.Parse(ddlManualBanMinutes.SelectedValue));

				TimeSpan banTime = banDate.Subtract(DateTime.MinValue);


				using (var db = new DataAccess.CSSDataContext())
				{
					var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, Page.User.Identity.Name);
					if (login != null)
					{
						administration.SetBan(new BanData()
						{
							Alias = txtCallsign.Value,
							BanMode = Allegiance.CommunitySecuritySystem.Common.Enumerations.BanMode.Custom,
							Duration = banTime,
							Reason = txtBanReason.Text,
							Password = login.Password,
							Username = login.Username
						});
					}
				}
			}

			BindData();
		}
	}
}