using System;
using System.Data;
using System.Data.SqlClient;

namespace TestFu.Data.SqlClient
{
	/// <summary>
	/// A <see cref="IDbFactory"/> implementation for MSSQL server.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
    ///		path='//example[contains(descendant-or-self::*,"SqlFactory")]'
    ///		/>
	public sealed class SqlFactory : DbFactoryBase
	{
        /// <summary>
        /// Creates a <see cref="SqlAdministrator"/> instance.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public override DbAdministratorBase CreateAdmin(string connectionString, string databaseName)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            if (databaseName == null)
                throw new ArgumentNullException("databaseName");
            return new SqlAdministrator(connectionString, databaseName);
        }

        /// <summary>
		/// Creates a <see cref="IDbConnection"/> instance.
		/// </summary>
		/// <param name="connectionString">
		/// Connection string to server
		/// </param>
		/// <returns>
		/// A <see cref="IDbConnection"/> instance.
		/// </returns>
        public override IDbConnection CreateConnection(string connectionString)
        {
			return new SqlConnection(connectionString);
		}

        public SqlCommand CreateCommand(
            string query,
			SqlConnection connection)
		{
			return new SqlCommand(query,connection);
		}

        public override IDbCommand CreateCommand(string query, IDbConnection connection)
        {
            return this.CreateCommand(query, connection as SqlConnection);
        }

        public SqlCommand CreateCommand(
            string query,
			SqlConnection connection,
			SqlTransaction transaction)
		{
			return new SqlCommand(query,connection,transaction);
		}

        public override IDbCommand CreateCommand(string query, IDbConnection connection, IDbTransaction transaction)
        {
            return CreateCommand(query, connection as SqlConnection, transaction as SqlTransaction);
        }

        protected override string GetSelectCountQuery(DataTable table)
        {
            return String.Format("SELECT COUNT(*) FROM [{0}]", table.TableName); ;
        }

        protected override string GetSelectQuery(DataTable table)
        {
            return String.Format("SELECT * FROM [{0}]", table.TableName);
        }
    }
}
