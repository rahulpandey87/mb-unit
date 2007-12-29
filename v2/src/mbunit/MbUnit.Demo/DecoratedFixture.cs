using System;
using MbUnit.Framework;


namespace MbUnit.Demo
{
    [TestFixture]
    [CurrentFixture]
    [ExpectedArgumentNullException]
    public class DecoratedFixture
    {
        [Test]
        public void ThrowArgumentNullException()
        {
            throw new ArgumentNullException();
        }

        [Test]
        public void ThrowNewArgumentException()
        {
            throw new ArgumentException();
        }
    }
}
