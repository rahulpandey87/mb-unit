using System;

namespace QuickGraph.Algorithms.Visitors
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// Summary description for VertexListPopulatorVisitor.
	/// </summary>
	public class PopulatorVisitor
	{
		private ISerializableVertexAndEdgeListGraph graph;

		public PopulatorVisitor(
			ISerializableVertexAndEdgeListGraph graph
			)
		{
			if (graph==null)
				throw new ArgumentNullException("graph");
			this.graph = graph;			
		}

		public void StartVertex(Object sender, VertexEventArgs e)
		{
			this.graph.AddVertex(e.Vertex);
		}

		public void TreeEdge(Object sender, EdgeEventArgs e)
		{
			this.graph.AddVertex(e.Edge.Target);
			this.graph.AddEdge(e.Edge);
		}

		public void BackForwardOrCrossEdge(Object sender, EdgeEventArgs e)
		{
			this.graph.AddEdge(e.Edge);
		}
	}
}
