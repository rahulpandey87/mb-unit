using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Framework;
using System.Data.OleDb;
using System.Data.Common;

namespace MbUnit.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DenyOleDbAttribute : DenyDBDataAttribute
    {
        protected override DBDataPermission CreateDataPermission()
        {
            return new OleDbPermission(PermissionState.Unrestricted);
        }
    }
}
