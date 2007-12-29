using System;
using MbUnit.Framework;
using MbUnit.Framework.Exceptions;

namespace MbUnit.Framework.Tests.Samples
{
    [TestFixture]
    [ExpectedException(typeof(ExpectedExceptionAttribute))]
    public class SickFixture
    {
        public SickFixture()
        {
            throw new Exception("boom");
        }

        [Test]
        public void Test1()
        { }

        [Test]
        public void Test2()
        { }
    }
}