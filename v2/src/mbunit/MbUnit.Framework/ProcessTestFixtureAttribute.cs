using System;
using MbUnit.Core;
using MbUnit.Core.Framework;
using System.Diagnostics;

namespace MbUnit.Framework {
    using MbUnit.Core.Runs;

    /// <summary>
    /// Tags a test fixture class as containing test methods which must be executed in a given sequence.
    /// The sequence is given by tagging each method with a <see cref="TestSequenceAttribute"/>
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
    /// <para>
    /// This is the example for the <a href="http://www.codeproject.com/csharp/autp3.asp">CodeProject</a> 
    /// article adapted to MbUnit.
    /// </para>
    /// <code>
    /// <b>[ProcessTestFixture]</b>
    /// public class POSequenceTest
    /// {	
    /// 	...
    /// 	<b>[TestSequence(1)]</b>
    /// 	public void POConstructor()
    /// 	{
    /// 		po=new PurchaseOrder();
    /// 		Assert.AreEqual(po.Number,"", "Number not initialized.");
    /// 		Assert.AreEqual(po.PartCount,0, "PartCount not initialized.");
    /// 		Assert.AreEqual(po.ChargeCount,0, "ChargeCount not initialized.");
    /// 		Assert.AreEqual(po.Invoice,null, "Invoice not initialized.");
    /// 		Assert.AreEqual(po.Vendor,null, "Vendor not initialized.");
    /// 	}
    /// 
    /// 	[TestSequence(2)]
    /// 	public void VendorConstructor()
    /// 	{
    /// 		vendor=new Vendor();
    /// 		Assert.AreEqual(vendor.Name,"", "Name is not an empty string.");
    /// 		Assert.AreEqual(vendor.PartCount,0, "PartCount is not zero.");
    /// 	}
    /// 	...
    /// </code>
    /// <para>
    /// Use <see cref="ProcessTestFixtureAttribute"/> to mark a class as process test fixture and use the 
    /// <see cref="TestSequenceAttribute"/> attribute to create the order of the process. The fixture also supports
    /// SetUp and TearDown methods.
    /// </para>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ProcessTestFixtureAttribute : TestFixturePatternAttribute {
        /// <summary>
        /// Initialize a <see cref="ProcessTestFixtureAttribute"/>
        /// instance.
        /// </summary>
        public ProcessTestFixtureAttribute()
            : base() { }

        /// <summary>
        /// Constructor with a fixture description
        /// </summary>
        /// <param name="description">fixture description</param>
        public ProcessTestFixtureAttribute(string description)
            : base(description) { }

        /// <summary>
        /// Gets the test runner class defining all the tests to be run and the test logic to be used within the tagged fixture class.
        /// </summary>
        /// <returns>A <see cref="SequenceRun"/> object</returns>
        public override IRun GetRun() {
            SequenceRun runs = new SequenceRun();

            // process tests
            ProcessMethodRun test = new ProcessMethodRun(typeof(TestSequenceAttribute));
            runs.Runs.Add(test);

            return runs;
        }
    }
}
