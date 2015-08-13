using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Stats.Data
{
	public class MemberData
	{
		public string Token { get; set; }
		public string Callsign { get; set; }
		public Boolean IsActive { get; set; }

		public string ActiveCssClass
		{
			get
			{
				if (IsActive == true)
					return "";
				else
					return "inactive";
			}
		}

		public string TokenHtml
		{
			get
			{
				if (String.IsNullOrEmpty(Token) == true)
					return "&nbsp;";
				else
					return Token;
			}
		}

		public int TokenValue
		{
			get
			{
				if (Token == "*")
					return 1;
				else if (Token == "^")
					return 2;
				else
					return 3;
			}
		}
	}
}
