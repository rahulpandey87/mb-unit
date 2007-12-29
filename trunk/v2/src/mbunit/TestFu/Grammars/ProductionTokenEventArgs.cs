using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Summary description for ProductionTokenEventArgs.
	/// </summary>
	public class ProductionTokenEventArgs : EventArgs
	{
		private IProductionToken token;
		public ProductionTokenEventArgs(IProductionToken token)
		{
			if (token==null)
				throw new ArgumentNullException("token");
			this.token=token;
		}

		public IProductionToken Token
		{
			get
			{
				return this.token;
			}
		}
	}
}
