using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Web;
using System.Security.Principal;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Management.AjaxProviders
{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	[ServiceContract(Namespace = "AjaxProviders")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Polls
	{
		private void CheckAccess()
		{
			IPrincipal principal = HttpContext.Current.User;

			if (Business.Authorization.IsAdminOrSuperAdmin(HttpContext.Current.User) == false)
				throw new Exception("Access is denied.");
		}

		[OperationContract]
		public int UpdatePollOption(int pollID, int? pollOptionId, string optionText)
		{
			CheckAccess();

			using (CSSDataContext db = new CSSDataContext())
			{
				if (pollOptionId == null)
				{
					PollOption newPollOption = new PollOption()
					{
						Option = optionText,
						VoteCount = 0
					};

					Poll poll = db.Polls.FirstOrDefault(p => p.Id == pollID);

					if (poll == null)
						throw new Exception("Couldn't get poll: " + pollID);

					poll.PollOptions.Add(newPollOption);
					db.SubmitChanges();

					return newPollOption.Id;
				}
				else
				{
					PollOption pollOption = db.PollOptions.FirstOrDefault(p => p.Id == pollOptionId);

					if (pollOption == null)
						throw new Exception("Couldn't find poll option: " + pollOptionId);

					pollOption.Option = optionText;

					return pollOptionId.Value;
				}
			}
		}


		
	}
}
