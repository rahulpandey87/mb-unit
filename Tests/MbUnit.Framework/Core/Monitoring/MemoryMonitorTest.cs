using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core;
using MbUnit.Core.Monitoring;
using System.Diagnostics;
using System.Reflection;

namespace MbUnit.Framework.Tests.Core.Monitoring
{
//    [TestFixture]
    public class MemoryMonitorTest
    {
        [Test]
        public void Constructor()
        {
            MemoryMonitor monitor = new MemoryMonitor();
        }

        [Test]
        public void Start()
        {
            MemoryMonitor monitor = new MemoryMonitor();
            monitor.Start();
            Assert.IsNotNull(monitor.StartStatus);
            Assert.IsNull(monitor.EndStatus);
        }

        [Test]
        [Ignore("There is a problem for non-admin to monitor memory")]
        public void StartStop()
        {
            MemoryMonitor monitor = new MemoryMonitor();
            monitor.Start();
            monitor.Stop();
            Assert.IsNotNull(monitor.StartStatus);
            Assert.IsNotNull(monitor.EndStatus);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UsageBeforeStartStop()
        {
            MemoryMonitor monitor = new MemoryMonitor();
            long usage = monitor.Usage;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StopBeforeStart()
        {
            MemoryMonitor monitor = new MemoryMonitor();
            monitor.Stop();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NowBeforeStartStop()
        {
            MemoryMonitor monitor = new MemoryMonitor();
            long usage = monitor.Now;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NowAfterStartStop()
        {
            MemoryMonitor monitor = new MemoryMonitor();
            monitor.Start();
            monitor.Stop();
            long usage = monitor.Now;
        }
    }
}
