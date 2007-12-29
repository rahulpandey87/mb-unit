using System;
namespace TestFu.Data
{
    interface IDbAdministratorBase
    {
        void BackupDatabase(DbBackupDevice device, string destination);
        string ConnectionString { get; set; }
        bool ContainsConstraint(string constraintName);
        bool ContainsDatabase();
        bool ContainsProcedure(string procedureName);
        bool ContainsTable(string tableName);
        bool ContainsView(string viewName);
        void CreateDatabase();
        string DatabaseConnectionString { get; }
        string DatabaseName { get; set; }
        string DatabaseOwner { get; set; }
        void DropConstraint(string tableName, string constraintName);
        void DropConstraints(string tableName);
        void DropDatabase();
        void DropProceduce(string procedureName);
        void DropTable(string tableName);
        void DropView(string viewName);
        int ExecuteNonQuery(string connString, string query, params object[] args);
        System.Data.IDataReader ExecuteReader(string connString, string query, params object[] args);
        object ExecuteScalar(string connString, string query, params object[] args);
        void RestoreDatabase(DbBackupDevice device, string destination);
    }
}
