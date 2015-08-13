using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Common.Enumerations
{
    public enum CheckInStatus
    {
        Ok                  = 1,
        InvalidCredentials  = 2,
        InvalidHash         = 3,
        Timeout             = 4,
		VirtualMachineBlocked = 5,
		AccountLinked		= 6,
		PermissionDenied	= 7,
		AccountLocked		= 8
    }
}