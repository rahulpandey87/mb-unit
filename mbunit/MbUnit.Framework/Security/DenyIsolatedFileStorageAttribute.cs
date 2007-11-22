using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyIsolatedFileStorageAttribute : DecoratorPatternAttribute
    {
        private IsolatedStorageContainment usageAllowed = IsolatedStorageContainment.None;
        private long usageQuota = 0;

        public IsolatedStorageContainment UsageAllowed
        {
            get { return this.usageAllowed; }
            set { this.usageAllowed = value; }
        }

        public long UsageQuota
        {
            get { return this.usageQuota; }
            set { this.usageQuota = value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyIsolatedRunInvoker(invoker, this);
        }

        private sealed class DenyIsolatedRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyIsolatedFileStorageAttribute attribute;
            public DenyIsolatedRunInvoker(
                IRunInvoker invoker,
                DenyIsolatedFileStorageAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
                this.PermitOnly = true;
            }

            protected override IStackWalk CreateStackWalk()
            {
                IsolatedStorageFilePermission permission = new IsolatedStorageFilePermission(PermissionState.Unrestricted);
                permission.UsageAllowed = attribute.UsageAllowed;
                permission.UserQuota = attribute.UsageQuota;
                return permission;
            }
        }
    }
}
