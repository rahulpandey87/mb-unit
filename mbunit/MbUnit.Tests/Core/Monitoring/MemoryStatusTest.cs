using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core;
using MbUnit.Core.Monitoring;
using System.Diagnostics;
using System.Reflection;

namespace MbUnit.Tests.Core.Monitoring
{
    [TestFixture]
    public class MemoryStatusTest
    {
        [Test]
        public void CallProperties()
        {
            MemoryStatus status = new MemoryStatus();
            TypeHelper.ShowPropertyValues(status);
        }
    }
}
