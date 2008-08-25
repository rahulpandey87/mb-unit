using System;
using System.Collections;
using MbUnit.Core.Framework;
using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Framework.Testers;
using MbUnit.Core.Runs;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags a class as a specialized fixture for testing classes derived from <see cref="IEnumerable"/> or <see cref="IEnumerator"/>.
    /// The fixture class will automatically test that objects returned by test methods within the class tagged with either
    /// <see cref="DataProviderAttribute"/> or <see cref="CopyToProviderAttribute"/> follow the enumerator specification.
    /// </summary>
    ///<remarks name="EnumerationFixtureAttribute">
    ///<para><em>Implements:</em>Enumeration Test Pattern</para>
    ///<para><em>Login:</em>
    ///<code>
    ///{DataProvider}
    ///{CopyToProvider}
    ///[SetUp] 
    ///(EnumerationTester)
    ///    - GetEnumerator
    ///    - Enumerate
    ///    - ElementWiseEquality
    ///    - Current
    ///    - CurrentWithoutMoveNet
    ///    - CurrentPastEnd
    ///    - Reset
    ///    - CollectionChanged 
    ///[TearDown]
    ///</code>
    ///</para>
    /// <example>
    /// The following example demonstrates the use of the EnumerationFixture, DataProvider and CopyToProvider tags.
    /// <code>
    /// [EnumerationFixture]
    /// public class AreTheseEnumerables
    /// {
    ///     private int count = 50;
    /// 
    ///     [DataProvider(typeof(ArrayList))]
    ///     public ArrayList GetArrayList()
    ///     {
    ///         ArrayList aList = new ArrayList();
    ///         for (int i=0; i&lt;= count; i+=5;)
    ///         {
    ///             list.Add(i);
    ///         }
    ///         return list;
    ///     }
    /// 
    ///     [CopyToProvider(typeof(ArrayList))]
    ///     public ArrayList ConvertToArrayList(IList anotherListType)
    ///     {
    ///         return new ArrayList(anotherListType);
    /// }
    /// </code>
    /// </example>
    ///</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class EnumerationFixtureAttribute : TestFixturePatternAttribute {
        public EnumerationFixtureAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationFixtureAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description of the fixture for your reference.</param>
        public EnumerationFixtureAttribute(string description)
            : base(description) { }


        /// <summary>
        /// Gets the test runner class defining all the test to be run within the tagged fixture class.
        /// </summary>
        /// <returns>A <see cref="SequenceRun"/> object</returns>
        public override IRun GetRun() {

            SequenceRun runs = new SequenceRun();

            // set up
            OptionalMethodRun setup = new OptionalMethodRun(typeof(SetUpAttribute), false);
            runs.Runs.Add(setup);

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
            OptionalMethodRun tearDown = new OptionalMethodRun(typeof(TearDownAttribute), false);
            runs.Runs.Add(tearDown);

            return runs;
        }
    }
}
