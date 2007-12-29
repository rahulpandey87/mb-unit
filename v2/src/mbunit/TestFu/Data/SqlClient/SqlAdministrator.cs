using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace TestFu.Data.SqlClient
{
	/// <summary>
	/// Helper class to performe task on a SQL server.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"SqlAdministrator")]'
	///		/>
	public class SqlAdministrator : DbAdministratorBase
	{
		/// <summary>
		/// Initializes an instance of <see cref="SqlAdministrator"/> with the connection string.
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
		public SqlAdministrator(string connectionString,string databaseName)
			:base(
				connectionString,
				databaseName, 
				new SqlFactory()
			)
		{
		}

		#region Properties
		/// <summary>
		/// Gets the connection string with Initial Catalog information.
		/// </summary>
		/// <value>
		/// Connection string with Initial catalog information.
		/// </value>
		public override string DatabaseConnectionString
		{
			get
			{
				return String.Format("{0};Initial Catalog={1}",
					this.ConnectionString,
					this.DatabaseName);
			}
		}
		#endregion
		
		#region Database Backup/Restore
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
		public override void BackupDatabase(DbBackupDevice device, string destination)
		{			
			if (destination==null)
				throw new ArgumentNullException("destination");
			
			string query=null;
			if (device==DbBackupDevice.Dump)
			{
				query = String.Format("BACKUP DATABASE [{0}] TO {1}",
			                             this.DatabaseName,
			                             destination
			                             );
			}
			else
			{
				query = String.Format("BACKUP DATABASE [{0}] TO {1} = '{2}'",
			                             this.DatabaseName,
			                             device.ToString().ToUpper(),
			                             destination
			                             );				
			}
			this.EnsureFileDestination(destination);
			this.ExecuteNonQuery(this.ConnectionString,query);	
		}

		
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
		/// <para>
		/// Priorly to restore the database, the method kills all the processes associeted
		/// to the database.
		/// </para>
		/// </remarks>
		public override void RestoreDatabase(DbBackupDevice device, string destination)
		{			
			if (destination==null)
				throw new ArgumentNullException("destination");
			
            // kill processes on db
            this.killOpenDBProcesses(this.ConnectionString, this.DatabaseName);

            string query=null;
			if (device==DbBackupDevice.Dump)
			{
				query = String.Format("RESTORE DATABASE [{0}] FROM {1}",
			                             this.DatabaseName,
			                             destination
			                             );
			}
			else
			{
				query = String.Format("RESTORE DATABASE [{0}] FROM {1} = '{2}'",
			                             this.DatabaseName,
			                             device.ToString().ToUpper(),
			                             destination
			                             );				
			}
				
			this.ExecuteNonQuery(this.ConnectionString,query);
		}
		
		/// <summary>
		/// Creates a new database on the server
		/// </summary>
		public override void CreateDatabase()
		{
			string query ="IF NOT EXISTS(SELECT * FROM master..sysdatabases WHERE Name='{0}') CREATE DATABASE {0}";	
			this.ExecuteNonQuery(this.ConnectionString,query,this.DatabaseName);
		}
		
		/// <summary>
		/// Drops an existing new database on the server
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="databaseName"/> is a null
		/// reference (Nothing in Visual Basic)
		/// </exception>		
		public override void DropDatabase()
		{
            // kill processes
            this.killOpenDBProcesses(this.ConnectionString, this.DatabaseName);

            // drop db
            string query ="IF EXISTS(SELECT * FROM master..sysdatabases WHERE Name='{0}') DROP DATABASE [{0}]";	
			this.ExecuteNonQuery(this.ConnectionString,query,this.DatabaseName);			
		}

		public override bool ContainsDatabase()
		{
			string query ="SELECT COUNT(*) FROM master..sysdatabases WHERE Name='{0}'";	
			return (int)ExecuteScalar(this.ConnectionString,query,this.DatabaseName)!=0;
        }

        #region Process Killing (from Roy Osherove - www.iserializable.com)
        private void killOpenDBProcesses(string connectionString, string dbName)
        {
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				string KILL_COMMAND = createKillCommand(connection, dbName);
				if(KILL_COMMAND==string.Empty)
				{
					return;
					//no need to try and kill anything	
				}
				using (SqlCommand killCommand = new SqlCommand(KILL_COMMAND, connection))
				{
					killCommand.ExecuteNonQuery();
				}
			}
        }

        private string createKillCommand(SqlConnection connection, string dbName)
        {
            string selecteTemplate = "Use Master; Select spid as Process_ID from sysprocesses where dbid = DB_ID('{0}')";
            string SELECT = string.Format(selecteTemplate, dbName);

            connection.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("Use master;");

            using (SqlDataReader dr =
                       new SqlCommand(SELECT, connection).ExecuteReader())
            {
            	if (!generateKillCommandFromDataReader(dr, sb))
            		return string.Empty;
            }
            string KILL_COMMAND = sb.ToString().TrimEnd(';');
            return KILL_COMMAND;
        }

        private bool generateKillCommandFromDataReader(SqlDataReader dr, StringBuilder sb)
        {
            int count = 0;
            while (dr.Read())
            {
                int processID = int.Parse(dr["Process_ID"].ToString());
                if (processID == Process.GetCurrentProcess().Id)
                {
                    continue;
                }
                sb.Append("KILL " + processID.ToString() + ";");
                count++;
            }
            return count != 0;
        }
        #endregion
		#endregion

		#region Constraints
		public override void DropConstraints(string tableName)
		{
            DropForeignKeyConstraints(tableName);
            DropUniqueConstraints(tableName);
        }

        private void DropForeignKeyConstraints(string tableName)
        {
            string query = 
                "SELECT RC.CONSTRAINT_NAME,TCFK.TABLE_NAME "+
                "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC "+
                "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC "+
                "ON RC.UNIQUE_CONSTRAINT_NAME = TC.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TCFK "+
                "ON RC.CONSTRAINT_NAME = TCFK.CONSTRAINT_NAME " +
                "WHERE TC.TABLE_NAME = '{0}'";
            ArrayList constraints = new ArrayList();
            using (IDataReader dr = this.ExecuteReader(
                        this.DatabaseConnectionString,
                        query,
                        tableName
                       )
                    )
            {
                while (dr.Read())
                {
                   constraints.Add(new DictionaryEntry(dr[0],dr[1]));
                }
            }

            foreach (DictionaryEntry de in constraints)
            {
                this.ExecuteNonQuery(
                    this.DatabaseConnectionString,
                    "ALTER TABLE [{0}].[{1}] DROP CONSTRAINT {2}",
                    this.DatabaseOwner,
                    de.Value,
                    de.Key
                    );
            }
        }

        private void DropUniqueConstraints(string tableName)
        {
            string query = "SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = '{0}'";
            ArrayList constraints = new ArrayList();
            using (IDataReader dr = this.ExecuteReader(
                        this.DatabaseConnectionString,
                        query,
                        tableName
                       )
                    )
            {
                while (dr.Read())
                {
                    constraints.Add(dr[0]);
                }
            }

            foreach (string name in constraints)
            {
                this.ExecuteNonQuery(
                    this.DatabaseConnectionString,
                    "ALTER TABLE [{0}].[{1}] DROP CONSTRAINT {2}",
                    this.DatabaseOwner,
                    tableName,
                    name
                    );
            }
        }

        public override void DropConstraint(string tableName, string constraintName)
		{
			string query="IF EXISTS (SELECT name FROM {0}.sysobjects WHERE name = '{2}' AND (type IN ('F','K')) ALTER TABLE {1} DROP CONSTRAINT {2}";

			this.ExecuteNonQuery(this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				tableName,
				constraintName
				);
		}

		public override bool ContainsConstraint(string constraintName)
		{
            string query = "SELECT COUNT(*) FROM {0}.sysobjects WHERE name = '{1}' AND (type IN ('F','K'))";

            return (int)this.ExecuteScalar(this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				constraintName
				)!=0;
		}
		#endregion

		#region Tables
		public override void DropTable(string tableName)
		{
			// drop constraints
			this.DropConstraints(tableName);

			// drop table
			string query=
				"IF EXISTS (SELECT name FROM dbo.sysobjects WHERE name = '{1}' AND type = 'U') DROP TABLE [{0}].[{1}]";
			this.ExecuteNonQuery(
                this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				tableName
				);
		}

		public override bool ContainsTable(string tableName)
		{

			string query="SELECT COUNT(name) FROM {0}.sysobjects WHERE name = '{1}' AND type = 'U'";
			return (int)ExecuteScalar(
				this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				tableName) 
				!= 0;
		}
		#endregion

		#region Procedures
		public override void DropProceduce(string procedureName)
		{
			string query=
				"IF EXISTS (SELECT name FROM {0}.sysobjects WHERE name = '{1}' AND type = 'P') DROP PROCEDURE {0}.[{1}]";
			this.ExecuteNonQuery(
				this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				procedureName);
		}

		public override bool ContainsProcedure(string procedureName)
		{
			string query=
				"SELECT COUNT(*) FROM {0}.sysobjects WHERE name = '{1}' AND type = 'P'";
			return (int)this.ExecuteScalar(this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				procedureName
				)!=0;
		}
		#endregion

		#region Views
		public override void DropView(string viewName)
		{
			if (viewName==null)
				throw new ArgumentNullException("viewName");

			string query=
				"IF EXISTS (SELECT name FROM {0}.sysobjects WHERE name = '{1}' AND type = 'V') DROP VIEW {0}.[{1}]";
			this.ExecuteNonQuery(
				this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				viewName);			
		}

		public override bool ContainsView(string viewName)
		{
			if (viewName==null)
				throw new ArgumentNullException("viewName");

			string query=
				"SELECT COUNT(*) FROM {0}.sysobjects WHERE name = '{1}' AND type = 'V'";
			return (int)this.ExecuteScalar(this.DatabaseConnectionString,
				query,
				this.DatabaseOwner,
				viewName
				)!=0;		
		}

		#endregion
	}
}
