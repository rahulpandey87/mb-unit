#if DEBUG

using System;
using System.Diagnostics;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Core.Exceptions;
using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Core.Monitoring
{
    [TestFixture]
    public class DebugMonitorTest
    {
        private string debugHash;
        private string HashListeners()
        {
            StringWriter sw = new StringWriter();
            foreach (System.Diagnostics.TraceListener listener in
                System.Diagnostics.Debug.Listeners)
                sw.Write(listener.GetType().FullName);
            return sw.ToString();
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            foreach (TraceListener listener in Debug.Listeners)
                Console.WriteLine(listener.GetType().FullName);
            debugHash = HashListeners();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            foreach (TraceListener listener in Debug.Listeners)
                Console.WriteLine(listener.GetType().FullName);
            Assert.AreEqual(debugHash, HashListeners(),
                "Debug.Listeners was not properly cleaned");
        }

        [Test]
        public void DebugWrite()
        {
            Debug.Write("testing");
        }
        [Test]
        public void DebugWriteLine()
        {
            Debug.WriteLine("testing");
        }
        [Test]
        public void DebugWriteCategory()
        {
            Debug.Write("testing", "mycategory");
        }
        [Test]
        public void DebugWriteLineCategory()
        {
            Debug.WriteLine("testing", "mycategory");
        }

        [Test]
        [ExpectedException(typeof(DebugFailException))]
        public void KickAssertAndCatchIt()
        {
            string message = "This message should be caught and dumped to the console";
            Debug.Assert(false, message);
        }
    }
}
#endif