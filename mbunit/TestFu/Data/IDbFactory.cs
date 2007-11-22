using System;
using System.Data;

namespace TestFu.Data
{
	/// <summary>
	/// A factory for <see cref="IDbConnection"/> and <see cref="IDbCommand"/>
	/// instances.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IDbFactory")]'
	///		/>
	public interface IDbFactory
	{
        DbAdministratorBase CreateAdmin(string connectionString, string databaseName);

        /// <summary>
		/// Creates a <see cref="IDbConnection"/> instance.
		/// </summary>
		/// <param name="connectionString">
		/// Connection string to server
		/// </param>
		/// <returns>
		/// A <see cref="IDbConnection"/> instance.
		/// </returns>
		IDbConnection CreateConnection(string connectionString);

		IDbCommand CreateCommand(
			string query,
			IDbConnection connection);
		IDbCommand CreateCommand(
			string query,
			IDbConnection connection,
			IDbTransaction transaction
			);

        int SelectCount(DataTable table, string connectionString);
        DataRow SelectRow(DataTable table, string connectionString, int index);
    }
}
