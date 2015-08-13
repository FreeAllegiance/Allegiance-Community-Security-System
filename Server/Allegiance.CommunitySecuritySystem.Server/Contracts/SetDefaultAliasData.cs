using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class SetDefaultAliasData : AuthenticatedData
    {
        #region Properties

        [DataMember]
        public int AliasId { get; set; }

        #endregion

        #region Methods

        internal bool SetDefaultAlias()
        {
            using (var db = new CSSDataContext())
            {
				
				var login = Login.FindLoginByUsernameOrCallsign(db, this.Username);
				var alias = login.Identity.Logins.SelectMany(p => p.Aliases).FirstOrDefault(p => p.Id == AliasId);

				if (alias != null)
				{
					alias.IsDefault = true;

					foreach (var otherAlias in login.Identity.Logins.SelectMany(p => p.Aliases).Where(p => p.Id != AliasId))
						otherAlias.IsDefault = false;

					db.SubmitChanges();

					return true;
				}

				return false;
            }
        }

        #endregion
    }
}