using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Framework;

namespace MbUnit.Framework.Security
{
        public abstract class DenyPermissionDecoratorRunInvoker : DecoratorRunInvoker
        {
            private bool permitOnly = false;

			protected DenyPermissionDecoratorRunInvoker(IRunInvoker invoker)
                :base(invoker)
            {}

            protected bool PermitOnly
            {
                get { return this.permitOnly; }
                set { this.permitOnly = value; }
            }

            protected abstract IStackWalk CreateStackWalk();

            public override object Execute(Object o, IList args)
            {
                IStackWalk permission = null;
                try
                {
                    permission = CreateStackWalk();
                    if (this.PermitOnly)
                        permission.PermitOnly();
                    else
                        permission.Deny();
                    return this.Invoker.Execute(o, args);
                }
                finally
                {
                    if (permission != null)
                    {
                        if (this.PermitOnly)
                            CodeAccessPermission.RevertPermitOnly();
                        else
                            CodeAccessPermission.RevertDeny();
                    }
                }
            }
        }
}
