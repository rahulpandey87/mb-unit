using System;

namespace MbUnit.Demo 
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[TestFixture]
	public class SimpleRollBackTest
	{
		[Test,RollBack]
		public void DoSomethingAndRollBack()
		{
			Console.WriteLine("booh");
		}
	}
}
