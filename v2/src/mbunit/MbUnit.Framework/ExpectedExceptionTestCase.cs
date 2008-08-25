using System;
using System.Collections;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Core.Exceptions;

namespace MbUnit.Framework {
    /// <summary>
    /// Used when dynamically adding <see cref="ITestCase"/>s that expect an exception to a <see cref="TestSuite"/> 
    /// </summary>
    /// <remarks>
    /// <para>This has the same purpose as applying a <see cref="ExpectedExceptionAttribute"/> to a static test method </para>
    /// </remarks>
    /// <example>
    /// <para>This example shows how an <see cref="ExpectedExceptionTestCase"/> is added to a <see cref="TestSuite"/></para>
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
    public sealed class ExpectedExceptionTestCase : TestCaseDecoratorBase {
        private Type exceptionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionTestCase"/> class.
        /// </summary>
        /// <param name="testCase">The test case to be added to the test suite.</param>
        /// <param name="exceptionType">Type of the exception expected to be thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exceptionType"/> is null</exception>
        public ExpectedExceptionTestCase(ITestCase testCase, Type exceptionType)
            : base(testCase) {
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");
            this.exceptionType = exceptionType;
        }

        /// <summary>
        /// Returns the <see cref="Type"/> of the exception expected to be thrown.
        /// </summary>
        /// <value>The <see cref="Type"/> of the exception expected to be thrown.</value>
        public Type ExceptionType {
            get {
                return this.exceptionType;
            }
        }

        /// <summary>
        /// Invokes the test case expecting an exception
        /// </summary>
        /// <param name="o">The <see cref="ITestCase"/> to run</param>
        /// <param name="args">Parameter values for the test case</param>
        /// <returns>null</returns>
        /// <exception cref="ExceptionNotThrownException">Thrown if the expected exception does not occur</exception>
        public override Object Invoke(Object o, IList args) {
            try {
                Object result = base.Invoke(o, args);
            } catch (Exception ex) {
                Exception catchedException = ex;
                if (catchedException is TargetInvocationException)
                    catchedException = ex.InnerException;

                Exception current = catchedException;

                while (!this.ExceptionType.IsInstanceOfType(current)) {
                    current = current.InnerException;
                    if (current == null)
                        throw new ExceptionTypeMistmachException(
                            this.ExceptionType,
                            this.TestCase.Description,
                            catchedException
                            );
                }

                return null;
            }
            // if we are here it did not throw
            throw new ExceptionNotThrownException(ExceptionType, this.Description);
        }
    }
}
