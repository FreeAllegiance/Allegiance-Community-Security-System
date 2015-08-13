using System.Collections.Generic;
using System.Linq;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using System;
using System.Data.Linq;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using Allegiance.CommunitySecuritySystem.DataAccess.Model;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Identity
    {
		/// <summary>
		/// used for logging account merges.
		/// </summary>
		private class MatchedMachineRecord
		{
			public MachineRecord machineRecord1;
			public MachineRecord machineRecord2;
		}

		public IEnumerable<PollVote> PollVotes
		{
			get
			{
				return this.Logins.SelectMany(p => p.PollVotes);
			}
		}

		public IEnumerable<MachineRecord> MachineRecords
		{
			get
			{
				var machineRecords = this.Logins.SelectMany(p => p.MachineRecords);
				return machineRecords;
			}
		}

		public IEnumerable<Ban> Bans
		{
			get
			{
				return this.Logins.SelectMany(p => p.Bans);

				//return this.Logins.SelectMany(p => p.Bans, (p, c) => c );
				//List<Ban> bans = new List<Ban>();

				//foreach (Login login in this.Logins)
				//    bans.AddRange(login.Bans);

				//return bans;
			}
		}

        #region Methods

		public static System.Web.Security.MembershipCreateStatus TryCreateIdentity(CSSDataContext db, string username, string password, string email, out Identity createdIdentity)
		{
			createdIdentity = null;

			if (BadWords.ContainsBadWord(username) == true)
				return System.Web.Security.MembershipCreateStatus.UserRejected;

			foreach (char letter in username.ToArray())
			{
				if(Char.IsLetterOrDigit(letter) == false && letter != '_')
					return System.Web.Security.MembershipCreateStatus.UserRejected;
			}

			if(username.Length < 3 || username.Length > 17)
				return System.Web.Security.MembershipCreateStatus.UserRejected;
			
			Identity newIdentity = new Identity();
			newIdentity.DateLastLogin = DateTime.Now;
			newIdentity.LastGlobalMessageDelivery = DateTime.Now;

			// See if the login belongs to this user.
			var login = Login.FindLoginByUsernameOrCallsign(db, username, password);

			if (login == null)
			{
				// See if the login belongs to any user.
				if(db.Logins.FirstOrDefault(p => p.Username == username) != null)
					return System.Web.Security.MembershipCreateStatus.DuplicateUserName;

				if(db.Alias.FirstOrDefault(p => p.Callsign == username) != null)
					return System.Web.Security.MembershipCreateStatus.DuplicateUserName;

				// login is available, create a new one.
				login = new Login()
				{
					Username = username,
					Password = PasswordHash.CreateHash(password),
					Email = email,
					DateCreated = DateTime.Now
				};
			}

			string callsignWithTags = username;
			Alias existingAlias = null;
			var aliasValidateResult = Alias.ValidateUsage(db, login, false, password, ref callsignWithTags, out existingAlias);

			if (aliasValidateResult == CheckAliasResult.InvalidLegacyPassword)
				return System.Web.Security.MembershipCreateStatus.InvalidPassword;

			else if (Alias.ValidateUsage(db, login, false, password, ref callsignWithTags, out existingAlias) != CheckAliasResult.Available)
				return System.Web.Security.MembershipCreateStatus.InvalidUserName;
			
			List<Alias> aliases = Alias.ListAliases(db, username);

			// Make sure no one else has this username as an alias.
			if (aliases.Count > 0)
				return System.Web.Security.MembershipCreateStatus.DuplicateUserName;

			var alias = new Alias()
			{
				Callsign = username,
				DateCreated = DateTime.Now,
				IsDefault = true,
				IsActive = true
			};

			login.Aliases.Add(alias);
			newIdentity.Logins.Add(login);
			login.Lobby_Logins.Add(new Lobby_Login() { Lobby = db.Lobbies.First(p => p.Name == "Production") });

			db.Identities.InsertOnSubmit(newIdentity);

			// Must be done by the caller. 
			//db.SubmitChanges();

			createdIdentity = newIdentity;

			return System.Web.Security.MembershipCreateStatus.Success;
		}

		public static void Delete(CSSDataContext db, Identity identity)
		{
			db.Bans.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Bans));
			db.Bans.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.IssuedBans));
			db.Group_Alias_GroupRoles.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Aliases.SelectMany(r => r.Group_Alias_GroupRoles)));
			db.GroupMessages.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Aliases.SelectMany(r => r.GroupMessages)));
			db.GroupMessage_Alias.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Aliases.SelectMany(r => r.GroupMessage_Alias)));
			db.GroupRequests.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Aliases.SelectMany(r => r.GroupRequests)));
			db.Lobby_Logins.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Lobby_Logins));
			db.Login_Roles.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Login_Roles));
			db.Login_UnlinkedLogins.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Login_UnlinkedLogins));
			db.Login_UnlinkedLogins.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Login_UnlinkedLogins1));
			db.MachineRecords.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.MachineRecords));
			db.PersonalMessages.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.PersonalMessages));
			db.PersonalMessages.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Aliases.SelectMany(r => r.PersonalMessages)));
			db.PollVotes.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.PollVotes));
			db.Sessions.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Sessions));
			db.UsedKeys.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.UsedKeys));

			db.Alias.DeleteAllOnSubmit(identity.Logins.SelectMany(p => p.Aliases));
			db.Logins.DeleteAllOnSubmit(identity.Logins);
			db.Identities.DeleteOnSubmit(identity);

			db.SubmitChanges();
		}

		public static void MatchIdentity(CSSDataContext db, Login login, MachineInformation info, out Identity principal, out bool wasMerged)
		{
			principal = login.Identity;
			wasMerged = false;

			var machineRecords = login.Identity.Logins.SelectMany(p => p.MachineRecords);
			foreach (var deviceInfo in info.MachineValues)
			{
				var existingMachineRecord = machineRecords.FirstOrDefault(
												p => p.RecordTypeId == (int) deviceInfo.Type 
												&& p.Identifier == deviceInfo.Value);

				if (existingMachineRecord == null)
				{
					login.MachineRecords.Add(new MachineRecord()
					{
						RecordTypeId = (int)deviceInfo.Type,
						Identifier = deviceInfo.Value,
						Login = login,
						LoginId = login.Id
					});
				}
			}

			// Find matching machine records for other logins. 
			List<MatchedMachineRecord> matchedMachineRecords = new List<MatchedMachineRecord>();
			List<Identity> matchingIdentities = new List<Identity>();
			matchingIdentities.Add(login.Identity);

			// Only match on hard drive serial numbers for now.
			var machineRecordsAttachedToLogin = login.Identity.Logins.SelectMany(p => p.MachineRecords).Where(p => p.DeviceType == DeviceType.HardDisk);

			var machineRecordsByIdentity = MachineRecordByIdentity.GetAllFromCache();

			foreach (var machineRecord in machineRecordsAttachedToLogin)
			{
				if (MachineRecordExclusion.IsMachineRecordExcluded(machineRecord) == true)
					continue;

				//var matchingMachineRecords = db.MachineRecords
				//    .Where(p => p.Login.Identity != login.Identity
				//        && p.RecordTypeId == machineRecord.RecordTypeId
				//        && p.Identifier == machineRecord.Identifier)
				//    .ToList() // Release from the database context so that AreIdentitiesPermanentlyUnlinked can be evaluated.
				//    .Where(p => AreIdentitiesPermanentlyUnlinked(db, p.Login.Identity, login.Identity) == false);

				var matchingMachineRecords = machineRecordsByIdentity
					.Where(p => p.IdentityId != login.IdentityId
						&& p.RecordTypeId == machineRecord.RecordTypeId
						&& p.Identifier == machineRecord.Identifier)
					.ToList() // Release from the database context so that AreIdentitiesPermanentlyUnlinked can be evaluated.
					.Where(p => AreIdentitiesPermanentlyUnlinked(db, p.IdentityId, login.IdentityId) == false);

				//matchedMachineRecords = matchingMachineRecords.Select(p => new MatchedMachineRecord()
				//{
				//    machineRecord1 = machineRecord,
				//    machineRecord2 = p
				//}).ToList();

				matchingIdentities.AddRange(
					db.Identities
						.Where(p => matchingMachineRecords.Select(r => r.IdentityId).Contains(p.Id))
						.ToList()
						);
				
				//matchingIdentities.AddRange(matchingMachineRecords.Select(p => p.Login.Identity).ToList());

				//var matchingMachineRecords = db.MachineRecords
				//    .Where(	p => p.RecordTypeId == machineRecord.RecordTypeId 
				//            && p.Identifier == machineRecord.Identifier
				//            && machineRecordsAttachedToLogin.Count(r => r.Identifier == p.Identifier && r.MachineRecordType == p.MachineRecordType) == 0);

				//foreach (var matchingMachineRecord in matchingMachineRecords)
				//{
				//    if (matchingIdentities.Contains(matchingMachineRecord.Login.Identity) == false)
				//    {
				//        matchingIdentities.Add(matchingMachineRecord.Login.Identity);

				//        matchedMachineRecords.Add(new MatchedMachineRecord()
				//        {
				//            machineRecord1 = machineRecord, 
				//            machineRecord2 = matchingMachineRecord
				//        });
				//    }
				//}
			}

			db.SubmitChanges();

			// If a matching machine record was found for a login that is not already linked to the principal identity, 
			// then link the login to the principal identity.
			if (matchingIdentities.Count > 1)
			{
				List<string> accounts = new List<string>();
				foreach (var joinedIdentity in matchingIdentities)
					accounts.Add("[" + String.Join(",", joinedIdentity.Logins.Select(p => p.Username).ToArray()) + "]");

				List<string> machineRecordMatches = new List<string>();
				foreach(var matchedMachineRecord in matchedMachineRecords)
					machineRecordMatches.Add(String.Format("Login: {0}, Id: {1}, Type: {2}, Value: {3} == Login: {4}, Id: {5}, Type: {6}, Value: {7}",
						matchedMachineRecord.machineRecord1.LoginId,
						matchedMachineRecord.machineRecord1.Id,
						matchedMachineRecord.machineRecord1.RecordTypeId,
						matchedMachineRecord.machineRecord1.Identifier,
						matchedMachineRecord.machineRecord2.LoginId,
						matchedMachineRecord.machineRecord2.Id,
						matchedMachineRecord.machineRecord2.RecordTypeId,
						matchedMachineRecord.machineRecord2.Identifier));

				principal = MergeIdentities(db, matchingIdentities);
				wasMerged = true;
				
				Log.Write(LogType.AuthenticationServer, "Accounts Merged by V3 Automation: "
					+ String.Join(",", accounts.ToArray())
					+ " -- Machine Record Matches: "
					+ String.Join(" -- ", machineRecordMatches.ToArray()));

				db.SubmitChanges();
			}
		}

		//public static Identity MatchIdentity_Old(CSSDataContext db, Login login, MachineInformation info)
		//{
		//    var principal = login.Identity;

		//    foreach (var i in info.MachineValues)
		//    {
		//        //Find all Machine Records in database which match this value
		//        var matches = MachineRecord.FindMatches(db, i).ToList();

		//        //If match is found, check associated identities,
		//        if (matches.Count > 0)
		//        {
		//            // Get all identities from the matching machine records.
		//            var identities = matches.Select(p => p.Login.Identity).Distinct().ToList();

		//            if (identities.Any(p => p.Id == principal.Id) == false)
		//                identities.Add(principal);

		//            if (identities.Count > 1)
		//            {
		//                List<string> accounts = new List<string>();
		//                foreach(var joinedIdentity in identities)
		//                    accounts.Add("[" + String.Join(",", joinedIdentity.Logins.Select(p => p.Username).ToArray()) + "]");
						
		//                principal = MergeIdentities(db, identities);

		//                Log.Write(LogType.AuthenticationServer, "Accounts Merged by Automation: "
		//                    + String.Join(",", accounts.ToArray()) 
		//                    + ", Matching machine record identifier = Name: " + i.Name + ", Type: " + i.Type + ", Value: " + i.Value);
		//            }
		//            //Otherwise, continue
		//            else
		//                continue;
		//        }
                
		//        //Otherwise, insert new records associated with principal
		//        else
		//        {
		//            var record = new MachineRecord()
		//            {
		//                DeviceType  = i.Type,
		//                Identifier  = i.Value,
		//                Login       = login
		//            };
		//            db.MachineRecords.InsertOnSubmit(record);
		//        }

		//        db.SubmitChanges();
		//    }

		//    return principal;
		//}

        public static Identity MergeIdentities(CSSDataContext db, IEnumerable<Identity> identities)
        {
            var principal   = identities.First();
            var filter      = identities.Where(p => p != principal);

			List<Identity> identitiesToDelete = new List<Identity>();
			List<LogIP> logIPsToDelete = new List<LogIP>();

            //Modify all references to identity to point to principal
            foreach (var ident in filter)
            {
				// If the identity has a login that has an unlinked login relation to the 
				// principal, then do not link the identity to the principal.
				if (AreIdentitiesPermanentlyUnlinked(db, principal.Id, ident.Id) == true)
					continue;

                principal.Logins.AddRange(ident.Logins);

				principal.LogIPs.AddRange(ident.LogIPs.Where(p => principal.LogIPs.Select(r => r.IPAddress).Contains(p.IPAddress) == false)
					.Select(r => new LogIP()
					{
						 IdentityId = principal.Id,
						 IPAddress = r.IPAddress,
						 LastAccessed = r.LastAccessed
					}));

				logIPsToDelete.AddRange(ident.LogIPs);

				identitiesToDelete.Add(ident);
            }

			db.LogIPs.DeleteAllOnSubmit(logIPsToDelete);
			db.Identities.DeleteAllOnSubmit(identitiesToDelete);

            return principal;
        }

		public static void UnlinkLogin(DataAccess.CSSDataContext db, Identity principal, Login loginToUnlink)
		{
			Identity newIdentity = new Identity()
			{
				DateLastLogin = DateTime.Now,
				LastGlobalMessageDelivery = DateTime.Now
			};

			db.Identities.InsertOnSubmit(newIdentity);

			newIdentity.Logins.Add(loginToUnlink);

			db.SubmitChanges();
		}

		//private static void CreateAccountLinkLog(DataAccess.CSSDataContext db, Identity principal, Identity ident)
		//{
		//    Link link = new Link()
		//    {
		//        DateCreated = DateTime.Now,
		//        IdentityId = principal.Id
		//    };

		//    db.Links.InsertOnSubmit(link);
		//    db.SubmitChanges();

		//    foreach (var login in ident.Logins)
		//    {
		//        db.LinkedItems.InsertOnSubmit(new LinkedItem()
		//        {
		//            LinkedItemTypeId = (int)Enumerations.LinkedItemType.Login,
		//            TargetId = login.Id,
		//            LinkId = link.Id
		//        });
		//    }

		//    foreach (var machineRecord in ident.MachineRecords)
		//    {
		//        db.LinkedItems.InsertOnSubmit(new LinkedItem()
		//        {
		//            LinkedItemTypeId = (int)Enumerations.LinkedItemType.MachineRecord,
		//            TargetId = machineRecord.Id,
		//            LinkId = link.Id
		//        });
		//    }

		//    foreach (var pollVote in ident.PollVotes)
		//    {
		//        db.LinkedItems.InsertOnSubmit(new LinkedItem()
		//        {
		//            LinkedItemTypeId = (int)Enumerations.LinkedItemType.PollVote,
		//            TargetId = pollVote.Id,
		//            LinkId = link.Id
		//        });
		//    }

		//    foreach (var ban in ident.Bans)
		//    {
		//        db.LinkedItems.InsertOnSubmit(new LinkedItem()
		//        {
		//            LinkedItemTypeId = (int)Enumerations.LinkedItemType.Ban,
		//            TargetId = ban.Id,
		//            LinkId = link.Id
		//        });
		//    }				
		//}

		//private static bool AreIdentitiesPermanentlyUnlinked(CSSDataContext db, Identity identity1, Identity identity2)
		//{
		//    bool foundUnlinkedLoginRecord = false;
		//    foreach (var login1 in identity1.Logins)
		//    {
		//        foreach (var login2 in identity2.Logins)
		//        {
		//            if (db.Login_UnlinkedLogins.Where(p => (p.LoginId1 == login1.Id && p.LoginId2 == login2.Id) || (p.LoginId2 == login1.Id && p.LoginId1 == login2.Id)).Count() > 0)
		//            {
		//                foundUnlinkedLoginRecord = true;
		//                break;
		//            }
		//        }

		//        if (foundUnlinkedLoginRecord == true)
		//            break;
		//    }

		//    return foundUnlinkedLoginRecord;
		//}

		private static bool AreIdentitiesPermanentlyUnlinked(CSSDataContext db, int identityId1, int identityId2)
		{
			List<Login_UnlinkedLogin> login_UnlinkedLogins = Login_UnlinkedLogin.GetAllFromCache();

			List<Login> allLogins = Login.GetAllFromCache();

			bool foundUnlinkedLoginRecord = false;
			foreach (var login1 in db.Logins.Where(p => p.IdentityId == identityId1))
			{
				foreach (var login2 in db.Logins.Where(p => p.IdentityId == identityId2))
				{
					if (login_UnlinkedLogins.Where(p => (p.LoginId1 == login1.Id && p.LoginId2 == login2.Id) || (p.LoginId2 == login1.Id && p.LoginId1 == login2.Id)).Count() > 0)
					{
						foundUnlinkedLoginRecord = true;
						break;
					}
				}

				if (foundUnlinkedLoginRecord == true)
					break;
			}

			return foundUnlinkedLoginRecord;
		}

        #endregion

		/// <summary>
		/// Returns true if any login associated with this user's identity has logged in with 
		/// a real physical machine.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="login"></param>
		/// <returns></returns>
		public static bool CanUseVirtualMachine(CSSDataContext db, Login login)
		{
			return login.Identity.Logins.Count(p => p.AllowVirtualMachineLogin == true) > 0;
		}

		/// <summary>
		/// Returns true if the username is linked to another account, and the username is not the oldest account.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public static bool IsLinkedToAnOlderAccount(CSSDataContext db, string username)
		{
			var alias = Alias.GetAliasByCallsign(db, username);

			if (alias != null)
			{
				if (alias.Login.Identity.Logins.Count > 1)
				{
					if (alias.Login != alias.Login.Identity.Logins.OrderBy(p => p.DateCreated).FirstOrDefault())
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the username of the oldest linked account to the username passed.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public static string GetOldestLinkedAcccountUsername(CSSDataContext db, string username)
		{
			var alias = Alias.GetAliasByCallsign(db, username);

			var oldestLogin = alias.Login.Identity.Logins.OrderBy(p => p.DateCreated).FirstOrDefault(p => p != alias.Login);

			return oldestLogin.Username;
		}

		public static void MergeLogin(CSSDataContext db, Login principalLogin, Login loginToMerge)
		{
			foreach (var item in loginToMerge.Aliases)
				item.LoginId = principalLogin.Id;

			foreach (var item in loginToMerge.Bans)
				item.LoginId = principalLogin.Id;

			foreach (var item in loginToMerge.IssuedBans)
				item.BannedByLoginId = principalLogin.Id;

			foreach (var item in loginToMerge.Login_UnlinkedLogins.Where(p => p.LoginId1 == loginToMerge.Id))
				item.LoginId1 = principalLogin.Id;

			foreach (var item in loginToMerge.Login_UnlinkedLogins.Where(p => p.LoginId2 == loginToMerge.Id))
				item.LoginId2 = principalLogin.Id;

			foreach (var item in loginToMerge.MachineRecords)
				item.LoginId = principalLogin.Id;

			foreach (var item in loginToMerge.PersonalMessages)
				item.LoginId = principalLogin.Id;

			foreach (var item in loginToMerge.PollVotes)
				item.LoginId = principalLogin.Id;

			db.Sessions.DeleteAllOnSubmit(loginToMerge.Sessions);
			//foreach (var item in loginToMerge.Sessions)
			//    item.LoginId = principalLogin.Id;

			Log.Write(LogType.ManagementWeb, "User " + loginToMerge.Username + " was merged to login " + principalLogin.Username);

			db.Login_Roles.DeleteAllOnSubmit(loginToMerge.Login_Roles);
			db.UsedKeys.DeleteAllOnSubmit(loginToMerge.UsedKeys);
			db.Lobby_Logins.DeleteAllOnSubmit(loginToMerge.Lobby_Logins);
			db.Logins.DeleteOnSubmit(loginToMerge);

			db.SubmitChanges();
		}
	}
}