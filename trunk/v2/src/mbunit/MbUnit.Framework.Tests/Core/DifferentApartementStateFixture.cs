using System;
using System.Threading;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Demo.ApartmentFixtures
{
    [TestFixture(ApartmentState = ApartmentState.MTA)]
    public class MTAFixtureTest
    {
        [Test]
        public void HelloWorld()
        {
            Console.WriteLine("Current thread ApartementState: {0}",
                Thread.CurrentThread.ApartmentState);
            Assert.AreEqual(
                ApartmentState.MTA, Thread.CurrentThread.ApartmentState);
        }
    }

    [TestFixture(ApartmentState = ApartmentState.STA)]
    public class STAFixtureTest
    {
        [Test]
        public void HelloWorld()
        {
            Console.WriteLine("Current thread ApartementState: {0}",
                Thread.CurrentThread.ApartmentState);
            Assert.AreEqual(
                ApartmentState.STA, Thread.CurrentThread.ApartmentState);
        }
    }
}
