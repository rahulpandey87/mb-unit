using System;
using MbUnit.Framework;

namespace MbUnit.Tests.Core.Graph
{
    [TestFixture]
    public class SickParentAssembly
    {
        [Test]
        public void Fail()
        {
            Assert.Fail();
        }
    }
}
