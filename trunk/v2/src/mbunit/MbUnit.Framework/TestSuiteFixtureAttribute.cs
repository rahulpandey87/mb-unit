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
    /// Idnetifies a class that will dynamically generate tests and group them in <see cref="TestSuite"/>s.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A TestSuite is a fixture populated at runtime, a dynamic fixture. A TestSuite can be filled with any number of TestCase. 
    /// A good example of TestSuite application is DataDrivenTesting where you want to create a TestCase for each data entry.
    /// </para>
    /// <para>
    /// To use suites, tag your class with the <see cref="TestSuiteFixtureAttribute"/> attribute. 
    /// Each method which creates a suite must be tagged with <see cref="TestSuiteAttribute"/> and return a <see cref="TestSuite"/>. 
    /// Of course, there can be multiple methods returning suites. A <see cref="TestSuite"/> can be filled with 
    /// <see cref="ITestCase"/> implementations, whether you implement your own version or you use the built-in implementations.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    ///     using System;
    ///     using MbUnit.Core.Framework;
    ///     using MbUnit.Framework;
    ///     namespace MyNamespace {
    ///         [TestSuiteFixture]
    ///         public class MyClass {
    ///             public delegate void TestDelegate(Object context);
    /// 
    ///             [TestSuite]
    ///             public TestSuite GetSuite() {
    ///                 TestSuite suite = new TestSuite("Suite1");
    /// 
    ///                 suite.Add("Test1", new TestDelegate(this.Test), "hello");
    ///                 suite.Add("Test2", new TestDelegate(this.AnotherTest), "another test");
    /// 
    ///                 return suite;
    ///             }
    /// 
    ///             public void Test(object testContext) {
    ///                 Console.WriteLine("Test");
    ///                 Assert.AreEqual("hello", testContext);
    ///             }
    ///             public void AnotherTest(object testContext) {
    ///                 Console.WriteLine("AnotherTest");
    ///                 Assert.AreEqual("another test", testContext);
    ///             }
    ///         }
    ///     }
    /// </code>
    /// </example>
    /// <seealso cref="TestCase"/>
    /// <seealso cref="MethodTestCase"/>
    /// <seealso cref="ExpectedExceptionTestCase"/>
    /// <seealso cref="VerifiedTestCase"/>
    /// <seealso cref="TestSuiteAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TestSuiteFixtureAttribute : TestFixturePatternAttribute
    {
        private static volatile IRun testSuiteRun = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuiteFixtureAttribute"/> class.
        /// </summary>
        public TestSuiteFixtureAttribute()
		:base()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuiteFixtureAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description of the test fixture.</param>
        public TestSuiteFixtureAttribute(string description)
		:base(description)
        { }

        /// <summary>
        /// Gets the test runner class defining all the tests to be run and the test logic to be used within the tagged fixture class.
        /// </summary>
        /// <returns>A <see cref="TestSuiteRun"/> object</returns>
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
