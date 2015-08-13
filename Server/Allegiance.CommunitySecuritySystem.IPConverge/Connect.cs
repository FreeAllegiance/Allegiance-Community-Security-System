using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.DataAccess;
using CookComputing.XmlRpc;

namespace Allegiance.CommunitySecuritySystem.Common.IPConverge
{
	public class Connect
	{
		public static DataAccess.IPConverge GetIPConverge()
		{
			using (var db = new CSSDataContext())
			{
				var ipConverge = db.IPConverges.FirstOrDefault();

				if (ipConverge == null)
					throw new Exception("IP.Converge Handshake has not been performed.");

				return ipConverge;
			}
		}

		public static DataAccess.IPConverge GetIPConverge(CSSDataContext db, string apiCode, string productId)
		{
			return GetIPConverge(db, apiCode, Int32.Parse(productId));
		}

		public static DataAccess.IPConverge GetIPConverge(CSSDataContext db, string apiCode, int productId)
		{
			var ipConverge = db.IPConverges.FirstOrDefault(p => p.ApiCode == apiCode && p.ProductId == productId);

			if (ipConverge == null)
				throw new Exception("Invalid API Code.");

				//throw new XmlRpcFaultException(500, "Invalid API Code.");

			return ipConverge;
		}

		public bool Authenticate(string username, string hashedPassword, out AuthenticationStatus authenticationStatus, out string email)
		{
			var ipConverge = GetIPConverge();

			IIPConvergeServer server = XmlRpcProxyGen.Create<IIPConvergeServer>();

			server.Url = ipConverge.Url + "/converge_master/converge_server.php";
			server.NonStandard = XmlRpcNonStandard.All;

			var authenticationRequest = new AuthenticateRequest();

			authenticationRequest.auth_key = ipConverge.ApiCode;
			authenticationRequest.email_address = String.Empty; // "wlnp03@gmail.com";
			authenticationRequest.md5_once_password = hashedPassword;
			authenticationRequest.product_id = ipConverge.ProductId.ToString();
			authenticationRequest.username = username;

			AuthenticateResponse response = server.Authenticate(authenticationRequest);

			authenticationStatus = response.AuthenticationStatus;
			email = response.email;

			return response.AuthenticationStatus == AuthenticationStatus.Success;
		}

	}
}
