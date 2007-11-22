using System;
using System.Data;
using System.IO;

namespace TestFu.Data
{
	/// <summary>
	/// Abstract class to perform administrative tasks on a database
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"DbAdministratorBase")]'
	///		/>
	public abstract class DbAdministratorBase : IDbAdministratorBase
	{
		private IDbFactory factory;
		private string connectionString;
		private string databaseName;
		private string databaseOwner="dbo";

		/// <summary>
		/// Initializes an instance of <see cref="DbAdministratorBase"/> with the connection string.
		/// </summary>
		/// <param name="connectionString">
		/// Connection string to the SQL server without initial catalog
		/// </param>
		/// <param name="databaseName">
		/// Catalog name
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="connectionString"/> is a null reference (Nothing in Visual Basic)
		/// </exception>
        public DbAdministratorBase(string connectionString, string databaseName, IDbFactory factory)
        {
			if (connectionString==null)
				throw new ArgumentNullException("connectionString");
			if (databaseName==null)
				throw new ArgumentNullException("databaseName");
			if(factory==null)
				throw new ArgumentNullException("factory");
			this.connectionString=connectionString.TrimEnd(';');
			this.databaseName=databaseName;
			this.factory=factory;
		}

		#region Properties
		/// <summary>
		/// Gets or sets the connection string with Initial Catalog information
		/// </summary>
		/// <value>
		/// Connection string.
		/// </value>
		public virtual string ConnectionString
		{
			get
			{
				return this.connectionString;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("ConnectionString");
				this.connectionString=value.TrimEnd(';');
			}
		}

		/// <summary>
		/// Gets or sets the database name
		/// </summary>
		/// <value>
		/// The database name.
		/// </value>
		public virtual string DatabaseName
		{
			get
			{
				return this.databaseName;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("databaseName");
				this.databaseName=value;
			}
		}

		/// <summary>
		/// Gets or sets the database owner.
		/// </summary>
		/// <value>
		/// Database owner name.
		/// </value>
		public virtual string DatabaseOwner
		{
			get
			{
				return this.databaseOwner;
			}
			set
			{
				this.databaseOwner=value;
			}
		}

		/// <summary>
		/// Gets the connection string with Initial Catalog information.
		/// </summary>
		/// <value>
		/// Connection string with Initial catalog information.
		/// </value>
		public abstract string DatabaseConnectionString {get;}
		#endregion

		#region Database Handling
		/// <summary>
		/// Creates a backup of the specified database using the specified <paramref name="device"/> 
		/// and <paramref name="destination"/>.
		/// </summary>
		/// <param name="device">
		/// A <see cref="DbBackupDevice"/> defining the type of output device.
		/// </param>
		/// <param name="destination">
		/// Device path.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="destination"/> is a null
		/// reference (Nothing in Visual Basic)
		/// </exception>
		public abstract void BackupDatabase(DbBackupDevice device, string destination);
		
		/// <summary>
		/// Restores a backup of the specified database using the specified <paramref name="device"/> 
		/// and <paramref name="destination"/>.
		/// </summary>
		/// <param name="device">
		/// A <see cref="DbBackupDevice"/> defining the type of output device.
		/// </param>
		/// <param name="destination">
		/// Device path.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="destination"/> is a null
		/// reference (Nothing in Visual Basic)
		/// </exception>
		/// <remarks>
		/// <para>
		/// If you plan to override an existing database, you must first drop this database.
		/// This method takes a conservative behavior and will not override an existing database.
		/// </para>
		/// </remarks>
		public abstract void RestoreDatabase(DbBackupDevice device, string destination);
		
		/// <summary>
		/// Creates a new database on the server
		/// </summary>
		public abstract void CreateDatabase();
		
		/// <summary>
		/// Drops an existing new database on the server
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="databaseName"/> is a null
		/// reference (Nothing in Visual Basic)
		/// </exception>		
		public abstract void DropDatabase();

		/// <summary>
		/// Gets a value indicating if the current database exists.
		/// </summary>
		/// <returns>
		/// true if it exists; otherwise, false.
		/// </returns>
		public abstract bool ContainsDatabase();
		#endregion

		#region Constraints
		public abstract void DropConstraint(string tableName, string constraintName);
		public abstract void DropConstraints(string tableName);
		public abstract bool ContainsConstraint(string constraintName);
		#endregion

		#region Tables
		/// <summary>
		/// Drops the table.
		/// </summary>
		/// <param name="tableName">
		/// Name of the table to drop
		/// </param>
		/// <remarks>
		/// <para>
		/// This method takes care of removing the constraints associated
		/// to the table before removing the table.
		/// </para>
		/// </remarks>
		public abstract void DropTable(string tableName);

		/// <summary>
		/// Gets a value indicating if the database contains
		/// the table.
		/// </summary>
		/// <param name="tableName">
		/// Name of the table to search
		/// </param>
		/// <returns>
		/// true if a table named <paramref name="tableName"/> is contained
		/// in the databse;oterwise false.
		/// </returns>
		public abstract bool ContainsTable(string tableName);
		#endregion

		#region Procedures
		public abstract void DropProceduce(string procedureName);
		public abstract bool ContainsProcedure(string procedureName);
		#endregion

		#region Views
		public abstract void DropView(string viewName);
		public abstract bool ContainsView(string viewName);
		#endregion

		#region Protected helper methods
		protected virtual void EnsureFileDestination(string fileName)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileName));
		}
		#endregion

		#region Execute methods
		/// <summary>
		/// Executes a non-query in a safe, transactional environement.
		/// </summary>
		/// <param name="query">
		/// Query to execute.
		/// </param>
		public virtual int ExecuteNonQuery(string connString, string query, params object[] args)
		{
			if(query==null)
				throw new ArgumentNullException("databaseName");
		
			using(IDbConnection connection = this.factory.CreateConnection(connString))
            {
				connection.Open();
				string fullQuery = string.Format(query,args);
				using(IDbCommand cmd = this.factory.CreateCommand(fullQuery,connection))
				{
					return cmd.ExecuteNonQuery();
				}				
			}
		}

		public virtual object ExecuteScalar(string connString, string query, params object[] args)
		{
			if(query==null)
				throw new ArgumentNullException("databaseName");
		
			using (IDbConnection connection = this.factory.CreateConnection(connString))
            {
				connection.Open();
				string fullQuery = string.Format(query,args);
				using(IDbCommand cmd = this.factory.CreateCommand(fullQuery,connection))
				{
					return cmd.ExecuteScalar();
				}				
			}
		}

		public virtual IDataReader ExecuteReader(string connString, string query, params object[] args)
		{
			if(query==null)
				throw new ArgumentNullException("databaseName");
		
			IDbConnection connection = null;
			try
			{
				connection = this.factory.CreateConnection(connString);
				connection.Open();
				string fullQuery = string.Format(query,args);
				using(IDbCommand cmd = this.factory.CreateCommand(fullQuery,connection))
				{
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
				}
			}
			catch(Exception ex)
			{
				if (connection!=null)
				{
                    connection.Close();
                    connection.Dispose();
				}
                throw new ApplicationException("Error while executing reader", ex);
            }		
		}
		#endregion

	}
}
