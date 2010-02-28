using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VMTool.Schema
{
    [XmlType]
    [XmlRoot("configuration")]
    public class XmlConfiguration
    {
        [XmlArray("profiles", IsNullable = false)]
        [XmlArrayItem("profile", IsNullable = false)]
        public XmlProfile[] Profiles { get; set; }

        public XmlProfile GetProfileById(string id)
        {
            return Profiles.Where(p => p.Id == id).SingleOrDefault();
        }

        public void Validate()
        {
            if (Profiles == null)
                throw new ConfigurationFileException("Profiles array must not be null.");
            if (Profiles.Contains(null))
                throw new ConfigurationFileException("Profiles array must not contain null.");

            foreach (XmlProfile profile in Profiles)
                profile.Validate();
        }
    }
}
