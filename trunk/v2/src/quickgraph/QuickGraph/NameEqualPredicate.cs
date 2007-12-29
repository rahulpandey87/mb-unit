using System;

namespace QuickGraph
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// Summary description for NameEqualVertexPredicate.
	/// </summary>
	public class NameEqualPredicate : IVertexPredicate, IEdgePredicate
	{
		private string name;
		public NameEqualPredicate(string name)
		{
			if (name==null)
				throw new ArgumentNullException("name");
			this.name = name;
		}
		#region IVertexPredicate Members

		public bool Test(IVertex v)
		{
			NamedVertex nv = v as NamedVertex;
			if (nv==null)
				return false;
			return nv.Name == name;
		}

		#endregion

		#region IEdgePredicate Members

		public bool Test(IEdge e)
		{
			NamedEdge ne = e as NamedEdge;
			if (ne==null)
				return false;
			return ne.Name == name;
		}

		#endregion
	}
}
