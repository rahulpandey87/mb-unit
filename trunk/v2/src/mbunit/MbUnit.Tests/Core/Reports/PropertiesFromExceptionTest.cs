using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Tests.Core.Reports
{
    [TestFixture]
    public class PropertiesFromExceptionTest
    {
        [Test]
        public void SerializeCustomProperty()
        {
            DummyException ex = new DummyException();
            ReportException rex = ReportException.FromException(ex);

            foreach(ReportProperty p in rex.Properties)
            {
                if (p.Name == "Dummy")
                {
                    Assert.AreEqual(p.Value, ex.Dummy);
                    return;
                }
            }
            Assert.Fail("Dummy property not found");
        }

        [Test]
        [Ignore("Data not supported in 1.0")]
        public void SerializeData()
        {
            string key = "DummyData";
            string value = "value";
            Exception ex = new Exception();
            //ex.Data.Add(key,value);
            ReportException rex = ReportException.FromException(ex);

            foreach (ReportProperty p in rex.Properties)
            {
                if (p.Name == key)
                {
                    Assert.AreEqual(p.Value, value);
                    return;
                }
            }
            Assert.Fail("Dummy property not found");
        }

        public class DummyException : ApplicationException
        {
            private string dummy = "hello";

            public String Dummy
            {
                get
                {
                    return dummy;
                }
            }
        }
    }
}
