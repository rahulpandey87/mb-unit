using System;
using System.Collections;
using System.Reflection;

namespace MbUnit.Framework
{
    /// <summary>
    /// Factory class that wraps test delegates inside instances <see cref="ITestCase"/> for inclusion in test suites.
    /// </summary>
    /// <example>
    /// <para>This example shows how <see cref="TestCases"/> is used to generate the correct type of test case before 
    /// it is added to a <see cref="TestSuite"/>. In this case, the <see cref="TestDelegate"/> is first wrapped in a 
    /// <see cref="TestCase"/> class which is then wrapped in an <see cref="ExpectedExceptionTestCase"/>.</para>
    /// <code>
    ///[TestSuite]
    ///public ITestSuite ExpectedExceptionSuite()
    ///{
    ///    TestSuite suite = new TestSuite("ExpectedExceptionSuite");
    ///
    ///    ITestCase case = TestCases.Case("This should raise an Exception", new TestDelegate(this.ThrowAnException));
    ///    ITestCase caseWithException = TestCases.ExpectedException(case, typeof(ArgumentException));
    ///    suite.Add(caseWithException);
    ///    return suite;
    ///}
    ///
    ///public object ThrowAnException()
    ///{
    ///    throw new ArgumentException();
    ///}
    /// </code>
    /// </example>
    public sealed class TestCases
    {
        private TestCases()
        {}

        /// <summary>
        /// Wraps the <paramref name="test"/> method in a <see cref="TestCase"/> class
        /// </summary>
        /// <param name="name">The name of the test case</param>
        /// <param name="test">A <see cref="Delegate"/> representing the test to include in the suite</param>
        /// <param name="parameters">The parameters for the test method</param>
        /// <returns>A <see cref="TestCase"/> object</returns>
        /// <exception cref="ArgumentNullException">Thrown if either<paramref name="name"/> or <paramref name="test"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is an empty string</exception>
        public static TestCase Case(string name, Delegate test, params Object[] parameters)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("name is empty");
            if (test == null)
                throw new ArgumentNullException("test");
            TestCase tc = new TestCase(name, test, parameters);
            return tc;
        }

        /// <summary>
        /// Wraps a <see cref="MethodInfo">method</see> in a <see cref="MethodTestCase"/> class
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="testedInstance">The instance of the object to call the method on.</param>
        /// <param name="test">a <see cref="MethodInfo"/> object identifying the test method.</param>
        /// <param name="parameters">The parameters to pass to the method when run</param>
        /// <returns>A <see cref="MethodTestCase"/> object</returns>
        /// <exception cref="ArgumentNullException">Thrown if either
        /// <paramref name="name"/>, <paramref name="test"/> or <paramref name="testedInstance"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is an empty string</exception>
        public static MethodTestCase Case(string name, Object testedInstance, MethodInfo test, params Object[] parameters)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("name is empty");
            if (test == null)
                throw new ArgumentNullException("test");
			if (testedInstance==null)
				throw new ArgumentNullException("testedInstance");
            MethodTestCase tc = new MethodTestCase(name, testedInstance, test, parameters);
            return tc;
        }

        /// <summary>
        /// Associates a <paramref name="testCase"/> with an expected result.
        /// </summary>
        /// <param name="testCase">The test case.</param>
        /// <param name="expectedResult">The expected result.</param>
        /// <returns>A <see cref="VerifiedTestCase"/> object</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="testCase"/> is null</exception>
        public static VerifiedTestCase Verified(ITestCase testCase, Object expectedResult)
        {
            if (testCase == null)
                throw new ArgumentNullException("testCase");
            return new VerifiedTestCase(testCase, expectedResult);
        }

        /// <summary>
        /// Associates a <paramref name="testCase"/> with an expected exception with the given <paramref name="exceptionType"/>.
        /// </summary>
        /// <param name="testCase">The test case.</param>
        /// <param name="exceptionType"><see cref="Type"/> of the expected exception.</param>
        /// <returns>An <see cref="ExpectedExceptionTestCase"/> object</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="testCase"/> or <paramref name="exceptionType"/> is null</exception>
        public static ExpectedExceptionTestCase ExpectedException(ITestCase testCase, Type exceptionType)
        {
            if (testCase == null)
                throw new ArgumentNullException("testCase");
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");
            return new ExpectedExceptionTestCase(testCase, exceptionType);
        }
    }
}
