using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Users.Data
{
	public class EditableUser
	{
		public int Id {get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime LastLogin { get; set; }
		public string LinkManagementLabel { get; set; }
	}
}
