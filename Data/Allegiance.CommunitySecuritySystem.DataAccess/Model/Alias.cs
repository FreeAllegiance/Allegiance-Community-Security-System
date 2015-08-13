using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using Allegiance.CommunitySecuritySystem.DataAccess.Properties;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Alias
    {
        #region Fields

        public const int MinAliasLength = 3;
		public const int MaxAliasLength = 17;
		
		public const int DefaultAliasLimit = 3;
		public const int ZoneLeaderAliasLimit = 6;
		public const int AdminAliasLimit = 9; // Set to Int32.MaxValue for unlimited.
		public const int ModeratorAliasLimit = 6;

        #endregion

        #region Methods

		public static int GetAliasLimit(CSSDataContext db, Login login)
		{
			int aliasLimit = DefaultAliasLimit;

			if (login.HasRole(Common.Enumerations.RoleType.Administrator) == true || login.HasRole(Common.Enumerations.RoleType.SuperAdministrator) == true)
				aliasLimit = AdminAliasLimit;
			else if (login.HasRole(Common.Enumerations.RoleType.ZoneLeader) == true)
				aliasLimit = ZoneLeaderAliasLimit;
			else if (login.HasRole(Common.Enumerations.RoleType.Moderator) == true)
				aliasLimit = ModeratorAliasLimit;

			return aliasLimit;
		}

		public static int GetAliasCount(CSSDataContext db, Login login)
		{
			int aliasLimit = GetAliasLimit(db, login);

			int currentAliasCount = 0;

			if (login.Identity != null)
			{
				// If you want the count to be based on this login + all linked logins, uncomment this line.
				//currentAliasCount = login.Identity.Logins.SelectMany(p => p.Aliases).Count();

				currentAliasCount = login.Aliases.Count();
				
				if (currentAliasCount >= aliasLimit)
					return 0;
			}

			return aliasLimit - currentAliasCount;
		}

		public static int GetAliasCount(CSSDataContext db, string username)
		{
			Login login = Login.FindLoginByUsernameOrCallsign(db, username);

			return GetAliasCount(db, login);
			
		}

		public static CheckAliasResult ValidateUsage(CSSDataContext db, 
			ref string callsignWithTags, out Alias alias)
		{
			return ValidateUsage(db, null, false, null, ref callsignWithTags, out alias);
		}

        /// <summary>
        /// Check if the selected alias is available for this Login Account,
        /// if the alias is not previously associated with this Login, and it
        /// is available, create it.
        /// </summary>
        public static CheckAliasResult ValidateUsage(CSSDataContext db,
            Login login, bool allowCreate, string legacyPassword, ref string callsignWithTags, out Alias alias)
        {
			alias = null;

            //Parse Callsign
			var match = Regex.Match(callsignWithTags,
				string.Concat(@"^(?<token>\W)?(?<callsign>[a-z]\w+)(@(?<tag>\w+))?$"), 
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var token			= match.Groups["token"].Value;
            var callsign    = match.Groups["callsign"].Value;
            var tag				= match.Groups["tag"].Value;

			if (callsign.Length < MinAliasLength || callsign.Length > MaxAliasLength)
				return CheckAliasResult.Unavailable;

			if (BadWords.ContainsBadWord(callsign) == true)
				return CheckAliasResult.ContainedBadWord;

			alias = db.Alias.FirstOrDefault(p => p.Callsign == callsign);

            IEnumerable<Group_Alias_GroupRole> group_roles = null;

            //Check if this callsign is not empty
            if (string.IsNullOrEmpty(callsign))
                return CheckAliasResult.Unavailable;

            //Validate that alias is a member of group associated with this tag
            if (!string.IsNullOrEmpty(tag))
            {
				if (alias == null)
                    return CheckAliasResult.Unavailable;

				group_roles = alias.Group_Alias_GroupRoles
                    .Where(p => p.Group.Tag == tag);

                if (group_roles.Count() == 0)
                    return CheckAliasResult.Unavailable;

                //Override input tag
                tag = group_roles.Select(p => p.Group.Tag).FirstOrDefault();
            }

            //Validate that the alias has the role which allows him to use this tag with this group
            if (!string.IsNullOrEmpty(token))
            {
				if (alias == null)
                    return CheckAliasResult.Unavailable;
                
                var tokenChar = token[0];

                //User has selected a @Tag
                if (!string.IsNullOrEmpty(tag))
                {
                    if (group_roles.Any(p => p.GroupRole.Token == tokenChar) == false)
                        return CheckAliasResult.Unavailable;
                }

                //User has not selected a @Tag
                else
                {
					group_roles = alias.Group_Alias_GroupRoles
                        .Where(p => p.GroupRole.Token == tokenChar && !p.Group.IsSquad);

                    if (group_roles.Count() == 0)
                        return CheckAliasResult.Unavailable;
                }
            }

            //Check if we can create this alias
			if (alias == null)
			{
				if (login != null)
				{
					// Check that the user has not used up all their aliases.
					if (GetAliasCount(db, login) <= 0)
						return CheckAliasResult.AliasLimit;
				}

				CheckAliasResult result = CheckAliasResult.Available;
				if(Settings.Default.UseAsgsForLegacyCallsignCheck == true)
					result = ValidateLegacyCallsignUsage(callsign, legacyPassword);

				if (result != CheckAliasResult.Available)
					return result;

				if (allowCreate && login != null)
				{
					alias = new Alias()
					{
						Callsign = callsign,
						DateCreated = DateTime.Now,
						IsDefault = false,
						IsActive = true
					};
					login.Aliases.Add(alias);
					db.SubmitChanges();
				}
				else //Alias does not exist, and we cannot create it.
					return CheckAliasResult.Available;
			}

			//Check if the user has this alias
			else if(login != null)
			{
				int aliasID = alias.Id;

				if (!login.Identity.Logins.SelectMany(p => p.Aliases).Any(p => p.Id == aliasID))
					return CheckAliasResult.Unavailable;
			}

            //Override input callsign
			callsignWithTags = string.Format("{0}{1}{2}", token, alias.Callsign, 
                (!string.IsNullOrEmpty(tag)) ? "@" + tag : string.Empty);

            return CheckAliasResult.Registered;
        }

		public static bool CheckLegacyAliasExists(string callsign)
		{
			if (Settings.Default.UseAsgsForLegacyCallsignCheck == false)
				return false;

			return (ValidateLegacyCallsignUsage(callsign, null) == CheckAliasResult.LegacyExists);
		}

		public static CheckAliasResult ValidateLegacyCallsignUsage(string callsign, string legacyPassword)
		{
			if (Settings.Default.UseAsgsForLegacyCallsignCheck == false)
				return CheckAliasResult.Available;

			Services services = new Services();
			services.Url = Settings.Default.AsgsServiceURL;
			string playerRankString = services.GetPlayerRank(callsign);

			Regex rankFinder = new Regex(
				  "(?<isInvalid>\\d+)\\|(?<rank>\\d+)\\|.*?",
				RegexOptions.Multiline
				| RegexOptions.Singleline
				| RegexOptions.ExplicitCapture
				| RegexOptions.CultureInvariant
				| RegexOptions.Compiled
				);

			Match rankMatch = rankFinder.Match(playerRankString);

			if (rankMatch.Success == false || Int32.Parse(rankMatch.Groups["rank"].Value) < 0 || Int32.Parse(rankMatch.Groups["isInvalid"].Value) > 0)
				return CheckAliasResult.Available;

			if(String.IsNullOrEmpty(legacyPassword) == true)
				return CheckAliasResult.LegacyExists;

			string asgsEncryptedPassword = ASGS.Encryption.EncryptAsgsPassword(legacyPassword, callsign);

			int authenticationStatus = services.Authenticate(callsign, asgsEncryptedPassword);
			if (authenticationStatus != 0)
				return CheckAliasResult.InvalidLegacyPassword;

			return CheckAliasResult.Available;
		}

        public static List<Alias> ListAliases(CSSDataContext db, string callsignOrUsername)
        {
			Login login = Login.FindLoginByUsernameOrCallsign(db, callsignOrUsername);

			if (login != null)
			{
				// If you want to show all aliases for this account and associated linked accounts, use this line:
				//var allAliases = login.Identity.Logins.SelectMany(p => p.Aliases);

				var allAliases = login.Aliases;
				return allAliases.ToList();
			}
			else
			{
				return new List<Alias>();
			}
        }

		public static List<Alias> ListDecoratedAliases(CSSDataContext db, string callsignOrUsername, bool useAliasLimit)
		{
			List<Alias> allAliases = ListAliases(db, callsignOrUsername);

			if (useAliasLimit == true)
			{
				Login login = Login.FindLoginByUsernameOrCallsign(db, callsignOrUsername);
				int aliasLimit = GetAliasLimit(db, login);

				allAliases = allAliases.OrderBy(p => p.DateCreated).Take(aliasLimit).ToList();
			}

			List<Alias> decoratedAliases = new List<Alias>();

			foreach (Alias alias in allAliases)
			{
				decoratedAliases.Add(alias);

				foreach(Group_Alias_GroupRole gagr in alias.Group_Alias_GroupRoles)
				{
					string decoratedAlias;
					if(String.IsNullOrEmpty(gagr.Group.Tag) == false)
						decoratedAlias = String.Format("{0}{1}@{2}", gagr.GroupRole.Token, alias.Callsign, gagr.Group.Tag);
					else
						decoratedAlias = String.Format("{0}{1}", gagr.GroupRole.Token, alias.Callsign);

					decoratedAliases.Add(new Alias()
					{
						Id = alias.Id,
						LoginId = alias.Id,
						Callsign = decoratedAlias,
						IsDefault = false,
						IsActive = true,
						DateCreated = alias.DateCreated
					});
				}
			}

			return decoratedAliases;
		}

        #endregion

		public static RankDetail GetRankForCallsign(CSSDataContext db, string callsign)
		{
			RankDetail rankDetail = new RankDetail();
			int? loginID = null;

			var alias = DataAccess.Alias.GetAliasByCallsign(db, callsign);

			// Get the oldest login from the alias for ranking.
			if (alias != null)
				loginID = alias.Login.Identity.Logins.OrderBy(p => p.DateCreated).FirstOrDefault().Id;

			if (loginID != null)
			{
				using (CSSStatsDataContext statsDB = new CSSStatsDataContext())
				{
					var stats = statsDB.StatsLeaderboards.FirstOrDefault(p => p.LoginID == loginID.GetValueOrDefault(0));

					if (stats != null)
					{
						double rank = stats.Rank;
						double sigma = stats.Sigma;
						double mu = stats.Mu;

						// mask the user's rank if they are logging in as ACS and they are in a pilot role.
						if (Char.IsLetter(callsign[0]) == true && callsign.EndsWith("@acs", StringComparison.InvariantCultureIgnoreCase) == true)
						{
							rank = 8;
							mu = 32.85;
							sigma = 6.54;
						}

						rankDetail.Rank = rank;
						rankDetail.Sigma = sigma;
						rankDetail.Mu = mu;
						rankDetail.CommandRank = stats.CommandRank;
						rankDetail.CommandSigma = stats.CommandSigma;
						rankDetail.CommandMu = stats.CommandMu;
					}
				}
			}

			return rankDetail;
		}


		public static DataAccess.Alias GetAliasByCallsign(CSSDataContext db, string callsign)
		{
			string cleanCallsign = GetCallsignFromStringWithTokensAndTags(db, callsign);
			var alias = db.Alias.FirstOrDefault(p => p.Callsign == cleanCallsign);
			return alias;
		}

		public static string GetCallsignFromStringWithTokensAndTags(CSSDataContext db, string callsignWithTokensAndTags)
		{
			string callsign = callsignWithTokensAndTags;

			if (callsignWithTokensAndTags.Length > 0 && Char.IsLetterOrDigit(callsignWithTokensAndTags[0]) == false)
			{
				if (db.GroupRoles.FirstOrDefault(p => p.Token == callsignWithTokensAndTags[0]) != null)
				{
					callsign = callsign.Substring(1);
				}
			}

			Regex tagFinder = new Regex(@"(?<callsign>.*?)((?<tag>@.*?)|(\(\d+\)))?$");
			Match tagMatch = tagFinder.Match(callsign);

			if (tagMatch.Success == true)
				callsign = tagMatch.Groups["callsign"].Value;

			return callsign;
		}


	}
}