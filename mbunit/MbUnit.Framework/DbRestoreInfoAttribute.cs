using System;
using MbUnit.Framework;
using MbUnit.Core;
using TestFu.Data;
using TestFu.Data.SqlClient;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class DbRestoreInfoAttribute : Attribute
    {
        private string connectionstring;
        private string databaseName;
        private string backupDestination;
        private DbBackupDevice backupDevice;
        private Type factoryType;

		protected DbRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination,
            Type factoryType
            )
            :this(
                connectionString,
                databaseName,
                backupDestination,
                factoryType,
                DbBackupDevice.Disk
                )
        { }

		protected DbRestoreInfoAttribute(
            string connectionString,
            string databaseName,
            string backupDestination,
            Type factoryType,
            DbBackupDevice backupDevice
            )
        {
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

        public string ConnectionString
        {
            get
            {
                return this.connectionstring;
            }
        }

        public string DatabaseName
        {
            get
            {
                return this.databaseName;
            }
        }

        public string BackupDestination
        {
            get
            {
                return this.backupDestination;
            }
        }

        public DbBackupDevice BackupDevice
        {
            get
            {
                return this.backupDevice;
            }
        }

        public Type FactoryType
        {
            get
            {
                return this.factoryType;
            }
        }

        public IDbFactory CreateFactory()
        {
            return (IDbFactory)Activator.CreateInstance(this.factoryType);
        }

        public static DbRestoreInfoAttribute GetInfo(Type t)
        {
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
