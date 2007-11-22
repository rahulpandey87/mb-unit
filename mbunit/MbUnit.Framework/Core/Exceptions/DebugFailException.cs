using System;
using System.IO;

namespace MbUnit.Core.Exceptions
{
    [Serializable]
    public sealed class DebugFailException : AssertionException
    {
        private string messageDetail = null;
        public DebugFailException(string message)
            :this(message,null)
        {}

        public DebugFailException(string message, string messageDetail)
            :base(message)
        {
            this.messageDetail = messageDetail;
        }

        public override string ToString()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine(base.Message);
            if (this.messageDetail!=null)
                sw.WriteLine(this.messageDetail);
            sw.WriteLine(base.ToString());
            return sw.ToString();
        }
    }
}
