using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VMTool.Schema
{
    [XmlType]
    public class Profile
    {
        public Profile()
        {
            Port = Constants.DefaultPort;
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("host", IsNullable = false)]
        public string Host { get; set; }

        [XmlElement("port", IsNullable = false)]
        public int Port { get; set; }

        [XmlElement("vm", IsNullable = false)]
        public string VM { get; set; }

        [XmlElement("snapshot", IsNullable = false)]
        public string Snapshot { get; set; }

        public void Validate()
        {
            if (Id == null)
                throw new ConfigurationFileException("Profile is missing the 'id' attribute.");
            if (Host == null)
                throw new ConfigurationFileException(string.Format("Profile '{0}' is missing the 'host' element.", Id));
            if (VM == null)
                throw new ConfigurationFileException(string.Format("Profile '{0}' is missing the 'vm' element.", Id));
        }
    }
}
