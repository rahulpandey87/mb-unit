using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Framework;
using System.Data.Common;
using System.Data.SqlClient;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenySqlClientAttribute : DenyDBDataAttribute
    {
        protected override DBDataPermission CreateDataPermission()
        {
            return new SqlClientPermission(PermissionState.Unrestricted);
        }
    }
}
