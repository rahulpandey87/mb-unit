using System;
using System.Collections;
namespace QuickGraph.Predicates
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Predicates;
    using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// Static helper class for creating predicates
	/// </summary>
	/// <remarks>
	/// This lets you quickly use the built-in predicates of QuickGraph.
	/// </remarks>
	public sealed class Preds
	{
		private Preds()
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="g"></param>
		/// <returns></returns>
		public static ConnectsEdgePredicate Connects(
			IVertex source,
			IVertex target,
			IGraph g)
		{
			return new ConnectsEdgePredicate(source,target,g);
		}

		/// <summary>
		/// Returns a edge predicate that always returns true.
		/// </summary>
		/// <returns></returns>
		public static KeepAllEdgesPredicate KeepAllEdges()
		{
			return new KeepAllEdgesPredicate();
		}

		/// <summary>
		/// Returns a vertex predicate that always returns true.
		/// </summary>
		/// <returns></returns>
		public static KeepAllVerticesPredicate KeepAllVertices()
		{
			return new KeepAllVerticesPredicate();
		}

		/// <summary>
		/// Checks ep(e) &amp;&amp; vp(e.Source) &amp;&amp; vp(e.Target)
		/// </summary>
		/// <param name="ep">predicate to apply to edge</param>
		/// <param name="vp">predicate to apply to edge source and target</param>
		/// <returns></returns>
		public static EdgePredicate Edge(IEdgePredicate ep, IVertexPredicate vp)
		{
			return new EdgePredicate(ep,vp);
		}

		/// <summary>
		/// Creates a predicate that check the edge and the edge source
		/// </summary>
		/// <param name="ep">edge predicate to apply to the edge</param>
		/// <param name="vp">vertex predicate to apply to the edge source</param>
		/// <returns>in-edge predicate</returns>
		public static InEdgePredicate InEdge(IEdgePredicate ep, IVertexPredicate vp)
		{
			return new InEdgePredicate(ep,vp);
		}

		/// <summary>
		/// Creates a predicate that check the edge and the edge target
		/// </summary>
		/// <param name="ep">edge predicate to apply to the edge</param>
		/// <param name="vp">vertex predicate to apply to the edge target</param>
		/// <returns>out-edge predicate</returns>
		public static OutEdgePredicate OutEdge(IEdgePredicate ep, IVertexPredicate vp)
		{
			return new OutEdgePredicate(ep,vp);
		}

		/// <summary>
		/// Creates a predicate that checks wheter an edge is adjacent to a
		/// given vertex.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>is adjacent predicate</returns>
		public static IsAdjacentEdgePredicate IsAdjacent(IVertex v)
		{
			return new IsAdjacentEdgePredicate(v);
		}

		/// <summary>
		/// Creates a predicate that checks if an edge is an in-edge of 
		/// a vertex.
		/// </summary>
		/// <param name="v">vertex to check</param>
		/// <returns>in-edge predicate</returns>
		public static IsInEdgePredicate IsInEdge(IVertex v)
		{
			return new IsInEdgePredicate(v);
		}

		/// <summary>
		/// Creates a predicate that checks if an edge is an out-edge of 
		/// a vertex.
		/// </summary>
		/// <param name="v">vertex to check</param>
		/// <returns>out-edge predicate</returns>
		public static IsOutEdgePredicate IsOutEge(IVertex v)
		{
			return new IsOutEdgePredicate(v);
		}

		/// <summary>
		/// Check if a vertex is equal to v
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>predicate</returns>
		public static VertexEqualPredicate Equal(IVertex v)
		{
			return new VertexEqualPredicate(v);
		}

		/// <summary>
		/// Check if vertex is in list
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static InCollectionVertexPredicate InCollection(IVertexCollection list)
		{
			return new InCollectionVertexPredicate(list);
		}

		/// <summary>
		/// Negates a predicate
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static NotVertexPredicate Not(IVertexPredicate predicate)
		{
			return new NotVertexPredicate(predicate);
		}

        /// <summary>
        /// Source vertex prodicate
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static SourceVertexPredicate SourceVertex(IBidirectionalGraph graph)
        {
            return new SourceVertexPredicate(graph);
        }

        public static SinkVertexPredicate SinkVertex(IImplicitGraph graph)
        {
            return new SinkVertexPredicate(graph);
        }
    }
}
