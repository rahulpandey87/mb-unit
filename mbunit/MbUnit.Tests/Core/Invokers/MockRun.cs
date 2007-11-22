using System;
using MbUnit.Core.Runs;

namespace MbUnit.Tests.Core.Invokers
{
    internal class MockRun : Run
    {
        public MockRun()
            :base("MockRun",false)
        {}

        public override void Reflect(MbUnit.Core.Invokers.RunInvokerTree tree, MbUnit.Core.Invokers.RunInvokerVertex parent, Type t)
        {
            throw new NotImplementedException();
        }
    }
}
