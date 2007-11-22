using System;
using System.Threading;

using MbUnit.Framework;

namespace MbUnit.Demo
{
    [TestFixture(ApartmentState = ApartmentState.MTA)]
    public class MTATest
    {
        [Test]
        public void HelloWorld()
        {
            Console.WriteLine("Current thread ApartementState: {0}",
                Thread.CurrentThread.ApartmentState);
        }
    }

    [TestFixture(ApartmentState = ApartmentState.STA)]
    public class STATest
    {
        [Test]
        public void HelloWorld()
        {
            Console.WriteLine("Current thread ApartementState: {0}",
                Thread.CurrentThread.ApartmentState);
        }
    }
}
