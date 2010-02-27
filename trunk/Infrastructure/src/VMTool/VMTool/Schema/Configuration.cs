using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VMTool.Schema
{
    [XmlType]
    [XmlRoot("configuration")]
    public class Configuration
    {
        [XmlArray("profiles", IsNullable = false)]
        [XmlArrayItem("profile", IsNullable = false)]
        public Profile[] Profiles { get; set; }

        public Profile GetProfileById(string id)
        {
            return Profiles.Where(p => p.Id == id).SingleOrDefault();
        }

        public void Validate()
        {
            if (Profiles == null)
                throw new ConfigurationFileException("Profiles must not be null.");

            foreach (Profile profile in Profiles)
                profile.Validate();
        }
    }
}
