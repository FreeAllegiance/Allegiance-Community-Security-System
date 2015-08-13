using System;
using System.Linq;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using System.Collections.Generic;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Ban
    {

		//public Login BanningLogin
		//{
		//    get { this.
		//}

        /// <summary>
        /// Calculate how long a person's ban will be based on the reason and who they are.
        /// http://web.archive.org/web/20070129051419/http://www.allegacademy.org/proposedbantime.shtml
        /// </summary>
        public static TimeSpan? CalculateDuration(Identity user, BanType banType)
        {
            TimeSpan? duration  = TimeSpan.MinValue;
			var days = banType.BanClass.Id == (int) Enumerations.BanClassType.Major ? 180 : 90;

            var recentBans = from b in user.Bans
                             where b.DateCreated > DateTime.Now.AddDays(-days)
								&& b.BanType != null 
								&& b.BanType.BanClassId == banType.BanClassId
                             select b;

            var sameOffenses = from b in recentBans
							   where b.BanTypeId == banType.Id
                               select b;

            //Get # of the same infractions; Adding 1 for this instance
            var likeInfractions = sameOffenses.Count() + 1;
			var minutes = banType.BaseTimeInMinutes;

            //2nd or more infraction
            if(likeInfractions >= 2)
            {
				if (banType.BanClassId == (int)Enumerations.BanClassType.Minor)
					minutes = (int)Math.Pow(banType.BaseTimeInMinutes, 2);
                else //Major
					minutes = banType.BaseTimeInMinutes * 4;

                //Third or more infraction
                if(likeInfractions >= 3)
                {
					if (banType.BanClassId == (int)Enumerations.BanClassType.Minor)
                        minutes *= 8;
                    else //Major
                        minutes *= 5;
                }
            }

            duration = TimeSpan.FromMinutes(minutes);

            //Calculate number of total infractions for this class; Adding 1 for this instance
            var totalInfractions = recentBans.Count() + 1;
            
            //If this is a minor ban, check if user has >= 6 minor bans
			if (banType.BanClassId == (int)Enumerations.BanClassType.Minor && (totalInfractions % 6 == 0))
            {
                switch (totalInfractions)
                {
                    case 6: //First-time habitual offender = 10 days
                        duration = TimeSpan.FromDays(10);
                        break;

                    case 12: //Second-time habitual offender = 30 days
                        duration = TimeSpan.FromDays(30);
                        break;

                    default: //Third or more time habitual offender = 90 days
                        duration = TimeSpan.FromDays(90);
                        break;
                }
            }

            //If this is a major ban, check if user has >= 4 major bans
			else if (banType.BanClassId == (int)Enumerations.BanClassType.Major && (totalInfractions % 4 == 0))
            {
                switch (totalInfractions)
                {
                    case 4: //First-time habitual offender = 30 days
                        duration = TimeSpan.FromDays(30);
                        break;

                    case 8: //Second-time habitual offender = 60 days
                        duration = TimeSpan.FromDays(60);
                        break;

                    default: //Third or more time habitual offender, results in permanent ban
                        duration = null;
                        break;
                }
            }

			// Check if the ban is permanent.
			if (banType.InfractionsBeforePermanentBan != null && totalInfractions > banType.InfractionsBeforePermanentBan.Value)
			{
				duration = TimeSpan.MaxValue;
			}

            return duration;
        }

		public static List<string> GetBanedUsernamesSinceTimestamp(DateTime timestamp)
		{
			List<string> returnValue;

			using (CSSDataContext db = new CSSDataContext())
			{
				returnValue = db.Bans.Where(p => p.DateCreated >= timestamp).Select(p => p.Login.Username).ToList();
			}

			return returnValue;
		}
    }
}