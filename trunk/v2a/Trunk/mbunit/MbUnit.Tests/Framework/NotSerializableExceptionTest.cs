using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Framework
{
    public class NotSerializableException : Exception
    { }

    [TestFixture]
    public class NotSerializableExceptionTest
    {
		[Test]
		public void ResultExceptionExceptionSerializable()
		{
            Console.WriteLine("ExecuteMe");
        }
	}
}
