using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Uniform random rule selector.
	/// </summary>
	public class RandomRuleSelector : IRuleSelector
	{
		private IRandom random = new TestFu.Grammars.Random();		

		/// <summary>
		/// Gets or sets the random generator
		/// </summary>
		/// <value>
		/// The <see cref="IRandom"/> instance used for random data generation
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null  reference
		/// </exception>
		public IRandom Random
		{
			get
			{
				return this.random;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("random");
				this.random=value;
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
			int index = random.Next(0, rules.Length);
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
		public virtual IRule Select(IRuleCollection rules)
		{
			if (rules==null)
				throw new ArgumentNullException("rules");
			int index = random.Next(0, rules.Count);
			
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
