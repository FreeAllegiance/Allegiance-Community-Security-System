using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CookComputing.XmlRpc;
using System.Runtime.InteropServices;

namespace Allegiance.CommunitySecuritySystem.Common.IPConverge
{
	internal interface IIPConvergeServer : IXmlRpcProxy
	{
		[XmlRpcMethod("convergeAuthenticate")]
		AuthenticateResponse Authenticate(AuthenticateRequest loginRequest);
	}

	public enum AuthenticationStatus
	{
		Success,
		WrongAuth,
		FlaggedRemote
	}

	public class AuthenticateRequest
	{
		public string auth_key { get; set; }
		public string product_id { get; set; }
		public string email_address { get; set; }
		public string md5_once_password { get; set; }
		public string username { get; set; }
	}

	public class AuthenticateResponse
	{
		public int complete { get; set; }
		public string response { get; set; }
		public string username { get; set; }
		public string email { get; set; }
		public string ipaddress { get; set; }
		public string joined { get; set; }

		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public AuthenticationStatus AuthenticationStatus
		{
			get
			{
				switch (response.ToUpper())
				{
					case "SUCCESS":
						return AuthenticationStatus.Success;

					case "FLAGGED_REMOTE":
						return AuthenticationStatus.FlaggedRemote;

					default:
						return AuthenticationStatus.WrongAuth;
				}
			}
		}
	}
}