using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMTool.Schema;
using System.Xml.Serialization;
using System.IO;

namespace VMTool
{
    public static class ConfigurationFileHelper
    {
        public static Configuration LoadConfiguration(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            try
            {
                Configuration configuration;
                using (StreamReader reader = new StreamReader(filePath))
                    configuration = (Configuration)serializer.Deserialize(reader);

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
