using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using System.Data.Common;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class DenyDBDataAttribute : DecoratorPatternAttribute
    {
        private bool allowBlankPassword = false;

        public bool AllowBlankPassword
        {
            get { return this.allowBlankPassword; }
            set { this.allowBlankPassword = value; }
        }

        protected abstract DBDataPermission CreateDataPermission();

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DBDataRunInvoker(invoker, this);
        }

        private sealed class DBDataRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyDBDataAttribute attribute;
            public DBDataRunInvoker(
                IRunInvoker invoker,
                DenyDBDataAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                DBDataPermission permission = attribute.CreateDataPermission();
                permission.AllowBlankPassword = attribute.AllowBlankPassword;
                return permission;
            }
        }

    }
}
