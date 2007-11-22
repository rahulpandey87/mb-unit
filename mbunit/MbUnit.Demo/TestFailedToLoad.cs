using System;

namespace MbUnit.Demo
{

	using MbUnit.Framework;

	[TestFixture]
	public class TestFailedToLoad
	{
		[Test]
		public void BadSignature(object context)
		{
			throw new Exception("This test should fail to load");
		}

		[Test]
		public void ThisOneWorks()
		{}
	}
}
