using System;

namespace QuickGraph.Representations
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.MutableTraversals;
	using QuickGraph.Algorithms;

	/// <summary>
	/// A mutable tree-like graph
	/// </summary>
	public class MutableTreeAdapterGraph : 
		TreeAdaptorGraph,
		IMutableTreeGraph
	{
		private bool allowCycles;
		private IMutableBidirectionalVertexAndEdgeListGraph mutableWrapped;

		/// <summary>
		/// Creates a mutable tree wrapper
		/// </summary>
		/// <param name="g">wrapped graph</param>
		/// <param name="allowCycles">cycle tolerance</param>
		public MutableTreeAdapterGraph(
			IMutableBidirectionalVertexAndEdgeListGraph g, 
			bool allowCycles)
			:base(g)
		{
			this.mutableWrapped = g;
			this.allowCycles = allowCycles;
		}

		protected IMutableBidirectionalVertexAndEdgeListGraph MutableWrapped
		{
			get
			{
				return this.mutableWrapped;
			}
		}

		/// <summary>
		/// Gets a value indicating if the tree allows cycles
		/// </summary>
		/// <value>
		/// true if it allows cycle, false otherwise
		/// </value>
		public bool AllowCycles
		{
			get
			{
				return this.allowCycles;
			}
		}

		bool IGraph.AllowParallelEdges
		{
			get
			{
				return false;
			}
		}
		bool IGraph.IsDirected
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Adds a child vertex to the tree
		/// </summary>
		/// <param name="parent">parent vertex</param>
		/// <returns>created vertex</returns>
		/// <exception cref="ArgumentNullException">parent is a null reference</exception>
		/// <exception cref="NonAcyclicGraphException">
		/// if <c>AllowCycles</c> is false and the edge creates a cycle
		/// </exception>
		public virtual IVertex AddChild(IVertex parent)
		{
			IVertex v = this.MutableWrapped.AddVertex();
			this.MutableWrapped.AddEdge(parent,v);

			// check non acyclic
			if (!this.allowCycles)
				AlgoUtility.CheckAcyclic(this.MutableWrapped,parent);

			return v;
		}

		/// <summary>
		/// Removes vertex and sub-tree
		/// </summary>
		/// <param name="v">vertex to remove</param>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		/// <exception cref="GraphNotStronglyConnectedExceptoin">
		/// Removing the vertex breaks the graph connectivity
		/// </exception>
		public virtual void RemoveTree(IVertex root)
		{
			// get vertex to delete
			foreach(IVertex v in Representation.OutVertexTree(
				this.MutableWrapped,root,int.MaxValue)
				)
			{
				this.MutableWrapped.ClearVertex(v);
				this.MutableWrapped.RemoveVertex(v);
			}
		}

		public virtual void Clear()
		{
			this.MutableWrapped.Clear();
		}
	}
}
