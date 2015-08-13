using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class PollData : AuthenticatedData
    {
        #region Properties

        [DataMember]
        public int OptionId { get; set; }

        #endregion

        #region Methods

        internal void ApplyVote()
        {
            using (var db = new CSSDataContext())
            {
                if (!Poll.HasVoted(db, OptionId, Username))
                {
                    db.PollVotes.InsertOnSubmit(new PollVote()
                    {
                        PollOptionId    = OptionId,
						LoginId = Login.FindLoginByUsernameOrCallsign(db, Username).Id

                    });

                    db.SubmitChanges();
                }
            }
        }

        #endregion
    }
}