using System;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
    /// <summary>
    /// <para>
    /// This attribute decorates a test method and causes it to be invoked repeatedly within the same thread.
    /// </para>
    /// </summary>
    /// <seealso cref="ThreadedRepeatAttribute"/>
	public sealed class RepeatTestAttribute : TestPatternAttribute
	{
        private int count;

        /// <summary>
        /// Gets the number of times the test will be repeated.
        /// </summary>
        /// <value>The the number of times the test will be repeated.</value>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatTestAttribute"/> class.
        /// </summary>
        /// <param name="count">The number of times to repeat the test</param>
        /// <example>
        /// <para>In this example the test will be repeated ten times</para>
        /// <code>
        /// [Test]
        /// [RepeatTest(10)]
        /// public void Test()
        /// {
        ///     // This test will be executed 10 times.
        /// }
        /// </code>
        /// </example>
        public RepeatTestAttribute(int count)
        {
            this.count = count;
        }
    }
}
