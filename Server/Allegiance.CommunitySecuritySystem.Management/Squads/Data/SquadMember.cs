using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Squads.Data
{
	public class SquadMember
	{
		public string Callsign { get; set; }
		public char Token { get; set; }
		public int SelectedRoleID { get; set; }
		public int GroupID { get; set; }
		public int AliasID { get; set; }
		public string SelectedRoleName { get; set; }
	}
}
