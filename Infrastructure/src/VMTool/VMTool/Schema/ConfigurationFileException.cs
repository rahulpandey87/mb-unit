using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VMTool.Schema
{
    [Serializable]
    public class ConfigurationFileException : Exception
    {
        public ConfigurationFileException()
        {
        }

        public ConfigurationFileException(string message)
            : base(message) 
        {
        }

        public ConfigurationFileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ConfigurationFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
