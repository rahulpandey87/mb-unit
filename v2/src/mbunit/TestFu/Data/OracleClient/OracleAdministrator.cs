using System;
using System.Text;

namespace TestFu.Data.OracleClient
{
    public sealed class OracleAdministrator : DbAdministrator
    {
        public OracleAdministrator(string connectionString, string databaseName)
            :base(connectionString,databaseName,new OracleFactory())
        {}

        public override string DatabaseConnectionString
        {
            get 
			{ 
				throw new NotImplementedException(); 
			}
        }

        public override void BackupDatabase(DbBackupDevice device, string destination)
        {
            throw new NotImplementedException();
        }

        public override void DropDatabase()
        {
            throw new NotImplementedException();
        }

        public override void RestoreDatabase(DbBackupDevice device, string destination)
        {
            throw new NotImplementedException();
        }

        public override void CreateDatabase()
        {
            throw new NotImplementedException();
        }
        public override bool ContainsDatabase()
        {
            throw new NotImplementedException();
        }

        public override bool ContainsConstraint(string constraintName)
        {
            throw new NotImplementedException();
        }

        public override bool ContainsProcedure(string procedureName)
        {
            throw new NotImplementedException();
        }

        public override bool ContainsTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public override bool ContainsView(string viewName)
        {
            throw new NotImplementedException();
        }

        public override void DropConstraint(string tableName, string constraintName)
        {
            throw new NotImplementedException();
        }

        public override void DropConstraints(string tableName)
        {
            throw new NotImplementedException();
        }

        public override void DropProceduce(string procedureName)
        {
            throw new NotImplementedException();
        }

        public override void DropTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public override void DropView(string viewName)
        {
            throw new NotImplementedException();
        }
    }
}
