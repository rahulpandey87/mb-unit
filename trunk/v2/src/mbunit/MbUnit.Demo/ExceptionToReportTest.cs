using System;

using MbUnit.Framework;

namespace MbUnit.Demo 
{
	[TestFixture]
	public class ExceptionToReportTest
	{
		[Test]
		public void ThrowInner()
		{
			Exception inner = new Exception("inner exception");
			throw new Exception("Outer", inner);
		}

		[Test]
		public void DummyTes()
		{Console.WriteLine("hello");
		}
	    [Test]
		public void DummyTes2()
		{}	
	}
}
