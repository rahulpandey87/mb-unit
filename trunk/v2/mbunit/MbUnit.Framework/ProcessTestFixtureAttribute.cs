using System;
using MbUnit.Core;
using MbUnit.Core.Framework;
using System.Diagnostics;

namespace MbUnit.Framework
{
    using MbUnit.Core.Runs;
	
	/// <summary>
	/// Process Test Pattern fixture.
	/// </summary>
	/// <remarks>
	/// <para><em>Implements:</em> Process Test Fixture</para>
	/// <para><em>Logic:</em>
	/// <code>
	/// [SetUp]
	/// {TestSequence}
	/// [TearDown]
	/// </code>
	/// </para>
	/// <para>
	/// This fixture implements the Process Test Fixture as described in the
	/// <a href="http://www.codeproject.com/csharp/autp3.asp">CodeProject</a>
	/// article from Marc Clifton.
	/// </para>
	/// <para>
	/// In this implementation, reverse traversal is not implemented.
	/// A process can be seen as a linear graph, a very simple model. If you
	/// need more evolved models, use Model Based Testing.
	/// </para>
	/// </remarks>
	/// <include file='MbUnit.Framework.doc.xml' path='doc/examples/example[@name="DbSequence"]'/>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class ProcessTestFixtureAttribute : TestFixturePatternAttribute
    {
		/// <summary>
		/// Initialize a <see cref="ProcessTestFixtureAttribute"/>
		/// instance.
		/// </summary>
		public ProcessTestFixtureAttribute()
		:base()
		{}
		
		/// <summary>
		/// Constructor with a fixture description
		/// </summary>
		/// <param name="description">fixture description</param>
		public ProcessTestFixtureAttribute(string description)
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
		public override IRun GetRun()
		{
			SequenceRun runs = new SequenceRun();
			
			// process tests
			ProcessMethodRun test = new ProcessMethodRun(typeof(TestSequenceAttribute));
			runs.Runs.Add(test);
			
			return runs;
		}
	}
}
