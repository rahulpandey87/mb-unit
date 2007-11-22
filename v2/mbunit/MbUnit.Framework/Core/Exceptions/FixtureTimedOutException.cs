using System;
namespace MbUnit.Core.Exceptions
{
    [Serializable]
    public sealed class FixtureTimedOutException : ApplicationException
    {
        public FixtureTimedOutException()
            :base("Fixture Timed Out")
        { }
    }
}
