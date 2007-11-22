using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using System.Net;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyDnsAttribute : DecoratorPatternAttribute
    {
        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyDnsRunInvoker(invoker, this);
        }

        private sealed class DenyDnsRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyDnsAttribute attribute;
            public DenyDnsRunInvoker(
                IRunInvoker invoker,
                DenyDnsAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                DnsPermission permission = new DnsPermission(PermissionState.Unrestricted);
                return permission;
            }
        }
    }
}
