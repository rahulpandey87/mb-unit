using System;
using System.IO;
using System.Data;
using TestFu.Data.Graph;
using QuickGraph.Algorithms;
using QuickGraph.Collections.Filtered;

namespace TestFu.Data
{
    public abstract class DbGenerator
    {
        private DbAdministratorBase admin;
        private DataSet dataSource;
        private DataGraph graph;


        public DbGenerator(DbAdministratorBase admin, DataSet dataSource)
        {
            this.admin = admin;
            this.dataSource = dataSource;
            this.graph = DataGraph.Create(this.DataSource);
        }

        public DataSet DataSource
        {
            get
            {
                return this.dataSource;
            }
        }

        public DbAdministratorBase Admin
        {
            get
            {
                return this.admin;
            }
        }
        public DataGraph Graph
        {
            get
            {
                return this.graph;
            }
        }

        public void Generate()
        {
            this.GenerateStructure();
            this.GenerateData();
        }

        #region Structure
        public virtual void GenerateStructure()
        {
            this.GenerateTables();
            this.GenerateUniques();
            this.GenerateForeignKeys();
        }

        #region Dropping
        public virtual void DropStructure()
        {
            this.DropConstraints();
            this.DropTables();
        }

        protected virtual void DropConstraints()
        {
            foreach (DataTable table in this.DataSource.Tables)
            {
                this.Admin.DropConstraints(table.TableName);
            }
        }

        protected virtual void DropTables()
        {
            foreach (DataTable table in this.DataSource.Tables)
            {
                this.Admin.DropTable(table.TableName);
            }
        }
        #endregion

        #region Generation
        protected virtual string ColumnToSqlType(DataColumn column)
        {
			if (column.DataType.IsAssignableFrom(typeof(System.Byte[])))
				return "BINARY";
			if (column.DataType.IsAssignableFrom(typeof(System.Boolean)))
				return "BIT";
			if (column.DataType.IsAssignableFrom(typeof(System.Byte)))
				return "BYTE";
			if (column.DataType.IsAssignableFrom(typeof(System.DateTime)))
				return "DATETIME";
			if (column.DataType.IsAssignableFrom(typeof(System.Decimal)))
				return "DECIMAL";
			if (column.DataType.IsAssignableFrom(typeof(System.Double)))
				return "FLOAT";
			if (column.DataType.IsAssignableFrom(typeof(System.Guid)))
				return "Guid";
			if (column.DataType.IsAssignableFrom(typeof(System.Int16)))
				return "SMALLINT";
			if (column.DataType.IsAssignableFrom(typeof(System.Int32)))
				return "INT";
			if (column.DataType.IsAssignableFrom(typeof(System.Int64)))
				return "BIGINT";
			if (column.DataType.IsAssignableFrom(typeof(System.Data.SqlTypes.SqlMoney)))
				return "MONEY";
			if (column.DataType.IsAssignableFrom(typeof(System.Single)))
				return "TINYINT";
			if (column.DataType.IsAssignableFrom(typeof(System.String)))
				return "TEXT";

            throw new Exception();
        }

        protected virtual void GenerateTables()
        {
            foreach (DataTable table in this.DataSource.Tables)
            {
                this.GenerateTable(table);
            }
        }
        protected abstract void GenerateTable(DataTable table);

        protected virtual void GenerateUniques()
        {
            foreach (DataTable table in this.DataSource.Tables)
            {
                foreach (Constraint constraint in table.Constraints)
                {
                    UniqueConstraint unique = constraint as UniqueConstraint;
                    if (unique != null)
                        this.GenerateUnique(unique);
                }
            }
        }
        protected abstract void GenerateUnique(UniqueConstraint unique);

        protected virtual void GenerateForeignKeys()
        {
            foreach (DataTable table in this.DataSource.Tables)
            {
                foreach (Constraint constraint in table.Constraints)
                {
                    ForeignKeyConstraint fk = constraint as ForeignKeyConstraint;
                    if (fk != null)
                        this.GenerateForeignKey(fk);
                }
            }
        }
        protected abstract void GenerateForeignKey(ForeignKeyConstraint fk);
        #endregion
        #endregion

        #region Data
        public virtual void GenerateData()
        {
            foreach (DataTable table in this.Graph.GetSortedTables())
            {
                this.GenerateData(table);
            }
        }
        protected abstract void GenerateData(DataTable table);

        public virtual void DropData()
        {
            DataTable[] tables = this.Graph.GetSortedTables();
            Array.Reverse(tables);

            foreach (DataTable table in this.Graph.GetSortedTables())
            {
                this.DropData(table);
            }
        }
        protected abstract void DropData(DataTable table);

        #endregion 
    }
}
