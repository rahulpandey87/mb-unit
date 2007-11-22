using System;

namespace MbUnit.Tests.Framework.SystemTest
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[SystemTest]
	public class VerifyExecutionOrder
	{
		public int testCounter = 0;
		[Step]
		public void Step()
		{
			Assert.AreEqual(0, testCounter++, "Test steps executed out of order");
		}
		[CriticalStep]
		public void AnotherStep()
		{
			Assert.AreEqual(1, testCounter++, "Test steps executed out of order");
		}
		[Step]
		public void YetAnotherStep()
		{
			Assert.AreEqual(2, testCounter++, "Test steps executed out of order");
		}
		[CriticalStep]
		public void AnotherStep2()
		{
			Assert.AreEqual(3, testCounter++, "Test steps executed out of order");
		}
	}
}
