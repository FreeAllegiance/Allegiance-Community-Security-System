using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Server.Utilities
{
    public class HeloRankCalculator
    {
        public void Calculate(DataAccess.CSSStatsDataContext statsDB, Game game)
        {
            List<GameTeamMember> winningCommanders = new List<GameTeamMember>();
            List<GameTeamMember> losingCommanders = new List<GameTeamMember>();

            if (IsGameScorable(game) == false)
                return;

            foreach (var gameTeam in game.GameTeams)
            {
                foreach (var gameTeamMember in gameTeam.GameTeamMembers)
                {
                    if (IsPlayerIsValidToScore(statsDB, game, gameTeamMember) == false)
                        continue;

                    int playerLevel = GetLevel(statsDB, gameTeamMember);
                    int avgWinXp = 0;
                    int avgLossXp = 0;
                    int winCount = 0;
                    int lossCount = 0;

                    foreach (var opposingTeamMember in gameTeam.GameTeamMembers.Where(p => p != gameTeamMember))
                    {
                        if (IsPlayerIsValidToScore(statsDB, game, opposingTeamMember) == false)
                            continue;


                        if (gameTeamMember.Score > opposingTeamMember.Score)
                        {
                            avgWinXp += GetXp(statsDB, opposingTeamMember);
                            winCount++;
                        }
                        else if (gameTeamMember.Score < opposingTeamMember.Score)
                        {
                            avgLossXp += GetXp(statsDB, opposingTeamMember);
                            lossCount++;
                        }

                    }

                    int winXpAdjustment = 0;
                    int lossXpAdjustment = 0;
                    int playerXp = GetXp(statsDB, gameTeamMember);

                    if (winCount > 0)
                    {
                        int avgWinLevel = GetLevel(statsDB, (int)(avgWinXp / winCount));
                        int winLevelDifference = GetLevelDifference(statsDB, playerXp, (int)(avgWinXp / winCount));
                        winXpAdjustment = GetRankAdjustment(statsDB, playerLevel, winLevelDifference, true);
                    }

                    if (lossCount > 0)
                    {
                        int avgLossLevel = GetLevel(statsDB, (int)(avgLossXp / lossCount));
                        int lossLevelDifference = GetLevelDifference(statsDB, playerXp, (int)(avgLossXp / lossCount));
                        lossXpAdjustment = GetRankAdjustment(statsDB, playerLevel, lossLevelDifference, false);
                    }

                    int totalPlayerXpAdjustment = winXpAdjustment - lossXpAdjustment;

                    if (gameTeam.GameTeamCommanderLoginID == gameTeamMember.GameTeamMemberLoginID)
                    {
                        if (gameTeam.GameTeamWinner == true)
                            winningCommanders.Add(gameTeamMember);
                        else
                            losingCommanders.Add(gameTeamMember);
                    }
                    
                    SetXp(statsDB, gameTeamMember, totalPlayerXpAdjustment);
                }
            }

            // Calculate commander's XP bonuses. 
            // Commander's games count twice. Once for thier performance vs. thier own team, and 
            // once for thier performance against the opposing commander. If it's a multi-team game, 
            // then each match up between comms is counted. If it's a 3 way, and you win vs. two comms, then
            // you get rewards for beating both. If you loose, you only lose to the winner.
            int averageWinningComamanderXp = GetAverageXp(statsDB, winningCommanders);
            int averageLosingComamanderXp = GetAverageXp(statsDB, losingCommanders);

            foreach (var commander in winningCommanders)
            {
                var commanderXp = GetXp(statsDB, commander);
                var commanderLevel = GetLevel(statsDB, commander);
                int levelDifference = GetLevelDifference(statsDB, commanderXp, averageLosingComamanderXp);
                var xpAdjustment = GetRankAdjustment(statsDB, commanderLevel, levelDifference, true);

                SetXp(statsDB, commander, xpAdjustment);
            }

            foreach (var commander in losingCommanders)
            {
                var commanderXp = GetXp(statsDB, commander);
                var commanderLevel = GetLevel(statsDB, commander);
                int levelDifference = GetLevelDifference(statsDB, commanderXp, averageWinningComamanderXp);
                var xpAdjustment = GetRankAdjustment(statsDB, commanderLevel, levelDifference, false);

                SetXp(statsDB, commander, -1 * xpAdjustment);
            }

        }

        /// <summary>
        /// Games must have at least two teams.
        /// At least one team must win.
        /// The game must have run for more than 5 minutes.
        /// All teams must have had at least 5 players who played for at least 5 minutes.
        /// </summary>
        /// <param name="statsDB"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private bool IsGameScorable(Game game)
        {
            // Was the game longer than 5 minutes?
            if ((game.GameEndTime - game.GameStartTime).TotalMinutes < 5)
                return false;

            // Was there at least two teams?
            if (game.GameTeams.Count() < 2)
                return false;

            // Was there a winner?
            if(game.GameTeams.FirstOrDefault(p => p.GameTeamWinner == true) == null)
                return false;
          
            // Did each team have 5 members who played at least 5 minutes?
            foreach (var gameTeam in game.GameTeams)
            {
                if(gameTeam.GameTeamMembers.Where(p => p.GameTeamMemberDuration >= 300).Count() < 5)
                    return false;
            }

            return true;
        }

        private int GetAverageXp(CSSStatsDataContext statsDB, List<GameTeamMember> commanders)
        {
            if (commanders.Count == 0)
                return 0;

            int xpTotal = 0;
            foreach (var commander in commanders)
                xpTotal += GetXp(statsDB, commander);

            return (int)Math.Floor(xpTotal / (decimal)commanders.Count);
        }

        private bool IsPlayerIsValidToScore(CSSStatsDataContext statsDB, Game game, GameTeamMember gameTeamMember)
        {
            return gameTeamMember.GameTeamMemberDuration >= 300;
        }

        private void SetXp(CSSStatsDataContext statsDB, GameTeamMember gameTeamMember, int totalPlayerXpAdjustment)
        {
            var leaderBoard = statsDB.StatsLeaderboards.FirstOrDefault(p => p.LoginID == gameTeamMember.GameTeamMemberLoginID);
            if (leaderBoard != null)
            {
                leaderBoard.Xp += totalPlayerXpAdjustment;
                if (leaderBoard.Xp < 0)
                    leaderBoard.Xp = 0;

                var level = statsDB.Levels.FirstOrDefault(p => p.MinXP <= leaderBoard.Xp && p.MaxXP >= leaderBoard.Xp);
                leaderBoard.PRank = level.Level1;

                statsDB.SubmitChanges();
            }
        }

        private int GetRankAdjustment(CSSStatsDataContext statsDB, int playerLevel, int levelDifference, bool winner)
        {
            int returnValue = 0;

            var experianceExchange = statsDB.ExperianceExchanges.FirstOrDefault(p => p.LevelDiffMin == 0 && p.LevelDiffMax == 0);

            foreach (var currExperianceExchange in statsDB.ExperianceExchanges)
            {
                if (Math.Abs(levelDifference) >= currExperianceExchange.LevelDiffMin && Math.Abs(levelDifference) <= currExperianceExchange.LevelDiffMax)
                {
                    experianceExchange = currExperianceExchange;
                    break;
                }
            }

            if (winner)
            {
                // The player's level was greater than the opposition's level.
                if (levelDifference >= 0)
                {
                    returnValue = experianceExchange.HigherWin;
                }
                else if (levelDifference < 0)
                {
                    returnValue = experianceExchange.LowerWin;
                }

                foreach (var winFactor in statsDB.WinFactors)
                {
                    if (playerLevel >= winFactor.MinLevel && playerLevel <= winFactor.MaxLevel)
                    {
                        returnValue = (int)Math.Floor(((double)returnValue * ((double)winFactor.Factor / 100.0)));
                        break;
                    }
                }
            }
            else
            {
                // The player's level was greater than the opposition's level.
                if (levelDifference >= 0)
                {
                    returnValue = experianceExchange.HigherLoss;
                }
                else if (levelDifference < 0)
                {
                    returnValue = experianceExchange.LowerLoss;
                }

                foreach (var lossFactor in statsDB.LossFactors)
                {
                    if (playerLevel >= lossFactor.MinLevel && playerLevel <= lossFactor.MaxLevel)
                    {
                        returnValue = (int)Math.Floor(((double)returnValue * (double)lossFactor.Factor / 100.0));
                        break;
                    }
                }
            }

            return (returnValue);
        }

        private int GetLevelDifference(CSSStatsDataContext statsDB, int playerXp, int opposingXp)
        {
            int returnValue = 0;
            int playerLevel = GetLevel(statsDB, playerXp);
            int opposingLevel = GetLevel(statsDB, opposingXp);

            returnValue = playerLevel - opposingLevel;

            return (returnValue);
        }

        private int GetLevel(CSSStatsDataContext statsDB, int playerXp)
        {
            var level = statsDB.Levels.FirstOrDefault(p => p.MinXP <= playerXp && p.MaxXP >= playerXp);
            if (level == null)
                return 0;

            return level.Level1;
        }

        private int GetXp(CSSStatsDataContext statsDB, GameTeamMember gameTeamMember)
        {
            var leaderBoard = statsDB.StatsLeaderboards.FirstOrDefault(p => p.LoginID == gameTeamMember.GameTeamMemberLoginID);
            if (leaderBoard != null)
            {
                return leaderBoard.Xp;
            }

            return 0;
        }

        private int GetLevel(DataAccess.CSSStatsDataContext statsDB, GameTeamMember gameTeamMember)
        {
            int xp = GetXp(statsDB, gameTeamMember);
            return GetLevel(statsDB, xp);
        }

    }
}
