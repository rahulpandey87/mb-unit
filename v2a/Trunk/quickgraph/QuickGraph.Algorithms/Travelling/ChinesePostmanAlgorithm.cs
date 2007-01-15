using System;

namespace QuickGraph.Algorithms.Travelling
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Algorithms;

	using QuickGraph.Algorithms.AllShortestPath;
	using QuickGraph.Algorithms.AllShortestPath.Reducers;
	using QuickGraph.Algorithms.AllShortestPath.Testers;

	using QuickGraph.Collections;

	/// <summary>
	/// Summary description for ChinesePostmanAlgorithm.
	/// </summary>
	/// <remarks>
	/// Implementation of http://www.cs.mdx.ac.uk/harold/cpp/new-Java-cpp.pdf
	/// </remarks>
	public class ChinesePostmanAlgorithm
	{
		private IVertexAndEdgeListGraph visitedGraph;
		private VertexPairDoubleDictionary distances = null;
		private VertexCollection negativeVertices = null;
		private VertexCollection positiveVertices = null;


		public ChinesePostmanAlgorithm(IVertexAndEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");

			this.visitedGraph = g;
		}

		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		protected VertexCollection PositiveVertices
		{
			get
			{
				return this.positiveVertices;
			}
		}

		protected VertexCollection NegativeVertices
		{
			get
			{
				return this.negativeVertices;
			}
		}

		public void Compute(IVertex s)
		{
			CheckInitialized();
			LeastCostPaths();
			FindFeasible();
				
			while( Improvements() );
				RecordPath(s);
		}

		internal void CheckInitialized()
		{
			this.distances = VertexPairDoubleDictionary();
		}

		internal void LeastCostPaths()
		{
			FloydWarshallDistanceTester tester = 
				new FloydWarshallDistanceTester(
				Distances,
				new FloydWarshallShortestPathDistanceTester()
				);

			FloydWarshallAllShortestPathAlgorithm fw = 
				new FloydWarshallAllShortestPathAlgorithm(
				VisitedGraph,
				tester
				);

			fw.Compute();
			fw.CheckConnectivityAndNegativeCycles(Distances);
		}

		internal void FindFeasible()
		{
			this.negativeVertices = new VertexCollection();
			this.positiveVertices = new VertexCollection();
			int nn = 0, np = 0;
			foreach(IVertex u in VisitedGraph.Vertices)
			{
				int degree=Degree(u);
				if( degree < 0 ) 
					NegativeVertices.Add(u);
				else if( degree > 0 ) 
					PositiveVertices.Add(u);
			}

			int du,dv,f;
			foreach(IVertex u in NegativeVertices)
			{
				du = Degrees[u];
				foreach(IVertex v in PositiveVertices)
				{
					dv = Degrees[v];
					if (-du<dv)
						Adjoins[u,v] = f = -du;
					else
						Adjoins[u,v] = f = dv;

					Degrees[u] += f;
					Degrees[v] -= f;
				}
			}
		}

		internal bool Improvements()
		{
			Graph R = new Graph(n);
			foreach(IVertex u NegativeVertices)
			{
				foreach(IVertex v in PositiveVertices)
				{
					R.AddEdge(u, v, Distances[u,v]);
					if( Adjoins[u,v] != 0 ) 
						R.AddEdge(v,u, -Distances[v,u]);
				}
			}
			
			// find a negative cycle
			R.LeastCostPaths();
				
			// cancel the cycle (if any)
			foreach(IVertex v in ) int i = 0; i < n; i++ )
			{
				if( R.c[i][i] < 0 )
				{
					int k = 0, u, v;
					boolean kunset = true;
					u = i; do// find k to cancel
						   {
							   v = R.path[u][i];
							   if( R.c[u][v] < 0 && (kunset || k > f[v][u]) )
							   {
								   k = f[v][u];
								   kunset = false;
							   }
						   } while( (u = v) != i );
					u = i; do// cancel k along the cycle
						   {
							   v = R.path[u][i];
							   if( R.c[u][v] < 0 ) f[v][u] -= k;
							   else f[u][v] += k;
						   } while( (u = v) != i );
					return true; // have another go
				}
			}			
			return false; // no improvements found
		} 

		internal void RecordPath(IVertex s)
		{

		}
	}
}
