using System;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
    /// <summary>
    /// Declares a row test when applied to a test method along with one or more
    /// <see cref="RowAttribute" /> attributes.
    /// </summary>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public sealed class RowTestAttribute : TestPatternAttribute
	{
        public RowTestAttribute() 
        { }
    }
}
