using System;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Core.Runs;

namespace MbUnit.Framework
{
    internal sealed class TestCaseRunInvoker : RunInvoker
    {
        private MethodInfo setUp;
        private MethodInfo tearDown;
        private ITestSuite testSuite;
        private ITestCase testCase;

        public TestCaseRunInvoker(
            IRun run,
            ITestSuite testSuite,
            ITestCase testCase,
            MethodInfo setUp,
            MethodInfo tearDown
            )
                :base(run)
        {
            this.testSuite = testSuite;
            this.testCase = testCase;

            this.setUp = setUp;
            this.tearDown = tearDown;
        }

        public override String Name
        {
            get
            {
                return String.Format("{0}.{1}",
                    this.testSuite.Name,
                    this.testCase.Name
                    );
            }
        }

        public override Object Execute(Object o, System.Collections.IList args)
        {
            if (this.setUp != null)
                this.setUp.Invoke(o, null);
            try
            {
                Object result = this.testCase.Invoke(o, args);
                return result;
            }
            finally
            {
                if (this.tearDown != null)
                    this.tearDown.Invoke(o, null);
            }
        }
    }
}
