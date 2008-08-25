using System;
using MbUnit.Core;
using MbUnit.Core.Framework;
using System.Diagnostics;

namespace MbUnit.Framework 
{
	using MbUnit.Core.Runs;
	using MbUnit.Core.Reflection;

    /// <summary>
    /// <para>
    /// The test fixture attribute is applied to a class that contains a suite
    /// of related test cases.  If an error occurs while initializing the fixture
    /// or if at least one of the test cases within the fixture fails,
    /// then the fixture itself will be deemed to have failed.  Otherwise the
    /// fixture will pass.
    /// Output from the fixture, such as text written to the console, is captured
    /// by the framework and will be included in the test report.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The class must have a public default constructor.  The class may not be static.
    /// </para>
    /// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    [RunFactory(typeof(TestRun))]
    [RunFactory(typeof(RowRun))]
    [RunFactory(typeof(CombinatorialRun))]
	public sealed class TestFixtureAttribute : TestFixturePatternAttribute 
	{
        private static readonly Object syncRoot = new Object();
        private static IRun runs = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixtureAttribute"/> class.
        /// </summary>
		public TestFixtureAttribute()
		:base()
		{}


        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixtureAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description of the test fixture.</param>
		public TestFixtureAttribute(string description)
		:base(description)
		{}

        /// <summary>
        /// Gets the test runner class defining all the tests to be run within the tagged fixture class.
        /// </summary>
        /// <returns>A <see cref="TestFixtureRun"/> object</returns>
		public override IRun GetRun()
		{
            lock (syncRoot)
            {
                if (runs == null)
                    runs = new TestFixtureRun();
                return runs;
            }
        }
    }
}
