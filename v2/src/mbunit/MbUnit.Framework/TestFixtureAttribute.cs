using System;
using MbUnit.Core;
using MbUnit.Core.Framework;
using System.Diagnostics;

namespace MbUnit.Framework 
{
	using MbUnit.Core.Runs;
	using MbUnit.Core.Reflection;

	/// <summary>
	/// Simple Test Pattern fixture.
	/// </summary>
	/// <include file="MbUnit.Framework.doc.xml" path="doc/remarkss/remarks[@name='TestFixtureAttribute']"/>
	/// <include file="MbUnit.Framework.doc.xml" path="doc/examples/example[@name='GraphicsBitmap']"/>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    [RunFactory(typeof(TestRun))]
    [RunFactory(typeof(RowRun))]
    [RunFactory(typeof(CombinatorialRun))]
	public sealed class TestFixtureAttribute : TestFixturePatternAttribute 
	{
        private static readonly Object syncRoot = new Object();
        private static IRun runs = null;

        /// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// </remarks>
		public TestFixtureAttribute()
		:base()
		{}
		
		/// <summary>
		/// Constructor with a fixture description
		/// </summary>
		/// <param name="description">fixture description</param>
		/// <remarks>
		/// </remarks>
		public TestFixtureAttribute(string description)
		:base(description)
		{}
		
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
            lock (syncRoot)
            {
                if (runs == null)
                    runs = new TestFixtureRun();
                return runs;
            }
        }
    }
}
