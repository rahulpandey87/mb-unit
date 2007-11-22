using System;

namespace QuickGraph.Representations
{
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Algorithms;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Algorithms.Visitors;
	using QuickGraph.Exceptions;

	/// <summary>
	/// Summary description for Representation.
	/// </summary>
	public sealed class Representation
	{
		private Representation()
		{}

		public static void CloneOutVertexTree(
			IVertexListGraph g,
			ISerializableVertexAndEdgeListGraph sub,
			IVertex v,
			int maxDepth
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (sub==null)
				throw new ArgumentNullException("sub");
			if (v==null)
				throw new ArgumentNullException("v");

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);

			PopulatorVisitor pop = new PopulatorVisitor(sub);
			dfs.StartVertex += new VertexEventHandler(pop.StartVertex);
			dfs.TreeEdge += new EdgeEventHandler(pop.TreeEdge);

			dfs.MaxDepth = maxDepth;
			dfs.Initialize();
			dfs.Visit(v,0);
		}

		/// <summary>
		/// Records all the vertices that are part of the out-subtree of v
		/// </summary>
		/// <param name="g">visited graph</param>
		/// <param name="v">root vertex</param>
		/// <param name="maxDepth">Maximum exploration depth</param>
		/// <returns></returns>
		public static VertexCollection OutVertexTree(
			IVertexListGraph g,
			IVertex v,
			int maxDepth
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (v==null)
				throw new ArgumentNullException("v");

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);
			dfs.BackEdge +=new EdgeEventHandler(dfs_BackEdge);
			VertexRecorderVisitor vis =new VertexRecorderVisitor();
			vis.Vertices.Add(v);
			dfs.TreeEdge +=new EdgeEventHandler(vis.RecordTarget);

			dfs.MaxDepth = maxDepth;
			dfs.Initialize();
			dfs.Visit(v,0);

			return vis.Vertices;
		}

		/// <summary>
		/// Records all the vertices that are part of the in-subtree of v
		/// </summary>
		/// <param name="g">visited graph</param>
		/// <param name="v">root vertex</param>
		/// <param name="maxDepth">Maximum exploration depth</param>
		/// <returns></returns>
		public static VertexCollection InVertexTree(
			IBidirectionalVertexAndEdgeListGraph g,
			IVertex v,
			int maxDepth
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (v==null)
				throw new ArgumentNullException("v");

			HeightFirstSearchAlgorithm dfs = new HeightFirstSearchAlgorithm(g);
			dfs.BackEdge +=new EdgeEventHandler(dfs_BackEdge);
			VertexRecorderVisitor vis =new VertexRecorderVisitor();
			vis.Vertices.Add(v);
			dfs.TreeEdge +=new EdgeEventHandler(vis.RecordTarget);

			dfs.MaxDepth = maxDepth;
			dfs.Initialize();
			dfs.Visit(v,0);

			return vis.Vertices;
		}

		/// <summary>
		/// Records all the edges that are part of the subtree of v
		/// </summary>
		/// <param name="g">visited graph</param>
		/// <param name="e">root edge</param>
		/// <param name="maxDepth">maximum expolration depth</param>
		/// <returns></returns>
		public static EdgeCollection OutEdgeTree(
			IEdgeListAndIncidenceGraph g,
			IEdge e,
			int maxDepth
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (e==null)
				throw new ArgumentNullException("e");

			EdgeDepthFirstSearchAlgorithm dfs = new EdgeDepthFirstSearchAlgorithm(g);
			dfs.BackEdge +=new EdgeEventHandler(dfs_BackEdge);
			EdgeRecorderVisitor vis =new EdgeRecorderVisitor();
			vis.Edges.Add(e);

			dfs.DiscoverTreeEdge +=new EdgeEdgeEventHandler(vis.RecordTarget);

			dfs.MaxDepth = maxDepth;
			dfs.Initialize();
			dfs.Visit(e,0);

			return vis.Edges;
		}

		/// <summary>
		/// Records all the edges that are part of the subtree of v
		/// </summary>
		/// <param name="g">visited graph</param>
		/// <param name="e">root edge</param>
		/// <param name="maxDepth">maximum expolration depth</param>
		/// <returns></returns>
		public static EdgeCollection InEdgeTree(
			IBidirectionalVertexAndEdgeListGraph g,
			IEdge e,
			int maxDepth
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (e==null)
				throw new ArgumentNullException("e");

			EdgeHeightFirstSearchAlgorithm dfs = new EdgeHeightFirstSearchAlgorithm(g);
			dfs.BackEdge +=new EdgeEventHandler(dfs_BackEdge);
			EdgeRecorderVisitor vis =new EdgeRecorderVisitor();
			vis.Edges.Add(e);

			dfs.DiscoverTreeEdge +=new EdgeEdgeEventHandler(vis.RecordTarget);

			dfs.MaxDepth = maxDepth;
			dfs.Initialize();
			dfs.Visit(e,0);

			return vis.Edges;
		}

		/// <summary>
		/// Used in OutEdgeTree
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal static void dfs_BackEdge(object sender, EdgeEventArgs e)
		{
			throw new NonAcyclicGraphException();	
		}
	}
}
