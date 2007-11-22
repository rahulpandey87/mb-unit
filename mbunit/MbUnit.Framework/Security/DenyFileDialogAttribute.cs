using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyFileDialogAttribute : DecoratorPatternAttribute
    {
        private FileDialogPermissionAccess access = FileDialogPermissionAccess.OpenSave;

        public FileDialogPermissionAccess Access
        {
            get { return this.access; }
            set { this.access = value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyFileDialogAccessRunInvoker(invoker, this);
        }

        private sealed class DenyFileDialogAccessRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyFileDialogAttribute attribute;
            public DenyFileDialogAccessRunInvoker(
                IRunInvoker invoker,
                DenyFileDialogAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                FileDialogPermission permission = new FileDialogPermission(attribute.Access);
                return permission;
            }
        }
    }
}
