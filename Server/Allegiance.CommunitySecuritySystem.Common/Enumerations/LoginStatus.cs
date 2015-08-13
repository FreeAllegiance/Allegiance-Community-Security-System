using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Common.Enumerations
{
    public enum LoginStatus
    {
        Authenticated       = 1,
        InvalidCredentials  = 2,
        AccountLocked       = 3,
        PermissionDenied    = 4
    }
}