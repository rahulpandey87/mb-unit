using System.Data;
using System;
using TestFu.Data.SqlClient;

namespace TestFu.Data.Populators
{
    public class SqlDatabasePopulator : DbDatabasePopulator
    {
        public SqlDatabasePopulator(string connectionString)
            :base(new SqlFactory(), connectionString)
        {}

        protected override void PopulateForDatabase()
        {
            foreach (ITablePopulator table in this.Tables)
            {
                // store uniques in temporary place
                IUniqueValidator[] uniques = new IUniqueValidator[table.Uniques.Count];
                table.Uniques.CopyTo(uniques, 0);

                // replace each unique validator by a SqlCommandUniqueValidator
                foreach (IUniqueValidator unique in uniques)
                {
                    // remove old unique
                    table.Uniques.Remove(unique);

                    // create new uniquevalidator
                    SqlCommandUniqueValidator sqlUnique =
                        new SqlCommandUniqueValidator(table, unique.Unique, this.ConnectionString);

                    // looking if autoincrement
                    if (unique.Unique.IsPrimaryKey && unique.Unique.Columns.Length == 1 && unique.Unique.Columns[0].AutoIncrement)
                        sqlUnique.IsIdentity = true;

                    table.Uniques.Add(sqlUnique);
                }
            }
        }
    }
}
