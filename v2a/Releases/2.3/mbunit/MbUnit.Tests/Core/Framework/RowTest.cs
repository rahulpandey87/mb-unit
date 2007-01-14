using System;
using System.Text;

using MbUnit.Framework;

namespace MbUnit.Tests.Core.Framework
{  
    [TestFixture]
    public class RowTest
    {
        [RowTest]
        [Row(1000, 10, 100.0000)]
        [Row(-1000, 10, -100.0000)]
        [Row(1000, 7, 142.85715)]
        [Row(1000, 0.00001, 100000000)]
        [Row(4195835, 3145729, 1.3338196)]
        public void DivTest(double numerator, double denominator, double result)
        {
            Assert.AreEqual(result, numerator / denominator, 0.00001);
        }

        [RowTest]
        [Row()]
        public void RowWithNoValues()
        {
            // Nothing.  It's enough to know that the method ran.
        }

        [RowTest]
        [Row("value")]
        public void RowWithOneValue(object value)
        {
            Assert.AreEqual("value", value);
        }

        [RowTest]
        [Row(null)]
        public void RowWithOneNullValue(object value)
        {
            Assert.IsNull(value);
        }
    }
}
