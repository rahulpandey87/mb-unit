using System;
using System.Text;

using MbUnit.Framework;

namespace MbUnit.Framework.Tests20.Attributes
{  
    [TestFixture]
    public class RowTestFixture_Test
    {
        [RowTest, Row(1)]
        public void NullableInt_Test(int? i)
        {
            Assert.AreEqual(1, i);
        }

        [RowTest, Row(null)]
        public void NullableInt_Test2(int? i)
        {
            Assert.AreEqual(null, i);
        }
    }
}
