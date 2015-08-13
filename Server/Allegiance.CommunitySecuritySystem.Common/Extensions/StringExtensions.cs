using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Allegiance.CommunitySecuritySystem.Common.Extensions
{
	// Based on a sample from: http://stackoverflow.com/questions/5417070/c-sharp-version-of-sql-like
	public static class StringExtensions
	{
		public static bool Like(this string toSearch, string toFind)
		{
			return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
		}
	}
}
