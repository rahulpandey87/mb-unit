using System;
using System.Collections;

namespace TestFu.Grammars
{	
	/// <summary>
	/// A grammar containing a set of rules, a <see cref="StartRule"/>.
	/// </summary>
	public class Grammar : RuleBase, IGrammar
	{
		private IRule startRule=null;
		private IProductionFactory productionFactory=new CountedProduction.Factory();
		
		/// <summary>
		/// Creates an empty grammar.
		/// </summary>
		public Grammar()
			:base(false)
		{}

		public event EventHandler ProductionFinished;

		protected virtual void OnProductionFinished()
		{
			if (this.ProductionFinished!=null)
				this.ProductionFinished(this,EventArgs.Empty);
		}

		/// <summary>
		/// Gets or sets the <see cref="IProductionFactory"/> instance.
		/// </summary>
		/// <value>
		/// <see cref="IProductionFactory"/> instance used for creating new
		/// productions.
		/// </value>
		public IProductionFactory ProductionFactory
		{
			get
			{
				return this.productionFactory;
			}
			set
			{
				this.productionFactory=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the starting rule.
		/// </summary>
		/// <value>
		/// The start <see cref="IRule"/>.
		/// </value>
		public IRule StartRule
		{
			get
			{
				return this.startRule;
			}
			set
			{
				this.startRule = value;
			}
		}	
		
		/// <summary>
		/// Launches a production.
		/// </summary>
		public virtual void Produce(Object seed)
		{
			IProduction prod = this.productionFactory.CreateProduction(seed);
			
			try
			{
				this.startRule.Produce(prod.RequestToken(this.startRule));	
				this.OnAction();
			}
			catch(ProductionException)
			{
			}
			finally
			{
				this.OnProductionFinished();
			}
		}		

		/// <summary>
		/// </summary>
		public override void Produce(IProductionToken token)
		{
			this.startRule.Produce(token.Production.RequestToken(this.startRule));
			this.OnAction();
		}
	}
}
