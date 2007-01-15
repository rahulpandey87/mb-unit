using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using TestFu.Data.SqlClient;

namespace TestFu.Data.Populators
{
    public class SqlCommandUniqueValidator : DbCommandUniqueValidatorBase
    {
        public SqlCommandUniqueValidator(
            ITablePopulator table, 
            UniqueConstraint unique,
            string connectionString
            )
            :base(table,unique,new SqlFactory(),connectionString)
        {}

        public override string GetSelectKeyQuery()
        {
            StringWriter sw = new StringWriter();
            sw.Write("SELECT ");
            for(int i = 0;i<this.Unique.Columns.Length;++i)
            {
                DataColumn column = this.Unique.Columns[i];
                if (i!=0)
                    sw.Write(", ");

                sw.Write("{0}", column.ColumnName);
            }
            sw.Write(" FROM [{0}]", this.Table.Table.TableName);

            return sw.ToString();
        }

        public override string GetSelectCountByKeyQuery(DataRow row)
        {
            StringWriter sw = new StringWriter();
            sw.Write("SELECT COUNT(*) FROM [{0}] WHERE ", this.Table.Table.TableName);
            for (int i = 0; i < this.Unique.Columns.Length; ++i)
            {
                DataColumn column = this.Unique.Columns[i];
                if (i != 0)
                    sw.Write(" AND ");

                if (row[column] is DBNull)
                {
                    sw.Write("{0} IS NULL", column.ColumnName, row[column]);
                    continue;
                }

                if (column.DataType == typeof(string))
                {
                    sw.Write("{0} = '{1}'", column.ColumnName, row[column]);
                    continue;
                }

                sw.Write("{0} = {1}", column.ColumnName, row[column]);
            }

            return sw.ToString();
        }
    }
}
