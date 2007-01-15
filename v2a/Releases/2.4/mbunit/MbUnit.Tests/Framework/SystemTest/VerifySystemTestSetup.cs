using System;

namespace MbUnit.Tests.Framework.SystemTest
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[SystemTest]
	public class VerifySystemTestSetup
	{
		private int SetupCount = 0;
		[SetUp]
		public void Setup()
		{
			++SetupCount;
		}
		[Step]
		public void Step1()
		{
			Assert.AreEqual(1, SetupCount, "SystemTest setup not executed exactly once.");
		}
		[Step]
		public void Step2()
		{
			Assert.AreEqual(1, SetupCount, "SystemTest setup not executed exactly once.");
		}
	}
}
