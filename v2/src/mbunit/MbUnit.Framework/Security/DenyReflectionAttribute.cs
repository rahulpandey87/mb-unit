using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyReflectionAttribute : DecoratorPatternAttribute
    {
        private ReflectionPermissionFlag flags = ReflectionPermissionFlag.AllFlags;

        public ReflectionPermissionFlag Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyReflectionRunInvoker(invoker, this);
        }

        private sealed class DenyReflectionRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyReflectionAttribute attribute;
            public DenyReflectionRunInvoker(
                IRunInvoker invoker,
                DenyReflectionAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                ReflectionPermission permission = new ReflectionPermission(PermissionState.Unrestricted);
                permission.Flags = attribute.Flags;
                return permission;
            }
        }
    }
}
