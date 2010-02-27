using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CCNet.VMTool.Plugin.Core
{
    [Serializable]
    public class RemoteContextException : Exception
    {
        public RemoteContextException()
        {
        }

        public RemoteContextException(string message)
            : base(message) 
        {
        }

        public RemoteContextException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RemoteContextException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
