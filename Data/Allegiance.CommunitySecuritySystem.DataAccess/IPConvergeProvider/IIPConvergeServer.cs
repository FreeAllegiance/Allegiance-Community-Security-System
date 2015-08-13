using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CookComputing.XmlRpc;
using System.Runtime.InteropServices;

namespace Allegiance.CommunitySecuritySystem.DataAccess.IPConvergeProvider
{
	public interface IIPConvergeServer : IXmlRpcProxy
	{
		[XmlRpcMethod("convergeAuthenticate")]
		AuthenticateResponse Authenticate(AuthenticateRequest request);

		[XmlRpcMethod("convergeChangePassword")]
		ChangePasswordResponse ChangePassword(ChangePasswordRequest request);

		[XmlRpcMethod("convergeAddMember")]
		AddMemberResponse AddMember(AddMemberRequest request);

		[XmlRpcMethod("convergeCheckEmail")]
		CheckEmailResponse CheckEmail(CheckEmailRequest request);

		[XmlRpcMethod("convergeCheckUsername")]
		CheckUsernameResponse CheckUsername(CheckUsernameRequest request);
	}


	public class RequestBase
	{
		public string auth_key { get; set; }
		public string product_id { get; set; }
	}

	public class ResponseBase
	{
		public int complete { get; set; }
		public string response { get; set; }
	}


	public enum AuthenticationStatus
	{
		Success,
		WrongAuth,
		NoUser,
		DetailsIncomplete,
		FlaggedLocal,
		FlaggedRemote,
		AuthFailure
	}

	public class AuthenticateRequest : RequestBase
	{
		public string email_address { get; set; }
		public string md5_once_password { get; set; }
		public string username { get; set; }
	}



	public class AuthenticateResponse : ResponseBase
	{
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

					case "NO_USER":
						return IPConvergeProvider.AuthenticationStatus.NoUser;

					case "DETAILS_INCOMPLETE":
						return IPConvergeProvider.AuthenticationStatus.DetailsIncomplete;

					case "FLAGGED_LOCAL":
						return AuthenticationStatus.FlaggedLocal;

					case "AUTH_FAILURE":
						return IPConvergeProvider.AuthenticationStatus.AuthFailure;

					default:
						return AuthenticationStatus.WrongAuth;
				}
			}
		}
	}

	public enum ChangePasswordStatus
	{
		Success,
		Failed
	}

	public class ChangePasswordRequest : RequestBase
	{
		public string email_address { get; set; }
		public string md5_once_password { get; set; }
		public string extra { get; set; }
	}

	public class ChangePasswordResponse : ResponseBase
	{
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public ChangePasswordStatus ChangePasswordStatus
		{
			get
			{
				switch (response.ToUpper())
				{
					case "SUCCESS":
						return ChangePasswordStatus.Success;

					default:
						return IPConvergeProvider.ChangePasswordStatus.Failed;
				}
			}
		}
	}

	public class AddMemberRequest : RequestBase
	{
		public string email_address { get; set; }
		public string username { get; set; }
		public string md5_once_password { get; set; }
		public string ip_address { get; set; }
		public double unix_join_date { get; set; }
		public int validating { get; set; }
		public string extra { get; set; }
		public int flag { get; set; }
	}

	public enum AddMemberStatus
	{
		Success,
		MissingData,
		EmailInUse,
		Failed
	}

	public class AddMemberResponse : ResponseBase
	{
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public AddMemberStatus AddMemberStatus
		{
			get
			{
				switch (response.ToUpper())
				{
					case "SUCCESS":
						return AddMemberStatus.Success;

					case "EMAIL_IN_USE":
						return AddMemberStatus.EmailInUse;

					case "MISSING_DATA":
						return AddMemberStatus.MissingData;

					default:
						return AddMemberStatus.Failed;
				}
			}
		}
	}

	public class CheckEmailRequest : RequestBase
	{
		public string email_address { get; set; }
	}

	public enum CheckEmailStatus
	{
		EmailInUse,
		EmailNotInUse
	}

	public class CheckEmailResponse : ResponseBase
	{
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public CheckEmailStatus CheckEmailStatus
		{
			get
			{
				switch (response.ToUpper())
				{
					case "EMAIL_NOT_IN_USE":
						return CheckEmailStatus.EmailNotInUse;

					default:
						return CheckEmailStatus.EmailInUse;
				}
			}
		}
	}


	public class CheckUsernameRequest : RequestBase
	{
		public string username { get; set; }
	}

	public enum CheckUsernameStatus
	{
		UsernameInUse,
		UsernameNotInUse
	}

	public class CheckUsernameResponse : ResponseBase
	{
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public CheckUsernameStatus CheckUsernameStatus
		{
			get
			{
				switch (response.ToUpper())
				{
					case "USERNAME_NOT_IN_USE":
						return CheckUsernameStatus.UsernameNotInUse;

					default:
						return CheckUsernameStatus.UsernameInUse;
				}
			}
		}
	}

}