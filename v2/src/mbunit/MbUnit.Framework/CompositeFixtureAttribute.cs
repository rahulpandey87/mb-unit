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
	/// Composite fixture pattern implementation.
	/// </summary>
	/// <include file="MbUnit.Framework.doc.xml" path="doc/remarkss/remarks[@name='CompositeFixtureAttribute']"/>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class CompositeFixtureAttribute : TestFixturePatternAttribute
    {
		private Type fixtureType;
		
		/// <summary>
		/// Creates a fixture for the <paramref name="fixtureType"/> type.
		/// </summary>
		/// <remarks>
		/// Initializes the attribute with <paramref name="fixtureType"/>.
		/// </remarks>
		/// <param name="fixtureType">type to apply the fixture to</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="fixtureType"/>
		/// is a null reference</exception>
		public CompositeFixtureAttribute(Type fixtureType)
		:base()
		{
			if (fixtureType==null)
				throw new ArgumentNullException("fixtureType");
			this.fixtureType = fixtureType;
		}
		
		/// <summary>
		/// Creates a fixture for the <paramref name="fixtureType"/> type 
		/// and a description
		/// </summary>
		/// <remarks>
		/// Initializes the attribute with <paramref name="fixtureType"/>.
		/// </remarks>
		/// <param name="fixtureType">type to apply the fixture to</param>
		/// <param name="description">description of the fixture</param>
		/// <exception cref="ArgumentNullException">fixtureType is a null reference</exception>
		public CompositeFixtureAttribute(Type fixtureType,string description)
		:base(description)
		{
			if (fixtureType==null)
				throw new ArgumentNullException("fixtureType");
			this.fixtureType = fixtureType;
		}
		
		/// <summary>
		/// Gets or sets the fixture type.
		/// </summary>
		/// <value>
		/// Fixture instance type.
		/// </value>
		public Type FixtureType
		{
			get
			{
				return this.fixtureType;
			}
			set
			{
				this.fixtureType = value;
			}
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
		public override IRun GetRun()
		{
			SequenceRun runs = new SequenceRun();	

			// add setup
			CustomRun setup = new CustomRun(
				this.fixtureType,
				typeof(SetUpAttribute),
				true // it is a test
				);	
			setup.FeedSender=false;
			runs.Runs.Add(setup);

			// fixture class provider
			FixtureDecoratorRun providerFactory = new FixtureDecoratorRun(
				typeof(ProviderFixtureDecoratorPatternAttribute)
				);
			runs.Runs.Add(providerFactory);

			// add tester for the order
			CustomRun test = new CustomRun(
				this.fixtureType,
				typeof(TestAttribute),
				true // it is a test
				);			
			test.FeedSender=false;
			runs.Runs.Add(test);

			
			// add tear down
			CustomRun tearDown = new CustomRun(
				this.fixtureType,
				typeof(TearDownAttribute),
				true // it is a test
				);			
			tearDown.FeedSender=false;
			runs.Runs.Add(tearDown);
			
			return runs;						
		}
	}
	
}
