using System;
using System.Collections;

namespace MbUnit.Framework
{
    public abstract class TestCaseDecoratorBase : ITestCase
    {
        private ITestCase testCase;

		protected TestCaseDecoratorBase(ITestCase testCase)
        {
            if (testCase == null)
                throw new ArgumentNullException("testCase");
            this.testCase = testCase;
        }

        public ITestCase TestCase
        {
            get
            {
                return this.testCase;
            }
        }

        public virtual string Description
        {
            get
            {
                return this.TestCase.Description;
            }
        }

        public virtual string Name
        {
            get
            {
                return this.TestCase.Name;
            }
        }

        public virtual Object Invoke(Object o, IList args)
        {
            return this.testCase.Invoke(o, args);
        }
    }
}
