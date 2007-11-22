using System;
using System.Collections;

namespace TestFu.Grammars
{
	/// <summary>
	/// Weighted random rule selector.
	/// </summary>
	public class WeightedRandomRuleSelector : RandomRuleSelector
	{
		/// <summary>
		/// Select a <see cref="IRule"/> from <paramref name="rules"/>
		/// </summary>
		/// <param name="rules">
		/// <see cref="IRule"/> array to select from
		/// </param>
		/// <returns>
		/// Select <see cref="IRule"/> instance
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="rules"/> is a null reference
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="rules"/> is empty
		/// </exception>
		public override IRule Select(params IRule[] rules)
		{
			if (rules==null)
				throw new ArgumentNullException("rules");
			if (rules.Length==0)
				throw new ArgumentException("rules is empty");
			
			// get rules weight
			double totalWeight = GetTotalWeight(rules);			
			// get index inside the wieght
			double index = this.Random.NextDouble()*totalWeight;
			// find which rules is targeted
			return GetRuleByWeightIndex(index,rules);
		}
		
		/// <summary>
		/// Select a <see cref="IRule"/> from <paramref name="rules"/>
		/// </summary>
		/// <param name="rules">
		/// <see cref="IRule"/> collection to select from
		/// </param>
		/// <returns>
		/// Select <see cref="IRule"/> instance
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="rules"/> is a null reference
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="rules"/> is empty
		/// </exception>
		public override IRule Select(IRuleCollection rules)
		{
			if (rules==null)
				throw new ArgumentNullException("rules");
			if (rules.Count==0)
				throw new ArgumentException("rules is empty");
			
			// get rules weight
			double totalWeight = GetTotalWeight(rules);			
			// get index inside the wieght
			double index = this.Random.NextDouble()*totalWeight;
			// find which rules is targeted
			return GetRuleByWeightIndex(index,rules);
		}
		
		public double GetTotalWeight(ICollection rules)
		{
			double tw = 0;
			foreach(IRule rule in rules)
				tw+= rule.Weight;
			return tw;
		}
		
		public IRule GetRuleByWeightIndex(double index, ICollection rules)
		{
			double currentWeight=0;
			foreach(IRule rule in rules)
			{
				if (index <= currentWeight + rule.Weight)
					return rule;
				currentWeight+=rule.Weight;
			}
			throw new Exception();
		}
	}

}
