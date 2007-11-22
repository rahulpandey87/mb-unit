using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using System.Web;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyAspNetHostingAttribute : DecoratorPatternAttribute
    {
        private AspNetHostingPermissionLevel level = AspNetHostingPermissionLevel.None;

        public AspNetHostingPermissionLevel Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyAspNetHostingRunInvoker(invoker, this);
        }

        private sealed class DenyAspNetHostingRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyAspNetHostingAttribute attribute;
            public DenyAspNetHostingRunInvoker(
                IRunInvoker invoker,
                DenyAspNetHostingAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
                this.PermitOnly = true;
            }

            protected override IStackWalk CreateStackWalk()
            {
                AspNetHostingPermission permission = new AspNetHostingPermission(PermissionState.Unrestricted);
                permission.Level = attribute.Level;
                return permission;
            }
        }
    }
}
