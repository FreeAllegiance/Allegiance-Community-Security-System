using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo
{
    public class MachineInformation : IXmlSerializable
    {
        #region Properties

        public List<DeviceInfo> MachineValues { get; set; }

        public string Token { get; set; }

        #endregion

        #region Methods

        public static MachineInformation Deserialize(byte[] data)
        {
            using (var ms = new MemoryStream(data, false))
            {
                var serializer = new XmlSerializer(typeof(MachineInformation));
                return serializer.Deserialize(ms) as MachineInformation;
            }
        }


        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader xreader)
        {
            MachineValues   = new List<DeviceInfo>();
            var xml         = xreader.ReadOuterXml();
            var xdoc        = new XmlDocument();

            xdoc.LoadXml(xml);

            var machineNode = xdoc.SelectSingleNode("//machinevalues");

            for(int i = 0; i < machineNode.ChildNodes.Count; i += 3)
            {
                var di          = new DeviceInfo();
                var nameNode    = machineNode.ChildNodes[i];
                var typeNode    = machineNode.ChildNodes[i + 1];
                var valueNode   = machineNode.ChildNodes[i + 2];

                di.Name         = nameNode.InnerText;
                di.Type         = (DeviceType)int.Parse(typeNode.InnerText);
                di.Value        = valueNode.InnerText;

                MachineValues.Add(di);
            }

            var tokenNode   = xdoc.SelectSingleNode("//token");
            this.Token      = tokenNode.InnerText;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}