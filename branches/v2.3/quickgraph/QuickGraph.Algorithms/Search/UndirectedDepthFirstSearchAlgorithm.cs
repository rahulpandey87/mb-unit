// QuickGraph Library 
// 
// Copyright (c) 2004 Jonathan de Halleux
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
//		Author: Jonathan de Halleux


namespace QuickGraph.Algorithms.Search
{
	using System;
	using System.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Visitors;

	/// <summary>
	/// Performs a undirected (depth first and height first) depth first
	/// search on a directed bidirectional graph.
	/// </summary>
	/// <remarks>
	/// <para>This algorithm is directly inspired from the
	/// BoostGraphLibrary implementation.
	/// </para> 
	/// </remarks>
	public class UndirectedDepthFirstSearchAlgorithm :
		IAlgorithm,
		IVertexColorizerAlgorithm,
		IPredecessorRecorderAlgorithm, 
		ITimeStamperAlgorithm
	{
		private IBidirectionalVertexAndEdgeListGraph visitedGraph;
		private VertexColorDictionary colors;
		private EdgeColorDictionary edgeColors;

		/// <summary>
		/// Create a undirected dfs algorithm
		/// </summary>
		/// <param name="g">Graph to search on.</param>
        public UndirectedDepthFirstSearchAlgorithm(IBidirectionalVertexAndEdgeListGraph g)
        {
			if(g == null)
				throw new ArgumentNullException("g");
			visitedGraph = g;
			edgeColors=new EdgeColorDictionary();
			colors = new VertexColorDictionary();
		}

		/// <summary>
		/// Visited graph
		/// </summary>
        public IBidirectionalVertexAndEdgeListGraph VisitedGraph
        {
			get
			{
				return visitedGraph;
			}
		}

		Object IAlgorithm.VisitedGraph
		{
			get
			{
				return this.VisitedGraph;
			}
		}

		/// <summary>
		/// Vertex color map
		/// </summary>
		public VertexColorDictionary Colors
		{
			get
			{
				return colors;
			}
		}

		IDictionary IVertexColorizerAlgorithm.Colors
		{
			get
			{
				return this.Colors;
			}
		}

		/// <summary>
		/// Edge color map
		/// </summary>
		public EdgeColorDictionary EdgeColors
		{
			get
			{
				return edgeColors;
			}	
		}

		#region Events
		/// <summary>
		/// Invoked on every vertex of the graph before the start of the graph 
		/// search.
		/// </summary>
		public event VertexEventHandler InitializeVertex;

		/// <summary>
		/// Raises the <see cref="InitializeVertex"/> event.
		/// </summary>
		/// <param name="v">vertex that raised the event</param>
		protected void OnInitializeVertex(IVertex v)
		{
			if (InitializeVertex!=null)
				InitializeVertex(this, new VertexEventArgs(v));
		}

		/// <summary>
		/// Invoked on the source vertex once before the start of the search. 
		/// </summary>
		public event VertexEventHandler StartVertex;

		/// <summary>
		/// Raises the <see cref="StartVertex"/> event.
		/// </summary>
		/// <param name="v">vertex that raised the event</param>
		protected void OnStartVertex(IVertex v)
		{
			if (StartVertex!=null)
				StartVertex(this, new VertexEventArgs(v));
		}

		/// <summary>
		/// Invoked when a vertex is encountered for the first time. 
		/// </summary>
		public event VertexEventHandler DiscoverVertex;


		/// <summary>
		/// Raises the <see cref="DiscoverVertex"/> event.
		/// </summary>
		/// <param name="v">vertex that raised the event</param>
		protected void OnDiscoverVertex(IVertex v)
		{
			if (DiscoverVertex!=null)
				DiscoverVertex(this, new VertexEventArgs(v));
		}

		/// <summary>
		/// Invoked on every out-edge of each vertex after it is discovered. 
		/// </summary>
		public event EdgeEventHandler ExamineEdge;


		/// <summary>
		/// Raises the <see cref="ExamineEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnExamineEdge(IEdge e)
		{
			if (ExamineEdge!=null)
				ExamineEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on each edge as it becomes a member of the edges that form 
		/// the search tree. If you wish to record predecessors, do so at this 
		/// event point. 
		/// </summary>
		public event EdgeEventHandler TreeEdge;


		/// <summary>
		/// Raises the <see cref="TreeEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnTreeEdge(IEdge e)
		{
			if (TreeEdge!=null)
				TreeEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on the back edges in the graph. 
		/// </summary>
		public event EdgeEventHandler BackEdge;


		/// <summary>
		/// Raises the <see cref="BackEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnBackEdge(IEdge e)
		{
			if (BackEdge!=null)
				BackEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on a vertex after all of its out edges have been added to 
		/// the search tree and all of the adjacent vertices have been 
		/// discovered (but before their out-edges have been examined). 
		/// </summary>
		public event VertexEventHandler FinishVertex;

		/// <summary>
		/// Raises the <see cref="FinishVertex"/> event.
		/// </summary>
		/// <param name="v">vertex that raised the event</param>
		protected void OnFinishVertex(IVertex v)
		{
			if (FinishVertex!=null)
				FinishVertex(this, new VertexEventArgs(v));
		}
		#endregion

		/// <summary>
		/// Computes the dfs
		/// </summary>
		public void Compute()
		{
			Compute(null);
		}

		/// <summary>
		/// Computes the dfs starting at s
		/// </summary>
		/// <param name="s">start vertex</param>
		public void Compute(IVertex s)
		{
			// init vertices
			foreach(IVertex u in VisitedGraph.Vertices)
			{
				Colors[u]=GraphColor.White;
				OnInitializeVertex(u);
			}
			//init edges
			foreach(IEdge e in VisitedGraph.Edges)
			{
				EdgeColors[e]=GraphColor.White;
			}

			// use start vertex
			if (s != null)
			{
				OnStartVertex(s);
				Visit(s);
			}
			// visit vertices
			foreach(IVertex v in VisitedGraph.Vertices)
			{
				if (Colors[v] == GraphColor.White)
				{
					OnStartVertex(v);
					Visit(v);
				}
			}
		}

		/// <summary>
		/// Visits vertex s
		/// </summary>
		/// <param name="u">vertex to visit</param>
		public void Visit(IVertex u)
		{
			VertexEventArgs uArgs = new VertexEventArgs(u);

			Colors[u]=GraphColor.Gray;
			OnDiscoverVertex(u);

            IEdgeEnumerable outEdges = VisitedGraph.OutEdges(u);
            VisitEdges(outEdges,true);
            IEdgeEnumerable intEdges = VisitedGraph.InEdges(u);
            VisitEdges(intEdges,false);

            Colors[u]=GraphColor.Black;
			OnFinishVertex(u);
		}

        private void VisitEdges(IEdgeEnumerable outEdges, bool forward)
        {
            IVertex target=null;
            foreach (IEdge e in outEdges)
            {
                OnExamineEdge(e);

                if (forward)
                    target = e.Target;
                else
                    target = e.Source;

                GraphColor vc = Colors[target];
                GraphColor ec = EdgeColors[e];

                EdgeColors[e] = GraphColor.Black;
                if (vc == GraphColor.White) // tree edge
                {
                    OnTreeEdge(e);
                    Visit(target);
                }
                else if (vc == GraphColor.Gray && ec == GraphColor.White)
                {
                    OnBackEdge(e);
                }

            }
        }

        /// <summary>
		/// Registers the predecessors handler
		/// </summary>
		/// <param name="vis"></param>
		public void RegisterPredecessorRecorderHandlers(IPredecessorRecorderVisitor vis)
		{
			if (vis == null)
				throw new ArgumentNullException("visitor");
			TreeEdge += new EdgeEventHandler(vis.TreeEdge);
			FinishVertex += new VertexEventHandler(vis.FinishVertex);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vis"></param>
		public void RegisterTimeStamperHandlers(ITimeStamperVisitor vis)
		{
			if (vis == null)
				throw new ArgumentNullException("visitor");

			DiscoverVertex += new VertexEventHandler(vis.DiscoverVertex);
			FinishVertex += new VertexEventHandler(vis.FinishVertex);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vis"></param>
		public void RegisterVertexColorizerHandlers(IVertexColorizerVisitor vis)
		{
			if (vis == null)
				throw new ArgumentNullException("visitor");

			InitializeVertex += new VertexEventHandler(vis.InitializeVertex);
			DiscoverVertex += new VertexEventHandler(vis.DiscoverVertex);
			FinishVertex += new VertexEventHandler(vis.FinishVertex);
		}
	}
}
