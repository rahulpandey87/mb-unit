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
    /// <see cref="TestFixture"/> for the <see cref="FailedLoadingRunInvoker"/> class.
    /// </summary>
    [TestFixture]
    public class FailedLoadingRunInvokerTest
    {
        [Test]
        [ExpectedException(typeof(FixtureFailedLoadingException))]
        public void CallFailedLoading()
        {
            FailedLoadingRunInvoker invoker = new FailedLoadingRunInvoker(new MockRun(),new Exception());
            invoker.Execute(null, new ArrayList());
        }
    }
}

