using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Common.Utility
{
	public class UnixDateTime
	{
		//http://codeclimber.net.nz/archive/2007/07/10/convert-a-unix-timestamp-to-a-.net-datetime.aspx
		public static DateTime ConvertFromUnixTimestamp(double timestamp)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return origin.AddSeconds(timestamp);
		}

		//http://codeclimber.net.nz/archive/2007/07/10/convert-a-unix-timestamp-to-a-.net-datetime.aspx
		public static double ConvertToUnixTimestamp(DateTime date)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeSpan diff = date - origin;
			return Math.Floor(diff.TotalSeconds);
		}
	}
}
