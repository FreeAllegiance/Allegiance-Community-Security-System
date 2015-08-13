using System.Linq;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class MachineRecord
    {
        #region Properties

        public DeviceType DeviceType
        {
            get { return (DeviceType)this.RecordTypeId; }
            set { this.RecordTypeId = (int)value; }
        }

        #endregion



    }
}