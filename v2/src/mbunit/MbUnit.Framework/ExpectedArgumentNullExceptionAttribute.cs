using System;

namespace MbUnit.Framework {
    using MbUnit.Framework;

    /// <summary>
    /// Subtype of the <see cref="ExpectedExceptionAttribute"/> that automatically expects an <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <remarks>
    /// Created as part of <a href="http://blog.dotnetwiki.org/CommentView,guid,54e713b6-7d54-48ba-b0e7-a6982bf54e7e.aspx">this demonstration</a>
    /// of combinatorial tests.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ExpectedArgumentNullExceptionAttribute : ExpectedExceptionAttribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedArgumentNullExceptionAttribute"/> class.
        /// </summary>
        public ExpectedArgumentNullExceptionAttribute()
            : base(typeof(ArgumentNullException)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedArgumentNullExceptionAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description about the expected exception for your reference.</param>
        public ExpectedArgumentNullExceptionAttribute(string description)
            : base(typeof(ArgumentNullException), description) { }
    }


    /// <summary>
    /// Subtype of the <see cref="ExpectedExceptionAttribute"/> that automatically expects an <see cref="ArgumentException"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ExpectedArgumentExceptionAttribute : ExpectedExceptionAttribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedArgumentExceptionAttribute"/> class.
        /// </summary>
        public ExpectedArgumentExceptionAttribute()
            : base(typeof(ArgumentException)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedArgumentExceptionAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description about the expected exception for your reference.</param>
        public ExpectedArgumentExceptionAttribute(string description)
            : base(typeof(ArgumentException), description) { }
    }
}
