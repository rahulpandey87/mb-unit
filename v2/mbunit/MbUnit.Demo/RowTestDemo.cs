using System;

using MbUnit.Framework;

namespace MbUnit.Demo
{
    [TestFixture]
    public class RowTestDemo
    {
        [RowTest]
        [Row(1000,10,100.0000)]
        [Row(-1000,10,-100.0000)]
        [Row(1000,7,142.85714)]
        [Row(1000,0.00001,100000000)]
        [Row(4195835,3145729,1.3338196)]
        public void DivTest(double numerator, double denominator, double result)
        {
            Assert.AreEqual(result, numerator / denominator, 0.00001  );
        }
    }
}
