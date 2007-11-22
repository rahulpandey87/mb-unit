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
            : base("TestSuite", true)
        { }


        public override void Reflect(
            RunInvokerTree tree,
            RunInvokerVertex parent,
            Type t)
        {
            // create type instance and create test suite
            Object fixture = null;
            try
            {
                fixture = TypeHelper.CreateInstance(t);

                // run TestSuiteSetUp if necessary
                MethodInfo testSuiteSetUp = TypeHelper.GetAttributedMethod(t, typeof(TestSuiteSetUpAttribute));
                if (testSuiteSetUp != null)
                    testSuiteSetUp.Invoke(fixture, null);

                // look for SetUp & TearDown methods
                MethodInfo setUp = TypeHelper.GetAttributedMethod(t, typeof(SetUpAttribute));
                MethodInfo tearDown = TypeHelper.GetAttributedMethod(t, typeof(TearDownAttribute));

                // explore type for methods with TestSuite
                foreach (MethodInfo mi in TypeHelper.GetAttributedMethods(t, typeof(TestSuiteAttribute)))
                {
                    // check signature
                    TypeHelper.CheckSignature(mi, typeof(ITestSuite));

                    // get test suite
                    ITestSuite suite = mi.Invoke(fixture, null) as ITestSuite;
                    Assert.IsNotNull(suite, "TestSuite method cannot return null");

                    // populated tree down...
                    foreach (ITestCase tc in suite.TestCases)
                    {
                        tree.AddChild(parent, new TestCaseRunInvoker(this, suite, tc, setUp, tearDown));
                    }
                }
            }
            catch (Exception ex)
            {
                TestSuiteGenerationFailedRunInvoker invoker = new TestSuiteGenerationFailedRunInvoker(this, ex);
                tree.AddChild(parent, invoker);
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
