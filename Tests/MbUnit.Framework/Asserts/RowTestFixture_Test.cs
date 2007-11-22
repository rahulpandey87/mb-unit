using System;
using System.Text;

using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Attributes
{  
    [TestFixture]
    public class RowTestFixture_Test
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
        [Row(null)]
        public void SingleNullParameter(string input)
        {
            Assert.IsNull(input);
        }

        [RowTest]
        [Row("key1", 1, "key2", 2, "key3", 3, "key4", 4)]
        public void ParamsTest1(params object[] args)
        {
            Assert.AreEqual(8, args.Length);
            Assert.AreEqual("key1", args[0]);
        }

        [RowTest]
        [Row("key1", 1)]
        public void ParamsTest2(string key, int value, params object[] args)
        {
            Assert.AreEqual("key1", key);
            Assert.AreEqual(1, value);
            Assert.AreEqual(0, args.Length);
        }

        [RowTest]
        [Row("key1", 1, "key2", 2, "key3", 3, "key4", 4)]
        public void ParamsTest3(string key, int value, params object[] args)
        {
            Assert.AreEqual("key1", key);
            Assert.AreEqual(1, value);
            Assert.AreEqual(6, args.Length);
            Assert.AreEqual("key2", args[0]);
        }
    }
}
