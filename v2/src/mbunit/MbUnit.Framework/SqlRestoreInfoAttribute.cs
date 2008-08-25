using System;
using TestFu.Data;

namespace MbUnit.Framework
{
    /// <summary>
    /// Tags a class with the information needed to restore a SQL Server database from a backup.
    /// Test method within the tagged class must themselves be tagged with an attribute derived 
    /// from <see cref="RestoreDatabaseAttribute"/> to make use of this information. 
    /// </summary>
    /// <example>
    /// <para>The following example shows restores being made to a SQL Server database before TestWithRestoreFirst is run</para>
    /// <code>
    /// [TestFixture]
    /// [SqlRestoreInfo("MyConnString", "MyDatabase", @"d:\data\pubs.bak")]
    /// public class PubsTest {
    /// 
    ///    [Test, RestoreDatabaseFirst]
    ///    public void TestWithRestoreFirst()
    ///    {...}
    /// 
    ///    [Test, Rollback]
    ///    public void TestWithRollback()
    ///    {...}
    /// } 
    /// </code>
    /// </example>
    /// <seealso cref="RestoreDatabaseAttribute"/>
    /// <seealso cref="RestoreDatabaseFirstAttribute"/>
    /// <seealso cref="DbRestoreInfoAttribute"/>
    /// <seealso cref="RollBackAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SqlRestoreInfoAttribute : DbRestoreInfoAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlRestoreInfoAttribute"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to the SQL database.</param>
        /// <param name="databaseName">Name of the database to be restored from backup.</param>
        /// <param name="backupDestination">The location of the backup file</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlRestoreInfoAttribute"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to the SQL database.</param>
        /// <param name="databaseName">Name of the database to be restored from backup.</param>
        /// <param name="backupDestination">The location of the backup file</param>
        /// <param name="backupDevice">The <see cref="DbBackupDevice">type</see> of backup device being used.</param>
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
