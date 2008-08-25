using System;
using MbUnit.Core.Collections;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Framework 
{
	/// <summary>
	/// Contributes additional tests and setup or teardown steps to the
    /// lifecycle defined by <see cref="TestFixtureAttribute" />.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public abstract class TestFixtureExtensionAttribute : PatternAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixtureExtensionAttribute"/> class.
        /// </summary>
		protected TestFixtureExtensionAttribute()
		{}

        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixtureExtensionAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description of the extension.</param>
		protected TestFixtureExtensionAttribute(string description)
			: base(description)
		{}

        /// <summary>
        /// Called to add runs to perform before setup.
        /// </summary>
        /// <param name="runs">The collection to update</param>
        public virtual void AddBeforeSetupRuns(RunCollection runs)
        {
        }

        /// <summary>
        /// Called to add runs to perform during the test execution cycle.
        /// </summary>
        /// <param name="runs">The collection to update</param>
        public virtual void AddTestRuns(RunCollection runs)
        {
        }

        /// <summary>
        /// Called to add runs to perform after teardown.
        /// </summary>
        /// <param name="runs">The collection to update</param>
        public virtual void AddAfterTearDownRuns(RunCollection runs)
        {
        }
    }
}
