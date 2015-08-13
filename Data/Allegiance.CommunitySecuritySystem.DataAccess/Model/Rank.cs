using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public class RankDetail
	{
		public double Rank { get; set; }
		public double Sigma { get; set; }
		public double Mu { get; set; }
		public double CommandRank { get; set; }
		public double CommandSigma { get; set; }
		public double CommandMu { get; set; }

		public RankDetail()
		{
			this.Rank = 0;
			this.Sigma = 25D / 3D; // Baker verified these are the correct starting numbers.
			this.Mu = 25D; // Baker verified these are the correct starting numbers.
			this.CommandRank = 0;
			this.CommandSigma = 25D / 3D;
			this.CommandMu = 25D;
		}
	}
}
