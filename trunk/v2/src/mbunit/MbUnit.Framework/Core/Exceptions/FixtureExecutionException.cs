using System;

namespace MbUnit.Core.Exceptions
{
    [Serializable]
    public sealed class FixtureExecutionException : ApplicationException
    {
        public FixtureExecutionException(string message, Exception innerException)
            :base(message, innerException)
        {}
    }
}
