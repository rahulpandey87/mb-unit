using System;
using MbUnit.Framework;

namespace MbUnit.Tests.Samples
{
    [TestFixture]
    public class SickFixture
    {
        public SickFixture()
        {
            throw new Exception("boom");
        }

        [Test]
        public void Test1()
        { }

        [Test]
        public void Test2()
        { }
    }
}
