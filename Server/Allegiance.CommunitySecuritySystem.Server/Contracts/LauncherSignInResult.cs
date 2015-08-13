using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
	public class LauncherSignInResult
    {
        #region Properties

        [DataMember] 
        public CheckInStatus Status { get; set; }

		[DataMember]
		public string LinkedAccount { get; set; }

		[DataMember]
		public int SessionID { get; set; }

		[DataMember]
		public string LoginUserName { get; set; }

		[DataMember]
		public int LoginID { get; set; }

        #endregion
    }
}