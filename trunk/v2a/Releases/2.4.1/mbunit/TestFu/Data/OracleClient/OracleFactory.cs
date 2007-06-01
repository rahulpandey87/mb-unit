using System;
using System.Data;
using System.Data.OracleClient;

namespace TestFu.Data.OracleClient
{
    public sealed class OracleFactory : IDbFactory
    {
        #region DbFactory Implementation for Oracle
        public OracleAdministrator CreateAdmin(string connectionString, string databaseName)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            if (databaseName == null)
                throw new ArgumentNullException("databaseName");
            return new OracleAdministrator(connectionString, databaseName);
        }

        public OracleConnection CreateConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }

        public OracleCommand CreateCommand(string query, OracleConnection connection)
        {
            return new OracleCommand(query, connection);
        }

        public OracleCommand CreateCommand(string query, OracleConnection connection, OracleTransaction transaction)
        {
            return new OracleCommand(query, connection, transaction);
        }
        #endregion

        #region IDbFactory Members
        DbAdministrator IDbFactory.CreateAdmin(string connectionString, string databaseName)
        {
            return this.CreateAdmin(connectionString, databaseName);
        }

        System.Data.IDbConnection IDbFactory.CreateConnection(string connectionString)
        {
            return this.CreateConnection(connectionString);
        }

        System.Data.IDbCommand IDbFactory.CreateCommand(string query, System.Data.IDbConnection connection)
        {
            return this.CreateCommand(query, (OracleConnection)connection);
        }

        System.Data.IDbCommand IDbFactory.CreateCommand(string query, System.Data.IDbConnection connection, System.Data.IDbTransaction transaction)
        {
            return this.CreateCommand(query, (OracleConnection)connection, (OracleTransaction)transaction);
        }
        #endregion
    }
}
