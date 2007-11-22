using System;

using MbUnit.Framework;

namespace MbUnit.Demo
{
    [TestFixture]
    public class PerfCounterDemo
    {
        private double[] bigArray;
        private int length = 10 * 1000 * 1000;

        [Test]
        [PerfCounter(".NET CLR Memory", "% Time in GC", 10)]
        public void AllocateALotOfObjects()
        {
            for (int i = 0; i < length; ++i)
            {
                Dummy dummy = new Dummy();
            }
        }

        [Test]
        [PerfCounter("Process", "% Processor Time", 10)]
        public void MonitorProcess()
        {
            this.CreateArray();
        }

        [Test]
        [PerfCounter(".NET CLR Loading", "% Time Loading", 10)]
        [PerfCounter(".NET CLR Security", "% Time in RT checks", 10000)]
        [PerfCounter(".NET CLR Security", "% Time Sig. Authenticating", 10)]
        [PerfCounter(".NET CLR Memory", "# Bytes in all Heaps", 5000000, Relative =true)]
        [PerfCounter(".NET CLR Jit", "% Time in Jit", 10)]
        public void MonitorMultipleCounters()
        {
            this.CreateArray();
        }

        protected void CreateArray()
        {
            bigArray = new double[length];
            for (int i = 0; i < bigArray.Length; ++i)
                bigArray[i] = 1;
        }
        public class Dummy
        {}
    }
}
