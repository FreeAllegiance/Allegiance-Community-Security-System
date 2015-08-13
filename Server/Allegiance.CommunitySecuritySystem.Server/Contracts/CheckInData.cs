using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.BlackboxGenerator;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class CheckInData : AuthenticatedData
    {
        #region Properties

        [DataMember]
        public byte[] EncryptedData { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calls BlackboxServer module to verify the envelope.
        /// </summary>
        internal CheckInResult Verify()
        {
            DataAccess.Session session;
            CheckInStatus result = Validation.CheckIn(Username, Password, EncryptedData, out session);

			string ticket = string.Empty;

			if (result == CheckInStatus.Ok)
				ticket = session.HashSession();

            return new CheckInResult()
            {
                Status = result,
				Ticket = ticket
            };
        }

        #endregion
    }
}