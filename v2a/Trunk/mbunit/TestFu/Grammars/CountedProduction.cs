using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A <see cref="IProduction"/> class that limits the number of
	/// terminal <see cref="IRule"/> execution.
	/// </summary>
	public class CountedProduction : IProduction
	{
		private int maxTokenCount;
		private int tokenCount=0;

		/// <summary>
		/// Creates an instance that limits the number of terminal rule execution
		/// to <paramref name="maxTokenCount"/>.
		/// </summary>
		/// <param name="maxTokenCount">
		/// Maximum number of terminal <see cref="IRule"/> execution.
		/// </param>
		public CountedProduction(int maxTokenCount)
		{
			this.maxTokenCount=maxTokenCount;
		}

		Object IProduction.Seed
		{
			get
			{
				return this.maxTokenCount;
			}
		}
		
		/// <summary>
		/// Processes the request for a <see cref="IProductionToken"/>
		/// done by a rule and returns the token or throws.
		/// </summary>
		/// <param name="rule">
		/// <see cref="IRule"/> instance that requests the token.
		/// </param>
		/// <returns>
		/// A valid <see cref="IProductionToken"/> instance.
		/// </returns>
		/// <exception cref="ProductionException">
		/// The maximum number of terminal rule execution was hitted.
		/// </exception>
		public IProductionToken RequestToken(IRule rule)
		{
			if (rule.Terminal)
			{
				if (tokenCount>=this.maxTokenCount)
					throw new ProductionException(this);
				this.tokenCount++;			
			}
			return new ProductionToken(this);
		}	
	
		/// <summary>
		/// Factory for <see cref="CountedProduction"/> instance.
 		/// </summary>
		public class Factory : IProductionFactory
		{
			/// <summary>
			/// Creates a factory of <see cref="CountedProduction"/>.
			/// </summary>
			public Factory()
			{}			

			/// <summary>
			/// Creates new instances of <see cref="CountedProduction"/>
			/// </summary>
			/// <returns>
			/// A <see cref="CountedProduction"/> instance
			/// </returns>
			public IProduction CreateProduction(Object seed)
			{
				if (!seed.GetType().IsAssignableFrom(typeof(int)))
					throw new ArgumentException("Seed should be a int");
				return new CountedProduction((int)seed);	
			}
		}		
	}
}
