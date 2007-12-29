namespace MbUnit.Tests.Core.Framework
{
	using System;
	
	using MbUnit.Core.Framework;
	using MbUnit.Framework;
	using MbUnit.Core.Exceptions;
	
	/// <summary>
	/// Test fixture of the <see cref="ConditionalExceptionAttribute"/>.
	/// </summary>
	/// <remark>
	/// </remarks>
	[TypeFixture(typeof(bool))]
	public class ConditionalExceptionAttributeTest
	{
		[Provider(typeof(bool))]
		public bool True()
		{
			return true;
		}

		[Provider(typeof(bool))]
		public bool False()
		{
			return false;
		}

		public bool IsTrue(bool b)
		{
			return b;
		}

		[Test]
		[ConditionalException(typeof(AssertionException),"IsTrue")]
		public void AssertValue(bool b)
		{
			Assert.IsFalse(b);
		}
	}
}

