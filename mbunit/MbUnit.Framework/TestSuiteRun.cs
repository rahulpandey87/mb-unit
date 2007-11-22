using System;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Core.Runs;
using MbUnit.Core.Invokers;
using MbUnit.Framework;

namespace MbUnit.Framework
{
    internal sealed class TestSuiteRun : Run
    {
        public TestSuiteRun()
                :base("TestSuite",true)
        { }


        public override void Reflect(
            RunInvokerTree tree,
            RunInvokerVertex parent,
            Type t)
        {
            MethodInfo setUp = TypeHelper.GetAttributedMethod(t, typeof(SetUpAttribute));
            MethodInfo tearDown = TypeHelper.GetAttributedMethod(t, typeof(TearDownAttribute));

            // explore type for methods with TestSuite
            foreach (MethodInfo mi in TypeHelper.GetAttributedMethods(
                        t,
                        typeof(TestSuiteAttribute)
                        )
                )
            {
                // check signature
                TypeHelper.CheckSignature(mi, typeof(ITestSuite));

                // create type instance and create test suite
                Object fixture = null;
                try
                {
                    fixture = TypeHelper.CreateInstance(t);

                    // get test suite
                    ITestSuite suite = mi.Invoke(fixture, null) as ITestSuite;
                    Assert.IsNotNull(suite, "TestSuite method cannot return null");

                    // populated tree down...
                    foreach (ITestCase tc in suite.TestCases)
                    {
                        TestCaseRunInvoker invoker = new TestCaseRunInvoker(
                            this, suite, tc, setUp, tearDown
                            );
                        tree.AddChild(parent, invoker);
                    }
                }
                finally
                {
                    IDisposable disposable = fixture as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
            }
        }
    }
}
