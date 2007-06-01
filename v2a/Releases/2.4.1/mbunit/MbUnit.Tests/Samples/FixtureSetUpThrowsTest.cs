using System;
using MbUnit.Framework;

namespace MbUnit.Tests.Samples
{
    [TestFixture]
    public class FixtureSetUpFailureTest
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            throw new Exception();
        }

        [Test]
        public void Test1()
        {


        }

        [Test]
        public void Test()
        {


        }
    }
}