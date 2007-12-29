using System;

using MbUnit.Framework;

namespace MbUnit.Demo
{
    [TestFixture]
    [CurrentFixture]
    public class WarningTest
    {
        [Test]
        public void Warning()
        {
            Assert.Warning("Be wary");
        }

        [Test]
        public void Warning2()
        {
            Assert.Warning("Something weird happened");
        }
    }
}
