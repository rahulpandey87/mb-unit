using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Method that returns a bool.
	/// </summary>
	/// <param name="token">
	/// Current <see cref="IProductionToken"/> instance.
	/// </param>
	public delegate bool ConditionDelegate(IProductionToken token);	
}
