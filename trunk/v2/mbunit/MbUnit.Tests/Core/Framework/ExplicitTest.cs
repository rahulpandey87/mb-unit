using MbUnit.Framework;

namespace MbUnitTest
{
    [TestFixture, FixtureCategory("a")]
    public class A
    {
        [Test, Explicit]
        public void Test1()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void Test2()
        {
            Assert.IsTrue(true);
        }
    }

    [TestFixture, Explicit]
    public class B
    {
        [Test]
        public void Test1()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void Test2()
        {
            Assert.IsTrue(true);
        }
    }
}
