#region Using directives

using System;

#endregion

using NUnit.Framework;

namespace MbUnit.Tests.Core.FrameworkBridges
{
    [TestFixture]
    public class NUnitFrameworkTest
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("TestFixtureSetUp");
        }

        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("SetUp");
        }

        [Test]
        public void Test()
        {
            Console.WriteLine("Test");
        }

        [Test]
        public void AnotherTest()
        {
            Console.WriteLine("Another test");
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void ExpectedException()
        {
            Assert.Fail("Should be intercepted");
        }

        [Test]
        [Ignore("Testing ignore")]
        public void Ignored()
        {
            Assert.Fail("Must be ignored");
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Console.WriteLine("TestFixtureTearDown");
        }
    }
}
