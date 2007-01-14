using System;
using System.Collections;

namespace QuickGraph.Algorithms.AllShortestPath
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Collections;

	using QuickGraph.Collections;

	using QuickGraph.Algorithms.AllShortestPath.Testers;
	using QuickGraph.Algorithms.AllShortestPath.Reducers;

	using QuickGraph.Exceptions;

	/// <summary>
	/// Floyd Warshall All Shortest Path Algorithm
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class FloydWarshallAllShortestPathAlgorithm : IAlgorithm
	{
		private IVertexAndEdgeListGraph visitedGraph;
		private IFloydWarshallTester tester;
		private Hashtable definedPaths;
	
		/// <summary>
		/// Default constructor - initializes all fields to default values
		/// </summary>
		public FloydWarshallAllShortestPathAlgorithm(
			IVertexAndEdgeListGraph visitedGraph,
			IFloydWarshallTester tester
			)
		{
			if (visitedGraph==null)
				throw new ArgumentNullException("visitedGraph");
			if (tester==null)
				throw new ArgumentNullException("test");
		
			this.visitedGraph = visitedGraph;
			this.tester = tester;
			this.definedPaths = null;
		}

		/// <summary>
		/// Gets the visited graph
		/// </summary>
		/// <value>
		/// Visited Graph
		/// </value>
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		Object IAlgorithm.VisitedGraph
		{
			get
			{
				return this.VisitedGraph;
			}
		}
	
		/// <summary>
		/// Gets the <see cref="IFloydWarshallTester"/> instance
		/// </summary>
		public IFloydWarshallTester Tester
		{
			get
			{
				return this.tester;
			}
		}

		/// <summary>
		/// Internal use
		/// </summary>
		private Hashtable DefinedPaths
		{
			get
			{
				return this.definedPaths;
			}
		}
	
		/// <summary>
		/// Raised when initializing a new path
		/// </summary>
		/// <remarks>
		/// </remarks>
		public event FloydWarshallEventHandler InitiliazePath;
	
		/// <summary>
		/// Raises the <see cref="InitializePath"/> event.
		/// </summary>
		/// <param name="source">source vertex</param>
		/// <param name="target">target vertex</param>
		protected virtual void OnInitiliazePath(IVertex source, IVertex target)
		{
			if (InitiliazePath!=null)
				InitiliazePath(this, new FloydWarshallEventArgs(source,target));
		}
	
		/// <summary>
		/// 
		/// </summary>
		public event FloydWarshallEventHandler ProcessPath;

		/// <summary>
		/// Raises the <see cref="ProcessPath"/> event.
		/// </summary>
		/// <param name="source">source vertex</param>
		/// <param name="target">target vertex</param>
		/// <param name="intermediate"></param>
		protected virtual void OnProcessPath(IVertex source, IVertex target, IVertex intermediate)
		{
			if (ProcessPath != null)
				ProcessPath(this, new FloydWarshallEventArgs(source, target,intermediate));
		}	
	
		/// <summary>
		/// Raised when a path is reduced
		/// </summary>
		public event FloydWarshallEventHandler ReducePath;

		/// <summary>
		/// Raises the <see cref="ReducePath"/> event.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="intermediate"></param>
		protected virtual void OnReducePath(IVertex source, IVertex target, IVertex intermediate)
		{
			if (ReducePath != null)
				ReducePath(this, new FloydWarshallEventArgs(source, target,intermediate));
		}

		/// <summary>
		/// Raised when a path is not reduced
		/// </summary>
		public event FloydWarshallEventHandler NotReducePath;

		/// <summary>
		/// Raises the <see cref="NotReducePath"/> event.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="intermediate"></param>
		protected virtual void OnNotReducePath(IVertex source, IVertex target, IVertex intermediate)
		{
			if (NotReducePath != null)
				NotReducePath(this, new FloydWarshallEventArgs(source, target,intermediate));
		}
		
		/// <summary>
		/// Compute the All shortest path problem.
		/// </summary>
		public void Compute()
		{
			this.definedPaths = new Hashtable();

			// initialize distance map
			foreach(IVertex i in VisitedGraph.Vertices)
				foreach(IVertex j in VisitedGraph.Vertices)
				{
					if (VisitedGraph.ContainsEdge(i,j))
						DefinedPaths.Add( new VertexPair(i,j), null );
					OnInitiliazePath(i,j);
				}
				
			// iterate
			foreach(IVertex k in VisitedGraph.Vertices)
			{
				foreach(IVertex i in VisitedGraph.Vertices)
				{	
					if (DefinedPaths.Contains(new VertexPair(i,k)))
					{
						foreach(IVertex j in VisitedGraph.Vertices)
						{
							OnProcessPath(i,j,k);
						
							bool defkj = DefinedPaths.Contains(new VertexPair(k,j));
							bool defij = DefinedPaths.Contains(new VertexPair(i,j));
							if (defkj && (defij || Tester.Test(i,j,k)))
							{
								DefinedPaths[new VertexPair(i,j)]=null;
								OnReducePath(i,j,k);
							}
							else
								OnNotReducePath(i,j,k);
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks the graph for connectivity and negative cycles
		/// </summary>
		/// <param name="costs">cost distionary</param>
		/// <exception cref="NegativeCycleException">graph has negatice cycle.</exception>
		/// <exception cref="GraphNotStronglyConnectedException">graph is not strongly connected</exception>
		public void CheckConnectivityAndNegativeCycles(IVertexDistanceMatrix costs)
		{
			foreach(IVertex u in VisitedGraph.Vertices)
			{
				if( costs!=null && costs.Distance(u,u) < 0 ) 
					throw new NegativeCycleException("Graph has negative cycle");

				foreach(IVertex v in VisitedGraph.Vertices)
					if(!DefinedPaths.Contains(new VertexPair(u,v))) 
						throw new Exception("Graph is not strongly connected");
			}
		}
	}
}
