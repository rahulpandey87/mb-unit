using System;

namespace QuickGraph.Concepts.Traversals
{
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// A small helper class for traversals
	/// </summary>
	public sealed class Traversal
	{
		private Traversal()
		{}

		/// <summary>
		/// Returns the first vertex of the enumerable that matches the predicate.
		/// </summary>
		/// <param name="vertices">enumerable collection of <see cref="IVertex"/></param>
		/// <param name="pred">vertex predicate</param>
		/// <returns>first vertex if any, otherwise a null reference</returns>
		public static IVertex FirstVertexIf(IVertexEnumerable vertices, IVertexPredicate pred)
		{
			if (vertices==null)
				throw new ArgumentNullException("vertices");
			if (pred==null)
				throw new ArgumentNullException("pred");

			IVertexEnumerator en = vertices.GetEnumerator();
			while(en.MoveNext())
			{
				if (pred.Test(en.Current))
					return en.Current;
			}
			return null;
		}

		/// <summary>
		/// Returns the first vertex of the enumerable
		/// </summary>
		/// <param name="vertices">enumerable collection of <see cref="IVertex"/></param>
		/// <returns>first vertex if any, otherwise a null reference</returns>
		public static IVertex FirstVertex(IVertexEnumerable vertices)
		{
			if (vertices==null)
				throw new ArgumentNullException("vertices");
			IVertexEnumerator en = vertices.GetEnumerator();
			if (!en.MoveNext())
				return null;
			else
				return en.Current;
		}

		/// <summary>
		/// Returns the first vertex of the graph
		/// </summary>
		/// <param name="g">graph</param>
		/// <returns>first vertex if any, otherwise a null reference</returns>
		public static IVertex FirstVertex(IVertexListGraph g)
		{
			return FirstVertex(g.Vertices);
		}

		/// <summary>
		/// Returns the first vertex of the enumerable
		/// </summary>
		/// <param name="vertices">enumerable collection of <see cref="IVertex"/></param>
		/// <returns>first vertex if any, otherwise a null reference</returns>
		public static IVertex LastVertex(IVertexEnumerable vertices)
		{
			if (vertices==null)
				throw new ArgumentNullException("vertices");
			IVertexEnumerator en = vertices.GetEnumerator();
			IVertex current = null;
			while(en.MoveNext())
			{
				current = en.Current;
			}
			return current;
		}


		/// <summary>
		/// Returns the last vertex of the graph
		/// </summary>
		/// <param name="g">graph</param>
		/// <returns>last vertex if any, otherwise a null reference</returns>
		public static IVertex LastVertex(IVertexListGraph g)
		{
			return LastVertex(g.Vertices);
		}


		/// <summary>
		/// Returns the first edge of the graph
		/// </summary>
		/// <param name="edges">graph</param>
		/// <returns>first edge if any, otherwise a null reference</returns>
		public static IEdge FirstEdge(IEdgeEnumerable edges)
		{
			if (edges==null)
				throw new ArgumentNullException("edges");
			IEdgeEnumerator en = edges.GetEnumerator();
			if (!en.MoveNext())
				return null;
			else
				return en.Current;
		}

		/// <summary>
		/// Returns the first edge of the graph
		/// </summary>
		/// <param name="g">graph</param>
		/// <returns>first edge if any, otherwise a null reference</returns>
		public static IEdge FirstEdge(IEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			return FirstEdge(g.Edges);
		}

		/// <summary>
		/// Returns the last edge of the edge collection
		/// </summary>
		/// <param name="edges">edge collection</param>
		/// <returns>last edge if any, otherwise a null reference</returns>
		public static IEdge LastEdge(IEdgeEnumerable edges)
		{
			if (edges==null)
				throw new ArgumentNullException("edges");
			IEdgeEnumerator en = edges.GetEnumerator();
			IEdge current = null;
			while(en.MoveNext())
			{
				current = en.Current;
			}

			return current;
		}

		/// <summary>
		/// Returns the last edge of the graph
		/// </summary>
		/// <param name="g">graph</param>
		/// <returns>last edge if any, otherwise a null reference</returns>
		public static IEdge LastEdge(IEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			return LastEdge(g.Edges);
		}

		/// <summary>
		/// Returns the first vertex of the enumerable
		/// </summary>
		/// <param name="edges">enumerable collection of <see cref="IEdge"/></param>
		/// <returns>first target vertex if any, otherwise a null reference</returns>
		public static IVertex FirstTargetVertex(IEdgeEnumerable edges)
		{
			IEdge e = FirstEdge(edges);
			if (e!=null)
				return e.Target;
			else
				return null;
		}

		/// <summary>
		/// Returns the first source vertex of the enumerable
		/// </summary>
		/// <param name="edges">enumerable collection of <see cref="IEdge"/></param>
		/// <returns>first source vertex if any, otherwise a null reference</returns>
		public static IVertex FirstSourceVertex(IEdgeEnumerable edges)
		{
			IEdge e = FirstEdge(edges);
			if (e!=null)
				return e.Source;
			else
				return null;
		}

		/// <summary>
		/// Returns the last vertex of the enumerable
		/// </summary>
		/// <param name="edges">enumerable collection of <see cref="IEdge"/></param>
		/// <returns>last target vertex if any, otherwise a null reference</returns>
		public static IVertex LastTargetVertex(IEdgeEnumerable edges)
		{
			IEdge e = LastEdge(edges);
			if (e!=null)
				return e.Target;
			else
				return null;
		}

		/// <summary>
		/// Returns the last source vertex of the enumerable
		/// </summary>
		/// <param name="edges">enumerable collection of <see cref="IEdge"/></param>
		/// <returns>last source vertex if any, otherwise a null reference</returns>
		public static IVertex LastSourceVertex(IEdgeEnumerable edges)
		{
			IEdge e = LastEdge(edges);
			if (e!=null)
				return e.Source;
			else
				return null;
		}


	}
}
