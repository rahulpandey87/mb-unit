using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace VMTool.Schema
{
    public static class ConfigurationFileHelper
    {
        public static XmlConfiguration LoadConfiguration(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlConfiguration));
            try
            {
                XmlConfiguration configuration;
                using (StreamReader reader = new StreamReader(filePath))
                    configuration = (XmlConfiguration)serializer.Deserialize(reader);

                configuration.Validate();
                return configuration;
            }
            catch (Exception ex)
            {
                throw new ConfigurationFileException(string.Format("Could not load configuration file '{0}'.",
                    filePath), ex);
            }
        }
    }
}
