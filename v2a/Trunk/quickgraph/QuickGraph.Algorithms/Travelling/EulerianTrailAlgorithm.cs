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


using System;
using System.Collections;

namespace QuickGraph.Algorithms.Travelling
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.MutableTraversals;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections.Filtered;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Algorithms.Visitors;
	using QuickGraph.Predicates;

	using QuickGraph.Collections;

	/// <summary>
	/// Under construction
	/// </summary>
	public class EulerianTrailAlgorithm : 
		IAlgorithm
	{
		private IVertexAndEdgeListGraph visitedGraph;
		private EdgeCollection circuit;
		private EdgeCollection temporaryCircuit;
		private IVertex currentVertex;
		private EdgeCollection temporaryEdges;

		/// <summary>
		/// Construct an eulerian trail builder
		/// </summary>
		/// <param name="g"></param>
		public EulerianTrailAlgorithm(IVertexAndEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			visitedGraph = g;
			circuit = new EdgeCollection();
			temporaryCircuit = new EdgeCollection();
			currentVertex = null;
			temporaryEdges = new EdgeCollection();
		}

		/// <summary>
		/// Visited Graph
		/// </summary>
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return visitedGraph;
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
		/// Eulerian circuit on modified graph
		/// </summary>
		public EdgeCollection Circuit
		{
			get
			{
				return circuit;
			}
		}

		/// <summary>
		/// Used internally
		/// </summary>
		internal EdgeCollection TemporaryCircuit
		{
			get
			{
				return temporaryCircuit;
			}
		}

		internal IVertex CurrentVertex
		{
			get
			{
				return currentVertex;
			}
			set
			{
				currentVertex = value;
			}
		}

		internal EdgeCollection TemporaryEdges
		{
			get
			{
				return temporaryEdges;
			}
		}

		private IEdgeEnumerable SelectOutEdgesNotInCircuit(IVertex v)
		{
			return new FilteredEdgeEnumerable(
				VisitedGraph.OutEdges(v),
				new NotInCircuitEdgePredicate(Circuit,TemporaryCircuit)
				);
		}

		private IEdge SelectSingleOutEdgeNotInCircuit(IVertex v)
		{
			IEdgeEnumerable en = this.SelectOutEdgesNotInCircuit(v);
			IEdgeEnumerator eor = en.GetEnumerator();
			if (!eor.MoveNext())
				return null;
			else
				return eor.Current;
		}

		/// <summary>
		/// 
		/// </summary>
		public event EdgeEventHandler TreeEdge;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTreeEdge(IEdge e)
		{
			if (TreeEdge!=null)
				TreeEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// 
		/// </summary>
		public event EdgeEventHandler CircuitEdge;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnCircuitEdge(IEdge e)
		{
			if (CircuitEdge!=null)
				CircuitEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// 
		/// </summary>
		public event EdgeEventHandler VisitEdge;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnVisitEdge(IEdge e)
		{
			if (VisitEdge!=null)
				VisitEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Search a new path to add to the current circuit
		/// </summary>
		/// <param name="u">start vertex</param>
		/// <returns>true if successfull, false otherwize</returns>
		protected bool Search(IVertex u)
		{
			foreach(IEdge e in SelectOutEdgesNotInCircuit(u))
			{
				OnTreeEdge(e);
				IVertex v = e.Target;
				// add edge to temporary path
				TemporaryCircuit.Add(e);
				// e.Target should be equal to CurrentVertex.
				if (e.Target == CurrentVertex)
					return true;

				// continue search
				if (Search(v))
					return true;
				else
					// remove edge
					TemporaryCircuit.Remove(e);
			}

			// it's a dead end.
			return false;
		}


		/// <summary>
		/// Looks for a new path to add to the current vertex.
		/// </summary>
		/// <returns>true if found a new path, false otherwize</returns>
		protected bool Visit()
		{
			// find a vertex that needs to be visited
			foreach(IEdge e in Circuit)
			{
				IEdge fe = SelectSingleOutEdgeNotInCircuit(e.Source);
				if (fe!=null)
				{
					OnVisitEdge(fe);
					CurrentVertex = e.Source;
					if(Search(CurrentVertex))
						return true;
				}
			}

			// Could not augment circuit
			return false;
		}

		/// <summary>
		/// Computes the number of eulerian trail in the graph. If negative,
		/// there is an eulerian circuit.
		/// </summary>
		/// <param name="g"></param>
		/// <returns>number of eulerian trails</returns>
		public static int ComputeEulerianPathCount(IVertexAndEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");

			int odd = AlgoUtility.OddVertices(g).Count;
			if (odd==0)
				return -1;

			if (odd%2!=0)
				return 0;
			else if (odd==0)
				return 1;
			else
				return odd/2;
		}

		/// <summary>
		/// Merges the temporary circuit with the current circuit
		/// </summary>
		/// <returns>true if all the graph edges are in the circuit</returns>
		protected bool CircuitAugmentation()
		{
			EdgeCollection newC=new EdgeCollection();
			int i,j;

			// follow C until w is found
			for(i=0;i<Circuit.Count;++i)
			{
				IEdge e = Circuit[i];
				if (e.Source==CurrentVertex)
					break;
				newC.Add(e);
			}

			// follow D until w is found again
			for(j=0;j<TemporaryCircuit.Count;++j)
			{
				IEdge e = TemporaryCircuit[j];
				newC.Add(e);
				OnCircuitEdge(e);
				if (e.Target==CurrentVertex)
					break;
			}
			TemporaryCircuit.Clear();

			// continue C
			for(;i<Circuit.Count;++i)
			{
				IEdge e = Circuit[i];
				newC.Add(e);
			}

			// set as new circuit
			circuit = newC;

			// check if contains all edges
			if (Circuit.Count == VisitedGraph.EdgesCount)
				return true;

			return false;
		}

		/// <summary>
		/// Computes the eulerian trails
		/// </summary>
		public void Compute()
		{
			CurrentVertex = Traversal.FirstVertex(VisitedGraph);

			// start search
			Search(CurrentVertex);
			if (CircuitAugmentation())
				return; // circuit is found

			do
			{
				if (!Visit())
					break; // visit edges and build path
				if (CircuitAugmentation())
					break; // circuit is found
			} while(true);
		}

		/// <summary>
		/// Adds temporary edges to the graph to make all vertex even.
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public EdgeCollection AddTemporaryEdges(IMutableVertexAndEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");

			// first gather odd edges.
			VertexCollection oddVertices = AlgoUtility.OddVertices(g);

			// check that there are an even number of them
			if (oddVertices.Count%2!=0)
				throw new Exception("number of odd vertices in not even!");

			// add temporary edges to create even edges:
			EdgeCollection ec = new EdgeCollection();

			bool found,foundbe,foundadjacent;
			while (oddVertices.Count > 0)
			{
				IVertex u = oddVertices[0];
				// find adjacent odd vertex.
				found = false;
				foundadjacent = false;
				foreach(IEdge e in g.OutEdges(u))
				{
					IVertex v = e.Target;
					if (v!=u && oddVertices.Contains(v))
					{
						foundadjacent=true;
						// check that v does not have an out-edge towards u
						foundbe = false;
						foreach(IEdge be in g.OutEdges(v))
						{
							if (be.Target==u)
							{
								foundbe = true;
								break;
							}
						}
						if (foundbe)
							continue;
						// add temporary edge
						IEdge tempEdge = g.AddEdge(v,u);
						// add to collection
						ec.Add(tempEdge);
						// remove u,v from oddVertices
						oddVertices.Remove(u);
						oddVertices.Remove(v);
						// set u to null
						found = true;
						break;
					}
				}

				if (!foundadjacent)
				{
					// pick another vertex
					if (oddVertices.Count<2)
						throw new Exception("Eulerian trail failure");
					IVertex v = oddVertices[1];
					IEdge tempEdge = g.AddEdge(u,v);
					// add to collection
					ec.Add(tempEdge);
					// remove u,v from oddVertices
					oddVertices.Remove(u);
					oddVertices.Remove(v);
					// set u to null
					found = true;
					
				}

				if (!found)
				{
					oddVertices.Remove(u);
					oddVertices.Add(u);
				}
			}

			temporaryEdges = ec;

			return ec;			
		}

		/// <summary>
		/// Removes temporary edges
		/// </summary>
		/// <param name="g"></param>
		public void RemoveTemporaryEdges(IEdgeMutableGraph g)
		{
			// remove from graph
			foreach(IEdge e in TemporaryEdges)
				g.RemoveEdge(e);
		}

		/// <summary>
		/// Computes the set of eulerian trails that traverse the edge set.
		/// </summary>
		/// <remarks>
		/// This method returns a set of disjoint eulerian trails. This set
		/// of trails spans the entire set of edges.
		/// </remarks>
		/// <returns>Eulerian trail set</returns>
		public EdgeCollectionCollection Trails()
		{
			EdgeCollectionCollection trails = new EdgeCollectionCollection();

			EdgeCollection trail = new EdgeCollection();
			foreach(IEdge e in this.Circuit)
			{
				if (TemporaryEdges.Contains(e))
				{
					// store previous trail and start new one.
					if(trail.Count != 0)
						trails.Add(trail);
					// start new trail
					trail = new EdgeCollection();
				}
				else
					trail.Add(e);
			}
			if(trail.Count != 0)
				trails.Add(trail);

			return trails;
		}

		/// <summary>
		/// Computes a set of eulerian trail, starting at <paramref name="s"/>
		/// that spans the entire graph.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method computes a set of eulerian trail starting at <paramref name="s"/>
		/// that spans the entire graph.The algorithm outline is as follows:
		/// </para>
		/// <para>
		/// The algorithms iterates throught the Eulerian circuit of the augmented
		/// graph (the augmented graph is the graph with additional edges to make
		/// the number of odd vertices even).
		/// </para>
		/// <para>
		/// If the current edge is not temporary, it is added to the current trail.
		/// </para>
		/// <para>
		/// If the current edge is temporary, the current trail is finished and
		/// added to the trail collection. The shortest path between the 
		/// start vertex <paramref name="s"/> and the target vertex of the
		/// temporary edge is then used to start the new trail. This shortest
		/// path is computed using the <see cref="BreadthFirstSearchAlgorithm"/>.
		/// </para>
		/// </remarks>
		/// <param name="s">start vertex</param>
		/// <returns>eulerian trail set, all starting at s</returns>
		/// <exception cref="ArgumentNullException">s is a null reference.</exception>
		/// <exception cref="Exception">Eulerian trail not computed yet.</exception>
		public EdgeCollectionCollection Trails(IVertex s)
		{
			if (s==null)
				throw new ArgumentNullException("s");
			if (this.Circuit.Count==0)
				throw new Exception("Circuit is empty");

			// find the first edge in the circuit.
			int i=0;
			for(i=0;i<this.Circuit.Count;++i)
			{
				IEdge e = this.Circuit[i];
				if (TemporaryEdges.Contains(e))
					continue;
				if (e.Source == s)
					break;
			}
			if (i==this.Circuit.Count)
				throw new Exception("Did not find vertex in eulerian trail?");

			// create collections
			EdgeCollectionCollection trails = new EdgeCollectionCollection();
			EdgeCollection trail = new EdgeCollection();
			BreadthFirstSearchAlgorithm bfs =
				new BreadthFirstSearchAlgorithm(VisitedGraph);
			PredecessorRecorderVisitor vis = new PredecessorRecorderVisitor();
			bfs.RegisterPredecessorRecorderHandlers(vis);
			bfs.Compute(s);

			// go throught the edges and build the predecessor table.
			int start = i;
			for (;i<this.Circuit.Count;++i)
			{
				IEdge e = this.Circuit[i];
				if (TemporaryEdges.Contains(e))
				{
					// store previous trail and start new one.
					if(trail.Count != 0)
						trails.Add(trail);
					// start new trail
					// take the shortest path from the start vertex to
					// the target vertex
					trail = vis.Path(e.Target);
				}
				else
					trail.Add(e);
			}

			// starting again on the circuit
			for (i=0;i<start;++i)
			{
				IEdge e = this.Circuit[i];
				if (TemporaryEdges.Contains(e))
				{
					// store previous trail and start new one.
					if(trail.Count != 0)
						trails.Add(trail);
					// start new trail
					// take the shortest path from the start vertex to
					// the target vertex
					trail = vis.Path(e.Target);
				}
				else
					trail.Add(e);
			}

			// adding the last element
			if (trail.Count!=0)
				trails.Add(trail);
		
			return trails;
		}
	}
}
