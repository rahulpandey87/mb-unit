using System;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
    /// <summary>
    /// Declares a row test when applied to a test method along with one or more
    /// <see cref="RowAttribute" /> attributes.
    /// </summary>
    /// <remarks>The MbUnit v3 data-driven testing features have been consolidated so the [RowTest] attribute is no longer necessary.
    /// Just use the [Test] attribute instead</remarks>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public sealed class RowTestAttribute : TestPatternAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="RowTestAttribute"/> class.
        /// </summary>
        public RowTestAttribute() 
        { }
    }
}
