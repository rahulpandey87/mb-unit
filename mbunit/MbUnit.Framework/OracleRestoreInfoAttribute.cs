using System;
using TestFu.Data;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class OracleRestoreInfoAttribute : DbRestoreInfoAttribute
    {
        public OracleRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination
            )
            :base(
                connectionString,
                databaseName,
                backupDestination,
                typeof(TestFu.Data.OracleClient.OracleFactory),
                DbBackupDevice.Disk
                )
        { }

        public OracleRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination,
            DbBackupDevice backupDevice
            )
            :base(
                connectionString,
                databaseName,
                backupDestination,
                typeof(TestFu.Data.OracleClient.OracleFactory),
                backupDevice
                )
        { }
    }
}
