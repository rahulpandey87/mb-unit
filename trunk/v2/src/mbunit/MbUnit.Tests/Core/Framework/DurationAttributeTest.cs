namespace MbUnit.Tests.Core.Framework
{
    using System;
	using System.Threading;
	
	using MbUnit.Core.Framework;
	using MbUnit.Framework;
	using MbUnit.Core.Exceptions;
	
	/// <summary>
	/// Test fixture of the <see cref="DurationAttribute"/>.
	/// </summary>
	/// <remark>
	/// </remarks>
	[TestFixture]
    [FixtureCategory("Attributes.Decorators")]
	public class DurationAttributeTest
	{
		[Test]
		[Duration(1)]
		public void EmptyMethod()
		{}
	}
}

