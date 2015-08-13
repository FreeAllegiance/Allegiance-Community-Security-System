using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class PollOption
	{
		public string ShortOption
		{
			get
			{
				if (this.Option.Length > 70)
					return this.Option.Substring(0, 70) + "...";

				return this.Option;
			}
		}

		public int VotePercentageInt32
		{
			get
			{
				int totalVotes = this.Poll.PollOptions.Select(p => p.VoteCount).Sum();
				double votePercentage = 0;

				if (totalVotes > 0)
					votePercentage = (double)this.VoteCount / (double)totalVotes;

				return (int)Math.Ceiling(votePercentage * 100);
			}
		}

		public string VotePercentage
		{
			get
			{
				int totalVotes = this.Poll.PollOptions.Select(p => p.VoteCount).Sum();
				double votePercentage = 0;

				if (totalVotes > 0)
					votePercentage = (double) this.VoteCount / (double) totalVotes;

				return votePercentage.ToString("P");
			}
		}
	}
}
