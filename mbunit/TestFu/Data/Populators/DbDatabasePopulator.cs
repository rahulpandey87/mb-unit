using System;
using System.Data;

namespace TestFu.Data.Populators
{
    public abstract class DbDatabasePopulator : DatabasePopulator
    {
        private IDbFactory dbFactory;
        private string connectionString;
        public DbDatabasePopulator(IDbFactory dbFactory, string connectionString)
        {
            if (dbFactory == null)
                throw new ArgumentNullException("dbFactory");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            this.dbFactory = dbFactory;
            this.connectionString = connectionString;
        }

        public IDbFactory DbFactory
        {
            get
            {
                return this.dbFactory;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
        }


        public override void Populate(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException("dataSet");
            base.Populate(dataSet);
            this.PopulateForDatabase();
        }

        protected abstract void PopulateForDatabase();

        public DataRow GetRandomRow(DataTable table)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            // get row count
            int count = this.DbFactory.SelectCount(table, this.ConnectionString);
            // choose random index
            int index = this.Rnd.Next(count);
            // select row by index
            return this.DbFactory.SelectRow(table,this.ConnectionString,index);
        }
    }
}
