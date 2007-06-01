using System;
using System.Data;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using TestFu.Data;
using TestFu.Data.Populators;

namespace TestFu.Tests.Data.Populators
{
	[TestFixture]
	public class DatabasePopulatorTest
	{
		private UserOrderProductDataSet db;
		private DatabasePopulator pop;
		private ITablePopulator users;
		private ITablePopulator orders;
		private ITablePopulator products;
		private ITablePopulator orderProducts;
	
		[SetUp]
		public void SetUp()
		{
            this.db = new UserOrderProductDataSet();
            this.pop = new DatabasePopulator();
			this.pop.Populate(this.db);

			this.users=this.pop.Tables[this.db.Tables["Users"]];
			this.orders=this.pop.Tables[this.db.Tables["Orders"]];
			this.products=this.pop.Tables[this.db.Tables["Products"]];
			this.orderProducts=this.pop.Tables[this.db.Tables["OrderProducts"]];
		}

		[Test]
		public void AddOneUser()
		{
			DataRow row = users.Generate();
			this.db.Users.Rows.Add(row);
		}

		[Test]
		public void AddOneUserOneOrder()
		{
			this.AddOneUser();
			DataRow row = orders.Generate();
			this.db.Orders.Rows.Add(row);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddOneOrder()
		{
			DataRow row = orders.Generate();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddOneOrderProduct()
		{
			DataRow row = orderProducts.Generate();
		}

		[Test]
		public void AddOneUserOneOrderOneProduct()
		{
			this.AddOneUserOneOrder();

			DataRow row = products.Generate();
			this.db.Products.Rows.Add(row);
		}

		[Test]
		public void AddOneUserOneOrderOneProductOneProductOrder()
		{
			this.AddOneUserOneOrderOneProduct();
			DataRow row=orderProducts.Generate();
			this.db.OrderProducts.Rows.Add(row);
		}

		[Test]
		public void AddTwoUsers()
		{
			AddOneUser();
			AddOneUser();
		}

		[TearDown]
		public void Check()
		{
			this.db.AcceptChanges();
			Console.WriteLine(db.ToString());
		}
	}
}
