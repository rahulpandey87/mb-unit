using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A rule that executes a <see cref="ProductionTokenDelegate"/>.
	/// </summary>	
	public class ProductionTokenDelegateRule : RuleBase
	{
		private ProductionTokenDelegate productionTokenDelegate;
		
		/// <summary>
		/// Creates a new instance around a <see cref="ProductionTokenDelegate"/>
		/// </summary>
        /// <param name="productionTokenDelegate">
		/// <see cref="ProductionTokenDelegateRule"/> to attach.
		/// </param>
		/// <exception cref="ArgumentNullException">
        /// <paramref name="productionTokenDelegate"/> is a null reference.
		/// </exception>
		public ProductionTokenDelegateRule(ProductionTokenDelegate productionTokenDelegate)
			:base(true)
		{
			if (productionTokenDelegate==null)
				throw new ArgumentNullException("productionTokenDelegate");
			this.productionTokenDelegate=productionTokenDelegate;
			this.Name = productionTokenDelegate.Method.Name;
		}
		
		/// <summary>
		/// Invokes the <see cref="ProductionTokenDelegateRule"/> instance.
		/// </summary>
		/// <param name="token">
		/// Autorization token
		/// </param>
		public override void Produce(IProductionToken token)
		{
			this.productionTokenDelegate(token);
			this.OnAction();
		}
	}
}
