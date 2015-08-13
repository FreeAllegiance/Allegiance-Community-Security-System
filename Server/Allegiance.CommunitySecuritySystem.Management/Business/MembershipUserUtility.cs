using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Web.Security;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public static class MembershipUserUtility
	{
		public static MembershipUserCollection FindUsersByPredicate(this Expression<Func<DataAccess.Login, bool>> predicate, int pageIndex, int pageSize, out int totalRecords)
		{
			MembershipUserCollection returnValue = new MembershipUserCollection();
			totalRecords = 0;

			using (var db = new DataAccess.CSSDataContext())
			{
				var logins = db.Logins.Where(predicate);

				totalRecords = logins.Count();

				foreach (var login in logins.Skip(pageIndex * pageSize).Take(pageSize))
				{
					returnValue.Add(MembershipUserUtility.CreateMembershipUserFromLogin(login));
				}
			}

			return returnValue;
		}

		public static MembershipUser FindUserByPredicate(this Expression<Func<DataAccess.Login, bool>> predicate)
		{
			MembershipUser membershipUser = null;

			using (var db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(predicate);

				if (login != null)
					membershipUser = MembershipUserUtility.CreateMembershipUserFromLogin(login);
			}

			return membershipUser;
		}

		public static MembershipUser CreateMembershipUserFromLogin(DataAccess.Login login)
		{
			return new MembershipUser("CssMembershipProvider", login.Username, login.Id, login.Email, String.Empty, String.Empty, true, login.IsBanned, login.DateCreated, login.Identity.DateLastLogin, login.Identity.DateLastLogin, DateTime.MinValue, DateTime.MinValue);
		}
	}
}
