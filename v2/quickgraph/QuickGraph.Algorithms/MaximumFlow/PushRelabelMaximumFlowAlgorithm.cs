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
	/// <summary>Push-Relabel Maximum Flow Algorithm</summary>
	/// <remarks>
	/// <para>The <see cref="PushRelabelMaximumFlowAlgorithm"/> class calculates the 
	/// maximum flow of a network. The calculated maximum flow will be the return value of 
	/// the <see cref="PushRelabelMaximumFlowAlgorithm.Compute"/>function. The function 
	/// also calculates the flow values <c>f(u,v)</c> for all <c>(u,v)</c> in E, which 
	/// are returned in the form of the residual capacity <c>r(u,v) = c(u,v) - f(u,v)</c>.
	/// </para>
	/// <para>
	///There are several special requirements on the input graph and property map 
	///parameters for this algorithm. First, the directed graph <c>G=(V,E)</c> that 
	///represents the network must be augmented to include the reverse edge for every 
	///edge in E. That is, the input graph should be <c>Gin = (V,{E U E^T})</c>. 
	///The <c>reversedEdges</c> argument must map each edge in the original graph to 
	///its reverse edge, that is <c>(u,v) -> (v,u)</c> for all <c>(u,v)</c> in E. 
	///The <c>capacities</c> argument must map each edge in E to a 
	///positive number, and each edge in E^T to 0. 
	///</para>
	///<para>
	///This algorithm was developed by Goldberg. 
	///</para>
	/// </remarks>
	public class PushRelabelMaximumFlowAlgorithm : MaximumFlowAlgorithm
	{
		private const double globalUpdateFrequency = 0.5;
		private const int alpha = 6;
		private const int beta = 12;

		private IIndexedVertexListGraph visitedGraph;
		private int 
			n, 
			nm, 
			maxDistance, 
			minActive, 
			maxActive, 
			workSinceLastUpdate;
		private IVertex src, sink;
		private VertexDoubleDictionary excessFlow;
		private VertexIntDictionary current;
		private VertexIntDictionary distances;
		private PreflowLayer[] layers;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="capacities"></param>
		/// <param name="reversedEdges"></param>
		public PushRelabelMaximumFlowAlgorithm(
			IIndexedVertexListGraph g, 
			EdgeDoubleDictionary capacities,
			EdgeEdgeDictionary reversedEdges
			)
			: base(g,capacities,reversedEdges)
		{
			this.visitedGraph = g;
			this.excessFlow = new VertexDoubleDictionary();
			this.current = new VertexIntDictionary();
			this.distances = new VertexIntDictionary();
		}

		#region Properties
		new public IIndexedVertexListGraph VisitedGraph
		{
			get { return this.visitedGraph; }
		}

		public VertexIntDictionary Distances
		{
			get { return this.distances; }
		}
		
		internal IVertexListGraph ResidualGraph
		{
			get
			{
				return new FilteredVertexListGraph(
					this.VisitedGraph,
					new ReversedResidualEdgePredicate(this.ResidualCapacities, this.ReversedEdges)
					);
			}
		}
		#endregion

		/// <summary>
		/// Computes the maximum flow between <paramref name="src"/> and
		/// <paramref name="sink"/>.
		/// </summary>
		/// <param name="src">The source node of the graph.</param>
		/// <param name="sink">The sink node of the graph.</param>
		/// <returns>The maximum flow of the graph.</returns>
		public override double Compute(IVertex src, IVertex sink)
		{    
			if (src==null)
				throw new ArgumentNullException("src");
			if (sink==null)
				throw new ArgumentNullException("sink");

			this.src = src;
			this.sink = sink;

			Initialize();
			double flow = MaximumPreflow();
			ConvertPreflowToFlow();

			return flow;
		}

		private void Initialize()
		{
			int m = 0;
			n = VisitedGraph.VerticesCount;
			foreach (IVertex u in VisitedGraph.Vertices)
				m += VisitedGraph.OutDegree(u);
			// Don't count the reverse edges
			m /= 2;
			nm = alpha * n + m;

			excessFlow.Clear();
			current.Clear();
			distances.Clear();

			layers = new PreflowLayer[n];
			for (int i = 0; i < n; i++)
				layers[i] = new PreflowLayer();

			// Initialize flow to zero which means initializing
			// the residual capacity to equal the capacity.
			foreach (IVertex u in VisitedGraph.Vertices)
			{
				foreach (IEdge e in VisitedGraph.OutEdges(u))
					this.ResidualCapacities[e] = this.Capacities[e];

				excessFlow[u] = 0;
				current[u] = 0;
			}

			bool overflowDetected = false;
			double testExcess = 0;

			foreach (IEdge a in VisitedGraph.OutEdges(src))
			{
				if (a.Target != src)
					testExcess += this.ResidualCapacities[a];
			}

			if (testExcess >= double.MaxValue || double.IsPositiveInfinity(testExcess))
				overflowDetected = true;

			if (overflowDetected)
			{
				excessFlow[src] = double.MaxValue;
			}
			else
			{
				excessFlow[src] = 0;
				foreach (IEdge a in this.VisitedGraph.OutEdges(src))
				{
					if (a.Target != src) 
					{
						double delta = this.ResidualCapacities[a];
						this.ResidualCapacities[a] -= delta;
						this.ResidualCapacities[this.ReversedEdges[a]] += delta;
						this.excessFlow[a.Target] += delta;
					}
				}
			}

			maxDistance = VisitedGraph.VerticesCount - 1;
			maxActive = 0;
			minActive = n;

			foreach (IVertex u in this.VisitedGraph.Vertices) 
			{
				if (u == sink)
				{
					distances[u] = 0;
					continue;
				}
				else if (u == src && !overflowDetected)
				{
					distances[u] = n;
				}
				else
				{
					distances[u] = 1;
				}

				if (excessFlow[u] > 0)
					this.AddToActiveList(u, layers[1]);
				else if (distances[u] < n)
					this.AddToInactiveList(u, layers[1]);
			}
		}

		/// <summary>
		/// This is the core part of the algorithm, "phase one."
		/// </summary>
		/// <returns></returns>
		private double MaximumPreflow()
		{
			workSinceLastUpdate = 0;

			while (maxActive >= minActive)		// "main" loop
			{
				PreflowLayer layer = layers[maxActive];

				if (layer.ActiveVertices.Count == 0)
					--maxActive;
				else
				{
					IVertex u = layer.ActiveVertices[0];
					RemoveFromActiveList(u);

					Discharge(u);

					if (workSinceLastUpdate * globalUpdateFrequency > nm) 
					{
						GlobalDistanceUpdate();
						workSinceLastUpdate = 0;
					}
				}
			}

			return excessFlow[sink];
		}


		//=======================================================================
		// remove excess flow, the "second phase"
		// This does a DFS on the reverse flow graph of nodes with excess flow.
		// If a cycle is found, cancel it.  Unfortunately it seems that we can't
		// easily take advantage of DepthFirstSearchAlgorithm
		// Return the nodes with excess flow in topological order.
		//
		// Unlike the prefl_to_flow() implementation, we use
		//   "color" instead of "distance" for the DFS labels
		//   "parent" instead of nl_prev for the DFS tree
		//   "topo_next" instead of nl_next for the topological ordering
		private void ConvertPreflowToFlow()
		{
			IVertex r, restart, u;
			IVertex bos = null, tos = null;

			VertexVertexDictionary parents = new VertexVertexDictionary();
			VertexVertexDictionary topoNext = new VertexVertexDictionary();
			
			foreach (IVertex v in VisitedGraph.Vertices) 
			{
				//Handle self-loops
				foreach (IEdge a in VisitedGraph.OutEdges(v))
					if (a.Target == v)
						ResidualCapacities[a] = Capacities[a];

				//Initialize
				Colors[v] = GraphColor.White;
				parents[v] = v;
				current[v] = 0;
			}

			// eliminate flow cycles and topologically order the vertices
			IVertexEnumerator vertices = VisitedGraph.Vertices.GetEnumerator();
			while (vertices.MoveNext())
			{
				u = vertices.Current;
				if (Colors[u] == GraphColor.White 
					&& excessFlow[u] > 0
					&& u != src && u != sink)
				{
					r = u;
					Colors[r] = GraphColor.Gray;

					while(true) 
					{
						for (; current[u] < VisitedGraph.OutDegree(u); ++current[u]) 
						{
							IEdge a = VisitedGraph.OutEdges(u)[current[u]];

							if (Capacities[a] == 0 && IsResidualEdge(a))
							{
								IVertex v = a.Target;
								if (Colors[v] == GraphColor.White) 
								{
									Colors[v] = GraphColor.Gray;
									parents[v] = u;
									u = v;
									break;
								} 
								else if (Colors[v] == GraphColor.Gray) 
								{
									//find minimum flow on the cycle
									double delta = ResidualCapacities[a];
									while (true)
									{
										IEdge e = VisitedGraph.OutEdges(v)[current[v]];
										delta = Math.Min(delta, ResidualCapacities[e]);
										if (v == u)
											break;
										else
											v = e.Target;
									}

									//remove delta flow units
									v = u;
									while (true) 
									{
										a = VisitedGraph.OutEdges(v)[current[v]];
										ResidualCapacities[a] -= delta;
										ResidualCapacities[ReversedEdges[a]] += delta;
										v = a.Target;
										if (v == u)
											break;
									}

									// back-out of DFS to the first saturated edge
									restart = u;

									for (v = VisitedGraph.OutEdges(u)[current[u]].Target;
										v != u; v = a.Target) 
									{
										a = VisitedGraph.OutEdges(v)[current[v]];

										if (Colors[v] == GraphColor.White
											|| IsSaturated(a)) 
										{
											Colors[VisitedGraph.OutEdges(v)[current[v]].Target] = GraphColor.White;
											if (Colors[v] != GraphColor.White)
												restart = v;
										}
									}
									
									if (restart != u) 
									{
										u = restart;
										++current[u];
										break;
									}
								}
							}
						}

						if (current[u] == VisitedGraph.OutDegree(u))
						{
							// scan of i is complete
							Colors[u] = GraphColor.Black;

							if (u != src) 
							{
								if (bos == null) 
								{
									bos = u;
									tos = u;
								}
								else 
								{
									topoNext[u] = tos;
									tos = u;
								}
							}
							if (u != r)
							{
								u = parents[u];
								++current[u];
							}
							else
								break;
						}
					}
				}
			}

			// return excess flows
			// note that the sink is not on the stack
			if (bos != null)
			{
				IEdgeEnumerator ai;

				for (u = tos; u != bos; u = topoNext[u])
				{
					ai = VisitedGraph.OutEdges(u).GetEnumerator();

					while (excessFlow[u] > 0 && ai.MoveNext())
					{
						if (Capacities[ai.Current] == 0 && IsResidualEdge(ai.Current))
							PushFlow(ai.Current);
					}
				}
				// do the bottom
				u = bos;
				ai = VisitedGraph.OutEdges(u).GetEnumerator();
				while (excessFlow[u] > 0 && ai.MoveNext())
				{
					if (Capacities[ai.Current] == 0 && IsResidualEdge(ai.Current))
						PushFlow(ai.Current);
				}
			}
		}

		//=======================================================================
		// This is a breadth-first search over the residual graph
		// (well, actually the reverse of the residual graph).
		// Would be cool to have a graph view adaptor for hiding certain
		// edges, like the saturated (non-residual) edges in this case.
		// Goldberg's implementation abused "distance" for the coloring.
		private void GlobalDistanceUpdate()
		{
			foreach (IVertex u in VisitedGraph.Vertices) 
			{
				Colors[u] = GraphColor.White;
				Distances[u] = n;
			}
			Distances[sink] = 0;

			for (int l = 0; l <= maxDistance; ++l)
			{
				layers[l].ActiveVertices.Clear();
				layers[l].InactiveVertices.Clear();
			}
			
			maxDistance = maxActive = 0;
			minActive = n;

			BreadthFirstSearchAlgorithm bfs = new BreadthFirstSearchAlgorithm(
				ResidualGraph, 
				new VertexBuffer(), 
				Colors
				);

			DistanceRecorderVisitor vis = new DistanceRecorderVisitor(Distances);
			bfs.TreeEdge += new EdgeEventHandler(vis.TreeEdge);
			bfs.DiscoverVertex += new VertexEventHandler(GlobalDistanceUpdateHelper);
			bfs.Compute(sink);
		}

		private void GlobalDistanceUpdateHelper(object sender, VertexEventArgs e)
		{
			if (e.Vertex != sink) 
			{
				current[e.Vertex] = 0;
				maxDistance = Math.Max(distances[e.Vertex], maxDistance);
			
				if (excessFlow[e.Vertex] > 0)
					AddToActiveList(e.Vertex, layers[distances[e.Vertex]]);
				else
					AddToInactiveList(e.Vertex, layers[distances[e.Vertex]]);
			}
		}


		//=======================================================================
		// This function is called "push" in Goldberg's h_prf implementation,
		// but it is called "discharge" in the paper and in hi_pr.c.
		private void Discharge(IVertex u)
		{
			while (true)
			{
				int ai;
				for (ai = current[u]; ai < VisitedGraph.OutDegree(u); ai++)
				{
					IEdge a = VisitedGraph.OutEdges(u)[ai];
					if (IsResidualEdge(a)) 
					{
						IVertex v = a.Target;
						if (IsAdmissible(u, v))
						{
							if (v != sink && excessFlow[v] == 0)
							{
								RemoveFromInactiveList(v);
								AddToActiveList(v, layers[distances[v]]);
							}
							PushFlow(a);
							if (excessFlow[u] == 0)
								break;
						}
					}
				}

				PreflowLayer layer = layers[distances[u]];
				int du = distances[u];

				if (ai == VisitedGraph.OutDegree(u))		// i must be relabeled
				{
					RelabelDistance(u);
					if (layer.ActiveVertices.Count == 0 
						&& layer.InactiveVertices.Count == 0)
						Gap(du);
					if (distances[u] == n)
						break;
				} 
				else 
				{								// i is no longer active
					current[u] = ai;
					AddToInactiveList(u, layer);
					break;
				}
			}
		}

		//=======================================================================
		// This corresponds to the "push" update operation of the paper,
		// not the "push" function in Goldberg's h_prf.c implementation.
		// The idea is to push the excess flow from from vertex u to v.
		private void PushFlow(IEdge e)
		{
			IVertex u = e.Source;
			IVertex v = e.Target;

			double flowDelta = Math.Min(excessFlow[u], ResidualCapacities[e]);
			ResidualCapacities[e] -= flowDelta;
			ResidualCapacities[ReversedEdges[e]] += flowDelta;

			excessFlow[u] -= flowDelta;
			excessFlow[v] += flowDelta;
		}

		//=======================================================================
		// The main purpose of this routine is to set distance[v]
		// to the smallest value allowed by the valid labeling constraints,
		// which are:
		// distance[t] = 0
		// distance[u] <= distance[v] + 1   for every residual edge (u,v)
		//
		private int RelabelDistance(IVertex u)
		{
			int minEdges = 0;

			workSinceLastUpdate += beta;

			int minDistance = VisitedGraph.VerticesCount;
			distances[u] = minDistance;

			// Examine the residual out-edges of vertex i, choosing the
			// edge whose target vertex has the minimal distance.
			for (int ai = 0; ai < VisitedGraph.OutDegree(u); ai++) {
				++workSinceLastUpdate;
				IEdge a = VisitedGraph.OutEdges(u)[ai];
				IVertex v = a.Target;
				if (IsResidualEdge(a) && distances[v] < minDistance)
				{
					minDistance = distances[v];
					minEdges = ai;
				}
			}
			++minDistance;

			if (minDistance < n)
			{
				distances[u] = minDistance;		// this is the main action
				current[u] = minEdges;
				maxDistance = Math.Max(minDistance, maxDistance);
			}
			return minDistance;
		}

		//=======================================================================
		// cleanup beyond the gap
		private void Gap(int emptyDistance)
		{
			int r;  // distance of layer before the current layer
			r = emptyDistance - 1;

			// Set the distance for the vertices beyond the gap to "infinity".
			for (int l = emptyDistance + 1; l < maxDistance; ++l)
			{
				foreach (IVertex v in layers[l].InactiveVertices)
					distances[v] = n;
				layers[l].InactiveVertices.Clear();
			}
			maxDistance = r;
			maxActive = r;
		}

		private bool IsFlow()
		{
			// check edge flow values
			foreach (IVertex u in VisitedGraph.Vertices) 
			{
				foreach (IEdge a in VisitedGraph.OutEdges(u))
				{
					if (Capacities[a] > 0)
						if ((ResidualCapacities[a] + ResidualCapacities[ReversedEdges[a]] 
							!= Capacities[a] + Capacities[ReversedEdges[a]])
							|| (ResidualCapacities[a] < 0)
							|| (ResidualCapacities[ReversedEdges[a]] < 0))
							return false;
				}
			}

			// check conservation
			double sum;
			foreach (IVertex u in VisitedGraph.Vertices)
			{
				if (u != src && u != sink) 
				{
					if (excessFlow[u] != 0)
						return false;
					sum = 0;
					foreach (IEdge a in VisitedGraph.OutEdges(u))
						if (Capacities[a] > 0)
							sum -= Capacities[a] - ResidualCapacities[a];
						else
							sum += ResidualCapacities[a];

					if (excessFlow[u] != sum)
						return false;
				}
			}
			return true;
		}

		private bool IsOptimal()
		{
			// check if mincut is saturated...
			GlobalDistanceUpdate();
			return Distances[src] >= n;
		}

		//=======================================================================
		// Some helper predicates

		private bool IsAdmissible(IVertex u, IVertex v) 
		{
			return distances[u] == distances[v] + 1;
		}

		private bool IsResidualEdge(IEdge a)
		{
			return 0 < ResidualCapacities[a];
		}

		private bool IsSaturated(IEdge a)
		{
			return ResidualCapacities[a] == 0;
		}

		#region PreflowLayer Management
		private void AddToActiveList(IVertex u, PreflowLayer layer)
		{
			layer.ActiveVertices.Insert(0, u);
			maxActive = Math.Max(Distances[u], maxActive);
			minActive = Math.Min(Distances[u], minActive);
		}

		private void RemoveFromActiveList(IVertex u)
		{
			layers[Distances[u]].ActiveVertices.Remove(u);
		}

		private void AddToInactiveList(IVertex u, PreflowLayer layer)
		{
			layer.InactiveVertices.Insert(0, u);
		}

		private void RemoveFromInactiveList(IVertex u)
		{
			layers[distances[u]].InactiveVertices.Remove(u);
		}

        private class PreflowLayer 
		{
			private VertexCollection activeVertices;
			private VertexCollection inactiveVertices;

			public PreflowLayer() 
			{
				this.activeVertices = new VertexCollection();
				this.inactiveVertices = new VertexCollection();
			}

			#region Properties
			public VertexCollection ActiveVertices
			{
				get { return this.activeVertices; }
			}

			public VertexCollection InactiveVertices
			{
				get { return this.inactiveVertices; }
			}
			#endregion
		}
		#endregion

	}
}
