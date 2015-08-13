using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Web;
using System.Security.Principal;

namespace Allegiance.CommunitySecuritySystem.Management.AjaxProviders
{

	[DataContract()]
	public class BanInfo
	{
		[DataMember]
		public int BanId { get; set; }

		[DataMember]
		public string BanReason { get; set; }

		[DataMember]
		public string TimeRemaining { get; set; }

		[DataMember]
		public string TotalTime { get; set; }
	}

	[DataContract]
	public class PlayerInfo
	{
		[DataMember]
		public string Callsign { get; set; }

		[DataMember]
		public string Token { get; set; }

		[DataMember]
		public string Tag { get; set; }

		[DataMember]
		public string Status { get; set; }

		[DataMember]
		public string LastLogin { get; set; }

		[DataMember]
		public string LastBanReason { get; set; }

		[DataMember]
		public string LastBanTime { get; set; }

		[DataMember]
		public string LastBanUser { get; set; }

		[DataMember]
		public string LastBanDuration { get; set; }

		[DataMember]
		public string[] Aliases { get; set; }

		[DataMember]
		public string DefaultAlias { get; set; }
	}


	[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	[ServiceContract(Namespace = "AjaxProviders")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Enforcer
	{
		private void CheckAccess()
		{
			IPrincipal principal = HttpContext.Current.User;

			if (Business.Authorization.IsModeratorOrZoneLeadOrAdminOrSuperAdmin(HttpContext.Current.User) == false)
				throw new Exception("Access is denied.");
		}

		// Add [WebGet] attribute to use HTTP GET
		[OperationContract]
		public string GetAutoBanDuration(string callsign, int banTypeId)
		{
			CheckAccess();

			using (var db = new CSSDataContext())
			{
				DataAccess.Login login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, callsign);
				DataAccess.BanType banType = db.BanTypes.FirstOrDefault(p => p.Id == banTypeId);

				if (login != null && banType != null)
				{
					TimeSpan? duration = DataAccess.Ban.CalculateDuration(login.Identity, banType);

					if(duration == null)
						return "Infinite";
					else
						return duration.Value.ToString();
				}
				else
				{
					throw new Exception("Could not find login or ban type.");
				}
			}
		}

		[OperationContract]
		public BanInfo GetBanInfo(string callsign)
		{
			CheckAccess();

			using (var db = new CSSDataContext())
			{
				DataAccess.Login login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, callsign);

				var activeBan = login.Identity.Bans.FirstOrDefault(p => p.InEffect == true && p.DateExpires > DateTime.Now);

				if (activeBan != null)
				{
					return new BanInfo()
					{
						BanId = activeBan.Id,
						BanReason = (activeBan.BanType == null) ? activeBan.Reason : activeBan.BanType.Description,
						TimeRemaining = ((TimeSpan)(activeBan.DateExpires - DateTime.Now)).ToString(),
						TotalTime = ((TimeSpan)(activeBan.DateExpires - activeBan.DateCreated)).ToString()
					};
				}
			}

			throw new Exception("Couldn't find ban info.");
		}

		

		[OperationContract]
		public PlayerInfo GetPlayerInfo(string callsign)
		{
			CheckAccess();

			using (var db = new CSSDataContext())
			{
				DataAccess.Login login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, callsign);
				var ban = login.Identity.Bans.OrderByDescending(p => p.DateCreated).FirstOrDefault();

				string lastBanReason = "None";
				string lastBanTime = String.Empty;
				string lastBanUser = String.Empty;
				string lastBanDuration = String.Empty;

				if(ban != null)
				{
					var bannedByLogin = db.Logins.FirstOrDefault(p => p.Id == ban.BannedByLoginId);
					string bannedByUsername = "Unknown";
					if (bannedByLogin != null)
						bannedByUsername = bannedByLogin.Username;

					lastBanReason = ban.BanType == null ? ban.Reason : ban.BanType.Description;
					lastBanTime = ban.DateCreated.ToString();
					lastBanUser = bannedByUsername;

					if (ban.DateExpires == null)
						lastBanDuration = "Permanent";
					else
						lastBanDuration = ban.DateExpires.Value.Subtract(ban.DateCreated).ToString();
				}

				var activeBan = login.Identity.Bans.FirstOrDefault(p => p.InEffect == true && p.DateExpires > DateTime.Now);
				string banStatus = "Ok";

				if(activeBan != null)
					banStatus = "Banned Until: " + activeBan.DateExpires.ToString();

				string tags = String.Empty;
				string tokens = String.Empty;

				var alias = login.Aliases.FirstOrDefault(p => p.Callsign == callsign);
				foreach(var groupAliasGroupRole in alias.Group_Alias_GroupRoles)
				{
					if(String.IsNullOrEmpty(groupAliasGroupRole.Group.Tag) == false)
					{
						if(String.IsNullOrEmpty(tags) == true)
							tags = groupAliasGroupRole.Group.Tag;
						else
							tags += ", " + groupAliasGroupRole.Group.Tag;
					}

					if(groupAliasGroupRole.GroupRole.Token != null)
					{
						if(String.IsNullOrEmpty(tags) == true)
							tokens = groupAliasGroupRole.GroupRole.Token.ToString();
						else
							tokens += ", " + groupAliasGroupRole.GroupRole.Token;
					}
				}

				IPrincipal principal = HttpContext.Current.User;

				string[] aliases;
				string defaultAlias;
				if (Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(HttpContext.Current.User) == true)
				{
					aliases = login.Aliases.OrderByDescending(p => p.IsDefault).Select(p => p.Callsign).ToArray();
					defaultAlias = login.Aliases.Where(p => p.IsDefault == true).Select(p => p.Callsign).FirstOrDefault();
				}
				else
				{
					aliases = new string [] { "Not Shown" };
					defaultAlias = String.Empty;
				}

				PlayerInfo returnValue = new PlayerInfo()
				{
					Callsign = callsign,
					LastLogin = login.Identity.DateLastLogin.ToString(),
					Status = banStatus,
					Tag = tags,
					Token = tokens,
					LastBanTime = lastBanTime,
					LastBanReason = lastBanReason,
					LastBanUser = lastBanUser,
					LastBanDuration = lastBanDuration,
					Aliases = aliases,
					DefaultAlias = defaultAlias
				};

				return returnValue;
			}
		}
	}
}
