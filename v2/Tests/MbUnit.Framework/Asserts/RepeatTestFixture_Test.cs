using System;
using System.Text;

using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Attributes
{  
    [TestFixture]
    public class RepeatTestFixture_Test
    {
        private bool flag = false;

        [SetUp]
        public void SetUp()
        {
            Assert.IsFalse(flag);
            flag = true;
        }

        [RepeatTest(5)]
        public void RepTest()
        {
            Assert.IsTrue(flag);
        }

        [TearDown]
        public void TearDown()
        {
            flag = false;
        }
    }
}
