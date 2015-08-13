using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer.Data
{
	public class Player
	{
		public int LoginId { get; set; }
		public string Callsign { get; set; }
		public DateTime LastContact { get; set; }
		public bool IsBanned { get; set; }
		public string BanImage { get; set; }
		public bool CanBeUnbanned { get; set; }
	}
}
