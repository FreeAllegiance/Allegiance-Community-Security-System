using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
	public static class Validation
	{
		public static bool ValidateAlias(string alias, out string errorMessage)
		{
			errorMessage = String.Empty;

			var match = Regex.Match(alias,
				string.Concat(@"^(?<token>\W)?(?<callsign>[a-z]\w+)(?<tag>@\w+)?$"),
				RegexOptions.Compiled | RegexOptions.IgnoreCase);

			var token = match.Groups["token"].Value;
			var callsign = match.Groups["callsign"].Value;
			var tag = match.Groups["tag"].Value;

			if (callsign.Length < GlobalSettings.MinAliasLength)
				errorMessage = "The alias length is too small or contains invalid characters, " + GlobalSettings.MinAliasLength + " character minimum.";

			if (callsign.Length > GlobalSettings.MaxAliasLength)
				errorMessage = "The alias length is too large, " + GlobalSettings.MaxAliasLength + " character maximum.";

			return String.IsNullOrEmpty(errorMessage);
		}
	}
}
