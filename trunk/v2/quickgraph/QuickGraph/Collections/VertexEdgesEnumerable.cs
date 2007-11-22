using System;
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// A simple IEnumerable class that provides an enumerator over
	/// the graph edges.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public sealed class VertexEdgesEnumerable : IEdgeEnumerable
	{
		private VertexEdgesDictionary vertexOutEdges;

		/// <summary>
		/// Construct an enumerable collection of edges
		/// </summary>
		/// <param name="vertexOutEdges">vertex out edges dictionary</param>
		/// <exception cref="ArgumentNullException">vertexOutEdges is null</exception>
		public VertexEdgesEnumerable(VertexEdgesDictionary vertexOutEdges)
		{
			if (vertexOutEdges == null)
				throw new ArgumentNullException("vertexOutEdges");
			this.vertexOutEdges = vertexOutEdges;
		}

		/// <summary>
		/// Provides an enumerator over the graph edges
		/// </summary>
		/// <returns>An enumerator</returns>
		public VertexEdgesEnumerator GetEnumerator()
		{
			return new VertexEdgesEnumerator(vertexOutEdges);
		}

		/// <summary>
		/// Implements the IEnumerable method.
		/// </summary>
		/// <returns>An enumerator over the edges</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEdgeEnumerator IEdgeEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
