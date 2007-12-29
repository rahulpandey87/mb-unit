using System;
using System.Data;

namespace TestFu.Data
{
	/// <summary>
	/// An abstract base class for test fixtures involving database testing.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"DbFixture")]'
	///		/>
	/// <seealso cref="TestFu.Data.SqlClient.SqlFixture"/>
	public abstract class DbFixture
	{
		private string connectionString;
        private string databaseName;
        private IDbFactory factory = null;
		private IDbConnection connection=null;
		private IDbTransaction transaction=null;
		private DbAdministratorBase admin=null;

		/// <summary>
		/// Initializes a <see cref="DbFixture"/> with a connection string.
		/// </summary>
		/// <param name="connectionString">Connection string for accessing the test database.</param>
        /// <param name="databaseName">The name of the database to use.</param>
        /// <param name="factory">The factory to use.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="connectionString"/> is a null reference
		/// (Nothing in Visual Basic)
		/// </exception>
		public DbFixture(string connectionString, string databaseName, IDbFactory factory)
		{
			if (connectionString==null)
				throw new ArgumentNullException("connectionString");
            if (databaseName==null)
                throw new ArgumentNullException("databaseName");
			if (factory==null)
				throw new ArgumentNullException("factory");
			this.connectionString=connectionString;
            this.databaseName=databaseName;
			this.factory=factory;
			this.admin=
                this.factory.CreateAdmin(this.connectionString,this.databaseName);
		}

		/// <summary>
		/// Gets the database <see cref="DbAdministratorBase"/> instance
		/// </summary>
		/// <value>
		/// A <see cref="DbAdministratorBase"/> instance.
		/// </value>
		public DbAdministratorBase Admin
		{
			get
			{
				return this.admin;
			}
		}

		/// <summary>
		/// Gets the current connection instance.
		/// </summary>
		/// <value>
		/// <see cref="IDbConnection"/> instance.
		/// </value>
		public IDbConnection Connection
		{
			get
			{
				return this.connection;
			}
		}

		/// <summary>
		/// Gets the current transaction.
		/// </summary>
		/// <value>
		/// A <see cref="IDbTransaction"/> instance if <see cref="BeginTransaction"/> was called
		/// and the connection not closed; otherwise, a null reference (Nothing in Visual Basic)
		/// </value>
		public IDbTransaction Transaction
		{
			get
			{
				return this.transaction;
			}
		}
		
		/// <summary>
		/// Gets the connection string to access the db server (without
		/// database information.
		/// </summary>
		public String ConnectionString
		{
			get
			{
				return this.connectionString;
			}
		}

        /// <summary>
        /// Gets the test database name.
        /// </summary>
        /// <value></value>
        public string DatabaseName
        {
            get
            {
                return this.databaseName;
            }
        }

        /// <summary>
        /// Gets the connection string to connecto the test database.
        /// </summary>
        public abstract string DatabaseConnectionString {get;}

        /// <summary>
		/// Opens a <see cref="IDbConnection"/> instance with the 
		/// <see cref="ConnectionString"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method must be implemented in inherited classes for different factorys.
		/// </para>
		/// </remarks>
		public virtual void Open()
		{
			if (this.Connection==null)
			{
				this.connection = this.factory.CreateConnection(this.DatabaseConnectionString);
			}
			if (this.Connection.State!=ConnectionState.Open)
				this.Connection.Open();
		}

		/// <summary>
		/// Executes a non-query command with the given parameters
		/// </summary>
		/// <param name="query">
		/// Query format string
		/// </param>
		/// <param name="args">
		/// Query arguments for the format string
		/// </param>
		/// <returns>
		/// Number of affected rows
		/// </returns>
		/// <remarks>
		/// <para>
		/// The connection is automatically opened if necessary.
		/// </para>
		/// </remarks>
		public virtual int ExecuteNonQuery(string query, params Object[] args)
		{
			string fullQuery=String.Format(query,args);
			this.Open();
			using (IDbCommand cmd = this.factory.CreateCommand(fullQuery,this.Connection))
			{
				return cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Executes a scalar query with the given parameters
		/// </summary>
		/// <param name="query">
		/// Query format string
		/// </param>
		/// <param name="args">
		/// Query arguments for the format string
		/// </param>
		/// <returns>
		/// Query result
		/// </returns>
		/// <remarks>
		/// <para>
		/// The connection is automatically opened if necessary.
		/// </para>
		/// </remarks>
		public virtual Object ExecuteScalar(string query, params Object[] args)
		{
			string fullQuery=String.Format(query,args);
			this.Open();
			using (IDbCommand cmd = this.factory.CreateCommand(fullQuery,this.Connection))
			{
				return cmd.ExecuteScalar();
			}
		}

		/// <summary>
		/// Executes query and returns the <see cref="IDataReader"/>
		/// instance
		/// </summary>
		/// <param name="query">
		/// Query format string
		/// </param>
		/// <param name="args">
		/// Query arguments for the format string
		/// </param>
		/// <returns>
		/// A <see cref="IDataReader"/> resulting from the query.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The connection is automatically opened if necessary and the reader
		/// is created with <see cref="CommandBehavior.CloseConnection"/> 
		/// option.
		/// </para>
		/// </remarks>
		public virtual IDataReader ExecuteReader(string query, params Object[] args)
		{
			string fullQuery=String.Format(query,args);
			this.Open();
			using (IDbCommand cmd = this.factory.CreateCommand(fullQuery,this.Connection))
			{
				return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Begins a new transaction.
		/// </summary>		
		/// <remarks>
		/// <para>
		/// If a previous transaction was opened, by default, it is rolled back.
		/// </para>
		/// </remarks>
		/// <exception cref="InvalidOperationException">
		/// The current connection is not created or not opened.
		/// </exception>
		protected virtual void BeginTransaction()
		{			
			if (this.connection==null)
				throw new InvalidOperationException("No connection has been set");
			if (this.connection.State!=ConnectionState.Open)
				throw new InvalidOperationException("Connection is closed");
			
			this.RollBack();
			this.transaction=this.connection.BeginTransaction();			
		}
		
		/// <summary>
		/// Commits the current transaction if any.
		/// </summary>
		protected virtual void Commit()
		{
			if (this.transaction!=null)
			{
				this.transaction.Commit();
				this.transaction.Dispose();
				this.transaction=null;
			}
		}
		
		/// <summary>
		/// Rollsback the current transaction if any.
		/// </summary>
		protected virtual void RollBack()
		{
			if (this.transaction!=null)
			{
				this.transaction.Rollback();
				this.transaction.Dispose();
				this.transaction=null;
			}			
		}

		/// <summary>
		/// Closes the current connection.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If a transaction was opened, it is first rolled back.
		/// </para>
		/// </remarks>
		public virtual void Close()
		{
			this.RollBack();
			if (this.connection!=null)
			{
				this.connection.Close();
				this.connection.Dispose();
				this.connection=null;
			}
		}
	}
}
