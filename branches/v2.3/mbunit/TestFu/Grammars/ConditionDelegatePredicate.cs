using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A <see cref="IPredicate"/> instance that executes
	/// a <see cref="ConditionDelegate"/>.
	/// </summary>
	public class ConditionDelegatePredicate : IPredicate
	{
		private ConditionDelegate condition;
		
		/// <summary>
		/// Creates a new instance arounda <see cref="ConditionDelegate"/>
		/// </summary>
		/// <param name="condition">
		/// <see cref="ConditionDelegate"/> to attach.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="condition"/> is a null reference.
		/// </exception>
		public ConditionDelegatePredicate(ConditionDelegate condition)
		{
			if (condition==null)
				throw new ArgumentNullException("condition");
			this.condition = condition;
		}

		/// <summary>
		/// Invokes the <see cref="ConditionDelegate"/> instance 
		/// and returns the result.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public bool Test(IProductionToken token)
		{
			bool b =(bool)this.condition.DynamicInvoke(new Object[]{token});	
			return b;
		}
	}
}
