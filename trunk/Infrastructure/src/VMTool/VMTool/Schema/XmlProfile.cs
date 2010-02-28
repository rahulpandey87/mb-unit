using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VMTool.Core;

namespace VMTool.Schema
{
    [XmlType]
    public class XmlProfile
    {
        public XmlProfile()
        {
            MasterPort = Constants.DefaultMasterPort;
            SlavePort = Constants.DefaultSlavePort;
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("master", IsNullable = false)]
        public string Master { get; set; }

        [XmlElement("master-port", IsNullable = false)]
        public int MasterPort { get; set; }

        [XmlElement("slave", IsNullable = false)]
        public string Slave { get; set; }

        [XmlElement("slave-port", IsNullable = false)]
        public int SlavePort { get; set; }

        [XmlElement("vm", IsNullable = false)]
        public string VM { get; set; }

        [XmlElement("snapshot", IsNullable = false)]
        public string Snapshot { get; set; }

        public void Validate()
        {
            if (Id == null)
                throw new ConfigurationFileException("Profile is missing its 'id' attribute.");
            if (Master == null)
                throw new ConfigurationFileException(string.Format("Profile '{0}' is missing its 'master' element.", Id));
            if (MasterPort < 1 || MasterPort > 65535)
                throw new ConfigurationFileException(string.Format("Profile '{0}' has an invalid 'master-port' element.", Id));
            if (SlavePort < 1 || SlavePort > 65535)
                throw new ConfigurationFileException(string.Format("Profile '{0}' has an invalid 'master-port' element.", Id));
            if (VM == null)
                throw new ConfigurationFileException(string.Format("Profile '{0}' is missing its 'vm' element.", Id));
        }

        public Profile ToProfile()
        {
            return new Profile()
            {
                Id = Id,
                Master = Master,
                MasterPort = MasterPort,
                Slave = Slave,
                SlavePort = SlavePort,
                VM = VM,
                Snapshot = Snapshot
            };
        }
    }
}
