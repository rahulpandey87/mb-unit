// created on 30/01/2004 at 11:15

namespace MbUnit.Framework
{
	using System;
	using System.Collections;
	using MbUnit.Core;
    using MbUnit.Core.Framework;
    using System.Diagnostics;
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Runs;

	/// <summary>
	/// Type fixture pattern implementation.
	/// </summary>
	/// <include file="MbUnit.Framework.doc.xml" path="doc/remarkss/remarks[@name='TypeFixtureAttribute']"/>
	/// <include file="MbUnit.Framework.doc.xml" path="doc/examples/example[@name='IDictionary']"/>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public sealed class TypeFixtureAttribute : TestFixturePatternAttribute 
	{
		private Type testedType;
		
		/// <summary>
		/// Creates a fixture for the <paramref name="testedType"/> type.
		/// </summary>
		/// <remarks>
		/// Initializes the attribute with <paramref name="testedType"/>.
		/// </remarks>
		/// <param name="testedType">type to apply the fixture to</param>
		/// <exception cref="ArgumentNullException">testedType is a null reference</exception>
		public TypeFixtureAttribute(Type testedType)
		:base()
		{
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			this.testedType = testedType;
		}
		
		/// <summary>
		/// Creates a fixture for the <paramref name="testedType"/> type 
		/// and a description
		/// </summary>
		/// <remarks>
		/// Initializes the attribute with <paramref name="testedType"/>.
		/// </remarks>
		/// <param name="testedType">type to apply the fixture to</param>
		/// <param name="description">description of the fixture</param>
		/// <exception cref="ArgumentNullException">testedType is a null reference</exception>
		public TypeFixtureAttribute(Type testedType,string description)
		:base(description)
		{
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			this.testedType = testedType;
		}
		
		/// <summary>
		/// Creates the execution logic
		/// </summary>
		/// <remarks>
		/// See summary.
		/// </remarks>
		/// <returns>A <see cref="IRun"/> instance that represent the type
		/// test logic.
		/// </returns>
		/// <include file="MbUnit.Framework.doc.xml" path="doc/examples/example[@name='IDictionary']"/>
		public override IRun GetRun()
		{
			SequenceRun runs = new SequenceRun();

			// creating parallel
			ParallelRun para= new ParallelRun();
            para.AllowEmpty = false;
            runs.Runs.Add(para);

            // method providers
			MethodRun provider = new MethodRun(
			                                   typeof(ProviderAttribute),
			                                   typeof(ArgumentFeederRunInvoker),
			                                   false,
			                                   true
			                                   );
			para.Runs.Add(provider);

			// fixture class provider
			FixtureDecoratorRun providerFactory = new FixtureDecoratorRun(
				typeof(ProviderFixtureDecoratorPatternAttribute)
				);
			para.Runs.Add(providerFactory);

            // setup
            OptionalMethodRun setup = new OptionalMethodRun(typeof(SetUpAttribute), false);
            runs.Runs.Add(setup);

            // tests
			MethodRun test = new MethodRun(typeof(TestPatternAttribute),true,true);
			runs.Runs.Add(test);
			
			// tear down
			OptionalMethodRun tearDown = new OptionalMethodRun(typeof(TearDownAttribute),false);
			runs.Runs.Add(tearDown);
			
			return runs;						
		}
	}
	
}
