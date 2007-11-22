#region Using directives

using System;
using System.Text;
using System.Collections;

#endregion

namespace MbUnit.Framework
{
    using MbUnit.Framework;

    /// <summary>
    /// A <see cref="TestCase"/> with verified result.
    /// </summary>
    public class VerifiedTestCase : TestCaseDecoratorBase
    {
        private Object expectedResult;
        public VerifiedTestCase(ITestCase testCase, Object expectedResult)
            :base(testCase)
        {
            this.expectedResult = expectedResult;
        }

        public virtual Object ExpectedResult
        {
            get
            {
                return this.expectedResult;
            }
        }

        public override object Invoke(Object o, IList args)
        {
            Object result = base.Invoke(o,args);
            Assert.AreEqual(this.ExpectedResult, result);

            return result;
        }
    }
}
