using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.CodeDom.Compiler;
using System.Collections;

namespace TestFu.Data.SqlClient
{
    public class SqlGenerator : DbGenerator
    {
        public SqlGenerator(
            String connectionString,
            string databaseName,
            DataSet dataSource
            )
            :base(new SqlAdministrator(connectionString,databaseName),dataSource)
        {
        }

        public string ConnectionString
        {
            get
            {
                return String.Format(@"{0};Initial Catalog={1}",
                    this.Admin.ConnectionString,
                    this.Admin.DatabaseName);
            }
        }

        protected override void GenerateTable(System.Data.DataTable table)
        {
            StringWriter sw = new StringWriter();
            IndentedTextWriter tw = new IndentedTextWriter(sw);
            tw.WriteLine("CREATE TABLE {0} (",
                formatTableName(table)
                );

            tw.Indent++;
            bool comma = false;
            foreach (DataColumn column in table.Columns)
            {
                if (comma)
                    tw.WriteLine(",");
                else
                    comma = true;
                GenerateColumn(column, tw);
            }
            tw.WriteLine(")");
            tw.Indent--;
            tw.Flush();

            this.Admin.ExecuteNonQuery(ConnectionString,sw.ToString());
        }

        protected virtual void GenerateColumn(DataColumn column, TextWriter writer)
        {
            writer.Write("{0} {1} {2}",
                column.ColumnName,
                this.ColumnToSqlType(column),
                (column.AllowDBNull) ? "NULL" : "NOT NULL"
                );
        }

        protected override void GenerateUnique(System.Data.UniqueConstraint unique)
        {
            StringWriter tw = new StringWriter();
            tw.WriteLine("ALTER TABLE {0} ADD CONSTRAINT {1} {2} (",
                formatTableName(unique.Table),
                unique.ConstraintName,
                (unique.IsPrimaryKey) ? "PRIMARY KEY" : "UNIQUE"
                );

            GenerateColumnList(tw, unique.Columns);
            this.Admin.ExecuteNonQuery(ConnectionString, tw.ToString());
        }

        protected override void GenerateForeignKey(System.Data.ForeignKeyConstraint fk)
        {
            StringWriter tw = new StringWriter();
            tw.WriteLine("ALTER TABLE {0} ADD CONSTRAINT {1} FOREIGN KEY (",
                formatTableName(fk.Table),
                fk.ConstraintName
                );

            GenerateColumnList(tw, fk.Columns);

            tw.WriteLine("REFERENCES {0} (",
                formatTableName(fk.RelatedTable)
                );

            GenerateColumnList(tw, fk.RelatedColumns);

            if (fk.DeleteRule == Rule.Cascade)
                tw.WriteLine("ON DELETE CASCADE");
            else if (fk.DeleteRule == Rule.None)
                tw.WriteLine("ON DELETE NO ACTION");
            if (fk.UpdateRule == Rule.Cascade)
                tw.WriteLine("ON UPDATE CASCADE");
            else if (fk.UpdateRule == Rule.None)
                tw.WriteLine("ON UPDATE NO ACTION");
            this.Admin.ExecuteNonQuery(ConnectionString, tw.ToString());
        }

        protected void GenerateColumnList(TextWriter tw, IEnumerable columns)
        {
            tw.Write("\t");
            bool comma = false;
            foreach (DataColumn column in columns)
            {
                if (comma)
                    tw.Write(',');
                else
                    comma = true;
                tw.Write(column.ColumnName);
            }
            tw.WriteLine(")");
        }

        protected override void GenerateData(DataTable table)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("INSERT {0} (",
                formatTableName(table));

            this.GenerateColumnList(sw, table.Columns);
            sw.Write("VALUES (");

            Console.WriteLine(sw.ToString());
            string query = sw.ToString() + "{0})";

            foreach (DataRow row in table.Rows)
            {
                string insert = GenerateRow(row);
                Console.WriteLine(insert);
                this.Admin.ExecuteNonQuery(this.ConnectionString, query, insert);
            }
        }

        protected string GenerateRow(DataRow row)
        {
            StringWriter sw = new StringWriter();
            foreach (Object value in row.ItemArray)
            {
                if (value is DBNull)
                    sw.Write("NULL,");
                else
                    sw.Write("{0},", value);
            }
            return sw.ToString().Trim(',');
        }

        protected override void DropData(DataTable table)
        {
            String query = "DELETE * FROM TABLE {0}";
            this.Admin.ExecuteNonQuery(this.ConnectionString,query,formatTableName(table));
        }

        private string formatTableName(DataTable table)
        {
            return string.Format("[{0}].[{1}]", this.Admin.DatabaseOwner, table.TableName);
        }
    }
}