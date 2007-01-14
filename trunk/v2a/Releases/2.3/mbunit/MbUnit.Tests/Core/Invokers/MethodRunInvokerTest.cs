using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using System.Reflection;
using MbUnit.Core.Invokers;
using MbUnit.Core;

namespace MbUnit.Tests.Core.Invokers
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="MethodRunInvoker"/> class.
    /// </summary>
    [TestFixture]
    public class MethodRunInvokerTest
    {
        [Test]
        [ExpectedArgumentNullException]
        public void MethodNull()
        {
            MethodRunInvoker invoker = new MethodRunInvoker(new MockRun(), null);
        }

        [Test]
        public void NoArgumentMethod()
        {
            MethodInfo mi = typeof(HelloWorld).GetMethod("NoArgument");
            MethodRunInvoker invoker = new MethodRunInvoker(new MockRun(), mi);
            HelloWorld hw = new HelloWorld();
            invoker.Execute(hw,new ArrayList());

            Assert.IsTrue(hw.Executed);
        }

        [Test]
        public void OneArgumentMethod()
        {
            MethodInfo mi = typeof(HelloWorld).GetMethod("OneArgument");
            MethodRunInvoker invoker = new MethodRunInvoker(new MockRun(), mi);
            HelloWorld hw = new HelloWorld();

            ArrayList args = new ArrayList();
            args.Add("Hello");
            invoker.Execute(hw,args);

            Assert.IsTrue(hw.Executed);
            Assert.AreEqual(args[0], hw.Arg);
        }

        internal class HelloWorld
        {
            private bool executed = false;
            private string arg=null;

            public bool Executed
            {
                get
                {
                    return executed;
                }
            }
            public string Arg
            {
                get
                {
                    return this.arg;
                }
            }

            public void NoArgument()
            {
                Console.WriteLine("NoArgument executed");
                this.executed = true;
            }

            public void OneArgument(string arg)
            {
                Console.WriteLine("OneArgument executed with {0}",arg);
                this.executed = true;
                this.arg = arg;
            }
        }
    }
}

