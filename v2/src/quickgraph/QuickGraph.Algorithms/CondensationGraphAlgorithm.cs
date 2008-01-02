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
//		QuickGraph Library HomePage: http://www.mbunit.com

using System;
using System.Collections;
using QuickGraph.Algorithms;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.MutableTraversals;
using QuickGraph.Collections;
using QuickGraph.Concepts.Collections;
using QuickGraph.Collections.Sort;

namespace QuickGraph.Algorithms
{
	/// <summary>
	/// Creates a condensation graph transformation
	/// </summary>
	/// <author name="Rohit Gadogkar" email="rohit.gadagkar@gmail.com" />
	public class CondensationGraphAlgorithm : IAlgorithm
	{
		private IVertexListGraph visitedGraph;
		private VertexIntDictionary components;
		private SortedList sccVertexMap;

		/// <summary>
		/// Condensation Graph constructor
		/// </summary>
		/// <param name="g">Input graph from 
		/// which condensation graph is created</param>
		public CondensationGraphAlgorithm(IVertexListGraph g)	{
			if (g==null)
				throw new ArgumentNullException("g");
			this.visitedGraph = g;
			this.components = null;
		}

		#region Properties

		/// <summary>
		/// Visited graph
		/// </summary>
		public IVertexListGraph VisitedGraph	
		{
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
		/// Maps a graph vertex to a strongly connected component
		/// </summary>
		/// <value>Map of IVertex to strongly connected component ID</value>
		public VertexIntDictionary VertexToSCCMap	{
			get	{
				return this.components;
			}
		}

		/// <summary>
		/// Read only map of vertices within each strongly connected component
		/// </summary>
		/// <value>map with StronglyConnectedComponent ID as key and IList of vertices as value</value>
		public SortedList SCCVerticesMap	{
			get	{
				return sccVertexMap;
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Raised when a new vertex is added in the condensation graph
		/// </summary>
		public event CondensationGraphVertexEventHandler InitCondensationGraphVertex;

		/// <summary>
		/// Raise the CondensationGraphVertex evt
		/// </summary>
		/// <param name="arg">Pack the CG vertex and a VertexCollection of it's constituent vertices</param>
		protected void OnInitCondensationGraphVertex(CondensationGraphVertexEventArgs arg)	{
			if(InitCondensationGraphVertex != null)
				InitCondensationGraphVertex(this, arg);
		}
		#endregion

		/// <summary>
		/// Clear the extracted strongly connected components
		/// </summary>
		public void ClearComponents()	
		{
			this.components = null;
			if(sccVertexMap != null)
				sccVertexMap.Clear();
		}

		internal void ComputeComponents()	
        {	
			if( components == null )
				components = new VertexIntDictionary();
			//	components is a MAP containing vertex number as key & component_id as value. It maps every vertex in input graph to SCC vertex ID which contains it
			StrongComponentsAlgorithm algo = new StrongComponentsAlgorithm(
				VisitedGraph,
				this.components
				);
			algo.Compute();			
		}

		private SortedList BuildSCCVertexMap( VertexIntDictionary vSccMap )	
        {
			//	Construct a map of SCC ID as key & IVertexCollection of vertices contained within the SCC as value
			SortedList h = new SortedList();
			VertexCollection vertices = null;
			foreach( DictionaryEntry de in vSccMap )	
            {
				IVertex v = (IVertex) de.Key;
				int scc_id = (int) de.Value;
				if( h.ContainsKey(scc_id) )
					((VertexCollection) h[scc_id]).Add(v);
				else	
                {
					vertices = new VertexCollection();
					vertices.Add(v);
					h.Add(scc_id, vertices);
				}
			}
			return h;
		}

		/// <summary>
		/// Compute the condensation graph and store it in the supplied graph 'cg'
		/// </summary>
		/// <param name="cg">
		/// Instance of mutable graph in which the condensation graph 
		/// transformation is stored
		/// </param>
		public void Create( IMutableVertexAndEdgeListGraph cg )	
        {
			if (cg==null)
				throw new ArgumentNullException("cg");			

			if (components==null)
				ComputeComponents();			
					
			//  components list contains collection of 
            // input graph Vertex for each SCC Vertex_ID 
            // (i.e Vector< Vector<Vertex> > )
			//	Key = SCC Vertex ID 
			sccVertexMap = BuildSCCVertexMap(components);
			
			//	Lsit of SCC vertices
			VertexCollection toCgVertices = new VertexCollection();
			IDictionaryEnumerator it = sccVertexMap.GetEnumerator();
			while( it.MoveNext() )	
				//	as scc_vertex_map is a sorted list, order of SCC IDs will match CG vertices
			{
				IVertex curr = cg.AddVertex();
				OnInitCondensationGraphVertex(new CondensationGraphVertexEventArgs(curr, (IVertexCollection)it.Value));
				toCgVertices.Add(curr);
			}
			
			for( int srcSccId=0; srcSccId<sccVertexMap.Keys.Count; srcSccId++ )	
            {   
				VertexCollection adj = new VertexCollection();
				foreach( IVertex u in (IVertexCollection)sccVertexMap[srcSccId] )	
                {					
					foreach(IEdge e in VisitedGraph.OutEdges(u))	{
						IVertex v = e.Target;        			
						int targetSccId = components[v];
						if (srcSccId != targetSccId)	
                        {
							// Avoid loops in the condensation graph
							IVertex sccV = toCgVertices[targetSccId];
							if( !adj.Contains(sccV) )		// Avoid parallel edges
								adj.Add(sccV);
						}
					}
				}
				IVertex s = toCgVertices[srcSccId];
				foreach( IVertex t in adj )
					cg.AddEdge(s, t);
			}
		}

	}

	#region Condensation Graph Vertex Event

	/// <summary>
	/// Encapsulates a vertex in the original graph and it's corresponding 
	/// vertex in a transformation of the graph
	/// </summary>
	public class CondensationGraphVertexEventArgs : EventArgs
	{
		private IVertex vCG;
		private IVertexCollection stronglyConnectedVertices;

		/// <summary>
		/// ctor()
		/// </summary>
		/// <param name="cgVertex">Vertex in the condensation graph</param>
		/// <param name="stronglyConnectedVertices">strongly connected 
		/// components 
		/// in the original graph which are represented by the condensation 
		/// graph node</param>
		public CondensationGraphVertexEventArgs(
            IVertex cgVertex, 
            IVertexCollection stronglyConnectedVertices)	
		{
			this.vCG = cgVertex; 
            this.stronglyConnectedVertices = stronglyConnectedVertices;
		}

		/// <summary>
		/// Condensation graph vertex
		/// </summary>
		public IVertex CondensationGraphVertex	
		{
			get	
            {
				return vCG;
			}
		}

		/// <summary>
		/// Strongly connected vertices from original graph represented by the 
		/// condensation graph node
		/// </summary>
		public IVertexCollection StronglyConnectedVertices	
		{
			get	
            {
				return stronglyConnectedVertices;
			}
		}
	}

	/// <summary>
	/// Delegate to handle the CondensationGraphVertexEvent
	/// </summary>
	public delegate void CondensationGraphVertexEventHandler( 
		Object sender, 
		CondensationGraphVertexEventArgs arg );

	#endregion

}
