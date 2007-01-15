using System;
using System.IO;
using NUnit.Framework;

namespace MbUnit.Framework.Tests.Core.FrameworkBridges
{
    [TestFixture]
    public class NUnitFixture
    {
        private StringWriter writer;
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.Out.Write("TestFixtureSetUp");
        }
        [SetUp]
        public void SetUp()
        {
            Console.Out.Write("SetUp");
        }
        [Test]
        public void Success()
        {
            Console.Out.Write("Success");
        }
        //[Test]
        //public void Failure()
        //{
        //    Console.Out.Write("Failure");
        //    Assert.Fail();
        //}
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedException()
        {
            Console.Out.Write("ExpectedException");
            throw new ArgumentNullException("boom");
        }
        [Test]
        [Ignore("Because I want")]
        public void Ignore()
        {
            Console.Out.Write("Ignore");
            throw new Exception("Ignored test");
        }
        [TearDown]
        public void TearDown()
        {
            Console.Out.Write("TearDown");
        }
        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Console.Out.Write("FixtureTearDown");
        }
    }
}
