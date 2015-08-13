using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Stats.Data
{
	public class BanData
	{
		public string Username { get; set; }
		public string BannedBy { get; set; }
		public string Reason { get; set; }
		public string DateCreated { get; set; }
		public string TimeLeft { get; set; }
		public string Duration { get; set; }
	}
}
