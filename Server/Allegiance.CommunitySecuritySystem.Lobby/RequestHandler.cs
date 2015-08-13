using System.Net;
using System.Web;
using Allegiance.CommunitySecuritySystem.BlackboxGenerator;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.Lobby
{
    public class RequestHandler : IHttpHandler
    {
        #region Properties

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion

        #region Methods

        public void ProcessRequest(HttpContext context)
        {
            var request     = context.Request;
            var response    = context.Response;

            response.Clear();

			string result = string.Empty;

			var action = QueryString.Get("Action");

			if (action == "GetRank")
			{
				result = GetRankForCallsign(QueryString.Get("Callsign"));
			}
			else
			{
				result = AuthorizeTicket(
					QueryString.Get("Callsign"),
					QueryString.Get("Ticket"),
					QueryString.Get("IP"));
			}




			response.Write(result);
            response.End();
        }

		private string AuthorizeTicket(string alias, string ticket, string ip)
		{
			var result = 0;

			//Verify user has an open and valid session
			if (!string.IsNullOrEmpty(alias)
				&& !string.IsNullOrEmpty(ticket)
				&& ip != null)
			{
				result = Validation.ValidateSession(alias, ticket, ip) ? 1 : 0;

				Log.Write(DataAccess.Enumerations.LogType.AuthenticationServer, "Lobby::AuthorizeTicket(): alias=" + alias + ", ticket=" + ticket + ", ip=" + ip + ", result=" + result);
			}

			return result.ToString();
		}

		private string GetRankForCallsign(string callsign)
		{
			string result = "0";

			using (CSSDataContext db = new CSSDataContext())
			{
				RankDetail rank = Alias.GetRankForCallsign(db, callsign);

				result = string.Format("0|{0}|{1}|{2}|{3}|{4}|{5}|{6}",
						(int) rank.Rank,
						GetRankNameFromRank((int)rank.Rank),
						rank.Sigma,
						rank.Mu,
						rank.CommandRank,
						rank.CommandSigma,
						rank.CommandMu);
			}

			return result;
		}

		private string GetRankNameFromRank(int rank)
		{
			string rankString;

			switch ((int) rank)
			{
				case 0: rankString = "Newbie"; break;

				case 1: rankString = "Novice 1"; break;
				case 2: rankString = "Novice 2"; break;
				case 3: rankString = "Novice 3"; break;
				case 4: rankString = "Novice 4"; break;
				case 5: rankString = "Novice 5"; break;
				case 6: rankString = "Novice 6"; break;
				case 7: rankString = "Novice 7"; break;

				case 8: rankString = "Inter. 1"; break;
				case 9: rankString = "Inter. 2"; break;
				case 10: rankString = "Inter. 3"; break;
				case 11: rankString = "Inter. 4"; break;
				case 12: rankString = "Inter. 5"; break;
				case 13: rankString = "Inter. 6"; break;
				case 14: rankString = "Inter. 7"; break;

				case 15: rankString = "Veteran 1"; break;
				case 16: rankString = "Veteran 2"; break;
				case 17: rankString = "Veteran 3"; break;
				case 18: rankString = "Veteran 4"; break;
				case 19: rankString = "Veteran 5"; break;
				case 20: rankString = "Veteran 6"; break;
				case 21: rankString = "Veteran 7"; break;

				case 22: rankString = "Expert 1"; break;
				case 23: rankString = "Expert 2"; break;
				case 24: rankString = "Expert 3"; break;
				case 25: rankString = "Expert 4"; break;
				case 26: rankString = "Expert 5"; break;
				case 27: rankString = "Expert 6"; break;
				case 28: rankString = "Expert 7"; break;

				default: rankString = "Uber Whore"; break;
			}

			return rankString;
		}

        #endregion
    }
}