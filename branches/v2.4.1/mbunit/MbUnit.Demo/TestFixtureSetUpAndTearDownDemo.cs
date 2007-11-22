using System;
using System.Diagnostics;

using MbUnit.Framework;

namespace MbUnit.Demo
{
    [TestFixture]
    public class TestFixtureSetUpAndTearDownDemo
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("TestFixtureSetUp");
        }
        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("TestFixtureSetUp");
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
        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");
        }
        [TestFixtureTearDown]
        public void TestFixtureDown()
        {
            Console.WriteLine("TestFixtureDown");
        }
    }
}
