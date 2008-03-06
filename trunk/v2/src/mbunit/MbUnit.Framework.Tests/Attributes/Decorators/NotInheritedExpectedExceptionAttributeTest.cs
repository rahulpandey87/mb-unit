using System;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Attributes.Decorators
{	
	/// <summary>
	/// Test fixture for the <see cref="NotInheritedExpectedExceptionAttribute" />.
	/// </summary>
	/// <remark>
	/// </remarks>
	[TestFixture, FixtureCategory("Attributes.Decorators")]
	public class NotInheritedExpectedExceptionAttributeTest
	{
        [Test, NotInheritedExpectedException(typeof(NotImplementedException))]
        public virtual void Exception()
        {
            throw new NotImplementedException();
        }
	}

    /// <summary>
    /// Test inheritance of <see cref="NotInheritedExpectedExceptionAttribute" />.
    /// </summary>
    /// <remark>
    /// </remarks>
    [TestFixture, FixtureCategory("Attributes.Decorators")]
    public class InheritedNotInheritedExpectedExceptionAttributeTest : NotInheritedExpectedExceptionAttributeTest
    {
        public override void Exception()
        { }
    }
}