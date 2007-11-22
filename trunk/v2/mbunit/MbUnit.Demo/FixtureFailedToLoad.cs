using System;

namespace MbUnit.Demo
{

	using MbUnit.Core.Framework;
    using MbUnit.Framework;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public class SickTestFixtureAttribute : TestFixturePatternAttribute
	{
		public override MbUnit.Core.Runs.IRun GetRun()
		{
			throw new Exception("boom");
		}
	}

	[SickTestFixture]
	[FixtureCategory("FailingTests")]
	public class FixtureFailedToLoad
	{
	}
}
