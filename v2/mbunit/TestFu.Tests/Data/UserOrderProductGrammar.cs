using System;
using System.Collections;
using System.Data;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using TestFu.Grammars;
using TestFu.Data;
using TestFu.Data.Collections;
using TestFu.Data.Populators;

namespace TestFu.Tests.Data
{
	using TestFu.Tests.Data.Generators;

	[GrammarFixture]
	public class UserOrderProductGrammar : Grammar
	{
        private UserOrderProductDataSet db;
        private IDatabasePopulator pop;
	
		private IRule addUser,addOrder,addProduct,addOrderProduct;
		private IRule guardAddOrder,guardAddOrderProduct;

		private IRule init;

		public UserOrderProductGrammar()
		{
            this.db = new UserOrderProductDataSet();
            this.pop = new DatabasePopulator();
			this.pop.Populate(this.db);

			this.ProductionFinished+=new EventHandler(UserOrderProductGrammar_ProductionFinished);

			// create rules
			this.addUser = Rules.Method(new MethodInvoker(this.AddUser));
			this.addOrder = Rules.Method(new MethodInvoker(this.AddOrder));
			this.addProduct = Rules.Method(new MethodInvoker(this.AddProduct));
			this.addOrderProduct = Rules.Method(new MethodInvoker(this.AddOrderProduct));

			// guarded rules
			this.guardAddOrder = Rules.Guard( this.addUser, typeof(InvalidOperationException) ); 
			this.guardAddOrderProduct = Rules.Guard( this.addOrderProduct, typeof(InvalidOperationException) ); 

			this.init = Rules.Seq(this.addUser, this.addProduct, this.addOrder);

			this.StartRule = Rules.Seq(this.init,
				Rules.Kleene(
					Rules.Alt(
							this.addUser,
							this.addOrder,
							this.addProduct,
							this.addOrderProduct
						)
					)
				);
		}

		#region Predicates
		public bool IsUsersEmpty(IProductionToken token)
		{
			return this.pop.Tables[this.db.Users].Uniques.PrimaryKey.IsEmpty;
		}
		public bool IsProductsEmpty(IProductionToken token)
		{
			return this.pop.Tables[this.db.Products].Uniques.PrimaryKey.IsEmpty;
		}
		public bool IsOrdersEmpty(IProductionToken token)
		{
			return this.pop.Tables[this.db.Orders].Uniques.PrimaryKey.IsEmpty;
		}
		public bool IsOrderAndProductEmpty(IProductionToken token)
		{
			return IsOrdersEmpty(token)&&IsProductsEmpty(token);
		}
		#endregion

		#region Rules
		public void AddUser()
		{
			ITablePopulator users = this.pop.Tables[this.db.Users];
			this.db.Users.Rows.Add( users.Generate() );
			Console.WriteLine("AddUser");
			this.Check();
		}
		public void AddProduct()
		{
			ITablePopulator products = this.pop.Tables[this.db.Products];
			this.db.Products.Rows.Add( products.Generate() );
			Console.WriteLine("AddProduct");
			this.Check();
		}
		public void AddOrder()
		{
			ITablePopulator orders = this.pop.Tables[this.db.Orders];
			this.db.Orders.Rows.Add( orders.Generate() );
			Console.WriteLine("AddOrder");
			this.Check();
		}
		public void AddOrderProduct()
		{
			ITablePopulator orderProducts = this.pop.Tables[this.db.OrderProducts];
			this.db.OrderProducts.Rows.Add( orderProducts.Generate() );

			Console.WriteLine("AddOrderProduct");
			this.Check();
		}
		#endregion

		public void Check()
		{
			this.db.AcceptChanges();
		}

		[Grammar]
		public UserOrderProductGrammar This()
		{
			return this;
		}
		[Seed]
		public int Seed10()
		{
			return 10;
		}
		[Seed]
		public int Seed100()
		{
			return 100;
		}
		private void UserOrderProductGrammar_ProductionFinished(object sender, EventArgs e)
		{
			Console.WriteLine(db.ToString());
		}
	}
}
