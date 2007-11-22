using System;
using System.Text;
using System.Data;

namespace MbUnit.Framework.Tests.Asserts
{
    [TestFixture]
    public class DataAssert_Test
    {
        [Test]
        public void AreEqualDataColumn()
        {
            DataColumn d1 = new DataColumn("Col");
            DataColumn d2 = d1;

            
            DataAssert.AreEqual(d1, d2);
        }
    }
}
