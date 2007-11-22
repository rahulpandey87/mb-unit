using System;

namespace MbUnit.Tests.Framework.AutoRollBack
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[TestFixture]
	public class SimpleRollBackTest
	{
		[Test]
		public void DoSomethingAndRollBack()
		{
			Console.WriteLine("booh");
		}
	}
}
