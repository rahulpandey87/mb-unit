using System;
using System.Threading;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.SqlClient
{
	using TestFu.Data;
	using TestFu.Data.SqlClient;

	[TestFixture]
    public class SqlAdministratorTest
    {
        private string databaseName = "TestDatabase2";
        private string connectionString = 
                @"Data Source=GRENIER\MARYLIN;Integrated Security=SSPI";
        private SqlAdministrator admin = null;

        [TearDown]
        public void TearDown()
        {
            if (this.admin != null)
                this.admin.DropDatabase();
            this.admin = null;
        }

		[Test]
		public void CreateAndDropDatabase()
		{
            this.admin = new SqlAdministrator(connectionString, databaseName);
            admin.CreateDatabase();
            Assert.IsTrue(admin.ContainsDatabase());
			admin.DropDatabase();
			Assert.IsFalse(admin.ContainsDatabase());

            admin.CreateDatabase();

            string leftTable = "LEFT";
            string createTable = String.Format(
                "CREATE TABLE [{0}].[{1}] ( ID int PRIMARY KEY )"
                , admin.DatabaseOwner
                , leftTable
                );
            admin.ExecuteNonQuery(admin.DatabaseConnectionString,
                createTable
                );
            string rightTable = "RIGHT";
            createTable = String.Format(
                "CREATE TABLE [{0}].[{1}] ( ID int PRIMARY KEY, LEFTID int REFERENCES [{0}].[{2}] )"
                , admin.DatabaseOwner
                , rightTable
                , leftTable
                );
            admin.ExecuteNonQuery(admin.DatabaseConnectionString,
                createTable
                );

            Assert.IsTrue(admin.ContainsTable(leftTable));
            Assert.IsTrue(admin.ContainsTable(rightTable));
            admin.DropConstraints(leftTable);
            admin.DropTable(leftTable);
            admin.DropConstraints(rightTable);
            admin.DropTable(rightTable);
            Assert.IsFalse(admin.ContainsTable(leftTable));
            Assert.IsFalse(admin.ContainsTable(rightTable));

            string tableName= "MYTABLE";
			admin.DropTable(tableName);
			admin.ExecuteNonQuery(
				admin.DatabaseConnectionString
				,"CREATE TABLE [{0}].[{1}] ( ID int  )"
                ,admin.DatabaseOwner
				,tableName
				);
			Assert.IsTrue(admin.ContainsTable(tableName));
			admin.DropTable(tableName);
			Assert.IsFalse(admin.ContainsTable(tableName));
			admin.DropTable(tableName);
			Assert.IsFalse(admin.ContainsTable(tableName));		

            tableName = "UNIQUETABLE";
            createTable = String.Format(
                "CREATE TABLE [{0}].[{1}] ( ID int PRIMARY KEY )"
                , admin.DatabaseOwner
                , tableName
                );

            admin.ExecuteNonQuery(admin.DatabaseConnectionString,
                createTable
                );

            Assert.IsTrue(admin.ContainsTable(tableName));
            admin.DropConstraints(tableName);
            admin.DropTable(tableName);
            Assert.IsFalse(admin.ContainsTable(tableName));

            admin.DropDatabase();

            admin.CreateDatabase();
            Assert.IsTrue(admin.ContainsDatabase());
            tableName = "MYTABLE";
            admin.ExecuteNonQuery(
                admin.DatabaseConnectionString,
                "CREATE TABLE [{0}].[{1}] ( ID int  )"
                ,admin.DatabaseOwner
                ,tableName
                );
            admin.DropDatabase();
            Assert.IsFalse(admin.ContainsDatabase());
        }

        [Test]
        public void BackupNorthwind()
        {
            SqlAdministrator admin = new SqlAdministrator(connectionString, "Northwind");
            Assert.IsTrue(admin.ContainsDatabase());
            admin.BackupDatabase(DbBackupDevice.Disk, @"c:\Backups\Northwind.bsql");
        }

        [Test]
        public void BackupRestore()
        {
            this.admin = new SqlAdministrator(connectionString, "BackupRestoreTest");
            admin.CreateDatabase();
            Assert.IsTrue(admin.ContainsDatabase());
            admin.BackupDatabase(DbBackupDevice.Disk, @"c:\Backups\BackupRestoreTest.bsql");
            admin.DropDatabase();
            Assert.IsFalse(admin.ContainsDatabase());
            admin.RestoreDatabase(DbBackupDevice.Disk, @"c:\Backups\BackupRestoreTest.bsql");
            Assert.IsTrue(admin.ContainsDatabase());
            admin.DropDatabase();
            Assert.IsFalse(admin.ContainsDatabase());
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullConnectionString()
        {
            this.admin = new SqlAdministrator(null, "   ");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullDatabaseName()
        {
            this.admin = new SqlAdministrator("", null);
        }

        [Test]
        public void Constructor()
        {
            this.admin = new SqlAdministrator(connectionString, databaseName);
        }
    }
}
