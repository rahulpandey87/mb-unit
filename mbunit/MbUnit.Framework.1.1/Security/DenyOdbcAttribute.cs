using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using System.Data.Odbc;
using System.Data.Common;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyOdbcAttribute : DenyDBDataAttribute
    {
        protected override DBDataPermission CreateDataPermission()
        {
            return new OdbcPermission(PermissionState.Unrestricted);
        }
    }
}
