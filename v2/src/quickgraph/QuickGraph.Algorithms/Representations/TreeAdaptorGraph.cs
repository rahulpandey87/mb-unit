using System;

namespace QuickGraph.Representations
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Exceptions;
	using QuickGraph.Collections;

	/// <summary>
	/// A tree-like wrapper for bidirectional graph
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface defines a DOM like tree node structure.
	/// </para>
	/// <para>
	/// Graphs used with this interface must be directed, not
	/// allowing parrallel edges. However, they can be cylic
	/// but the in-degree of each vertex must be equal to 1.
	/// </para>
	/// </remarks>
	public class TreeAdaptorGraph : ITreeGraph
	{
		private IBidirectionalGraph wrapped;

		/// <summary>
		/// Wraps a graph into a tree-like structure
		/// </summary>
		/// <param name="wrapped"></param>
		public TreeAdaptorGraph(IBidirectionalGraph wrapped)
		{
			if (wrapped==null)
				throw new ArgumentNullException("wrapped");
			this.wrapped = wrapped;
		}

		/// <summary>
		/// Gets the wrapped <see cref="IBidirectionalGraph"/> instance.
		/// </summary>
		protected IBidirectionalGraph Wrapped
		{
			get
			{
				return this.wrapped;
			}
		}

		/// <summary>
		/// Gets the <see cref="IVertex"/> parent.
		/// </summary>
		/// <param name="v">current vertex</param>
		/// <returns>
		/// parent vertex if any, null reference otherwize
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		/// <exception cref="MultipleInEdgeException">
		/// <paramref name="v"/> has multiple in-edges
		/// </exception>
		public IVertex ParentVertex(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			if (this.Wrapped.InDegree(v)>1)
				throw new MultipleInEdgeException(v.ID.ToString());
			
			return Traversal.FirstSourceVertex(this.Wrapped.InEdges(v));
		}

		/// <summary>
		/// Gets the first adjacent vertex
		/// </summary>
		/// <param name="v">current vertex</param>
		/// <returns>first out-vertex</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		public IVertex FirstChild(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");

			return Traversal.FirstTargetVertex(this.Wrapped.OutEdges(v));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		public IVertex LastChild(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");

			return Traversal.LastTargetVertex(this.Wrapped.OutEdges(v));
		}

		/// <summary>
		/// Gets a value indicating if the <see cref="IVertex"/> has out-edges
		/// </summary>
		/// <param name="v"><see cref="IVertex"/> to test</param>
		/// <returns>true if <paramref name="v"/> has out-edges.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		public bool HasChildVertices(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");

			return this.Wrapped.OutEdgesEmpty(v);
		}

		/// <summary>
		/// Gets an enumerable collection of child <see cref="IVertex"/>
		/// </summary>
		/// <param name="v">current <see cref="IVertex"/></param>
		/// <returns>An enumerable collection of adjacent vertices</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		public IVertexEnumerable ChildVertices(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");

			return new TargetVertexEnumerable(this.Wrapped.OutEdges(v));
		}
	}
}
