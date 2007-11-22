using System;
using MbUnit.Core.Runs;
using MbUnit.Framework;
using MbUnit.Core.Reflection;

namespace MbUnit.Framework
{
    internal sealed class TestRun : MethodRun
    {
        public TestRun()
            :base(typeof(TestAttribute),true,true)
        {
            this.Checker = new SignatureChecker(typeof(void));
        }
    }
}
