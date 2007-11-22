using System;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;
using MbUnit.Core.Runs;

namespace MbUnit.Framework.Tests.Core.Invokers
{
    public class MockRunInvoker : RunInvoker
    {
        public MockRunInvoker()
            :base(new MockRun())
        { }

        public override String Name
        {
            get { return "MockRunInvoker"; }
        }

        public override Object Execute(Object o, System.Collections.IList args)
        {
            throw new NotImplementedException();
        }
    }
}
