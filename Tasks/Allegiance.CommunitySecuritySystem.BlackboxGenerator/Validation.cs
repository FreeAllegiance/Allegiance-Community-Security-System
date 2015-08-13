using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess.MembershipProviders;

namespace Allegiance.CommunitySecuritySystem.BlackboxGenerator
{
    public static class Validation
    {



        #region Methods

		public static LoginStatus CreateSession(string ipAddress, string username,
		   string password, bool debugMode, ref string callsignWithTags, out byte[] blackBoxData)
		{
			return CreateSession(ipAddress, username, password, null, debugMode, ref callsignWithTags, out blackBoxData);
		}

        /// <summary>
        /// Creates a new session for these credentials; with a status of
        /// pending verification.
        /// </summary>
		public static LoginStatus CreateSession(string ipAddress, string username, 
            string password, int? lobbyId, bool debugMode, ref string callsignWithTags, out byte[] blackBoxData)
        {
            using (var db = new CSSDataContext())
            {
                blackBoxData = null;

				Login login;
				LoginStatus loginStatus;
				if (Login.TryGetAuthenticatedLogin(db, username, password, out login, out loginStatus) == false)
					return loginStatus;

				if (lobbyId != null)
				{
					//Ensure that the user has permission to log into this lobby
					var lobby = db.Lobbies.FirstOrDefault(p => p.Id == lobbyId.GetValueOrDefault(0));
					if (lobby == null || !lobby.IsEnabled)
						return LoginStatus.PermissionDenied;
					else if (lobby.IsRestrictive && !login.Lobby_Logins.Any(p => p.LobbyId == lobbyId.GetValueOrDefault(0)))
						return LoginStatus.PermissionDenied;
				}

				// Ensure that the alias is in the user's list of available callsigns.
				var validAliases = login.Identity.Logins.SelectMany(p => p.Aliases).OrderBy(p => p.DateCreated).Take(Alias.GetAliasLimit(db, login));
				if (validAliases.Select(p => p.Callsign).Contains(Alias.GetCallsignFromStringWithTokensAndTags(db, callsignWithTags)) == false)
				{
					if (validAliases.Count() > 0)
						callsignWithTags = validAliases.FirstOrDefault().Callsign;
					else
						return LoginStatus.InvalidCredentials;
				}

                //Ensure that the alias is available for login
				Alias alias;
                if(Alias.ValidateUsage(db, login, true, password, ref callsignWithTags, out alias) != CheckAliasResult.Registered)
                    return LoginStatus.InvalidCredentials;

                //Destroy any existing sessions for this account
                db.Sessions.DeleteAllOnSubmit(login.Sessions);

                //Retrieve a blackbox for this user, if none exists, create one.
                var firstAvailableKey = RetrieveUnusedKey(db, login);

                if (firstAvailableKey == null)
					firstAvailableKey = Task.GenerateBlackbox(db, debugMode);

                var root        = ConfigurationManager.AppSettings["OutputRoot"];
                var path        = Path.Combine(root, firstAvailableKey.Filename);
                blackBoxData    = File.ReadAllBytes(path);

                //Create a record of the usage of this key.
                var usedKey = new UsedKey()
                {
                    Login       = login,
                    ActiveKey   = firstAvailableKey,
                    DateUsed    = DateTime.Now
                };
                db.UsedKeys.InsertOnSubmit(usedKey);

				//string ipAddress = string.Empty;
				//var context = OperationContext.Current;
				//if (context != null)
				//{
				//    var remoteEndPoint = context
				//        .IncomingMessageProperties.[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
				//    ipAddress = remoteEndPoint.Address;
				//}

                //Create a new session for this login
                login.Identity.DateLastLogin = DateTime.Now;
                var session = new Session()
                {
                    Login               = login,
                    Id                  = Guid.NewGuid(),
                    DateLastCheckIn     = DateTime.Now,
                    SessionStatusType   = SessionStatusEnum.PendingVerification,
                    ActiveKeyId         = firstAvailableKey.Id,
                    IPAddress           = ipAddress,
					AliasId				= alias.Id
                };
                db.Sessions.InsertOnSubmit(session);

				// Add the identity's IP address to the log.
				var existingLogIP = db.LogIPs.FirstOrDefault(p => p.IdentityId == login.IdentityId && p.IPAddress == ipAddress);

				if (existingLogIP != null)
				{
					existingLogIP.LastAccessed = DateTime.Now;
				}
				else
				{
					existingLogIP = new LogIP()
					{
						LastAccessed = DateTime.Now,
						IdentityId = login.IdentityId,
						IPAddress = ipAddress
					};

					db.LogIPs.InsertOnSubmit(existingLogIP);
				}

                db.SubmitChanges();

                return LoginStatus.Authenticated;
            }
        }

        /// <summary>
        /// Finds associated active key for these credentials and then
        /// decrypts the envelope.
        /// </summary>
        /// <param name="machineInfo">Resultant Machine Information retrieved from envelope.</param>
        public static CheckInStatus CheckIn(string username, string password, byte[] encryptedEnvelope, out Session session)
        {
            using (var db = new CSSDataContext())
            {
                var succeeded   = false;
                session         = null;

                MachineInformation machineInfo = null;

                //Check if username/password is valid, and account is not banned/locked.
				Login login;
				LoginStatus loginStatus;
				if (Login.TryGetAuthenticatedLogin(db, username, password, out login, out loginStatus) == false)
					return CheckInStatus.InvalidCredentials;

                //Find user's session
                var currentSession = login.FindCurrentSession();

                //If session is not found, envelope cannot be decrypted.
                if (currentSession == null)
                    return CheckInStatus.InvalidCredentials;

                try
                {
                    //Retrieve the machine information
                    var activeKey       = currentSession.ActiveKey;
                    var decryptedData   = Encryption.DecryptRSA(encryptedEnvelope, activeKey.RSACspBlob.ToArray());
                    machineInfo         = MachineInformation.Deserialize(decryptedData);

                    //Record machine info & match identities
					bool wasMerged;
					Identity identity;
					Identity.MatchIdentity(db, login, machineInfo, out identity, out wasMerged);

                    if (login.Identity != identity)
                        login.Identity = identity;

					if (VirtualMachineMarker.IsMachineInformationFromAVirtualMachine(db, machineInfo, login) == true)
					{
						if (Identity.CanUseVirtualMachine(db, login) == false)
							return CheckInStatus.VirtualMachineBlocked;
					}
					else
					{
						if (login.AllowVirtualMachineLogin == false)
							login.AllowVirtualMachineLogin = true;
					}

                    //Check if black box has timed out
                    if (!currentSession.ValidateSessionTimeout())
                        return CheckInStatus.Timeout;
                    else //Otherwise, ensure session status is marked active
                        currentSession.SessionStatusType = SessionStatusEnum.Active;

                    //Verify the token is correct
                    if (machineInfo.Token != activeKey.Token)
                        return CheckInStatus.InvalidHash;

                    //Verify the login is not banned
                    if (login.IsBanned)
                        return CheckInStatus.InvalidCredentials;

                    //Check-in succeeded
                    if (currentSession.SessionStatusType == SessionStatusEnum.Active)
                    {
                        succeeded                       = true;
                        currentSession.DateLastCheckIn  = DateTime.Now;
                        session                         = currentSession;

						if (wasMerged == true)
							return CheckInStatus.AccountLinked;
						else
							return CheckInStatus.Ok;
                    }

                    return CheckInStatus.InvalidCredentials;
                }
                catch (Exception error)
                {
                    Error.Write(error);
                    return CheckInStatus.InvalidHash;
                }
                finally
                {
                    //If not successful, mark session closed.
                    if (!succeeded)
                        currentSession.SessionStatusType = SessionStatusEnum.Closed;

                    db.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Ensure that the alias selected is valid for the input credentials
        /// </summary>
		public static CheckAliasResult CreateAlias(string username, string password, string callsignWithTags, string legacyPassword)
        {
            using (var db = new CSSDataContext())
            {
				Login login;
				LoginStatus loginStatus;
				if (Login.TryGetAuthenticatedLogin(db, username, password, out login, out loginStatus) == false)
					return CheckAliasResult.InvalidLogin;

				Alias alias;
                return Alias.ValidateUsage(db, login, true, legacyPassword, ref callsignWithTags, out alias);
            }
        }

        /// <summary>
        /// Ensure that the alias selected is valid for the input credentials
        /// </summary>
		public static CheckAliasResult ValidateAlias(string username, string password, string callsignWithTags)
        {
            using (var db = new CSSDataContext())
            {
				Login login;
				LoginStatus loginStatus;
				if (Login.TryGetAuthenticatedLogin(db, username, password, out login, out loginStatus) == false)
					return CheckAliasResult.InvalidLogin;

                Alias alias;
				return Alias.ValidateUsage(db, login, false, password, ref callsignWithTags, out alias);
            }
        }

		public static CheckAliasResult ValidateAlias(string callsignWithTags)
		{
			using (var db = new CSSDataContext())
			{
				Alias alias;
				return Alias.ValidateUsage(db, ref callsignWithTags, out alias);
			}
		}

        /// <summary>
        /// Ensure the credentials are correct and that user is a member of all specified roles.
        /// </summary>
		public static bool ValidateLogin(string usernameOrCallsign, string password, out string username, params RoleType[] requiredRoles)
        {
			username = null;

            using (var db = new CSSDataContext())
            {
				Login login;
				LoginStatus loginStatus;
				if (Login.TryGetAuthenticatedLogin(db, usernameOrCallsign, password, out login, out loginStatus) == false)
					return false;

				username = login.Username;

                if (login == null)
                    return false;

                //If roles are specified, ensure user has all roles
                if (requiredRoles.Length > 0)
                    return login.HasAnyRole(requiredRoles);

                return true;
            }
        }

        public static bool ValidateSession(string alias, string ticket, string ipAddress)
        {
            using (var db = new CSSDataContext())
            {
                var login = Login.FindLoginByUsernameOrCallsign(db, alias);
                if (login != null)
                {
                    var session = login.FindCurrentSession();

					if (session == null)
						return false;

					//bool ipAddressesMatch = session.IPAddress.Equals(ipAddress);
					bool ipAddressesMatch = true;

#if DEBUG
					// When debugging with a local lobby, it's going to randomly pick
					// an interface IP. Sometimes the launcher uses 127.0.0.1 while the 
					// lobby sees the external IP.
					if (ipAddressesMatch == false && session.IPAddress == "127.0.0.1")
						ipAddressesMatch = true;
#endif

                    var valid = session != null
                        && session.ValidateSessionTimeout()
                        && session.SessionStatusType == SessionStatusEnum.Active
                        && session.HashSession() == ticket
						&& ipAddressesMatch;

                    //Validate session
                    if (valid)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Destroy all sessions associated with this login.
        /// </summary>
        public static void EndSession(string username, string password)
        {
            using (var db = new CSSDataContext())
            {
				Login login;
				LoginStatus loginStatus;
				if (Login.TryGetAuthenticatedLogin(db, username, password, out login, out loginStatus) == true)
				{
					db.Sessions.DeleteAllOnSubmit(login.Sessions);
					db.SubmitChanges();
				}
            }
        }

        /// <summary>
        /// Retrieve an unused ActiveKey for this Login.
        /// </summary>
        private static ActiveKey RetrieveUnusedKey(CSSDataContext db, Login login)
        {
            var random          = new Random();
            var earliest        = DateTime.Now.AddHours(-ActiveKey.PreferredMinLifetime);
            var availableKeys   = db.AvailableKey(login.Id)
                                    .Where(p => p.DateCreated > earliest && p.IsValid == true);
            var length          = availableKeys.Count();

            if (length == 0)
                return null;

            //Find a key (at random) which this Login has not already used.
            var index       = random.Next(0, length - 1);
            var keyresult   = availableKeys.Skip(index).Take(1).FirstOrDefault();

            return db.ActiveKeys.FirstOrDefault(p => p.Id == keyresult.Id);
        }

        #endregion


	}
}