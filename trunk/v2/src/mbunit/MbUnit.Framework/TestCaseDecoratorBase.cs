using System;
using System.Collections;

namespace MbUnit.Framework
{

    /// <summary>
    /// Base class for test case decorators
    /// </summary>
    /// <seealso cref="ExpectedExceptionTestCase"/>
    /// <seealso cref="VerifiedTestCase"/>
    /// <seealso cref="MethodTestCase"/>
    public abstract class TestCaseDecoratorBase : ITestCase
    {
        private ITestCase testCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseDecoratorBase"/> class.
        /// </summary>
        /// <param name="testCase">The test case.</param>
		protected TestCaseDecoratorBase(ITestCase testCase)
        {
            if (testCase == null)
                throw new ArgumentNullException("testCase");
            this.testCase = testCase;
        }

        /// <summary>
        /// Gets the test case.
        /// </summary>
        /// <value>The test case.</value>
        public ITestCase TestCase
        {
            get
            {
                return this.testCase;
            }
        }

        /// <summary>
        /// Gets the description of the test case.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description
        {
            get
            {
                return this.TestCase.Description;
            }
        }

        /// <summary>
        /// Gets the name of the test case.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get
            {
                return this.TestCase.Name;
            }
        }

        /// <summary>
        /// Invokes the specified testcase <paramref name="o"/> with the given <see cref="IList"/> of <paramref name="args">arguments</paramref>.
        /// </summary>
        /// <param name="o">The test case to invoke.</param>
        /// <param name="args">The arguments to pass to the test case.</param>
        /// <returns></returns>
        public virtual Object Invoke(Object o, IList args)
        {
            return this.testCase.Invoke(o, args);
        }
    }
}
