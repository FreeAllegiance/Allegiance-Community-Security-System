using System;
using System.Web;
using System.IO;
using System.Text;
using System.Linq;
using CookComputing.XmlRpc;
using Allegiance.CommunitySecuritySystem.Server.Properties;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.IPConvergeProvider;
using System.Collections;
using Allegiance.CommunitySecuritySystem.Server.Utilities;



namespace Allegiance.CommunitySecuritySystem.Server
{
	public class IPConvergeHandler : XmlRpcService, IHttpHandler //, IIPConvergeHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			LogIncomingRequest(context);

			if (String.IsNullOrEmpty(context.Request["info"]) == false)
			{
				StringBuilder output = new StringBuilder();

				output.AppendLine("<info>");
				output.AppendLine("\t<productname>ACSS IP.Converge Integration</productname>");
				output.AppendLine("\t<productcode>AADD29C0-94FB-4BE4-BEB7-718BB78612AD</productcode>");
				output.AppendLine("</info>");

				context.Response.Write(output);
			}
			else if (String.IsNullOrEmpty(context.Request["login"]) == false)
			{

				AuthenticationStatus authenticationStatus;
				string email; 

				
				//var ipConverge = GetIPConverge();

				//IIPConvergeServer server = XmlRpcProxyGen.Create<IIPConvergeServer>();

				//server.Url = ipConverge.Url + "/converge_master/converge_server.php";
				//server.NonStandard = XmlRpcNonStandard.All;

				//var authenticationRequest = new AuthenticateRequest();

				//authenticationRequest.auth_key = ipConverge.ApiCode;
				//authenticationRequest.email_address = ""; // "wlnp03@gmail.com";
				//authenticationRequest.md5_once_password = "a46cf985e7ac57468b104e565854564d";
				//authenticationRequest.product_id = ipConverge.ProductId.ToString();
				//authenticationRequest.username = "backtrak";

				//AuthenticateResponse response = server.Authenticate(authenticationRequest);

				var connect = new Connect();

				// TODO: Determine how to handle IPC MD5 passwords if you want to use converge.
				//connect.Authenticate("backtrak", Allegiance.CommunitySecuritySystem.Common.Utility.Encryption.MD5Hash("xxxxxx"), out authenticationStatus, out email);
				//context.Response.Write(authenticationStatus + " " + email);
			}
			else
			{
				base.HandleHttpRequest(new XmlRpcHttpRequest(context.Request), new XmlRpcHttpResponse(context.Response));
			}
		}

		private void LogIncomingRequest(HttpContext context)
		{
			var streamWriter = new StreamWriter(@"c:\php_requests.log", true);

			streamWriter.WriteLine("\r\n\r\n" + DateTime.Now.ToString() + " Request");
			streamWriter.WriteLine("========================================================");

			streamWriter.WriteLine("Request.Form Vars");
			foreach (string key in context.Request.Form.AllKeys)
			{
				streamWriter.WriteLine("\t" + key + ": " + context.Request.Form[key]);
			}

			streamWriter.WriteLine("Request.RawUrl: " + context.Request.RawUrl);

			if (context.Request.InputStream.Length > 0)
			{
				byte[] buffer = new byte[context.Request.InputStream.Length];
				context.Request.InputStream.Read(buffer, 0, (int)context.Request.InputStream.Length);
				context.Request.InputStream.Position = 0;

				string inputStreamContents = ASCIIEncoding.ASCII.GetString(buffer);

				streamWriter.WriteLine("context.Request.InputStream Length: " + context.Request.InputStream.Length);
				streamWriter.WriteLine(inputStreamContents + "\r\n\r\n");
			}


			streamWriter.Close();
		}

		

		private void EnsureValidRequest(ProcessBaseRequest processRequest)
		{
			using (var db = new CSSDataContext())
			{
				var ipConverge = GetIPConverge(db, processRequest.auth_key, processRequest.product_id);

				if (ipConverge == null)
					throw new XmlRpcFaultException(500, "Invalid request.");
			}
		}

		//http://codeclimber.net.nz/archive/2007/07/10/convert-a-unix-timestamp-to-a-.net-datetime.aspx
		private DateTime ConvertFromUnixTimestamp(double timestamp)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return origin.AddSeconds(timestamp);
		}

		//http://codeclimber.net.nz/archive/2007/07/10/convert-a-unix-timestamp-to-a-.net-datetime.aspx
		private double ConvertToUnixTimestamp(DateTime date)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeSpan diff = date - origin;
			return Math.Floor(diff.TotalSeconds);
		}

		public static IPConverge GetIPConverge(CSSDataContext db, string apiCode, string productId)
		{
			return GetIPConverge(db, apiCode, Int32.Parse(productId));
		}

		public static IPConverge GetIPConverge(CSSDataContext db, string apiCode, int productId)
		{
			var ipConverge = db.IPConverges.FirstOrDefault(p => p.ApiCode == apiCode && p.ProductId == productId);

			if (ipConverge == null)
				throw new Exception("Invalid API Code.");

			//throw new XmlRpcFaultException(500, "Invalid API Code.");

			return ipConverge;
		}

		#region IIPConvergeHandler Members


		////////////////////////////////////////////////////
		// Handshake Methods.
		////////////////////////////////////////////////////

		[XmlRpcMethod("ipConverge.handshakeStart")]
		public HandshakeStartResponse HandshakeStart(HandshakeStartRequest handshakeStartRequest)
		{
			if (handshakeStartRequest.acp_email.Equals(Settings.Default.IPConvergeAdminEmail) == false 
				|| handshakeStartRequest.acp_md5_password.Equals(Settings.Default.IPConvergeAdminPasswordMD5) == false)
			{
				throw new XmlRpcFaultException(500, "ipConverge.handshakeStart: Invalid credentials");
			}

			using (var db = new CSSDataContext())
			{
				db.IPConverges.DeleteAllOnSubmit(db.IPConverges);

				db.IPConverges.InsertOnSubmit(new IPConverge()
				{
					Active = false,
					Added = ConvertFromUnixTimestamp(handshakeStartRequest.reg_date),
					ApiCode = handshakeStartRequest.reg_code,
					HttpPassword = handshakeStartRequest.http_pass,
					HttpUser = handshakeStartRequest.http_user,
					IpAddress = HttpContext.Current.Request.UserHostAddress,
					ProductId = handshakeStartRequest.reg_product_id,
					Url = handshakeStartRequest.converge_url
				});

				db.SubmitChanges();
			}
		
			return new HandshakeStartResponse()
				{
					acp_email = handshakeStartRequest.acp_email,
					acp_md5_password = handshakeStartRequest.acp_md5_password,
					converge_url = handshakeStartRequest.converge_url,
					http_pass = handshakeStartRequest.http_pass,
					http_user = handshakeStartRequest.http_user,
					master_response = 1,
					reg_code = handshakeStartRequest.reg_code,
					reg_date = handshakeStartRequest.reg_date.ToString(),
					reg_id = handshakeStartRequest.reg_id,
					reg_product_id = handshakeStartRequest.reg_product_id
				};
		}

		[XmlRpcMethod("ipConverge.handshakeEnd")]
		public HandshakeEndResponse HandshakeEnd(HandshakeEndRequest handshakeEndRequest)
		{
			File.AppendAllText(@"c:\php_requests.log", "ipConverge.handshakeEnd " + handshakeEndRequest.reg_code + ", " + handshakeEndRequest.reg_product_id + "\r\n");

			using (var db = new CSSDataContext())
			{
				var ipConverge = GetIPConverge(db, handshakeEndRequest.reg_code, handshakeEndRequest.reg_product_id);

				ipConverge.Active = true;

				db.SubmitChanges();
			}

			File.AppendAllText(@"c:\php_requests.log", "ipConverge.handshakeEnd 2\r\n");

			return new HandshakeEndResponse()
			{
				handshake_updated = 1
			};
		}

		[XmlRpcMethod("ipConverge.handshakeRemove")]
		public HandshakeRemoveResponse HandshakeRemove(HandshakeRemoveRequest handshakeRemoveRequest)
		{
			using (var db = new CSSDataContext())
			{
				var ipConverge = GetIPConverge(db, handshakeRemoveRequest.reg_code, handshakeRemoveRequest.reg_product_id);

				db.IPConverges.DeleteOnSubmit(ipConverge);

				db.SubmitChanges();
			}

			return new HandshakeRemoveResponse()
			{
				handshake_removed = 1
			};
		}


		////////////////////////////////////////////////////
		// Process Methods.
		////////////////////////////////////////////////////

		[XmlRpcMethod("ipConverge.getMembersInfo")]
		public ProcessGetMembersInfoResponse ProcessGetMembersInfo(ProcessGetMembersInfoRequest processGetMembersInfoRequest)
		{
			EnsureValidRequest(processGetMembersInfoRequest);

			using (var db = new CSSDataContext())
			{
				var loginCount = 0;
				var lastId = 1;

				if (db.Logins.Count() > 0)
				{
					loginCount = db.Logins.Count();
					lastId = db.Logins.Max(p => p.Id);
				}

				return new ProcessGetMembersInfoResponse()
				{
					count = loginCount,
					last_id = lastId
				};
			}
		}

		[XmlRpcMethod("ipConverge.onMemberDelete")]
		public ProcessOnMemberDeleteResponse ProcessOnMemberDelete(ProcessOnMemberDeleteRequest request)
		{
			EnsureValidRequest(request);

			var response = new ProcessOnMemberDeleteResponse();

			using (var db = new CSSDataContext())
			{
				var login = Login.FindLoginByUsernameOrCallsign(db, request.auth);
				if (login == null)
					login = db.Logins.FirstOrDefault(p => p.Email == request.auth);

				if (login != null)
				{
					response.completed = 1;
					response.response = "FAILED";
				}
				else
				{
					response.completed = 1;
					response.response = "SUCCESS";
				}
			}

			return response;
		}

		[XmlRpcMethod("ipConverge.onPasswordChange")]
		public ProcessOnPasswordChangeResponse ProcessOnPasswordChange(ProcessOnPasswordChangeRequest request)
		{
			EnsureValidRequest(request);

			var response = new ProcessOnPasswordChangeResponse();

			using (var db = new CSSDataContext())
			{
				var login = Login.FindLoginByUsernameOrCallsign(db, request.auth);
				if (login == null)
					login = db.Logins.FirstOrDefault(p => p.Email == request.auth);

				if (login == null)
				{
					response.completed = 1;
					response.response = "FAILED";
				}
				else
				{
					login.Password = request.hashed_password;
					response.completed = 1;
					response.response = "SUCCESS";
				}
			}

			return response;
		}


		[XmlRpcMethod("ipConverge.onUsernameChange")]
		public ProcessOnUsernameChangeResponse ProcessOnUsernameChange(ProcessOnUsernameChangeRequest request)
		{
			File.AppendAllText(@"c:\php_requests.log", "ipConverge.onUsernameChange " + request.auth + ", " + request.new_username + "\r\n");

			EnsureValidRequest(request);

			var response = new ProcessOnUsernameChangeResponse();

			using (var db = new CSSDataContext())
			{
				var login = Login.FindLoginByUsernameOrCallsign(db, request.auth);
				if (login == null)
					login = db.Logins.FirstOrDefault(p => p.Email == request.auth);

				if (login == null)
				{
					response.completed = 1;
					response.response = "FAILED";
				}
				else
				{
					login.Username = request.new_username;
					response.completed = 1;
					response.response = "SUCCESS";
				}

				db.SubmitChanges();
			}

			return response;
		}


		[XmlRpcMethod("ipConverge.onValidate")]
		public ProcessOnValidateResponse ProcessOnValidate(ProcessOnValidateRequest request)
		{
			EnsureValidRequest(request);

			var response = new ProcessOnValidateResponse();

			using (var db = new CSSDataContext())
			{
				var login = Login.FindLoginByUsernameOrCallsign(db, request.auth);
				if (login == null)
					login = db.Logins.FirstOrDefault(p => p.Email == request.auth);

				if (login == null)
				{
					response.completed = 1;
					response.response = "FAILED";
				}
				else
				{
					// TODO: If we ever need to support email validation, set the flag in ACSS here.
					// Example: login.EmailValidated = true;

					response.completed = 1;
					response.response = "SUCCESS";
				}

				db.SubmitChanges();
			}

			return response;
		}


		[XmlRpcMethod("ipConverge.importMembers")]
		public ProcessOnImportMembersResponse ProcessOnImportMembers(ProcessOnImportMembersRequest request)
		{
			EnsureValidRequest(request);

			var response = new ProcessOnImportMembersResponse();

			using (var db = new CSSDataContext())
			{
				int startingIndex = Convert.ToInt32(request.limit_a);
				int count =  Convert.ToInt32(request.limit_b);

				var logins = db.Logins.Skip(startingIndex).Take(count);

				int complete = 0;
				if (logins.Count() < count)
				 complete = 1;

				var members = new Hashtable();
				foreach (var login in logins)
				{
					var member = new Hashtable();
					member.Add("email_address", login.Email);
					member.Add("username", login.Username);
					member.Add("pass_salt", String.Empty);
					member.Add("password", login.Password);
					member.Add("ip_address", "0.0.0.0");
					member.Add("join_date", ConvertToUnixTimestamp(login.DateCreated));
					member.Add("extra", String.Empty);
					member.Add("flag", 1);

					members.Add(login.Id, member);
				}

				var data = new Hashtable();
				data.Add("complete", complete);
				data.Add("members", members);

				PHPSerializer serializer = new PHPSerializer();
				string serializedData = serializer.Serialize(data);

				File.AppendAllText(@"c:\php_requests.log", "ipConverge.importMembers serializedData = " + serializedData + "\r\n");

//                string data = @"
//Array(
//	""complete"" => 1,
//	""members"" => array (
//		5 => array(
//			""email_address"" => ""nick@chi-town.com"",
//			""username"" => ""backtrak"",
//			""pass_salt"" => """",
//			""password"" => ""12345abcdef"",
//			""ip_address"" => ""1.1.1.1"",
//			""join_date"" => 2000000,
//			""extra"" => """",
//			""flag"" => 1,
//			),
//		6 => array(
//			""email_address"" => ""nick2@chi-town.com"",
//			""username"" => ""backtrak2"",
//			""pass_salt"" => """",
//			""password"" => ""12345abcdef222"",
//			""ip_address"" => ""1.1.1.2"",
//			""join_date"" => 2000002,
//			""extra"" => """",
//			""flag"" => 1,
//			)
//	)
//)
//";
				//string encodedData = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(data));
				//File.AppendAllText(@"c:\php_requests.log", "ipConverge.importMembers encodedData = " + encodedData + "\r\n");
				//response.__serialized64__ = encodedData;
				//response.__serialized64__ = "YToyOntzOjg6ImNvbXBsZXRlIjtpOjE7czo3OiJtZW1iZXJzIjthOjI6e2k6MTthOjg6e3M6MTM6ImVtYWlsX2FkZHJlc3MiO3M6MTc6Im5pY2tAY2hpLXRvd24uY29tIjtzOjg6InVzZXJuYW1lIjtzOjg6ImJhY2t0cmFrIjtzOjk6InBhc3Nfc2FsdCI7czowOiIiO3M6ODoicGFzc3dvcmQiO3M6MTE6IjEyMzQ1YWJjZGVmIjtzOjEwOiJpcF9hZGRyZXNzIjtzOjc6IjEuMS4xLjEiO3M6OToiam9pbl9kYXRlIjtpOjIwMDAwMDA7czo1OiJleHRyYSI7czowOiIiO3M6NDoiZmxhZyI7aToxO31pOjI7YTo4OntzOjEzOiJlbWFpbF9hZGRyZXNzIjtzOjE4OiJuaWNrMkBjaGktdG93bi5jb20iO3M6ODoidXNlcm5hbWUiO3M6OToiYmFja3RyYWsyIjtzOjk6InBhc3Nfc2FsdCI7czowOiIiO3M6ODoicGFzc3dvcmQiO3M6MTQ6IjEyMzQ1YWJjZGVmMjIyIjtzOjEwOiJpcF9hZGRyZXNzIjtzOjc6IjEuMS4xLjIiO3M6OToiam9pbl9kYXRlIjtpOjIwMDAwMDI7czo1OiJleHRyYSI7czowOiIiO3M6NDoiZmxhZyI7aToxO319fQ==";

				string encodedData = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(serializedData));
				File.AppendAllText(@"c:\php_requests.log", "ipConverge.importMembers encodedData = " + encodedData + "\r\n");

				response.__serialized64__ = encodedData;
				response.completed = 1;
				response.response = "SUCCESS";
			}

			return response;
		}

		#endregion
	}


	////////////////////////////////////////////////////
	// Handshake contracts.
	////////////////////////////////////////////////////

	public class HandshakeStartRequest
	{
		public int reg_id; 
		public string reg_code;
		public int reg_date;
		public int reg_product_id;
		public string converge_url;
		public string acp_email;
		public string acp_md5_password;
		public string http_user;
		public string http_pass;
	}

	public class HandshakeStartResponse
	{
		public int master_response = 1;
		public int reg_id;
		public string reg_code;
		public string reg_date;
		public int reg_product_id;
		public string converge_url;
		public string acp_email;
		public string acp_md5_password;
		public string http_user;
		public string http_pass;

	}

	public class HandshakeEndRequest : HandshakeStartRequest
	{
		public int handshake_completed;
	}

	public class HandshakeEndResponse
	{
		public int handshake_updated;
	}

	public class HandshakeRemoveRequest
	{
		public int reg_product_id;
		public string reg_code;
	}

	public class HandshakeRemoveResponse
	{
		public int handshake_removed;
	}

	////////////////////////////////////////////////////
	// Process contracts.
	////////////////////////////////////////////////////

	public class ProcessBaseRequest
	{
		public string auth_key;
		public string product_id;
	}

	public class ProcessBaseResponse
	{
		public int completed;
		public string response;
	}

	public class ProcessGetMembersInfoRequest : ProcessBaseRequest
	{
	}


	public class ProcessGetMembersInfoResponse
	{
		public int count;
		public int last_id;
	}


	public class ProcessOnMemberDeleteRequest : ProcessBaseRequest
	{
		public string auth;
	}

	public class ProcessOnMemberDeleteResponse : ProcessBaseResponse
	{
	}


	public class ProcessOnPasswordChangeRequest : ProcessBaseRequest
	{
		public string auth;
		public string hashed_password;
	}


	public class ProcessOnPasswordChangeResponse : ProcessBaseResponse
	{
	}

	public class ProcessOnUsernameChangeRequest : ProcessBaseRequest
	{
		public string auth;
		public string old_username;
		public string new_username;
	}

	public class ProcessOnUsernameChangeResponse : ProcessBaseResponse
	{
	}

	public class ProcessOnValidateRequest : ProcessBaseRequest
	{
		public string auth;
	}

	public class ProcessOnValidateResponse : ProcessBaseResponse
	{
	}

	public class ProcessOnImportMembersRequest : ProcessBaseRequest
	{
		/// <summary>
		/// The offset to start returning member records from
		/// </summary>
		public string limit_a;

		/// <summary>
		/// The number of records to return.
		/// </summary>
		public string limit_b;
	}

	public class ProcessOnImportMembersResponse : ProcessBaseResponse
	{
		public string __serialized64__;
	}

}
