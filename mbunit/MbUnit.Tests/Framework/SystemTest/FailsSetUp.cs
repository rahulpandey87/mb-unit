using System;

namespace MbUnit.Tests.Framework.SystemTest
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[SystemTest]
	internal class FailsSetUp
	{
		[SystemTestSetUp] public void FailiingSetup()
		{
			throw new Exception("I'm supposed to die");
		}
		[Step] public void DummyTest()
		{
			DummyTestExecuted = false;
		}
		public bool DummyTestExecuted = false;
	}
}
