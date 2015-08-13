using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.BlackboxGenerator;
using System.Configuration;
using System;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class LoginData : AuthenticatedData
    {
        #region Properties

        [DataMember]
        public string Alias { get; set; }

		/// <summary>
		/// Used to check if the person requesting the alias the owner of the ASGS version of the alias.
		/// </summary>
		[DataMember]
		public string LegacyPassword { get; set; }

        [DataMember]
        public int? LobbyId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calls BlackboxServer module to verify login and retrieve blackbox information.
        /// </summary>
        internal LoginResult Verify(bool useDebugBlackbox)
        {
            byte[] data;

            var alias   = Alias;

			string ipAddress = "127.0.0.1";
			if (HttpContext.Current != null)
				ipAddress = HttpContext.Current.Request.UserHostAddress;

			var result = Validation.CreateSession(ipAddress, Username, Password, LobbyId, useDebugBlackbox, ref alias, out data);

			int rank = 0;

			using (CSSDataContext db = new CSSDataContext())
			{
				var rankDetail = DataAccess.Alias.GetRankForCallsign(db, alias);
				if (rankDetail != null)
					rank = (int)rankDetail.Rank;
			}

            return new LoginResult()
            {
                Status          = result,
                BlackboxData    = data,
                AcceptedAlias   = alias,
				Rank			= rank
            };
        }
        #endregion


	}
}