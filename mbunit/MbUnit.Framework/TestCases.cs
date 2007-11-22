using System;
using System.Collections;
using System.Reflection;

namespace MbUnit.Framework
{
    public sealed class TestCases
    {
        private TestCases()
        {}

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

        public static VerifiedTestCase Verified(ITestCase testCase, Object expectedResult)
        {
            if (testCase == null)
                throw new ArgumentNullException("testCase");
            return new VerifiedTestCase(testCase, expectedResult);
        }

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
