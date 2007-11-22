using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using System.Drawing.Printing;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyPrintingAttribute : DecoratorPatternAttribute
    {
        private PrintingPermissionLevel level = PrintingPermissionLevel.NoPrinting;

        public PrintingPermissionLevel Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyPrintingRunInvoker(invoker, this);
        }

        private sealed class DenyPrintingRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyPrintingAttribute attribute;
            public DenyPrintingRunInvoker(
                IRunInvoker invoker,
                DenyPrintingAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                PrintingPermission permission = new PrintingPermission(PermissionState.Unrestricted);
                permission.Level = attribute.Level;
                return permission;
            }
        }
    }
}
