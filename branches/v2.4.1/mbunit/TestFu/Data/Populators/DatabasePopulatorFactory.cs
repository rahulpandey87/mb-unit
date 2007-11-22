using System;
using System.Data;

namespace TestFu.Data.Populators
{
    public sealed class DatabasePopulatorFactory
    {
        private DatabasePopulatorFactory()
        {}

        public static DatabasePopulator ForDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException("dataSet");
            DatabasePopulator pop = new DatabasePopulator();
            pop.Populate(dataSet);

            return pop;
        }

        public static SqlDatabasePopulator ForSql(DataSet dataSet, string connectionString)
        {
            if (dataSet == null)
                throw new ArgumentNullException("dataSet");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            SqlDatabasePopulator pop = new SqlDatabasePopulator(connectionString);
            pop.Populate(dataSet);
            return pop;
        }
    }
}
