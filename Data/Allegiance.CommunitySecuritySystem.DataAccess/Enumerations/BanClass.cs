using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.DataAccess.Enumerations
{
    /// <summary>
    /// Determines the category of ban (or infraction)
    /// </summary>
    public enum BanClassType : byte
    {
        Minor = 1,
        Major = 2,
		MinorHabitual = 3,
		MajorHabitual = 4
    }
}
