using System;

using MbUnit.Framework;

namespace MbUnit.Demo
{
    [TestFixture]
    public class FailureInTestFixtureSetUp
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            throw new Exception("Boom");
        }
        [Test]
        public void Test1()
        {
            Console.WriteLine("Test1");
        }
        [Test]
        public void Test2()
        {
            Console.WriteLine("Test2");
        }

    }

    [TestFixture]
    public class FailureInTestFixtureTearDown
    {
        [Test]
        public void Test1()
        {
            Console.WriteLine("Test1");
        }
        [Test]
        public void Test2()
        {
            Console.WriteLine("Test2");
        }
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            throw new Exception("Boom");
        }
    }
}