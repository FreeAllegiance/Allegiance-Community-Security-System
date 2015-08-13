using System;
using System.Collections.Generic;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo
{
    public class DeviceInfo
    {
        private DeviceType _type;
        private string _name;
        private string _value;

        public DeviceType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
