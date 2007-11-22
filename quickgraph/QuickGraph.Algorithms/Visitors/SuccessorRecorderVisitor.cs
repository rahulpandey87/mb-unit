using System;

namespace QuickGraph.Algorithms.Visitors
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;

	/// <summary>
	/// Summary description for SuccessorRecorderVisitor.
	/// </summary>
	public class SuccessorRecorderVisitor
	{
		private VertexEdgeDictionary successors = new VertexEdgeDictionary();

		public SuccessorRecorderVisitor()
		{}

		public VertexEdgeDictionary Successors
		{
			get
			{
				return this.successors;
			}
		}

		/// <summary>
		/// Removes 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void InitializeVertex(Object sender, VertexEventArgs args)
		{
			this.Successors.Remove(args.Vertex);
		}

		/// <summary>
		/// Let e = (u,v), p[u]=e
		/// </summary>
		public void TreeEdge(Object sender, EdgeEventArgs args)
		{
			if (args.Edge is ReversedEdge)
				this.Successors[args.Edge.Source]=((ReversedEdge)args.Edge).Wrapped;
			else
				this.Successors[args.Edge.Source]=args.Edge;
		}

		public void ClearTreeVertex(Object sender, VertexEventArgs args)
		{
			this.Successors.Remove(args.Vertex);
		}
	}
}
