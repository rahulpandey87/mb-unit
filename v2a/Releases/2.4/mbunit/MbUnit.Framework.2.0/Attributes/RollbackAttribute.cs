using System;
using System.Collections;
using System.Text;
using System.Transactions;

using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited = true)]
    public sealed class RollBack2Attribute : DecoratorPatternAttribute
    {
        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new RollBackRunInvoker(invoker);       
        }

        private class RollBackRunInvoker : DecoratorRunInvoker
        {
            public RollBackRunInvoker(IRunInvoker invoker) : base(invoker) { }

            public override object Execute(object o, IList args)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    return base.Invoker.Execute(o, args);
                }
            }
        }
    }

}
