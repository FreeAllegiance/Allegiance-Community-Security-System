using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Group
    {
        #region Methods

        public static void AddAlias(CSSDataContext db, string groupName, string alias)
        {
            var add = new Group_Alias_GroupRole()
            {
                AliasId     = db.Alias.FirstOrDefault(a => a.Callsign == alias).Id,
                GroupId     = db.Groups.FirstOrDefault(g => g.Name == groupName).Id,
                GroupRoleId = db.GroupRoles.FirstOrDefault(gr => gr.Name == "Member").Id //default to member
            };

            //TODO: Check for condition where alias is already in group
            db.Group_Alias_GroupRoles.InsertOnSubmit(add);
            db.SubmitChanges();
        }

        public static void CreateGroup(CSSDataContext db, string name, bool isSquad, string tag)
        {
            var g = new Group()
            {
                DateCreated = DateTime.Now,
                Name        = name,
                IsSquad     = isSquad,
                Tag         = tag
            };
            db.Groups.InsertOnSubmit(g);
            db.SubmitChanges();
        }

		public static IQueryable<DataAccess.Group> GetGroupsForLogin(CSSDataContext db, string username, bool squadsOnly)
		{
			var login = Login.FindLoginByUsernameOrCallsign(db, username);

			if (login != null)
			{

				var groups = db.Groups.Where(
							p => (squadsOnly == false || p.IsSquad == true)
							&& db.Group_Alias_GroupRoles.Where(
								q => q.GroupId == p.Id
								&& db.Alias.Where(
									r => r.Id == q.AliasId
									&& r.LoginId == login.Id
									).Count() > 0
								).Count() > 0
							);

				return groups;
			}

			return new List<DataAccess.Group>().AsQueryable();
		}

        #endregion
    }
}
