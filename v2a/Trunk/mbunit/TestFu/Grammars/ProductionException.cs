using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Expection class used to stop production.
	/// </summary>
	public class ProductionException : Exception
	{
		private IProduction production;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="production"></param>
		public ProductionException(IProduction production)
		{
			this.production = production;
		}
		
		/// <summary>
		/// Gets the production that stopped.
		/// </summary>
		public IProduction Production
		{
			get
			{
				return this.production;
			}
		}
	}
}
