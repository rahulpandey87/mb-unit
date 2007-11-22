using System;

namespace QuickGraph.Algorithms.Visitors
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Visitors;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Collections;
	using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// Visitor that records the sink vertices in the visited tree.
	/// </summary>
	/// <remarks>
	/// A sink vertex is a vertex that has no out-edges.
	/// </remarks>
	public class SinkRecorderVisitor : IVertexColorizerVisitor
	{
		private IIncidenceGraph visitedGraph;
		private VertexCollection sinks = new VertexCollection();

		/// <summary>
		/// Create a <see cref="sinkRecorderVisitor"/> instance.
		/// </summary>
		/// <param name="g">visited graph</param>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		public SinkRecorderVisitor(IIncidenceGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			this.visitedGraph = g;
		}

		/// <summary>
		/// Create a <see cref="sinkRecorderVisitor"/> instance.
		/// </summary>
		/// <param name="g">visited graph</param>
		/// <param name="sinks">collection that will hold the sinks</param>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		public SinkRecorderVisitor(
			IIncidenceGraph g,
			VertexCollection sinks)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (sinks==null)
				throw new ArgumentNullException("sinks");
			this.visitedGraph = g;
			this.sinks = sinks;
		}

		/// <summary>
		/// Gets the visited <see cref="IIncidenceGraph"/> instance
		/// </summary>
		/// <value>
		/// The visited graph
		/// </value>
		public IIncidenceGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		/// <summary>
		/// Gets the sink collection
		/// </summary>
		/// <value>
		/// A <see cref="VertexCollection"/> of sink vertices
		/// </value>
		public VertexCollection Sinks
		{
			get
			{
				return this.sinks;
			}
		}

		#region IVertexColorizerVisitor Members

		public void FinishVertex(object sender, VertexEventArgs args)
		{
			if (this.VisitedGraph.OutEdgesEmpty(args.Vertex))
				this.sinks.Add(args.Vertex);
		}

		public void DiscoverVertex(object sender, VertexEventArgs args)
		{}

		public void InitializeVertex(object sender, VertexEventArgs args)
		{}

		#endregion
	}
}
