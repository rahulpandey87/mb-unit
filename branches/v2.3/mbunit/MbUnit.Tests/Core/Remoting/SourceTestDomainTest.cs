using System;
using MbUnit.Core.Framework;
using MbUnit.Core.Remoting;
using MbUnit.Framework;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Tests.Core.Remoting
{
    [TestFixture]
    public class SourceTestDomainTest
    {
        [Test]
        public void CompileFixture()
        {
            string source = MbUnit.Tests.Samples.SampleHelper.TestFixture();
            using (SourceTestDomain domain = new SourceTestDomain())
            {
                domain.Sources.Add(source);
                domain.SandBox = false;
                domain.Load();
                domain.TestEngine.RunPipes();
            }
        }

        [Test]
        public void CompileAndCountTests()
        {
            string source = MbUnit.Tests.Samples.SampleHelper.TestFixture();
            using (SourceTestDomain domain = new SourceTestDomain())
            {
                domain.Sources.Add(source);
                domain.SandBox = false;
                domain.Load();
                domain.TestEngine.RunPipes();

                ReportResult result = domain.TestEngine.Report.Result;
                Assert.AreEqual(4,result.Counter.RunCount);
                Assert.AreEqual(2, result.Counter.SuccessCount);
                Assert.AreEqual(1, result.Counter.IgnoreCount);
                Assert.AreEqual(1, result.Counter.FailureCount);
            }
        }
    }
}
