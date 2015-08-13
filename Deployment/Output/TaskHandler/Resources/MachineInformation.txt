using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Blackbox
{
    public class MachineInformation : IXmlSerializable
    {
        #region Fields

        private List<DeviceInfo> _machineValues = new List<DeviceInfo>();

        private string _token;

        #endregion

        #region Properties

        public List<DeviceInfo> MachineValues
        {
            get { return _machineValues; }
            set { _machineValues = value; }
        }

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        #endregion

        #region Methods

        internal byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(ms, this);

                return ms.ToArray();
            }
        }

        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("machinevalues");

            foreach (DeviceInfo v in MachineValues)
            {
                writer.WriteElementString("name", v.Name);
                writer.WriteElementString("type", ((int)v.Type).ToString());
                writer.WriteElementString("value", v.Value);
            }

            writer.WriteEndElement();

            writer.WriteElementString("token", Token);
        }

        #endregion
    }
}