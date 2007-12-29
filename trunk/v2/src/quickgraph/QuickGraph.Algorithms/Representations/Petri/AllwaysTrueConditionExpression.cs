using System;

namespace QuickGraph.Representations.Petri
{
	using QuickGraph.Concepts.Petri;

	public class AllwaysTrueConditionExpression : IConditionExpression
	{
		#region IConditionExpression Members

		public bool IsEnabled(QuickGraph.Concepts.Petri.Collections.ITokenCollection tokens)
		{
			return true;
		}

		#endregion
	}
}
