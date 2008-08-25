#region Using directives

using System;
using System.Text;
using System.Collections;

#endregion

namespace MbUnit.Framework
{
    using MbUnit.Framework;

    /// <summary>
    /// Used when dynamically adding <see cref="ITestCase"/>s with an expected result to a <see cref="TestSuite"/> 
    /// </summary>
    /// <example>
    /// <para>This example shows how a <see cref="VerifiedTestCase"/> is added to a <see cref="TestSuite"/></para>
    /// <code>
    /// [TestSuite]
    /// public ITestSuite VerifiedSuite() {
    ///     TestSuite suite = new TestSuite("VerifiedSuite");
    /// 
    ///     ITestCase tc = TestCases.Case("This should return true", new TestDelegate(ReturnTrue));
    ///     ITestCase tcr = TestCases.Verified(tc, true);
    ///     suite.Add(tcr);
    ///     return suite;
    /// }
    /// 
    /// public object ReturnTrue(object dummy) {
    ///     return true;
    /// }
    /// </code>
    /// </example>
    public class VerifiedTestCase : TestCaseDecoratorBase
    {
        private Object expectedResult;
        /// <summary>
        /// Initializes a new instance of the <see cref="VerifiedTestCase"/> class.
        /// </summary>
        /// <param name="testCase">The test case.</param>
        /// <param name="expectedResult">The expected result.</param>
        public VerifiedTestCase(ITestCase testCase, Object expectedResult)
            :base(testCase)
        {
            this.expectedResult = expectedResult;
        }

        /// <summary>
        /// Returns the expected result for the method.
        /// </summary>
        /// <value>The expected result.</value>
        public virtual Object ExpectedResult
        {
            get
            {
                return this.expectedResult;
            }
        }

        /// <summary>
        /// Invokes the specified testcase <paramref name="o"/> with the given <see cref="IList"/> of <paramref name="args">arguments</paramref>.
        /// </summary>
        /// <param name="o">The test case to invoke.</param>
        /// <param name="args">The arguments to pass to the test case.</param>
        /// <returns></returns>
        public override object Invoke(Object o, IList args)
        {
            Object result = base.Invoke(o,args);
            Assert.AreEqual(this.ExpectedResult, result);

            return result;
        }
    }
}
