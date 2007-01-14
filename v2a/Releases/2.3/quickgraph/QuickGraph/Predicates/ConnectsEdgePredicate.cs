using System;

namespace QuickGraph.Predicates
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// A predicate that filter edge connecting two vertices
	/// </summary>
	public class ConnectsEdgePredicate  : IEdgePredicate
	{
		private IVertex source;
		private IVertex target;
		private IGraph graph;

		/// <summary>
		/// Constructs a new predicate
		/// </summary>
		/// <param name="src">source vertex</param>
		/// <param name="trg">target vertex</param>
		/// <param name="g">underlying graph</param>
		public ConnectsEdgePredicate(IVertex src, IVertex trg, IGraph g)
		{
			if (src==null)
				throw new ArgumentNullException("src");
			if (trg==null)
				throw new ArgumentNullException("trg");
			if (g==null)
				throw new ArgumentNullException("g");

			this.source = src;
			this.target = trg;
			this.graph = g;
		}

		/// <summary>
		/// Test if edge connects source and target vertex
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>true if e connects source and target</returns>
		public bool Test(IEdge e)
		{
			if (graph.IsDirected)
				return e.Source == source && e.Target == target;
			else
				return (e.Source == source && e.Target == target)
					||(e.Source == target && e.Target == source);
		}
	}
}
