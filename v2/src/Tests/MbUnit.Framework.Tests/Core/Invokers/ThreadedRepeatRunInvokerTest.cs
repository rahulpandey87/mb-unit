#region Includes
using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using System.Reflection;
#endregion

using MbUnit.Core.Invokers;

namespace MbUnit.Framework.Tests.Core.Invokers
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="RepeatRunInvoker"/> class.
    /// </summary>
    [TestFixture]
    public class ThreadedRepeatRunInvokerTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CountNegative()
        {
            new ThreadedRepeatRunInvoker(new MockRunInvoker(), -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CountZero()
        {
            new ThreadedRepeatRunInvoker(new MockRunInvoker(), 0);
        }

        [Test]
        public void CallTwoTimes()
        {
            MethodInfo mi = typeof(MethodRepeatWatcher).GetMethod("CallMe");
            MethodRunInvoker methodRun = new MethodRunInvoker(new MockRun(), mi);
            ThreadedRepeatRunInvoker invoker = new ThreadedRepeatRunInvoker(methodRun, 2);

            MethodRepeatWatcher watcher = new MethodRepeatWatcher();
            invoker.Execute(watcher, new ArrayList());

            Assert.AreEqual(2, watcher.count);
        }

        internal class MethodRepeatWatcher
        {
            public int count = 0;

            public void CallMe()
            {
                this.count++;
            }
        }
    }
}

