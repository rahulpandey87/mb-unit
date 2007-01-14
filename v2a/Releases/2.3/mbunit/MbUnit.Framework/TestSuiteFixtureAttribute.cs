using System;
using System.Reflection;
using MbUnit.Core.Framework;
using MbUnit.Core;
using MbUnit.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
    using MbUnit.Core.Runs;

    /// <summary>
    /// Test Suite fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TestSuiteFixtureAttribute : TestFixturePatternAttribute
    {
        private static volatile IRun testSuiteRun = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TestSuiteFixtureAttribute()
		:base()
        { }

        /// <summary>
        /// Constructor with a fixture description
        /// </summary>
        /// <param name="description">fixture description</param>
        /// <remarks>
        /// </remarks>
        public TestSuiteFixtureAttribute(string description)
		:base(description)
        { }

        /// <summary>
        /// Creates the execution logic
        /// </summary>
        /// <remarks>
        /// See summary.
        /// </remarks>
        /// <returns>A <see cref="IRun"/> instance that represent the type
        /// test logic.
        /// </returns>
        /// <include file="MbUnit.Framework.doc.xml" path="doc/examples/example[@name='GraphicsBitmap']"/>
        public override IRun GetRun()
        {
            lock(typeof(TestSuiteFixtureAttribute))
            {
                if (testSuiteRun == null)
                    testSuiteRun = new TestSuiteRun();
                return testSuiteRun;
            }
        }
    }
}
