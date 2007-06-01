using System;

using MbUnit.Framework;
using MbUnit.Core.Exceptions;

namespace MbUnit.Demo
{
	[TestFixture]
	[FixtureCategory("FailingTests")]
	public class FixtureWithSickConstructorTest
	{
		public FixtureWithSickConstructorTest()
		{
			throw new Exception("Constructor throwed");
		}

		[Test]
		[ExpectedException(typeof(ConstructorThrowedException))]
		public void ConstructorThrows()
		{}
	}
}
