using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    partial class GameTeam
    {
        public float AveragePRank
        {
            get
            {
                using (CSSStatsDataContext statsDB = new CSSStatsDataContext())
                {
                    int count = 0;
                    float total = 0;
                    foreach (var gameTeamMember in this.GameTeamMembers)
                    {
                        var leaderboard = statsDB.StatsLeaderboards.FirstOrDefault(p => p.LoginID == gameTeamMember.GameTeamMemberLoginID);
                        if (leaderboard != null)
                        {
                            count++;
                            total += leaderboard.PRank;
                        }
                    }

                    return total / count;
                }
            }
        }
    }
}
