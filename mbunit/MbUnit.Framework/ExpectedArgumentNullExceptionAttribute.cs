using System;

namespace MbUnit.Framework
{
	using MbUnit.Framework;

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,AllowMultiple=false,Inherited=true)]
    public sealed class ExpectedArgumentNullExceptionAttribute : ExpectedExceptionAttribute
    {
		public ExpectedArgumentNullExceptionAttribute()
			:base(typeof(ArgumentNullException))
		{}
		public ExpectedArgumentNullExceptionAttribute(string description)
			:base(typeof(ArgumentNullException),description)
		{}
	}
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ExpectedArgumentExceptionAttribute : ExpectedExceptionAttribute
    {
        public ExpectedArgumentExceptionAttribute()
			:base(typeof(ArgumentException))
        { }
        public ExpectedArgumentExceptionAttribute(string description)
			:base(typeof(ArgumentException),description)
        { }
    }
}
