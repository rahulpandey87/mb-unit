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
    public class ArgumentFeederRunInvokerTest
    {
        [Test]
        [ExpectedArgumentNullException]
        public void MethodNull()
        {
            ArgumentFeederRunInvoker invoker = 
                new ArgumentFeederRunInvoker(new MockRun(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MethodReturnsVoid()
        {
            MethodInfo mi = typeof(HelloWorld).GetMethod("ReturnsVoid");
            ArgumentFeederRunInvoker invoker =
                new ArgumentFeederRunInvoker(new MockRun(), mi);
        }

        [Test]
        public void CallWithArgument()
        {
            MethodInfo mi = typeof(HelloWorld).GetMethod("Identity");
            ArgumentFeederRunInvoker invoker = new ArgumentFeederRunInvoker(new MockRun(), mi);
            HelloWorld hw = new HelloWorld();
            ArrayList args = new ArrayList();
            args.Add("test");
            Object result = invoker.Execute(hw,args);

            Assert.IsTrue(hw.Executed);
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual(args[0],args[1]);
        }

        internal class HelloWorld
        {
            private bool executed = false;
            private string arg = null;

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

            public void ReturnsVoid()
            {
                Console.WriteLine("NoArgument executed");
                this.executed = true;
            }

            public string Identity(string arg)
            {
                Console.WriteLine("OneArgument executed with {0}", arg);
                this.executed = true;
                this.arg = arg;
                return arg;
            }
        }
    }
}

