using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.DataAccess;
using CookComputing.XmlRpc;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.DataAccess.IPConvergeProvider
{
	public class Connect
	{
		private static IPConverge GetIPConverge()
		{
			using (var db = new CSSDataContext())
			{
				var ipConverge = db.IPConverges.FirstOrDefault();

				if (ipConverge == null)
					throw new Exception("IP.Converge Handshake has not been performed.");

				return ipConverge;
			}
		}


		private static IIPConvergeServer GetIPConvergeServer(RequestBase request)
		{
			var ipConverge = GetIPConverge();

			IIPConvergeServer server = XmlRpcProxyGen.Create<IIPConvergeServer>();

			server.Url = ipConverge.Url + "/converge_master/converge_server.php";
			server.NonStandard = XmlRpcNonStandard.All;

			request.auth_key = ipConverge.ApiCode;
			request.product_id = ipConverge.ProductId.ToString();

			return server;
		}


		public bool Authenticate(string username, string hashedPassword, out AuthenticationStatus authenticationStatus, out string email)
		{
			var authenticationRequest = new AuthenticateRequest();

			var server = GetIPConvergeServer(authenticationRequest);

			authenticationRequest.email_address = String.Empty; // "wlnp03@gmail.com";
			authenticationRequest.md5_once_password = hashedPassword.ToLower();
			authenticationRequest.username = username;

			AuthenticateResponse response = server.Authenticate(authenticationRequest);

			authenticationStatus = response.AuthenticationStatus;
			email = response.email;

			return response.AuthenticationStatus == AuthenticationStatus.Success;
		}

		public bool ChangePassword(string email, string hashedPassword)
		{
			var changePasswordRequest = new ChangePasswordRequest();
			var server = GetIPConvergeServer(changePasswordRequest);

			changePasswordRequest.email_address = email;
			changePasswordRequest.md5_once_password = hashedPassword.ToLower();
			changePasswordRequest.extra = String.Empty;

			var response = server.ChangePassword(changePasswordRequest);

			return response.ChangePasswordStatus == ChangePasswordStatus.Success;
		}

		public bool CheckEmail(string email)
		{
			var request = new CheckEmailRequest();
			var server = GetIPConvergeServer(request);

			request.email_address = email;

			var response = server.CheckEmail(request);

			return response.CheckEmailStatus == CheckEmailStatus.EmailNotInUse;
		}

		public bool CheckUsername(string username)
		{
			var request = new CheckUsernameRequest();
			var server = GetIPConvergeServer(request);

			request.username = username;

			var response = server.CheckUsername(request);

			return response.CheckUsernameStatus == CheckUsernameStatus.UsernameNotInUse;
		}

		public bool AddMember(string email, string username, string hashedPassword, string ipAddress)
		{
			var request = new AddMemberRequest();
			var server = GetIPConvergeServer(request);

			request.email_address = email;
			request.username = username;
			request.md5_once_password = hashedPassword.ToLower();
			request.ip_address = ipAddress;
			request.unix_join_date = UnixDateTime.ConvertToUnixTimestamp(DateTime.Now);
			request.validating = 0;
			request.extra = String.Empty;
			request.flag = 0;

			var response = server.AddMember(request);

			return response.AddMemberStatus == AddMemberStatus.Success;
		}

	}
}
