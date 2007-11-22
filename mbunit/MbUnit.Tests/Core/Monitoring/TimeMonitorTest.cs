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
    public class TimeMonitorTest
    {
        [Test]
        public void Constructor()
        {
            TimeMonitor timer = new TimeMonitor();
        }

        [Test]
        public void StartStopShowValues()
        {
            TimeMonitor timer = new TimeMonitor();
            timer.Start();
            Random rnd = new Random();
            for (int i = 0; i < 10000; ++i)
                rnd.NextDouble();
            timer.Stop();
            TypeHelper.ShowPropertyValues(timer);
        }

        [Test]
        public void ShowValuesBeforeStartStop()
        {
            TimeMonitor timer = new TimeMonitor();
            TypeHelper.ShowPropertyValues(timer);
        }

        [Test]
        public void StartStopImmediately()
        {
            TimeMonitor timer = new TimeMonitor();
            timer.Start();
            timer.Stop();
            Assert.LowerEqualThan(0, timer.Duration);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StopBeforeStart()
        {
            TimeMonitor timer = new TimeMonitor();
            timer.Stop();
        }
    }
}
