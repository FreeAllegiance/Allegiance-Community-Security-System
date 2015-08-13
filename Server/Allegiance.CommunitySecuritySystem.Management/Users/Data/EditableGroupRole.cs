using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Users.Data
{
	public class EditableGroupRole
	{
		public int GroupID { get; set; }
		public int AliasID { get; set; }
		public string GroupName { get; set; }
		public int SelectedRoleID { get; set; }
		public char? Token { get; set; }
		public string Tag { get; set; }
		//public EditableRole[] AvailableRoles { get; set; }
	}
}
