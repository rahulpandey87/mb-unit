using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Core
{
    [TestFixture]
    [ExpectedArgumentNullException]
    public class DecoratedFixtureTest
    {
        [Test]
        public void DefaultTest()
        {
            throw new ArgumentNullException();
        }

        [RowTest]
        [Row(0, 1)]
        [Row(0, 2)]
        public void RowTest(int a, int b)
        {
            throw new ArgumentNullException();
        }

        [CombinatorialTest]
        public void CombinatorialTest(
            [UsingLiterals("0;1")] int arg0,
            [UsingLiterals("0;1")] int arg1)
        {
            throw new ArgumentNullException();
        }
    }
}
