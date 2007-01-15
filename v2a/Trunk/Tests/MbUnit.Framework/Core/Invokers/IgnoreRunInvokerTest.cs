using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Exceptions;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework.Tests.Core.Invokers
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="IgnoreRunInvoker"/> class.
    /// </summary>
    [TestFixture]
    public class IgnoreRunInvokerTest
    {
        [Test]
        [ExpectedException(typeof(IgnoreRunException))]
        public void CallIgnore()
        {
            IgnoreRunInvoker invoker = new IgnoreRunInvoker(new MockRunInvoker());
            invoker.Execute(null, new ArrayList());
        }
    }
}

