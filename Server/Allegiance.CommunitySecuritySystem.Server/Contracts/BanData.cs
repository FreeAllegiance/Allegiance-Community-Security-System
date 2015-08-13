using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Authentication;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Data.SqlTypes;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class BanData : AuthenticatedData
    {
        #region Properties

        [DataMember]
        public int? BanId { get; set; }

        [DataMember]
        public int? BanTypeId { get; set; }

        [DataMember]
        public string Alias { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public BanMode BanMode { get; set; }

        [DataMember]
        public TimeSpan? Duration { get; set; }

        #endregion

        #region Methods

        internal bool SetBan()
        {
            using (var db = new CSSDataContext())
            {
                Ban ban = null;
                if (BanId != null)
                    ban = db.Bans.Single(p => p.Id == BanId);

				Login admin;
				LoginStatus loginStatus;
				if (Login.TryGetAuthenticatedLogin(db, Username, Password, out admin, out loginStatus) == false)
					return false;

				var bannedLogin = Login.FindLoginByUsernameOrCallsign(db, Alias);
                if(bannedLogin == null)
                    return false;

				var bannedAlias = DataAccess.Alias.GetAliasByCallsign(db, Alias);
				if (bannedAlias == null)
					return false;

                var bannedIdentity = bannedLogin.Identity;

                DateTime? dateExpires = null;
                BanType banType = null;
                if (BanMode == BanMode.Auto && BanTypeId != null)
                {
					banType = db.BanTypes.FirstOrDefault(p => p.Id == BanTypeId);
					var duration = Ban.CalculateDuration(bannedIdentity, banType);

					if (duration == TimeSpan.MaxValue)
						dateExpires = SqlDateTime.MaxValue.Value;
					else
						dateExpires = DateTime.Now.Add(duration.Value);
                }
				else if (BanMode == BanMode.Custom && Duration != null)
				{
					if (Duration.Value == TimeSpan.MinValue)
						dateExpires = SqlDateTime.MinValue.Value;
					else
						dateExpires = DateTime.Now.Add(Duration.Value);
				}
				else if (BanMode != BanMode.Permanent)
					throw new Exception("One or more needed parameters were not provided.");
				else if (BanMode == BanMode.Permanent && !admin.Login_Roles.Any(p => p.RoleId == (int)RoleType.SuperAdministrator))
					throw new AuthenticationException("Only Super-Administrators may permanently ban a user.");

                if(BanId == null)
                {
                    ban = new Ban()
                    {
                        DateCreated     = DateTime.Now,
                        DateExpires     = dateExpires,
                        BannedByLoginId = admin.Id,
						LoginId			= bannedLogin.Id,
                        Reason          = Reason,
                        BanType			= banType,
                        InEffect        = true,
						AliasID			= bannedAlias.Id
                    };
                    db.Bans.InsertOnSubmit(ban);

					// Kill off any active sessions for the user which will log them out of the game immediatly.
					db.Sessions.DeleteAllOnSubmit(db.Sessions.Where(p => p.LoginId == bannedLogin.Id));
                }
                else
                {
                    if (!admin.Login_Roles.Any(p => p.RoleId == (int)RoleType.SuperAdministrator))
                        throw new AuthenticationException("Only Super-Administrators may adjust a ban.");

                    if (Duration == null)
                        throw new Exception("Duration must be specified in order to edit a ban.");

                    if(!string.IsNullOrEmpty(Reason))
                        ban.Reason = Reason;

                    if (BanTypeId != null)
                        banType = db.BanTypes.FirstOrDefault(p => p.Id == BanTypeId);
                    else
						banType = null;

					ban.BanType = banType;

					if (Duration.Value == TimeSpan.MinValue)
						ban.DateExpires = SqlDateTime.MinValue.Value;
					else
						ban.DateExpires = DateTime.Now.Add(Duration.Value);
                }

                db.SubmitChanges();
                return true;
            }
        }

        internal void RemoveBan()
        {
            using (var db = new CSSDataContext())
            {
				// If the ban is deleted, then the ban history would be lost.
				// Disable ban instead.
                //db.Bans.DeleteOnSubmit(db.Bans.Single(p => p.Id == BanId));
				
				var targetBan = db.Bans.FirstOrDefault(p => p.Id == BanId);
				if (targetBan != null)
				{
					targetBan.InEffect = false;
					db.SubmitChanges();
				}
            }
        }

        internal static List<Ban> ListBans(string alias)
        {
            using (var db = new CSSDataContext())
            {
                var login = Login.FindLoginByUsernameOrCallsign(db, alias);
                return login.Identity.Bans.ToList();
            }
        }

        #endregion
    }
}