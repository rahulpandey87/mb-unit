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
		[ExpectedException(typeof(NotImplementedException), Description="Not implemented")]
		public void ExceptionAndDescription()
		{
            throw new NotImplementedException();
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException), "This should match.")]
        public void ExceptionAndExpectedMessage()
        {
            throw new NotImplementedException("This should match.");
        }

        [Test, ExpectedException(typeof(NotImplementedException), typeof(ArgumentException))]
        public void ExceptionAndInnerException()
        {
            throw new NotImplementedException("", new ArgumentException());
        }

        [Test, ExpectedException(typeof(NotImplementedException), "This should match.", typeof(ArgumentException))]
        public void ExceptionAndInnerExceptionAndExpectedMessage()
        {
            throw new NotImplementedException("This should match.", new ArgumentException());
        }
	}
}