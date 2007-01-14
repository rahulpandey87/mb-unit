using System;

namespace MbUnit.Tests.Framework.SystemTest
{
	using MbUnit.Framework;
	/// <summary>
	/// Make public if you want to see in the GUI.  Step 3 should show red, and 4 should be yellow.
	/// </summary>
	[SystemTest]
	public class Sample
	{
		[Step]
		public void Step1()
		{
		}
		[Step]
		public void Step2()
		{
		}
		[CriticalStep]
		public void Step3()
		{
			throw new Exception("I'm supposed to die!");
		}
		[Step]
		public void Step4()
		{
		}
	}
}
