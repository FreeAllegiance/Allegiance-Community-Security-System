using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class LogIP
	{
		public static void Clean()
		{
			using (var db = new CSSDataContext())
			{
				db.LogIPs.DeleteAllOnSubmit(db.LogIPs.Where(p => p.LastAccessed < DateTime.Now.AddDays(-180)));
				db.SubmitChanges();
			}
		}
	}
}
