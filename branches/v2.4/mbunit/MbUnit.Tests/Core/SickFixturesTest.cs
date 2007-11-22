using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Filters;
using MbUnit.Core.Reports.Serialization;

using MbUnit.Tests.Samples;

namespace MbUnit.Tests.Core
{
    [TestFixture]    
    public class SickFixturesTest
    {
        [Test]
        public void ConstructorThrows()
        {
            SampleCompiler compiler = new SampleCompiler("FixtureConstructorThrowsTest.cs");
            compiler.AddMbUnitReferences();

            compiler.LoadAndRunFixtures();
            Assert.AreEqual(2, compiler.Result.Counter.FailureCount);
            Assert.AreEqual(2, compiler.Result.Counter.RunCount);
        }

        [Test]
        public void TestFixtureSetUpThrows()
        {
            SampleCompiler compiler = new SampleCompiler("FixtureSetUpThrowsTest.cs");
            compiler.AddMbUnitReferences();

            compiler.LoadAndRunFixtures();
            Assert.AreEqual(2, compiler.Result.Counter.FailureCount);
            Assert.AreEqual(2, compiler.Result.Counter.RunCount);
        }
    }
}
