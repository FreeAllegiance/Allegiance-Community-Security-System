using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.DataAccess.Enumerations
{
    /// <summary>
    /// Determines the category of log
    /// </summary>
    public enum LogType : byte
    {
        AuthenticationServer    = 1,
        TaskHandler             = 2,
        AutoUpdate              = 3,
        BlackBoxGenerator       = 4,
		ManagementWeb			=5
    }
}