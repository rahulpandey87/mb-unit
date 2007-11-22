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

namespace QuickGraph.Algorithms.RandomWalks
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Visitors;
	using QuickGraph.Collections;

	/// <summary>
	/// Wilson-Propp Cycle-Popping Algorithm for Random Tree Generation.
	/// </summary>
	/// <include file='QuickGraph.Algorithms.Doc.xml' path='doc/remarkss/remarks[@name="CyclePoppingRandomTreeAlgorithm"]'/>
	public class CyclePoppingRandomTreeAlgorithm : 
		IAlgorithm
	{
		private IVertexListGraph visitedGraph = null;
		private VertexColorDictionary colors = new VertexColorDictionary();
		private IMarkovEdgeChain edgeChain = new NormalizedMarkovEdgeChain();
		private VertexEdgeDictionary successors = new VertexEdgeDictionary();
		private Random rnd = new Random((int)DateTime.Now.Ticks);

		/// <summary>
		/// Constructs the algorithm around <paramref name="g"/>.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <param name="g">visted graph</param>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		public CyclePoppingRandomTreeAlgorithm(
			IVertexListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			this.visitedGraph = g;
		}

		/// <summary>
		/// Constructs the algorithm around <paramref name="g"/> using
		/// the <paramref name="edgeChain"/> Markov chain.
		/// </summary>
		/// <param name="g">visited graph</param>
		/// <param name="edgeChain">
		/// Markov <see cref="IEdge"/> chain generator
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="g"/> or <paramref name="edgeChain"/>
		/// is a null reference
		/// </exception>
		public CyclePoppingRandomTreeAlgorithm(
			IVertexListGraph g,
			IMarkovEdgeChain edgeChain
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (edgeChain==null)
				throw new ArgumentNullException("edgeChain");

			this.visitedGraph = g;
			this.edgeChain = edgeChain;
		}

		/// <summary>
		/// Gets the visited <see cref="IVertexListGraph"/> instance
		/// </summary>
		/// <value>
		/// Visited <see cref="IVertexListGraph"/> instance
		/// </value>
		public IVertexListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		/// <summary>
		/// Get the <see cref="IVertex"/> color dictionary
		/// </summary>
		/// <value>
		/// Vertex color dictionary
		/// </value>
		public VertexColorDictionary Colors
		{
			get
			{
				return this.colors;
			}
		}

		/// <summary>
		/// Gets or sets the Markov <see cref="IEdge"/> chain.
		/// </summary>
		/// <value>
		/// Markov <see cref="IEdge"/> chain.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference.
		/// </exception>
		public IMarkovEdgeChain EdgeChain
		{
			get
			{
				return this.edgeChain;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("edgeChain");
				this.edgeChain = value;
			}
		}

		/// <summary>
		/// Gets or sets the random number generator used in <c>RandomTree</c>.
		/// </summary>
		/// <value>
		/// <see cref="Random"/> number generator
		/// </value>
		public Random Rnd
		{
			get
			{
				return this.rnd;
			}
			set
			{
				this.rnd = value;
			}
		}

		/// <summary>
		/// Gets the dictionary of vertex edges successors in the generated
		/// random tree.
		/// </summary>
		/// <value>
		/// Vertex - Edge successor dictionary.
		/// </value>
		public VertexEdgeDictionary Successors		
		{
			get
			{
				return this.successors;
			}
		}

		#region IAlgorithm Members
		object IAlgorithm.VisitedGraph
		{
			get
			{
				return this.VisitedGraph;
			}
		}
		#endregion

		/// <summary>
		/// Occurs when a vertex is initialized
		/// </summary>
		/// <remarks/>
		public event VertexEventHandler InitializeVertex;

		/// <summary>
		/// Raises the <see cref="InitializeVertex"/> event.
		/// </summary>
		/// <param name="v">vertex being initialized</param>
		protected void OnInitializeVertex(IVertex v)
		{
			if (this.InitializeVertex!=null)
				this.InitializeVertex(this, new VertexEventArgs(v));
		}

		/// <summary>
		/// Occurs when a vertex is added to the tree.
		/// </summary>
		/// <remarks/>
		public event VertexEventHandler FinishVertex;

		/// <summary>
		/// Raises the <see cref="FinishVertex"/> event.
		/// </summary>
		/// <param name="v">vertex being terminated</param>
		protected void OnFinishVertex(IVertex v)
		{
			if (this.FinishVertex!=null)
				this.FinishVertex(this, new VertexEventArgs(v));
		}

		/// <summary>
		/// Occurs when an edge is added to the tree.
		/// </summary>
		/// <remarks/>
		public event EdgeEventHandler TreeEdge;

		/// <summary>
		/// Raises the <see cref="TreeEdge"/> event.
		/// </summary>
		/// <param name="e">edge being added to the tree</param>
		protected void OnTreeEdge(IEdge e)
		{
			if (this.TreeEdge!=null)
				this.TreeEdge(this,new EdgeEventArgs(e));
		}

		/// <summary>
		/// Occurs when a vertex is removed from the tree.
		/// </summary>
		/// <remarks/>
		public event VertexEventHandler ClearTreeVertex;

		/// <summary>
		/// Raises the <see cref="ClearTreeVertex"/> event.
		/// </summary>
		/// <param name="v">vertex being removed</param>
		protected void OnClearTreeVertex(IVertex v)
		{
			if (this.ClearTreeVertex!=null)
				this.ClearTreeVertex(this,new VertexEventArgs(v));
		}

		/// <summary>
		/// Initializes the tree.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes the color dictionary and raises
		/// the <see cref="InitializeVertex"/> event for each 
		/// <see cref="IVertex"/> in the graph.
		/// </para>
		/// </remarks>
		protected void Initialize()
		{
			this.successors.Clear();
			this.colors.Clear();
			foreach(IVertex v in this.VisitedGraph.Vertices)
			{
				this.colors[v]=GraphColor.White;
				OnInitializeVertex(v);
			}
		}

		/// <summary>
		/// Gets a value indicating if <paramref name="u"/> is 
		/// not in the tree.
		/// </summary>
		/// <remarks>
		/// This method checks that <paramref name="u"/> color is white.
		/// </remarks>
		/// <param name="u">vertex to test</param>
		/// <returns>true if not in the tree, false otherwise.</returns>
		protected bool NotInTree(IVertex u)
		{
			return this.colors[u]==GraphColor.White;
		}

		/// <summary>
		/// Adds <paramref name="u"/> to the tree and raises the
		/// <see cref="FinishVertex"/> event.
		/// </summary>
		/// <remarks>
		/// Set <paramref name="u"/> color to black.
		/// </remarks>
		/// <param name="u">vertex to add</param>
		protected void SetInTree(IVertex u)
		{
			this.colors[u]=GraphColor.Black;
			OnFinishVertex(u);
		}

		/// <summary>
		/// Gets the next <see cref="IEdge"/> out-edge according to
		/// the Markov Chain generator.
		/// </summary>
		/// <remarks>
		/// This method uses the <see cref="IMarkovEdgeChain"/> instance
		/// to compute the next random out-edge. If <paramref name="u"/> has
		/// no out-edge, null is returned.
		/// </remarks>
		/// <param name="u">Source vertex</param>
		/// <returns>next edge in the chain, null if u has no out-edges</returns>
		protected IEdge RandomSuccessor(IVertex u)
		{
			return this.EdgeChain.Successor(this.VisitedGraph,u);
		}

		/// <summary>
		/// Sets <paramref name="next"/> as the next edge of <paramref name="u"/>
		/// in the tree, and raises the <see cref="TreeEdge"/> event.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If <paramref name="next"/> is null, nothing is done.
		/// </para>
		/// </remarks>
		/// <param name="u">source vertex</param>
		/// <param name="next">next edge in tree</param>
		protected void Tree(IVertex u, IEdge next)
		{
			if (next==null)
				return;

			this.successors[u]=next;
			OnTreeEdge(next);
		}

		/// <summary>
		/// Gets the next vertex in the tree.
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <returns>next vertex in tree if any, null otherwise</returns>
		protected IVertex NextInTree(IVertex u)
		{
			IEdge next = this.successors[u];
			if (next==null)
				return null;
			else
				return next.Target;
		}

		/// <summary>
		/// Gets a value that is true with a probability <paramref="eps"/>.
		/// </summary>
		/// <param name="eps">probability threshold</param>
		/// <returns>true with probability <paramref name="eps"/>.</returns>
		protected bool Chance(double eps)
		{
			return this.rnd.NextDouble() <= eps;
		}

		/// <summary>
		/// Clears <paramref name="u"/> from the tree and raises the
		/// <see cref="ClearTreeVertex"/> event.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <param name="u">vertex to clear</param>
		protected void ClearTree(IVertex u)
		{
			this.successors[u]=null;
			OnClearTreeVertex(u);
		}

		/// <summary>
		/// Generates a random tree rooted at <see cref="root"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method implements the Cycle-Hopping Random Tree generation
		/// algorithm proposed in Propp-Wilson paper. See class summary for
		/// further details.
		/// </para>
		/// </remarks>
		/// <param name="root">root vertex</param>
		/// <exception cref="ArgumentNullException">root is a null reference</exception>
		public void RandomTreeWithRoot(IVertex root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			// initialize vertices to white
			Initialize();
		
			// process root
			ClearTree(root);
			SetInTree(root);

			IVertex u = null;
			foreach(IVertex i in this.VisitedGraph.Vertices)
			{
				u = i;

				// first pass exploring
				while (u!=null && NotInTree(u))
				{
					Tree(u,RandomSuccessor(u));
					u = NextInTree(u);
				}

				// second pass, coloring
				u = i;
				while (u!=null && NotInTree(u))
				{
					SetInTree(u);
					u = NextInTree(u);
				}
			}
		}

		/// <summary>
		/// Generates a random tree with no specified root.
		/// </summary>
		public void RandomTree()
		{
			double eps = 1;
			bool success;
			do {
				eps/=2;
				success = Attempt(eps);
			} while(!success);
		}

		/// <summary>
		/// Attemps to create a new random tree with probability transition
		/// <paramref name="eps"/>.
		/// </summary>
		/// <param name="eps">probability transition</param>
		/// <returns>true if random tree generated, false otherwise</returns>
		protected bool Attempt(double eps)
		{
			Initialize();
			int numRoots = 0;

			IVertex u = null;
			foreach(IVertex i in this.VisitedGraph.Vertices)
			{
				u = i;

				// first pass exploring
				while (u!=null && NotInTree(u))
				{
					if (Chance(eps))
					{
						ClearTree(u);
						SetInTree(u);
						++numRoots;
						if (numRoots>1)
							return false;
					}
					else
					{
						Tree(u,RandomSuccessor(u));
						u = NextInTree(u);
					}
				}

				// second pass, coloring
				u = i;
				while (u!=null && NotInTree(u))
				{
					SetInTree(u);
					u = NextInTree(u);
				}
			}
			return true;
		}

	}
}
