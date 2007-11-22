namespace QuickGraphTest
{
	using System;
	using QuickGraph;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms;
	using QuickGraph.Algorithms.ShortestPath;

	/// <summary>
	/// A DijkstraShortestPath visitor for testing purpose
	/// </summary>
	public class TestDijkstraShortestPathVisitor
	{
		private VertexStringDictionary m_Names;

		/// <summary>
		/// Test for the <seealso cref="DijkstraShortestPath"/> algorithm.
		/// </summary>
		/// <param name="dij">alog.</param>
		/// <param name="names">Vertex name map</param>
		public TestDijkstraShortestPathVisitor(
			DijkstraShortestPathAlgorithm dij, 
			VertexStringDictionary names
			)
		{
			m_Names = names;

			dij.InitializeVertex += new VertexHandler(this.InitializeVertex);
			dij.DiscoverVertex += new VertexHandler(this.DiscoverVertex);
			dij.ExamineVertex += new VertexHandler(this.ExamineVertex);
			dij.ExamineEdge += new EdgeHandler(this.ExamineEdge);
			dij.EdgeRelaxed += new EdgeHandler(this.EdgeRelaxed);
			dij.EdgeNotRelaxed += new EdgeHandler(this.EdgeNotRelaxed);
			dij.FinishVertex += new VertexHandler(this.FinishVertex);
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void InitializeVertex(Object sender, VertexEventArgs args)
		{
			Vertex u = args.Vertex;
			Console.WriteLine("InitializeVertex: {0}", m_Names[u]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void DiscoverVertex(Object sender, VertexEventArgs args)
		{
			Vertex u = args.Vertex;
			Console.WriteLine("DiscoverVertex: {0}", m_Names[u]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void ExamineVertex(Object sender, VertexEventArgs args)
		{
			Vertex u = args.Vertex;
			Console.WriteLine("DiscoverVertex: {0}", m_Names[u]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void ExamineEdge(Object sender, EdgeEventArgs args)
		{
			Edge e = args.Edge;
			Console.WriteLine("ExamineEdge: {0} -> {1}", 
				m_Names[e.Source],
				m_Names[e.Target]
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void EdgeRelaxed(Object sender, EdgeEventArgs args)
		{
			Edge e = args.Edge;
			Console.WriteLine("EdgeRelaxed: {0} -> {1}", 
				m_Names[e.Source],
				m_Names[e.Target]
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void EdgeNotRelaxed(Object sender, EdgeEventArgs args)
		{
			Edge e = args.Edge;
			Console.WriteLine("EdgeNotRelaxed: {0} -> {1}", 
				m_Names[e.Source],
				m_Names[e.Target]
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishVertex(Object sender, VertexEventArgs args)
		{
			Vertex u = args.Vertex;
			Console.WriteLine("FinishVertex: {0}", m_Names[u]);
		}
	}
}
