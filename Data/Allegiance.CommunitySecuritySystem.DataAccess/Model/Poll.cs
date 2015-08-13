using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Poll
    {
		public string ShortQuestion
		{
			get
			{
				if (this.Question.Length > 20)
					return this.Question.Substring(0, 20) + "...";
				else
					return this.Question;
			}
		}

        public static List<Poll> FindActivePolls(string alias)
        {
            using (var db = new CSSDataContext())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Poll>(p => p.PollOptions);
                db.LoadOptions = options;

                var ident = Login.FindLoginByUsernameOrCallsign(db, alias).Identity;

                var polls = from p in db.Polls
                            where DateTime.Now < p.DateExpires &&
                                !p.PollOptions.Any(q => q.PollVotes.Any(w => w.Login.Identity == ident))
                            select p;

                return polls.ToList();
            }
        }

        public static void NewPoll(CSSDataContext db, string question, string[] options, DateTime expires)
        {
            var poll = new Poll()
            {
                Question    = question,
                DateCreated = DateTime.Now,
                DateExpires = expires,
				LastRecalculation = DateTime.Now
            };

            foreach (var po in options)
            {
                poll.PollOptions.Add(new PollOption()
                {
                    Option = po,
                    PollId = poll.Id,
					VoteCount = 0
                });
            }

            db.Polls.InsertOnSubmit(poll);
            db.SubmitChanges();
        }

        public static bool HasVoted(CSSDataContext db, int optionId, string username)
        {
            var identId = Login.FindLoginByUsernameOrCallsign(db, username).IdentityId;
            var pollId  = db.PollOptions.FirstOrDefault(po => po.Id == optionId).PollId;
            var options = db.PollOptions.Where(po => po.PollId == pollId);

            foreach (PollOption option in options)
            {
                if (db.PollVotes.Any(pv => pv.PollOptionId == option.Id && pv.Login.IdentityId == identId))
                    return true;
            }

            return false;
        }

		public static void RecalculateAllPolls()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				foreach (var poll in db.Polls)
					poll.Recalculate();

				db.SubmitChanges();
			}
		}

		public void Recalculate()
		{
			List<Identity> votedIdentities = new List<Identity>();
			foreach (var pollOption in this.PollOptions)
			{
				var votesByIdentity = pollOption.PollVotes
					.Where(p => votedIdentities.Contains(p.Login.Identity) == false)
					.GroupBy(p => p.Login.Identity)
					.Select(p => new
						{
							PollVote = p.FirstOrDefault()
						}
					);

				pollOption.VoteCount = votesByIdentity.Count();

				votedIdentities.AddRange(votesByIdentity.Select(p => p.PollVote.Login.Identity));
			}

			this.LastRecalculation = DateTime.Now;
		}
	}
}
