using System;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Core.Exceptions
{	
	/// <summary>
	/// Test fixture of the <see cref="ExpectedExceptionAttribute"/>.
	/// </summary>
	/// <remark>
	/// </remarks>
	[TestFixture]
    [FixtureCategory("Attributes.Decorators")]
	public class ExpectedExceptionAttributeTest
	{
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void Exception()
        {
            throw new NotImplementedException();
        }

		[Test]
		[ExpectedException(typeof(NotImplementedException), "Not implemented")]
		public void ExceptionAndDescription()
		{
            throw new NotImplementedException();
        }

        [Test, ExpectedException(typeof(NotImplementedException), typeof(ArgumentException))]
        public void ExceptionAndInnerException()
        {
            throw new NotImplementedException("", new ArgumentException());
        }
	}
}