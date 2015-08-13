using System;
using System.Collections.Generic;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    public delegate void TaskEventHandler<T>(object sender, T args);

    public delegate void TaskDelegate(object data);
}