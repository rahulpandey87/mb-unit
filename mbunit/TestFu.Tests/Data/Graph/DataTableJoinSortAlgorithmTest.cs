using System;
using System.Data;
using System.Collections;

using TestFu.Data;
using TestFu.Data.Graph;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.Graph
{
    [TestFixture]
    public class DataTableJoinSortAlgorithmTest
    {
        private UserOrderProductDataSet dataSource;
        private DataJoinGraph graph;
        private DataTableJoinSortAlgorithm algo;

        [SetUp]
        public void SetUp()
        {
            this.dataSource = new UserOrderProductDataSet();
            this.graph = new DataJoinGraph(DataGraph.Create(this.dataSource));
        }

        [Test]
        public void EmptyGraph()
        {
            algo = new DataTableJoinSortAlgorithm(this.graph);
            algo.Compute();

            Assert.IsNull(algo.StartVertex);
            CollectionAssert.AreCountEqual(0, algo.Joins);
        }

        [Test]
        public void UsersOnly()
        {
            DataTableJoinVertex users = this.graph.AddVertex(this.dataSource.Users, "U");

            algo = new DataTableJoinSortAlgorithm(this.graph);
            algo.Compute();

            Assert.IsNotNull(algo.StartVertex);
            Assert.AreEqual(users, algo.StartVertex);
            CollectionAssert.AreCountEqual(0, algo.Joins);
            ShowJoins();
        }

        [Test]
        public void JoinUsersAndOrders()
        {
            DataTableJoinVertex users = this.graph.AddVertex(this.dataSource.Users, "U");
            DataTableJoinVertex orders = this.graph.AddVertex(this.dataSource.Orders, "O");
            DataRelationJoinEdge uo = this.graph.AddEdge(users, orders, JoinType.Inner);

            algo = new DataTableJoinSortAlgorithm(this.graph);
            algo.Compute();

            Assert.IsNotNull(algo.StartVertex);
            Assert.AreEqual(1, algo.Joins.Count);
            ShowJoins();
        }

        [Test]
        public void JoinUsersOrdersProducts()
        {
            DataTableJoinVertex users = this.graph.AddVertex(this.dataSource.Users, "U");
            DataTableJoinVertex orders = this.graph.AddVertex(this.dataSource.Orders, "O");
            DataTableJoinVertex products = this.graph.AddVertex(this.dataSource.Products, "P");
            DataTableJoinVertex orderProducts = this.graph.AddVertex(this.dataSource.OrderProducts, "OP");

            DataRelationJoinEdge pop = this.graph.AddEdge(products, orderProducts, JoinType.Inner);
            DataRelationJoinEdge uo = this.graph.AddEdge(users, orders, JoinType.Inner);
            DataRelationJoinEdge oop = this.graph.AddEdge(orders, orderProducts, JoinType.Inner);

            algo = new DataTableJoinSortAlgorithm(this.graph);
            algo.Compute();

            Assert.IsNotNull(algo.StartVertex);
            Assert.AreEqual(3, algo.Joins.Count);
            ShowJoins();
        }

        protected void ShowJoins()
        {
            DataTableJoinVertex v = this.algo.StartVertex;
            Console.WriteLine("{0} AS {1}",v.Table.TableName, v.Alias);

            foreach (DataRelationJoinEdge edge in algo.Joins)
            {
                DataTableJoinVertex next = null;
                if (edge.Source==v)
                    next = (DataTableJoinVertex)edge.Target;
                else
                    next = (DataTableJoinVertex)edge.Source;

                Console.WriteLine("{0} JOIN {1} AS {2} ON ",
                    edge.JoinType.ToString().ToUpper(),
                    next.Table.TableName,
                    next.Alias
                    );
                bool needAnd=false;
                for (int i = 0; i < edge.Relation.ParentColumns.Length; ++i)
                {
                    if (needAnd)
                        Console.Write(" AND");
                    else
                        needAnd=true;
                    Console.Write(" {0}.{1} = {2}.{3}",
                        edge.Source.Alias,
                        edge.Relation.ParentColumns[i].ColumnName,
                        edge.Target.Alias,
                        edge.Relation.ChildColumns[i].ColumnName
                        );
                }
                Console.WriteLine();

                v = next;
            }
        }
    }
}
