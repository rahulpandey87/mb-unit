using System;
using TestFu.Data;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SqlRestoreInfoAttribute : DbRestoreInfoAttribute
    {
        public SqlRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination
            )
            :base(
                connectionString,
                databaseName,
                backupDestination,
                typeof(TestFu.Data.SqlClient.SqlFactory),
                DbBackupDevice.Disk
                )
        { }

        public SqlRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination,
            DbBackupDevice backupDevice
            )
            :base(
                connectionString,
                databaseName,
                backupDestination,
                typeof(TestFu.Data.SqlClient.SqlFactory),
                backupDevice
                )
        {}
    }
}
