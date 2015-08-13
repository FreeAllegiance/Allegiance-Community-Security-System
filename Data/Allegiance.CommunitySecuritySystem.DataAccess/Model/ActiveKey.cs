using System;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class ActiveKey
    {
        #region Fields

        public const int PreferredMinUsedKeys   = 10;

        public const int PreferredMinLifetime   = 12; //Hours

        private const string MutexId            = "Global\\{CSSActiveKey}";

        #endregion
    }
}