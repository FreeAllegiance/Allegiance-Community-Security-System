using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public static class Format
	{
		public static string DateTime(DateTime dateToFormat)
		{
			return dateToFormat.ToString("MM/dd/yyyy - HH:mm:ss");
		}
	}
}
