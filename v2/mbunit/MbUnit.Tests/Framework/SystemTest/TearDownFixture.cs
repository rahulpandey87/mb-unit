using System;

namespace MbUnit.Tests.Framework.SystemTest
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[SystemTest]
	internal class TearDownFixture
	{
		public int TearDownCount = 0;
		[Step] public void Step1()
		{
		}

		[Step] public void Step2()
		{
		}

		[TearDown]
		public void TearDown()
		{
			++TearDownCount;
		}
	}
}
