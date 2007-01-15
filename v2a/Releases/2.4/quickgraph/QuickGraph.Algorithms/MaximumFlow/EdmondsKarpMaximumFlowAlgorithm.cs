using System;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.Collections;
using QuickGraph.Collections;
using QuickGraph.Collections.Filtered;
using QuickGraph.Predicates;
using QuickGraph.Algorithms.Visitors;
using QuickGraph.Algorithms.Search;

namespace QuickGraph.Algorithms.MaximumFlow
{


	/// <summary>Edmonds-Karp Maximum Flow Algorithm</summary>
	/// <remarks>
	/// <para>
	/// The <see cref="EdmondsKarpMaximumFlowAlgorithm"/> class calculates 
	/// the maximum flow of a network. The calculated maximum flow will be 
	/// the return value of the function <see cref="EdmondsKarpMaximumFlowAlgorithm.Compute"/>. 
	/// The function also calculates the flow values <c>f[e]</c> for all e in E, 
	/// which are returned in the form of the residual capacity 
	/// <c>r[e] = c[e] - f[e]</c>. 
	/// </para>
	/// <para>
	/// There are several special requirements on the input graph 
	/// and property map parameters for this algorithm. First, 
	/// the directed graph <c>G=(V,E)</c> that represents the network must be augmented 
	/// to include the reverse edge for every edge in E. That is, the input graph 
	/// should be <c>Gin = (V,{E U ET})</c>. 
	/// The <c>reversedEdges</c> argument 
	/// must map each edge in the original graph to its reverse edge, 
	/// that is <c>e=(u,v) -> (v,u)</c> for all e in E. 
	/// The <c>capacities</c> argument must map each edge in E to a positive number, 
	/// and each edge in ET to 0. 
	/// </para>
	/// <para>
	/// The algorithm is due to Edmonds and Karp, 
	/// though we are using the variation called the <em>labeling algorithm</em> 
	/// described in Network Flows. 
	/// </para>
	/// <para>
	/// This algorithm provides a very simple and easy to implement solution 
	/// to the maximum flow problem. However, there are several reasons why 
	/// this algorithm is not as good as the <see cref="PushRelabelMaximumFlowAlgorithm"/>
	/// algorithm.  
	/// </para>
	/// <para>
	/// In the non-integer capacity case, the time complexity is O(V E^2) 
	/// which is worse than the time complexity of the <see cref="PushRelabelMaximumFlowAlgorithm"/> 
	/// O(V^2E^1/2) for all but the sparsest of graphs. 
	/// </para>
	/// <para>
	/// In the integer capacity case, if the capacity bound U is very large then the 
	/// algorithm will take a long time. 
	/// </para>
	/// </remarks>
	public class EdmondsKarpMaximumFlowAlgorithm : MaximumFlowAlgorithm
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="capacities"></param>
		/// <param name="reversedEdges"></param>
		public EdmondsKarpMaximumFlowAlgorithm(
			IVertexListGraph g,
			EdgeDoubleDictionary capacities,
			EdgeEdgeDictionary reversedEdges
			)
			: base(g,capacities,reversedEdges)
		{}
	
		internal IVertexListGraph ResidualGraph
		{
			get
			{
				return new FilteredVertexListGraph(
					VisitedGraph,
					new ResidualEdgePredicate(ResidualCapacities)
					);
			}
		}
	
		internal void Augment(
			IVertex src,
			IVertex sink
			)
		{
			IEdge e=null;
			IVertex u=null;

			// find minimum residual capacity along the augmenting path
			double delta = double.MaxValue;
			e = Predecessors[sink];
			do 
			{
				delta = Math.Min(delta, ResidualCapacities[e]);
				u = e.Source;
				e = Predecessors[u];
			} while (u != src);

			// push delta units of flow along the augmenting path
			e = Predecessors[sink];
			do 
			{
				ResidualCapacities[e] -= delta;
				ResidualCapacities[ ReversedEdges[e] ] += delta;
				u = e.Source;
				e = Predecessors[u];
			} while (u != src);
		}
    
		/// <summary>
		/// Computes the maximum flow between <paramref name="src"/> and
		/// <paramref name="sink"/>
		/// </summary>
		/// <param name="src"></param>
		/// <param name="sink"></param>
		/// <returns></returns>
		public override double Compute(IVertex src, IVertex sink)
		{    
			if (src==null)
				throw new ArgumentNullException("src");
			if (sink==null)
				throw new ArgumentNullException("sink");

			foreach(IVertex u in VisitedGraph.Vertices)
			{
				foreach(IEdge e in VisitedGraph.OutEdges(u))
				{
					ResidualCapacities[e] = Capacities[e];   			
				}
			}
    
			Colors[sink] = GraphColor.Gray;
			while (Colors[sink] != GraphColor.White)
			{
				PredecessorRecorderVisitor vis = new PredecessorRecorderVisitor(
					Predecessors
					);
				VertexBuffer Q = new VertexBuffer();
				BreadthFirstSearchAlgorithm bfs = new BreadthFirstSearchAlgorithm(
					ResidualGraph,
					Q,
					Colors
					);
				bfs.RegisterPredecessorRecorderHandlers(vis);
				bfs.Compute(src);
    		
				if (Colors[sink] != GraphColor.White)
					Augment(src, sink);
			} // while
    
			double flow=0;
			foreach(IEdge e in VisitedGraph.OutEdges(src))
				flow += (Capacities[e] - ResidualCapacities[e]);

			return flow;
		} 

	}

}