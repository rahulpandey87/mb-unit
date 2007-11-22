using System;
using System.Collections;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,AllowMultiple =false,Inherited =true)]
    public sealed class RemotingAttribute : DecoratorPatternAttribute
    {
        public RemotingAttribute()
        { }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new RemotingRunInvoker(invoker, this);
        }

        private sealed class RemotingRunInvoker : DecoratorRunInvoker
        {
            private RemotingAttribute attribute;
            public RemotingRunInvoker(IRunInvoker invoker, RemotingAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            public override Object Execute(Object o, IList args)
            {
                AppDomain domain = null;
                Object remote = null;
                try
                {
                    // creating remote domain
                    domain = AppDomain.CreateDomain(o.GetType().FullName);

                    // create fixture type
                    remote = domain.CreateInstanceAndUnwrap(
                        o.GetType().Assembly.GetName().Name,
                        o.GetType().FullName
                        );

                    // run invoker on remoted type
                    return this.Invoker.Execute(remote, args);
                }
                finally
                {
                    IDisposable disposable = remote as IDisposable;
                    if (disposable!=null)
                        disposable.Dispose();
                    remote = null;
                    if (domain != null)
                    {
                        AppDomain.Unload(domain);
                    }
                }
            }
        }
    }
}
