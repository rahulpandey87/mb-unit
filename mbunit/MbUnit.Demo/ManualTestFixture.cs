using System;

using MbUnit.Framework;

namespace MbUnit.Demo
{
    public class ManualTestFixture
    {
        [TestFixture(TimeOut = 1)]
        public class NestedFixture
        {
            [Test]
            public void Manual()
            {
                ManualTester.DisplayForm("do this", "do that", "etc...");
            }
        }
    }
}
