using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A static helper class for creating <see cref="IPredicate"/>.
	/// </summary>
	public sealed class Predicates
	{
		private Predicates(){}
		
		/// <summary>
		/// Creates a <see cref="ConditionDelegatePredicate"/> around
		/// <paramref name="condition"/>
		/// </summary>
		/// <param name="condition">
		/// condition to wrap</param>
		/// <returns>
		/// A <see cref="ConditionDelegatePredicate"/>
		/// </returns>
		public static ConditionDelegatePredicate If(ConditionDelegate condition)
		{
			return new ConditionDelegatePredicate(condition);
		}
	}
}
