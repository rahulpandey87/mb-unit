using System;
using MbUnit.Framework;
using MbUnit.Core;
using MbUnit.Framework.Exceptions;
using TestFu.Data;
using TestFu.Data.SqlClient;

namespace MbUnit.Framework {
    /// <summary>
    /// Abstract class describing attribute used to tag a class and declare a backup be restored to
    /// it before the test is executed on request by tagging a test method with a <see cref="RestoreDatabaseAttribute"/>. 
    /// This class represents the generic case. The specific implementation for SQL Server is <see cref="SqlRestoreInfoAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class DbRestoreInfoAttribute : Attribute {
        private string connectionstring;
        private string databaseName;
        private string backupDestination;
        private DbBackupDevice backupDevice;
        private Type factoryType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbRestoreInfoAttribute"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to the database server.</param>
        /// <param name="databaseName">Name of the database to be restored.</param>
        /// <param name="backupDestination">The location of the backup file.</param>
        /// <param name="factoryType">The factory class derived from <see cref="IDbFactory"/> that will execute the restore</param>
        /// <exception cref="ArgumentNullException">Thrown if either the <paramref name="connectionString"/>,
        /// <paramref name="databaseName"/>, <paramref name="backupDestination"/>, or <paramref name="factoryType"/> are null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="factoryType"/> does not derive from <see cref="IDbFactory"/></exception>
        protected DbRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination,
            Type factoryType
            )
            : this(
                connectionString,
                databaseName,
                backupDestination,
                factoryType,
                DbBackupDevice.Disk
                ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbRestoreInfoAttribute"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to the database server.</param>
        /// <param name="databaseName">Name of the database to be restored.</param>
        /// <param name="backupDestination">The location of the backup file.</param>
        /// <param name="factoryType">The factory class derived from <see cref="IDbFactory"/> that will execute the restore</param>
        /// <param name="backupDevice">The <see cref="DbBackupDevice">type</see> of backup device.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the <paramref name="connectionString"/>,
        /// <paramref name="databaseName"/>, <paramref name="backupDestination"/>, or <paramref name="factoryType"/> are null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="factoryType"/> does not derive from <see cref="IDbFactory"/></exception>
        protected DbRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination,
            Type factoryType,
            DbBackupDevice backupDevice
            ) {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            if (databaseName == null)
                throw new ArgumentNullException("databaseName");
            if (backupDestination == null)
                throw new ArgumentNullException("backupDestination");
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            if (!typeof(IDbFactory).IsAssignableFrom(factoryType))
                throw new ArgumentException("factorType must implement IDbFactory");

            this.connectionstring = connectionString;
            this.databaseName = databaseName;
            this.backupDestination = backupDestination;
            this.backupDevice = backupDevice;
            this.factoryType = factoryType;
        }

        /// <summary>
        /// Gets the connection string for the database to be restored.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString {
            get {
                return this.connectionstring;
            }
        }

        /// <summary>
        /// Gets the name of the database to be restored.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName {
            get {
                return this.databaseName;
            }
        }

        /// <summary>
        /// Gets the location of the backup file.
        /// </summary>
        /// <value>The location of the backup file.</value>
        public string BackupDestination {
            get {
                return this.backupDestination;
            }
        }

        /// <summary>
        /// Gets the <see cref="DbBackupDevice">type</see> of backup device.
        /// </summary>
        /// <value>The <see cref="DbBackupDevice">type</see> of backup device.</value>
        public DbBackupDevice BackupDevice {
            get {
                return this.backupDevice;
            }
        }

        /// <summary>
        /// Gets the factory class derived from <see cref="IDbFactory"/> that will execute the restore
        /// </summary>
        /// <value>The factory class derived from <see cref="IDbFactory"/> that will execute the restore</value>
        public Type FactoryType {
            get {
                return this.factoryType;
            }
        }

        /// <summary>
        /// Creates and returns an instance of the factory class derived from <see cref="IDbFactory"/> that will execute the restore.
        /// </summary>
        /// <returns>An instance of the factory class</returns>
        public IDbFactory CreateFactory() {
            return (IDbFactory)Activator.CreateInstance(this.factoryType);
        }

        /// <summary>
        /// Gets the custom attributes on the attribute
        /// </summary>
        /// <param name="t">The attribute <see cref="Type"/>.</param>
        /// <returns>The custom attributes (database information) for the backup</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="t"/> is null</exception>
        /// <exception cref="MissingDbInfoException">Thrown if no database information can be found attached to the attribute taggign the class</exception>
        public static DbRestoreInfoAttribute GetInfo(Type t) {
            if (t == null)
                throw new ArgumentNullException("t");
            // get database information
            DbRestoreInfoAttribute info =
                (DbRestoreInfoAttribute)TypeHelper.TryGetFirstCustomAttribute(t, typeof(DbRestoreInfoAttribute));
            if (info == null)
                throw new MbUnit.Framework.Exceptions.MissingDbInfoException(t);
            return info;
        }
    }
}
