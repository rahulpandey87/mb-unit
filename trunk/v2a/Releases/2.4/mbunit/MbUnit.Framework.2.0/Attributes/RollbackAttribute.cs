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

        private TimeSpan TimeOut = new TimeSpan(1, 0, 0);

        public void RollBackAttribute2()
        {

        }

        public void RollBackAttribute2(TimeSpan timeOutValue)
        {
            TimeOut = timeOutValue;
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new RollBackRunInvoker(invoker, TimeOut);       
        }

        private class RollBackRunInvoker : DecoratorRunInvoker
        {
            private TimeSpan TimeOut;

            public RollBackRunInvoker(IRunInvoker invoker, TimeSpan timeOut) : base(invoker) 
            {
                TimeOut = timeOut;
            }

            public override object Execute(object o, IList args)
            {             
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeOut))
                {
                    return base.Invoker.Execute(o, args);
                }               
              
            }
        }
    }

}
