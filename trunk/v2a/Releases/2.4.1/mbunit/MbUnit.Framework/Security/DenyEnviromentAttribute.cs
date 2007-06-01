using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class DenyEnvironmentAttribute : DecoratorPatternAttribute
    {
        private EnvironmentPermissionAccess flag = EnvironmentPermissionAccess.NoAccess;
        private string pathList;

        public DenyEnvironmentAttribute(string pathList)
        {
            if (pathList == null)
                throw new ArgumentNullException("pathList");
            this.pathList = pathList;
        }

        public EnvironmentPermissionAccess Flag
        {
            get { return this.flag; }
            set { this.flag = value; }
        }

        public string PathList
        {
            get { return this.pathList; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new DenyEnvironmentRunInvoker(invoker, this);
        }

        private sealed class DenyEnvironmentRunInvoker : DenyPermissionDecoratorRunInvoker
        {
            private DenyEnvironmentAttribute attribute;
            public DenyEnvironmentRunInvoker(
                IRunInvoker invoker,
                DenyEnvironmentAttribute attribute)
                :base(invoker)
            {
                this.attribute = attribute;
            }

            protected override IStackWalk CreateStackWalk()
            {
                EnvironmentPermission permission = new EnvironmentPermission(PermissionState.Unrestricted);
                permission.AddPathList(attribute.Flag, attribute.PathList);
                return permission;
            }
        }
    }
}
