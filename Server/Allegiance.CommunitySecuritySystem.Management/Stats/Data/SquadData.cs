using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Stats.Data
{
	public class SquadData
	{
		public string SquadName { get; set; }


		private List<MemberData> _members = new List<MemberData>();
		public List<MemberData> Members
		{
			get
			{
				return _members;
			}
			set
			{
				_members = value;
			}
		}

		public int ActiveMembers
		{
			get
			{
				return _members.Where(p => p.IsActive == true).Count();
			}
		}

		public int InactiveMembers
		{
			get
			{
				return _members.Where(p => p.IsActive == false).Count();
			}
		}
	}
}
