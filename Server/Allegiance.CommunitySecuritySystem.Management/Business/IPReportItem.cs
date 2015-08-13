using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public class IPReportItem
	{
		public string IPAddress { get; set; }
		public string User1 { get; set; }
		public string User1Encoded
		{
			get
			{
				return System.Web.HttpUtility.UrlEncode(User1);
			}
		}

		public DateTime User1Date { get; set; }
		public string User2 { get; set; }
		public string User2Encoded
		{
			get
			{
				return System.Web.HttpUtility.UrlEncode(User2);
			}
		}
		public DateTime User2Date { get; set; }
	}
}