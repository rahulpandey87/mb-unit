// QuickGraph Library 
// 
// Copyright (c) 2004 Rohit Gadagkar
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		QuickGraph Library HomePage: http://mbunit.tigris.org

using System;
using System.Collections;
using QuickGraph.Algorithms;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.MutableTraversals;
using QuickGraph.Collections;
using QuickGraph.Collections.Sort;
using QuickGraph.Representations;
using QuickGraph.Providers;
using QuickGraph.Concepts.Collections;

namespace QuickGraph.Algorithms
{
	/// <summary>
	/// Creates a transitive closure of the input graph
	/// </summary>
	/// <remarks>
	/// <para>
	/// The core algorithm for Transitive Closure (TC) is inspired by the 
	/// Boost Graph Library implementation
	/// and Nuutila, E's work "Efficient Transitive Closure Computation in 
	/// Large Digraphs".
	/// </para>
	/// <para>
	/// Event <see cref="InitTransitiveClosureVertex"/> is raised when a new 
	/// vertex is added to the TC graph.
	/// It maps a vertex in the original graph to the corresponding vertex in 
	/// the TC graph
	/// </para>
	///	<para>
	///	Event <see cref="ExamineEdge"/> is rasied when an edge is added to the 
	///	TC graph
	///	</para>
	/// </remarks>
	/// <author name="Rohit Gadogkar" email="rohit.gadagkar@gmail.com" />
	public class TransitiveClosureAlgorithm : IAlgorithm
	{
		private IVertexListGraph visitedGraph;
		private IMutableVertexAndEdgeListGraph cg;
		private VertexVertexDictionary graphTransitiveClosures ;

		/// <summary>
		/// Transitive closure constructor
		/// </summary>
		/// <param name="g">
		/// Graph whose transitive closre needs to be 
		/// computed
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="g"/> is a <null/>.
		/// </exception>
		public TransitiveClosureAlgorithm(IVertexListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			this.visitedGraph = g;	
			cg = new AdjacencyGraph();
		}

		#region Properties

		/// <summary>
		/// Visited Graph
		/// </summary>
		public IVertexListGraph VisitedGraph	{
			get	{
				return this.visitedGraph;
			}
		}

		Object IAlgorithm.VisitedGraph	{
			get	{
				return this.VisitedGraph;
			}
		}		

		/// <summary>
		/// Map of vertex in Original graph to corresponding vertex in Transitive Closure
		/// </summary>
		public VertexVertexDictionary OrigToTCVertexMap	
		{
			get	
			{
				return this.graphTransitiveClosures;
			}
		}
		#endregion

		#region Events

		/// <summary>
		/// Invoked when a new vertex is added to the Transitive Closure graph
		/// </summary>
		public event TransitiveClosureVertexEventHandler InitTransitiveClosureVertex;

		/// <summary>
		/// Raises the <see cref="InitTransitiveClosureVertex"/> event.
		/// </summary>
		/// <param name="arg"></param>
		protected void OnInitTransitiveClosureVertex(TransitiveClosureVertexEventArgs arg)
		{
			if (InitTransitiveClosureVertex!=null)
				InitTransitiveClosureVertex(this, arg);
		}

		/// <summary>
		/// Invoked when a new edge is added to the transitive closure graph.
		/// </summary>
		public event EdgeEventHandler ExamineEdge;

		/// <summary>
		/// Raises the <see cref="ExamineEdge"/> event.
		/// </summary>
		/// <param name="e">New edge that was added to the transitive closure graph</param>
		protected void OnExamineEdge(IEdge e)
		{
			if (ExamineEdge!=null)
				ExamineEdge(this, new EdgeEventArgs(e));
		}

		#endregion

		/// <summary>
		/// Compute the transitive closure and store it in the supplied graph 'tc'
		/// </summary>
		/// <param name="tc">
		/// Mutable Graph instance to store the transitive closure
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="tc"/> is a <null/>.
		/// </exception>
		public void Create( IMutableVertexAndEdgeListGraph tc )
		{
			if (tc==null)
				throw new ArgumentNullException("tc");
			CondensationGraphAlgorithm cgalgo = new CondensationGraphAlgorithm(VisitedGraph);
			cgalgo.Create(cg);
			
			ArrayList topo_order = new ArrayList(cg.VerticesCount);
			TopologicalSortAlgorithm topo = new TopologicalSortAlgorithm(cg, topo_order);
			topo.Compute();

			VertexIntDictionary in_a_chain= new VertexIntDictionary();
			VertexIntDictionary topo_number = new VertexIntDictionary();
			for( int order=0; order<topo_order.Count; order++ )	{
				IVertex v = (IVertex)topo_order[order];
				topo_number.Add( v, order );
				if(!in_a_chain.Contains(v))		// Initially no vertex is present in a chain
					in_a_chain.Add(v, 0);
			}			
			
			VertexListMatrix chains = new VertexListMatrix();

			int position = -1;
			foreach( IVertex v in topo_order )	
			{
				if(in_a_chain[v]==0)	{
					//	Start a new chain
					position = chains.AddRow();
					IVertex next = v;
					for(;;)	{
						chains[position].Add(next);
						in_a_chain[next] = 1;
						//	Get adjacent vertices ordered by topological number
						//	Extend the chain by choosing the adj vertex with lowest topo#
						ArrayList adj = TopoSortAdjVertices(next, cg, topo_number);
						if( (next = FirstNotInChain(adj, in_a_chain)) == null )
							break;						
					}
				}
			}

			VertexIntDictionary chain_number = new VertexIntDictionary();
			VertexIntDictionary pos_in_chain = new VertexIntDictionary();

			// Record chain positions of vertices
			SetChainPositions(chains, chain_number, pos_in_chain);	
						
			VertexListMatrix successors = new VertexListMatrix();
			successors.CreateObjectMatrix(cg.VerticesCount, chains.RowCount, int.MaxValue);
		
			if(topo_order.Count > 0)	
			{				
				for( int rtopo=topo_order.Count-1; rtopo>-1; rtopo--)	
				{
					IVertex u = (IVertex) topo_order[rtopo];
					foreach( IVertex v in TopoSortAdjVertices(u, cg, topo_number) )	
					{
						if(topo_number[v] < (int)successors[u.ID][chain_number[v]])	
						{
							//	{succ(u)} = {succ(u)} U {succ(v)}
							LeftUnion(successors[u.ID], successors[v.ID]);
							//	{succ(u)} = {succ(u)} U {v}
							successors[u.ID][chain_number[v]] = topo_number[v];
						}
					}
				}
			}

			//	Create transitive closure of condensation graph
			//	Remove existing edges in CG & rebuild edges for TC from 
			//  successor set (to avoid duplicating parallel edges)
			ArrayList edges = new ArrayList();	
			foreach( IEdge e in cg.Edges )
				edges.Add(e);
			foreach(IEdge e in edges)
				cg.RemoveEdge(e);
			foreach(IVertex u in cg.Vertices)	
			{
				int i = u.ID;
				for(int j=0; j<chains.RowCount; j++)	
				{
					int tnumber = (int) successors[i][j];
					if(tnumber < int.MaxValue)	
					{
						IVertex v = (IVertex) topo_order[tnumber];
						for(int k=pos_in_chain[v]; k<chains[j].Count; k++)	
						{
							cg.AddEdge( u, (IVertex)chains[j][k]);
						}
					}
				}
			}

			// Maps a vertex in input graph to it's transitive closure graph
			graphTransitiveClosures = new VertexVertexDictionary();
			//	Add vertices to transitive closure graph
			foreach(IVertex v in visitedGraph.Vertices)	
			{
				if(!graphTransitiveClosures.Contains(v))	
				{
					IVertex vTransform = tc.AddVertex();
					OnInitTransitiveClosureVertex( 
						new TransitiveClosureVertexEventArgs(
						v, vTransform)
						);		
					// Fire the TC Vertex Event
					graphTransitiveClosures.Add(v, vTransform);
				}
			}

			//Add edges connecting vertices within SCC & adjacent 
			// SCC (strongly connected component)
			IVertexCollection scc_vertices = null;
			foreach(IVertex s_tccg in cg.Vertices)	
			{
				scc_vertices = (IVertexCollection)cgalgo.SCCVerticesMap[s_tccg.ID];
				if(scc_vertices.Count > 1)	
				{
					foreach(IVertex u in scc_vertices)
						foreach(IVertex v in scc_vertices)
							OnExamineEdge(tc.AddEdge(graphTransitiveClosures[u], graphTransitiveClosures[v]));
				}
				foreach(IEdge adj_edge in cg.OutEdges(s_tccg))	
				{
					IVertex t_tccg = adj_edge.Target;
					foreach(IVertex s in (IVertexCollection)cgalgo.SCCVerticesMap[s_tccg.ID])	
					{
						foreach(IVertex t in (IVertexCollection)cgalgo.SCCVerticesMap[t_tccg.ID])
							OnExamineEdge(tc.AddEdge(graphTransitiveClosures[s], graphTransitiveClosures[t]));
					}
				}
			}
		}

		#region Private Utility Functions

		private void LeftUnion(IList u, IList v)	
		{
			//	Specific to chain decomposition
			if(u.Count != v.Count)
				throw new ArgumentException("Exception in LeftUnion. The 2 lists must be of the same size");
			for(int i=0; i<u.Count; i++)
				if((u[i] is int) && (v[i] is int))
					u[i] = Math.Min((int)u[i], (int)v[i]);
		}

		private void SetChainPositions( VertexListMatrix chains, VertexIntDictionary chain_number, VertexIntDictionary pos_in_chain )	
			//	Record chain number and position in chain of each vertex in chains
		{
			if (chain_number == null)
				throw new ArgumentNullException("chain_number");
			if (pos_in_chain == null)
				throw new ArgumentNullException("pos_in_chain");
			for(int i=0; i<chains.RowCount; i++)	{
				for( int j=0;j<chains[i].Count; j++)	{
					IVertex v = (IVertex) chains[i][j];
					if(! chain_number.ContainsKey(v))
						chain_number.Add(v, i);
					if(! pos_in_chain.ContainsKey(v))
						pos_in_chain.Add(v,j);
				}
			}			
		}

		private IVertex FirstNotInChain( ArrayList adj_topo_vertices, VertexIntDictionary vertices_in_a_chain )	
			//	Return the first adjacent vertex which is not already present in any of the chains
		{
			if(adj_topo_vertices == null)
				throw new ArgumentNullException("Argument <adj_topo_vertices> in function FirstNotInChain cannot be null");
			if( adj_topo_vertices.Count == 0 )
				return null;
			foreach( IVertex v in adj_topo_vertices )
				if(vertices_in_a_chain[v] == 0)	
					return v;
			return null;
		}

		private ArrayList TopoSortAdjVertices( IVertex v, IIncidenceGraph g, VertexIntDictionary topo_ordering )	
			//	return adjacent vertices to "v" sorted in topological order
		{
			IEdgeEnumerator it = g.OutEdges(v).GetEnumerator();
			bool valid = false;		
			ArrayList adj = new ArrayList();
			while(it.MoveNext())	{
				valid = true;
				adj.Add(it.Current.Target);
			}
			if(!valid)		// no outgoing edges				
				return adj;			
			CompareTopo ctopo = new CompareTopo(topo_ordering);
			SwapTopo stopo = new SwapTopo();
			QuickSorter qs = new QuickSorter(ctopo, stopo);
			qs.Sort(adj);
			return adj;
		}
		#endregion


		#region TopologicalSortImplementation

		//	QuickSorter sorting interfaces
		internal class CompareTopo : IComparer	
		{
			private VertexIntDictionary m_vid;
			public CompareTopo(VertexIntDictionary topological_ordering)	
			{
				m_vid = topological_ordering;
			}
			public int Compare( object x, object y )	
			{
				IVertex x_v = (IVertex)x;
				IVertex y_v = (IVertex)y;
				return (m_vid[x_v]<m_vid[y_v])? -1: ((m_vid[x_v]==m_vid[y_v])? 0 : 1);
			}
		}

		internal class SwapTopo : ISwap
		{
			public void Swap( IList lst, int lindex, int rindex )	
			{
				object temp = lst[lindex];
				lst[lindex] = lst[rindex];
				lst[rindex] = temp;
			}
		}

		#endregion
	
		#region VertexListMatrix Collection Implementation

		internal class VertexListMatrix : CollectionBase
		{
			public VertexListMatrix() : base() {}

			public VertexListMatrix( int create_num_of_rows, int create_num_of_columns )	
			{
				for(int i=0; i<create_num_of_rows; i++)	
				{
					IVertex[] arr = new IVertex[create_num_of_columns];
					base.List.Add(arr);
				}
			}

			public VertexListMatrix( int create_num_of_rows )	
			{
				for(int i=0; i<create_num_of_rows; i++)
					base.List.Add( new ArrayList());
			}

			public void CreateObjectMatrix( int num_of_rows, int num_of_columns, object init_value )	
			{
				object[] arr = null;
				for(int i=0; i<num_of_rows; i++)	
				{
					arr = new object[num_of_columns];
					for(int j=0; j<num_of_columns; j++)
						arr[j] = init_value;
					base.List.Add(arr);				
				}
			}

			public virtual IList this[ IVertex v ]	
			{
				get	
				{
					int position = FetchIndex(v);
					if(position == -1)
						throw new ArgumentException("The vertex supplied to the indexer was not found");
					return (IList) base.List[position];
				}
				set	
				{
					int position = FetchIndex(v);
					if(position == -1)
						throw new ArgumentException("The vertex supplied to the indexer was not found");
					base.List[position] = value;
				}
			}

			private int FetchIndex( IVertex v )	
			{
				//	Get the index corresponding to a Vertex
				for(int index=0; index<base.List.Count; index++)	
				{
					IList curr = (IList) base.List[index];
					if(curr.Count>0)
						if(((IVertex)curr[0]).ID == v.ID)
							return index;
				}
				return -1;
			}

			public virtual IList this[ int index ]	
			{
				get	
				{
					return (IList) base.List[index];
				}
				set	
				{
					base.List[index] = value;				
				}
			}

			public virtual int AddRow( IList array_or_list )	
			{
				if(array_or_list == null)
					throw new ArgumentException("The <array_or_list> argument of the AddRow method overload cannot be null");			
				if(array_or_list.Count != 0)
					if(!(array_or_list[0] is IVertex))
						throw new Exception("The inner ArrayList in VertexListMatrix must contain IVertex elements only");			
				return base.List.Add( array_or_list );
			}

			public virtual int AddRow()	
			{
				//	Add a blank row 
				return base.List.Add(new ArrayList());
			}

			public virtual int AddRow( IVertex vertex )	
			{
				if( vertex == null )
					throw new ArgumentException("The <vertex> argument of the AddRow method overload cannot be null");
				ArrayList temp = new ArrayList();
				temp.Add(vertex);			
				return base.List.Add(temp);
			}

			public int RowCount	
			{
				get	
				{
					return Count;
				}
			}

			public int MaxColumnCount	
			{
				get	
				{
					int max=0;
					foreach(object row in base.List)	
					{
						int temp = ((IList)row).Count;
						max = (temp > max)? temp: max;
					}
					return max;
				}
			}

			public virtual new IList RemoveAt(int index)	
			{
				IList temp = (IList) base.List[index];
				base.List.RemoveAt(index);
				return temp;
			}

			public virtual IList RemoveAt( IVertex v )	
			{
				int position = FetchIndex(v);
				if(position == -1)
					throw new ArgumentException("The vertex supplied to the RemoveAt overload was not found");
				return this.RemoveAt(position);
			}

			public new IEnumerator GetEnumerator()	
			{
				return new VertexListEnumerator(this);
			}

			internal class VertexListEnumerator : IEnumerator
			{
				IEnumerator it; 
		
				public VertexListEnumerator(VertexListMatrix arg)	{ it = ((IList) arg).GetEnumerator(); }

				public IList Current	
				{
					get	
					{
						return (IList)it.Current;
					}
				}
				object IEnumerator.Current	
				{
					get	
					{
						return it.Current;
					}
				}

				public bool MoveNext()	{ return it.MoveNext(); }

				public void Reset()	{ it.Reset(); }
			}
		}

		#endregion
	}

	#region Transitive Closure Vertex Event

	/// <summary>
	/// Encapsulates a vertex in the original graph and it's corresponding vertex in a transformation of the graph
	/// </summary>
	public class TransitiveClosureVertexEventArgs : EventArgs
	{
		private IVertex vOriginal;
		private IVertex vTransform;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="original_vertex">Vertex in original graph</param>
		/// <param name="transform_vertex">Equivalent Vertex in the transformation graph</param>
		public TransitiveClosureVertexEventArgs(IVertex original_vertex, IVertex transform_vertex)	{
			vOriginal = original_vertex; vTransform = transform_vertex;
		}

		/// <summary>
		/// Vertex in original graph
		/// </summary>
		public IVertex VertexInOriginalGraph	{
			get	{
				return vOriginal;
			}
		}

		/// <summary>
		/// Equivalent Vertex in the transformation graph
		/// </summary>
		public IVertex VertexInTransformationGraph	{
			get	{
				return vTransform;
			}
		}
	}

	/// <summary>
	/// Delegate to handle the TransformVertexEvent
	/// </summary>
	public delegate void TransitiveClosureVertexEventHandler( Object sender, TransitiveClosureVertexEventArgs arg );

	#endregion
}
