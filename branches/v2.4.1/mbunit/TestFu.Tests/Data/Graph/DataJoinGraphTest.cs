using System;
using System.Data;
using System.Collections;
using TestFu.Data.Graph;
using QuickGraph.Concepts;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.Graph
{
    [TestFixture]
    public class DataGraphJoinTest
    {
        #region Fields and setup
        private UserOrderProductDataSet dataSource;
        private DataGraph graph;
        private DataJoinGraph joinGraph;

        [SetUp]
        public void SetUp()
        {
            this.dataSource = new UserOrderProductDataSet();
            this.graph = DataGraph.Create(dataSource);
            this.joinGraph = new DataJoinGraph(this.graph);
        }
        #endregion

        [Test]
        public void GetDatabaseGraph()
        {
            Assert.AreEqual(this.graph, this.joinGraph.DataGraph);
        }


        #region DataTable <-> Alias
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDuplicateAlias()
        {
            this.joinGraph.AddVertex(this.dataSource.Users, "U");
            this.joinGraph.AddVertex(this.dataSource.Orders, "U");
        }
        [Test]
        public void ContainsVertexByAlias()
        {
            this.joinGraph.AddVertex(this.dataSource.Users, "U");
            Assert.IsTrue(this.joinGraph.ContainsVertex("U"));
        }
        #endregion

        #region DataRelation
        [Test]
        public void JoinUserAndOrders()
        {
            DataTableJoinVertex users = this.joinGraph.AddVertex(this.dataSource.Users, "U");
            DataTableJoinVertex orders = this.joinGraph.AddVertex(this.dataSource.Orders, "O");

            this.joinGraph.AddEdge(users, orders, JoinType.Inner);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void JoinUserAndProducts()
        {
            DataTableJoinVertex users = this.joinGraph.AddVertex(this.dataSource.Users, "U");
            DataTableJoinVertex products = this.joinGraph.AddVertex(this.dataSource.Products, "P");

            this.joinGraph.AddEdge(users, products, JoinType.Inner);
        }

        #endregion
    }
}
