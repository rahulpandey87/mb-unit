using System;
using MbUnit.Framework;
using MbUnit.Core.Remoting;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Tests.Core.Framework
{
    [TestFixture]
    [Ignore("A fully ignored test fixture.")]
    public class FixtureIgnoreTest
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Assert.Fail("Test fixture set up should not be run in an ignored fixture.");
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Assert.Fail("Test fixture tear down should not be run in an ignored fixture.");
        }

        [SetUp]
        public void SetUp()
        {
            Assert.Fail("Test set up should not be run in an ignored fixture.");
        }

        [TearDown]
        public void TearDown()
        {
            Assert.Fail("Test tear down should not be run in an ignored fixture.");
        }

        [Test]
        public void IgnoreMe()
        {
            Assert.Fail("Test should not be run in an ignored fixture.");
        }
    }
}
