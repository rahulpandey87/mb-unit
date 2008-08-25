using System;
using System.Collections;
using MbUnit.Core.Framework;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags a method that returns a <see cref="TestSuite"/>.
    /// Use within a class tagged with a <see cref="TestSuiteFixtureAttribute"/>
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
    /// <seealso cref="TestSuiteFixtureAttribute"/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class TestSuiteAttribute : PatternAttribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuiteAttribute"/> class.
        /// </summary>
        public TestSuiteAttribute() { }
    }
}
