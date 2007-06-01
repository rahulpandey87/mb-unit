using System;
using MbUnit.Framework;
using MbUnit.Core.Remoting;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Tests.Core.Framework
{
    [TestFixture]
    public class IgnoreTest
    {
        [TestFixture]
        internal class IgnoreTraverser
        {
            [Test]
            [ExpectedException(typeof(NullReferenceException))]
            public void IgnoreCanTraverseExpectedException()
            {
                Assert.Ignore("Just trying");
            }
        }

        [Test]
        public void IgnoreTraversesExpectedException()
        {
            using (TypeTestDomain domain = new TypeTestDomain(typeof(IgnoreTraverser)))
            {
                domain.Load();
                domain.TestEngine.RunPipes();

                ReportResult result = domain.TestEngine.Report.Result;
                Console.WriteLine(result.Counter);
                MbUnit.Core.Reports.XmlReport.RenderToXml(result, Console.Out);
                Assert.AreEqual(1, result.Counter.RunCount);
                Assert.AreEqual(1, result.Counter.IgnoreCount);
            }
        }
    }
}
