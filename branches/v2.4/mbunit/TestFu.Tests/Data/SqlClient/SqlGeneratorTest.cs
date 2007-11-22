using System;
using System.Data;

using TestFu.Data.Populators;
using TestFu.Data;
using TestFu.Data.Graph;
using TestFu.Data.SqlClient;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.SqlClient
{
    [TypeFixture(typeof(DataSet))]
    [ProviderFactory(typeof(DataSetFactory), typeof(DataSet))]
    public class SqlGeneratorTest : SqlFixture
    {
        private DataGraph graph;
        private IDatabasePopulator pop;
        private SqlGenerator gen;

        public SqlGeneratorTest()
            :base(
                @"Data Source=GRENIER\MARYLIN;Integrated Security=SSPI",
                @"SqlGenerateTest"
            )
        {}

        [SetUp]
        public void SetUp(DataSet dataSource)
        {
            if (dataSource is NorthwindDataSet)
                Assert.Ignore("Northwind is buggy... because import in VC# is buggy");
            this.Admin.CreateDatabase();

            dataSource.EnforceConstraints = true;
            this.graph = DataGraph.Create(dataSource);

            this.pop = new DatabasePopulator();
            this.pop.Populate(dataSource);

            // add one row data per table
            foreach (DataTable table in this.graph.GetSortedTables())
            {
                ITablePopulator tp = this.pop.Tables[table];
                tp.Generate();
                table.Rows.Add(tp.Row);
                table.AcceptChanges();
            }

            this.gen = new SqlGenerator(
                this.ConnectionString,
                this.DatabaseName,
                dataSource
                );
        }

        [Test]
        public void GenerateStructureAndVerifyTables(DataSet dataSource)
        {
            gen.DropStructure();
            gen.GenerateStructure();

            // verify structure
            foreach (DataTable table in dataSource.Tables)
            {
                bool containsTable = this.Admin.ContainsTable(table.TableName);
                Assert.IsTrue(containsTable,
                    "Database does not contain table {0}", table.TableName);
            }
        }

        [Test]
        public void GenerateStructureAndVerifyContraints(DataSet dataSource)
        {
            gen.GenerateStructure();

            // verify structure
            foreach (DataRelation relation in dataSource.Relations)
            {
                string name = relation.ChildKeyConstraint.ConstraintName;
                bool contains = this.Admin.ContainsConstraint(name);
                Assert.IsTrue(contains,
                    "Database does not contain constraint {0}", name);

                name = relation.ParentKeyConstraint.ConstraintName;
                contains = this.Admin.ContainsConstraint(name);
                Assert.IsTrue(contains,
                    "Database does not contain constraint {0}", name);
            }
        }

        [TearDown]
        public void TearDown(DataSet dataSource)
        {
            gen.DropStructure();
        }
    }
}
