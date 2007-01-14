using System;
using MbUnit.Core;
using MbUnit.Core.Framework;
using System.Diagnostics;

namespace MbUnit.Framework 
{
	using MbUnit.Core.Runs;
	
	/// <summary>
	/// Data Test fixture.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class DataFixtureAttribute : TestFixturePatternAttribute
    {
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// </remarks>
		public DataFixtureAttribute()
		:base()
		{}
		
		/// <summary>
		/// Constructor with a fixture description
		/// </summary>
		/// <param name="description">fixture description</param>
		/// <remarks>
		/// </remarks>
		public DataFixtureAttribute(string description)
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
			SequenceRun runs = new SequenceRun();
			
			// setup
			OptionalMethodRun setup = new OptionalMethodRun(typeof(SetUpAttribute),false);
			runs.Runs.Add( setup );
			
			// the tests
			DataFixtureRun tests = new DataFixtureRun();
			runs.Runs.Add(tests);
			
			// tear down
			OptionalMethodRun tearDown = new OptionalMethodRun(typeof(TearDownAttribute),false);
			runs.Runs.Add(tearDown);
			
			return runs;						
		}


	}
}
