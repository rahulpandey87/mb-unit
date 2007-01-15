using System;
using System.Data;

namespace TestFu.Data
{
    public abstract class DbFactoryBase : IDbFactory
    {
        public abstract DbAdministratorBase CreateAdmin(string connectionString, string databaseName);
        public abstract System.Data.IDbConnection CreateConnection(string connectionString);
        public abstract System.Data.IDbCommand CreateCommand(string query, System.Data.IDbConnection connection);
        public abstract System.Data.IDbCommand CreateCommand(string query, System.Data.IDbConnection connection, System.Data.IDbTransaction transaction);

        protected abstract string GetSelectCountQuery(DataTable table);
        protected abstract string GetSelectQuery(DataTable table);

        public virtual int SelectCount(DataTable table, string connectionString)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            string query = this.GetSelectCountQuery(table);
            using (IDbConnection connection = this.CreateConnection(connectionString))
            {
                connection.Open();
                using (IDbCommand command = this.CreateCommand(query, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public virtual DataRow SelectRow(DataTable table, string connectionString, int index)
        {
            if (table == null)
                throw new ArgumentNullException("tableName");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index is negative");


            int i = 0;
            string query = GetSelectQuery(table);
            using (IDbConnection connection = this.CreateConnection(connectionString))
            {
                connection.Open();
                using (IDbCommand command = this.CreateCommand(query, connection))
                {
                    using (IDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            // if i == index, we get the row
                            if (i == index)
                            {
                                // fill a datarow with the data
                                DataRow row = table.NewRow();
                                for (int j = 0; j < dr.FieldCount; ++j)
                                    row[j] = dr[j];
                                return row;
                            }
                            // increment i
                            i++;
                        }
                        throw new Exception("Error while looking for a key");
                    }
                }
            }
        }
    }
}
