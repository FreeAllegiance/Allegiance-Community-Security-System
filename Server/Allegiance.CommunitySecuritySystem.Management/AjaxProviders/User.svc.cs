using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Security.Principal;
using System.Web;
using System.Collections.Generic;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Management.AjaxProviders
{
	// Delete this after checkin, this is unused.

	//[DataContract]
	//public class Alias
	//{
	//    [DataMember]
	//    public string Callsign { get; set; }

	//    [DataMember]
	//    public DateTime DateCreated { get; set; }

	//    [DataMember]
	//    public string TagsAndTokens { get; set; }

	//    [DataMember]
	//    public int GroupRoleCount { get; set; }
	//}


	//[DataContract]
	//public class UserData
	//{
	//    [DataMember]
	//    public string UserName { get; set; }

	//    [DataMember]
	//    public string Email { get; set; }

	//    [DataMember]
	//    public string DateCreated { get; set; }

	//    [DataMember]
	//    public string LastLogin { get; set; }

	//    [DataMember]
	//    public Alias[] Aliases { get; set; }

	//    [DataMember]
	//    public int[] RoleIDs { get; set; }
	//}


	//[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	//[ServiceContract(Namespace = "AjaxProviders")]
	//[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	//public class User
	//{
	//    private void CheckAccess()
	//    {
	//        if (Business.Authorization.IsAdminOrSuperAdmin(HttpContext.Current.User) == false)
	//            throw new Exception("Access is denied.");
	//    }

	//    [OperationContract]
	//    public UserData LoadUser(int loginID)
	//    {
	//        CheckAccess();

	//        using (var db = new DataAccess.CSSDataContext())
	//        {
	//            var login = db.Logins.FirstOrDefault(p => p.Id == loginID);

	//            if (login != null)
	//            {
	//                List<Alias> aliases = new List<Alias>();
	//                foreach (var alias in login.Aliases)
	//                {
	//                    StringBuilder tagsAndTokens = new StringBuilder();

	//                    foreach (var groupRole in alias.Group_Alias_GroupRoles)
	//                    {
	//                        if (tagsAndTokens.Length > 0)
	//                            tagsAndTokens.Append(", ");

	//                        tagsAndTokens.AppendFormat("{0}{1}{2}",
	//                            groupRole.GroupRole.Token,
	//                            (String.IsNullOrEmpty(groupRole.Group.Tag) == false) ? "@" : "",
	//                            groupRole.Group.Tag);
	//                    }

	//                    aliases.Add(new Alias()
	//                        {
	//                            Callsign = alias.Callsign,
	//                            DateCreated = alias.DateCreated,
	//                            TagsAndTokens = tagsAndTokens.ToString(),
	//                            GroupRoleCount = alias.Group_Alias_GroupRoles.Count()
	//                        });
	//                }

	//                List<int> roleIDs = new List<int>();
	//                foreach (var role in login.Login_Roles)
	//                    roleIDs.Add(role.RoleId);

	//                return new UserData()
	//                {
	//                    Aliases = aliases.ToArray(),
	//                    DateCreated = Format.DateTime(login.DateCreated),
	//                    Email = login.Email,
	//                    LastLogin = Format.DateTime(login.Identity.DateLastLogin),
	//                    RoleIDs = roleIDs.ToArray(),
	//                    UserName = login.Username 
	//                };
	//            }
	//            else
	//            {
	//                throw new Exception("User record not found.");
	//            }
	//        }
	//    }
	//}
}
