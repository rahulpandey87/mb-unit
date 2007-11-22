using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Default implementation of <see cref="IProductionToken"/>
	/// </summary>
	public class ProductionToken : IProductionToken
	{
		private bool authorized=true;
		private IProduction production;

		/// <summary>
		/// Creates a token from <paramref name="production"/>
		/// </summary>
		/// <param name="production">
		/// production to wrap.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="production"/> is a null reference (Nothing in 
		/// Visual Basic).
		/// </exception>
		public ProductionToken(IProduction production)
		{
			if (production==null)
				throw new ArgumentNullException("production");
			this.production=production;
		}
		
		/// <summary>
		/// Gets the <see cref="IProduction"/> that emited the token.
		/// </summary>
		/// <value>
		/// The <see cref="IProduction"/> instance that emited the token.
		/// </value>
		public IProduction Production
		{
			get
			{
				return this.production;
			}
		}
		
		/// <summary>
		/// Gets a value indicating if the production is authorized
		/// </summary>
		/// <value>
		/// true if authorized, otherwise false.
		/// </value>
		public bool Authorized
		{
			get
			{
				return this.authorized;
			}
		}
	}
}
