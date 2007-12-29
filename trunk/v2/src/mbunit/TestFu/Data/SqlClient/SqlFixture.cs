using System;
using System.Data;
using System.Data.SqlClient;

namespace TestFu.Data.SqlClient
{
	/// <summary>
	/// Abstract  base class for MSSQL server database testing.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"SqlFixture")]'
	///		/>
	public abstract class SqlFixture : DbFixture
	{
		/// <summary>
		/// Initializes a <see cref="DbFixture"/> with a connection string.
		/// </summary>
		/// <param name="connectionString">
		/// Connection string for accessing the test database.
		/// </param>
		/// <param name="database">
		/// database name
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="connectionString"/> is a null reference
		/// (Nothing in Visual Basic)
		/// </exception>
		public SqlFixture(string connectionString, string database)
			:base(connectionString,database,new SqlFactory())
		{}

        public override string DatabaseConnectionString
        {
            get 
            {
                return String.Format(
                    "{0};Initial Catalog={1}",
                    this.ConnectionString,
                    this.DatabaseName
                    );
            }
        }



        /// <summary>
		/// Gets the current connection instance.
		/// </summary>
		/// <value>
		/// <see cref="SqlConnection"/> instance.
		/// </value>
		public new SqlConnection Connection
		{
			get
			{
				return (SqlConnection)base.Connection;
			}
		}

		/// <summary>
		/// Gets the current transaction.
		/// </summary>
		/// <value>
		/// A <see cref="SqlTransaction"/> instance if <see cref="DbFixture.BeginTransaction"/> was called
		/// and the connection not closed; otherwise, a null reference (Nothing in Visual Basic)
		/// </value>
		public new SqlTransaction Transaction
		{
			get
			{
				return (SqlTransaction)base.Transaction;
			}
		}
	}
}
