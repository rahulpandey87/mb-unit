using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyUIAttribute : DecoratorPatternAttribute
    {
        private UIPermissionClipboard clipboard = UIPermissionClipboard.AllClipboard;
        private UIPermissionWindow window = UIPermissionWindow.AllWindows;

        public UIPermissionClipboard Clipboard
        {
            get { return this.clipboard; }
            set { this.clipboard = value; }
        }

        public UIPermissionWindow Window
        {
            get { return this.window; }
            set { this.window = value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyWindowRunInvoker(invoker, this);
        }

        private sealed class DenyWindowRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyUIAttribute attribute;
            public DenyWindowRunInvoker(
                IRunInvoker invoker,
                DenyUIAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                UIPermission permission = new UIPermission(PermissionState.Unrestricted);
                permission.Clipboard = attribute.Clipboard;
                permission.Window = attribute.Window;
                return permission;
            }
        }
    }
}
