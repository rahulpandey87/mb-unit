using System;
using System.Collections;
using MbUnit.Core.Framework;
using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Framework.Testers;
using MbUnit.Core.Runs;

namespace MbUnit.Framework
{
	/// <summary>
	/// Enumeration Pattern implementations.
	/// </summary>
	/// <include file="MbUnit.Framework.doc.xml" path="doc/remarkss/remarks[@name='EnumerationFixtureAttribute']"/>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class EnumerationFixtureAttribute : TestFixturePatternAttribute
    {
		public EnumerationFixtureAttribute()
		{}
		
		public EnumerationFixtureAttribute(string description)
		:base(description)
		{}
		
		/// <summary>
		/// </summary>		
		public override IRun GetRun()
		{

			SequenceRun runs = new SequenceRun();

			// set up
			OptionalMethodRun setup = new OptionalMethodRun(typeof(SetUpAttribute),false);			
			runs.Runs.Add( setup );

			// create data
			MethodRun dataProvider = new MethodRun(
			                                   typeof(DataProviderAttribute),
			                                   typeof(ArgumentFeederRunInvoker),
			                                   false,
			                                   true
			                                   );
			runs.Runs.Add(dataProvider);

			// collection providers	for collection		
			MethodRun copyTo = new MethodRun(
			                                   typeof(CopyToProviderAttribute),
			                                   typeof(ArgumentFeederRunInvoker),
			                                   false,
			                                   true
			                                   );
			runs.Runs.Add(copyTo);
						
			// add tester for the order
			CustomRun test = new CustomRun(
				typeof(EnumerationTester),
				typeof(TestAttribute),
				true // it is a test
				);
			runs.Runs.Add(test);
									

			// tear down
			OptionalMethodRun tearDown = new OptionalMethodRun(typeof(TearDownAttribute),false);
			runs.Runs.Add(tearDown);

			return runs;						
		}		
	}
}
