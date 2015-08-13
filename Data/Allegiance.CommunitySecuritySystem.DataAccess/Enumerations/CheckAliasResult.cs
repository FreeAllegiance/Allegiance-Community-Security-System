using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Allegiance.CommunitySecuritySystem.DataAccess.Enumerations
{
    /// <summary>
    /// Registered: You already have this alias
    /// Available: Not registered, but you can get this alias
    /// Unavailable: Registered to someone else or you do not have any available aliases.
    /// InvalidLogin: Input credentials were not recognized
    /// </summary>
    public enum CheckAliasResult
    {
        Registered      = 1,
        Available       = 2,
        Unavailable     = 3,
        InvalidLogin    = 4,
		CaptchaFailed	= 5,
		ContainedBadWord = 6,
		AliasLimit = 7,
		LegacyExists = 8,
		InvalidLegacyPassword = 9
    }
}