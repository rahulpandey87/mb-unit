using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Cons;

namespace MbUnit.Tests.Core.Cons
{
	[TestFixture]
	public class MainClassTest
	{
		[Test]
		public void MainRun()
		{
			MainClass mc = new MainClass();
			string[] args = new string[] {"hello.exe"};
			mc.Main(args);
		}
	}
}
