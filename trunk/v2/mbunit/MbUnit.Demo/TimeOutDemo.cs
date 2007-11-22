using System;
using System.Threading;


using MbUnit.Framework;

namespace MbUnit.Demo
{
    [TestFixture(TimeOut = 0)]
    public class TimeOutDemo
    {
        [Test]
        public void LongTest()
        {
            Thread.Sleep(2000);
        }
    }
}
