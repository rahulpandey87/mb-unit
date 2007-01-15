using System;
using System.Collections;

namespace TestFu.Grammars
{
	/// <summary>
	/// Round Robin rule selector.
	/// </summary>
	public class RoundRobinRuleSelector : IRuleSelector
	{
		private int index=-1;
		
		/// <summary>
		/// Gets or sets the current rule index.
		/// </summary>
		/// <value>
		/// Current rule index
		/// </value>
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index=value;
			}
		}
		
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
		public virtual IRule Select(params IRule[] rules)
		{
			if (rules==null)
				throw new ArgumentNullException("rules");
			if (rules.Length==0)
				throw new ArgumentException("rules is empty");
			
			this.index = Math.Max(0, (this.index++) % rules.Length);
			return rules[index];
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
		public IRule Select(IRuleCollection rules)
		{
			if (rules==null)
				throw new ArgumentNullException("rules");
			if (rules.Count==0)
				throw new ArgumentException("rules is empty");
			
			this.index = Math.Max(0, (this.index++) % rules.Count);
			int i =0;
			foreach(IRule r in rules)
			{
				if (i==index)
					return r;
				++i;
			}
			throw new Exception();		
		}
	}

}
