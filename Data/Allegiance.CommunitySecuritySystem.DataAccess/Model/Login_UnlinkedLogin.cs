using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class Login_UnlinkedLogin
	{
		public static List<Login_UnlinkedLogin> GetAllFromCache()
		{
			return CacheManager<List<Login_UnlinkedLogin>>.Get("DataAccessIdentity::AreIdentitiesPermanentlyUnlinked:Login_UnlinkedLogins", CacheSeconds.TenSeconds, delegate()
			{
				using (CSSDataContext db = new CSSDataContext())
				{
					return db.Login_UnlinkedLogins.ToList();
				}
			});
		}
	}
}
