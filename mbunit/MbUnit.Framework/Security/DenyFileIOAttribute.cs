using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,AllowMultiple  = false, Inherited =true)]
    public sealed class DenyFileIOAttribute : DecoratorPatternAttribute
    {
        private FileIOPermissionAccess allFiles = FileIOPermissionAccess.NoAccess;
        private FileIOPermissionAccess allLocalFiles = FileIOPermissionAccess.NoAccess;

        public FileIOPermissionAccess AllFiles
        {
            get { return this.allFiles; }
            set { this.allFiles = value; }
        }

        public FileIOPermissionAccess AllLocalFiles
        {
            get { return this.allLocalFiles; }
            set { this.allLocalFiles = value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyFileIORunInvoker(invoker, this);
        }

        private sealed class DenyFileIORunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyFileIOAttribute attribute;
            public DenyFileIORunInvoker(
                IRunInvoker invoker,
                DenyFileIOAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                FileIOPermission permission = new FileIOPermission(PermissionState.Unrestricted);
                permission.AllFiles = attribute.AllFiles;
                permission.AllLocalFiles = attribute.AllLocalFiles;
                return permission;
            }
        }
    }
}
