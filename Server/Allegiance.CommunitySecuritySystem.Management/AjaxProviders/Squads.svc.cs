using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.AjaxProviders
{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	[ServiceContract(Namespace = "AjaxProviders")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Squads
	{
		private void ValidateMessage(string subject, string message)
		{
			if (String.IsNullOrEmpty(subject) == true || subject.Length > 50)
				throw new Exception("Invalid subject");

			if (String.IsNullOrEmpty(message) == true || message.Length > 255)
				throw new Exception("Invalid message");
		}

		[OperationContract]
		public void SendMessageToGroup(int groupID, string subject, string message, string sendOnOrAfterDateTime, string expiresAfterDateTime)
		{
			ValidateMessage(subject, message);

			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, HttpContext.Current.User.Identity.Name);

				// Get the groups the login has rights to send messages to.
				var availableGroups = DataAccess.Group.GetGroupsForLogin(db, login.Username, false);

				// Get the target group.
				var group = availableGroups.FirstOrDefault(p => p.Id == groupID);

				if (group == null)
					throw new Exception("Couldn't find group id: " + groupID);

				// Get the GAGR for the login assigned to the group that has the SL or ASL role.
				var gagrSender = group.Group_Alias_GroupRoles.FirstOrDefault(p => p.Alias.Login.Username.Equals(login.Username, StringComparison.InvariantCultureIgnoreCase) && (p.GroupRole.Name == "Squad Leader" || p.GroupRole.Name == "Assistant Squad Leader" || p.GroupRole.Name == "Zone Lead"));

				if (gagrSender == null)
					throw new Exception(HttpContext.Current.User.Identity.Name + " does not have rights to send this message.");

				DateTime dateToSend = DateTime.Parse(sendOnOrAfterDateTime);
				DateTime dateExpires = DateTime.Parse(expiresAfterDateTime);

				DataAccess.GroupMessage groupMessage = new Allegiance.CommunitySecuritySystem.DataAccess.GroupMessage()
				{
					DateCreated = DateTime.Now,
					DateExpires = dateExpires,
					DateToSend = dateToSend,
					GroupId = group.Id,
					Message = message,
					SenderAliasId = gagrSender.Alias.Id,
					Subject = subject
				};

				db.GroupMessages.InsertOnSubmit(groupMessage);
				db.SubmitChanges();

				foreach (var targetAlias in group.Group_Alias_GroupRoles.Select(p => p.Alias).Distinct())
				{
					db.GroupMessage_Alias.InsertOnSubmit(new DataAccess.GroupMessage_Alias()
					{
						Alias = targetAlias,
						DateViewed = null,
						GroupMessage = groupMessage
					});
				}

				db.SubmitChanges();
			}
		}


		[OperationContract]
		public void SendMessageToCallsigns(string [] callsigns, string subject, string message, string sendOnOrAfterDateTime, string expiresAfterDateTime)
		{
			ValidateMessage(subject, message);

			List<string> callsignsToTest = new List<string>(callsigns);
	
			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, HttpContext.Current.User.Identity.Name);

				// Get the groups the login has rights to send messages to.
				var availableSquads = DataAccess.Group.GetGroupsForLogin(db, login.Username, false);

				List<int> loginIDsAlreadyMessaged = new List<int>();

				foreach(DataAccess.Group group in availableSquads)
				{
					// Get the alias assigned to the login that is tied to the group. the alias must be an ASL or SL.
					var gagrSender = group.Group_Alias_GroupRoles.FirstOrDefault(p => p.Alias.Login.Username.Equals(login.Username, StringComparison.InvariantCultureIgnoreCase) && (p.GroupRole.Name == "Squad Leader" || p.GroupRole.Name == "Assistant Squad Leader" || p.GroupRole.Name == "Zone Lead"));

					if(gagrSender != null)
					{
						// Get all the callsigns assigned to the group that are also in the target list that have not already been messaged.
						var loginIDsToSendMessageTo = group.Group_Alias_GroupRoles.Where(p => callsigns.Contains(p.Alias.Callsign) == true && loginIDsAlreadyMessaged.Contains(p.Alias.LoginId) == false).Select(p => p.Alias.LoginId).Distinct();

						foreach (var loginID in loginIDsToSendMessageTo)
						{
							SendMessageToCallsign(db, subject, message, gagrSender.Alias, loginID, sendOnOrAfterDateTime, expiresAfterDateTime);
							loginIDsAlreadyMessaged.Add(loginID);
						}
					}
				}

				db.SubmitChanges();
			}
		}

		private void SendMessageToCallsign(DataAccess.CSSDataContext db, string subject, string message, DataAccess.Alias senderAlias, int receiverLoginID, string sendOnOrAfterDateTime, string expiresAfterDateTime)
		{
			DateTime dateToSend = DateTime.Parse(sendOnOrAfterDateTime);
			DateTime dateExpires = DateTime.Parse(expiresAfterDateTime);

			var personalMessage = new DataAccess.PersonalMessage()
			{
				DateCreated = DateTime.Now,
				DateExpires = dateExpires,
				DateToSend = dateToSend,
				Subject = subject,
				Message = message,
				SenderAliasId = senderAlias.Id,
				LoginId = receiverLoginID
			};

			db.PersonalMessages.InsertOnSubmit(personalMessage);
		}
	}
}
