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

        public int TimeOutValue = 0;

        public void RollBackAttribute2()
        {

        }

        public void RollBackAttribute2(int timeOutValue)
        {
            TimeOutValue = timeOutValue;
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new RollBackRunInvoker(invoker, TimeOutValue);       
        }

        private class RollBackRunInvoker : DecoratorRunInvoker
        {
            private int TimeOut = 0;

            public RollBackRunInvoker(IRunInvoker invoker, int timeOut) : base(invoker) 
            {
                TimeOut = timeOut;
            }

            public override object Execute(object o, IList args)
            {
                if (TimeOut != 0)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, timeOut))
                    {
                        return base.Invoker.Execute(o, args);
                    }
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        return base.Invoker.Execute(o, args);
                    }
                }
            }
        }
    }

}
