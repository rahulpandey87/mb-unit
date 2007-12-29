using System;

using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Core
{
    [ProcessTestFixture]
    public class ProcessTestFixtureTest
    {
        int i = 0;

        [SetUp]
        public void SetUp()
        {
            i++;
        }

        [Test, TestSequence(2)]
        public void Test2()
        {
            Assert.AreEqual(7, i++);
        }

        [Test, TestSequence(1)]
        public void Test1()
        {
            Assert.AreEqual(4, i++);
        }

        [Test, TestSequence(0)]
        public void Test0()
        {
            Assert.AreEqual(1, i++);
        }

        [TearDown]
        public void TearDown()
        {
            i++;
        }
    }
}
