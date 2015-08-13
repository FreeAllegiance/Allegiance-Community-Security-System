using System.Linq;
using System.Collections.Generic;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Lobby
    {
        #region Properties

        public IEnumerable<Login> Logins
        {
            get { return this.Lobby_Logins.Select(p => p.Login); }
        }

        #endregion

        #region Methods

        public static IQueryable<Lobby> FindAvailable(CSSDataContext db, Login account)
        {
            return from l in db.Lobbies
                   where l.IsEnabled &&
                   (
                        !l.IsRestrictive ||
                        l.Lobby_Logins.Any(p => p.Login == account)
                   )
                   select l;
        }

        #endregion
    }
}