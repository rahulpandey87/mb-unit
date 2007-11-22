using System.Collections;
using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// An adaptor class to enumerate edges.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The enumerator works be succesively iterating the vertices 
	/// out-edges.
	/// </para>
	/// <para>
	/// This class should not be constructed directly. It is created on
	/// a <c>GetEnumerator()</c> call.
	/// </para>
	/// </remarks>
	public sealed class VertexEdgesEnumerator : IEdgeEnumerator
	{
		private IEnumerator VertexOutEdgeEnumerator;
		private IEdgeEnumerator OutEdgeEnumerator;

		/// <summary>
		/// Construct an enumerator over the out-edges
		/// </summary>
		/// <param name="vertexOutEdges">Out edge dictionary to iterate</param>
		/// <exception cref="ArgumentNullException">vertexOutEdges is null</exception>
		public VertexEdgesEnumerator(VertexEdgesDictionary vertexOutEdges)
		{
			if (vertexOutEdges == null)
				throw new ArgumentNullException("vertexOutEdges");
			VertexOutEdgeEnumerator = vertexOutEdges.GetEnumerator();
			OutEdgeEnumerator = null;
		}

		/// <summary>
		/// Sets the enumerator to its initial position, 
		/// which is before the first element in the collection.
		/// </summary>
		public void Reset()
		{
			VertexOutEdgeEnumerator.Reset();
			OutEdgeEnumerator=null;
		}

		/// <summary>
		/// Advances the enumerator to the next element of the 
		/// collection.
		/// </summary>
		/// <returns>
		/// true if the enumerator was successfully advanced to the 
		/// next edge; false if the enumerator has passed the end of 
		/// the collection.
		/// </returns>
		public bool MoveNext()
		{
			// check if first time.
			if (OutEdgeEnumerator == null)
			{
				if (!MoveNextVertex())
					return false;
			}

			// getting next valid entry
			do 
			{
				// try getting edge in the current out edge list
				if (OutEdgeEnumerator.MoveNext())
					return true;

				// move to the next outedge list
				if (!MoveNextVertex())
					return false;
			} 
			while(true);
		}

		/// <summary>
		/// Gets the current element in the collection.
		/// </summary>
		/// <exception cref="InvalidOperationException">The enumerator 
		/// is positioned before the first element of the collection 
		/// or after the last element.</exception>
		public IEdge Current
		{
			get
			{
				if (OutEdgeEnumerator == null)
					throw new InvalidOperationException();
				return OutEdgeEnumerator.Current;
			}
		}

		/// <summary>
		/// Implement IEnumerator.Current.
		/// </summary>
		Object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		/// <summary>
		/// Move the vertex iterator to the next vertex.
		/// </summary>
		/// <returns></returns>
		public bool MoveNextVertex()
		{
			// check if empty vertex set
			if (!VertexOutEdgeEnumerator.MoveNext())
			{
				OutEdgeEnumerator=null;
				return false;
			}

			// getting enumerator
			OutEdgeEnumerator = ((EdgeCollection)((DictionaryEntry)VertexOutEdgeEnumerator.Current).Value).GetEnumerator();
			return true;
		}
	}
}
