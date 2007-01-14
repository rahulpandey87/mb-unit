using System;

namespace QuickGraph.Representations.Petri
{
	using QuickGraph.Concepts.Petri;
	using QuickGraph.Concepts.Petri.Collections;

	public class IdentityExpression  :IExpression
	{
		#region IExpression Members

		public ITokenCollection Eval(ITokenCollection marking)
		{
			return marking;
		}

		#endregion
	}
}
