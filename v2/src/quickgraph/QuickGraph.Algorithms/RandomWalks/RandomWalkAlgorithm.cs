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
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux



using System;
using System.Collections;

namespace QuickGraph.Algorithms.RandomWalks
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Visitors;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Collections;
	using QuickGraph.Concepts.Modifications;

	/// <summary>
	/// Stochastic Random Walk Generation.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This algorithms walks randomly across a directed graph. The probability
	/// to choose the next out-edges is provided by a <see cref="IMarkovEdgeChain"/>
	/// instance.
	/// </para>
	/// <para><b>Events</b></para>
	/// <para>
	/// The <see cref="StartVertex"/> is raised on the root vertex of the walk.
	/// This event is raised once.
	/// </para>
	/// <para>
	/// On each new edge in the walk, the <see cref="TreeEdge"/> is raised with
	/// the edge added to the walk tree.
	/// </para>
	/// <para>
	/// The <see cref="EndVertex"/> is raised on the last vertex of the walk.
	/// This event is raised once.
	/// </para>
	/// <para>
	/// Custom end of walk condition can be provided by attacing a
	/// <see cref="IEdgePredicate"/> instance to the <see cref="EndPredicate"/>
	/// property.
	/// </para>
	/// </remarks>
	/// <example>ea
	/// In this example, we walk in a graph and output the edge using 
	/// events:
	/// <code>
	/// // define delegates
	/// public void TreeEdge(Object sender, EdgeEventArgs e)
	/// {
	///     Console.WriteLine(
	///         "{0}->{1}",
	///         e.Edge.Source.ToString(), 
	///	        e.Edge.Target.ToString());
	/// }
	/// 
	/// ...
	/// 
	/// IVertexListGraph g = ...;
	/// // create algo
	/// RandomWalkAlgorithm walker = new RandomWalkAlgorithm(g);
	/// 
	/// // attach event handler
	/// walker.TreeEdge += new EdgeEventHandler(this.TreeEdge);
	/// // walk until we read a dead end.
	/// waler.Generate(int.MaxValue);
	/// </code> 
	/// </example>
	public class RandomWalkAlgorithm : 
		IAlgorithm
	{
        private IImplicitGraph visitedGraph = null;
        private IEdgePredicate endPredicate = null;
		private IMarkovEdgeChain edgeChain = new NormalizedMarkovEdgeChain();
		private Random rnd = new Random((int)DateTime.Now.Ticks);

		/// <summary>
		/// Constructs the algorithm around <paramref name="g"/>.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <param name="g">visted graph</param>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		public RandomWalkAlgorithm(
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
		public RandomWalkAlgorithm(
			IVertexListGraph g,
			IMarkovEdgeChain edgeChain
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (edgeChain == null)
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
        public IImplicitGraph VisitedGraph
        {
			get
			{
				return this.visitedGraph;
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
		/// Gets or sets an end of traversal predicate.
		/// </summary>
		/// <value>
		/// End of traversal predicate.
		/// </value>
		public IEdgePredicate EndPredicate
		{
			get
			{
				return this.endPredicate;
			}
			set
			{
				this.endPredicate = value;
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
		/// Raised on the source vertex once before the start of the search. 
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
		/// Raised on the sink vertex once after the end of the search. 
		/// </summary>
		public event VertexEventHandler EndVertex;

		/// <summary>
		/// Raises the <see cref="EndVertex"/> event.
		/// </summary>
		/// <param name="v">vertex that raised the event</param>
		protected void OnEndVertex(IVertex v)
		{
			if (EndVertex!=null)
				EndVertex(this, new VertexEventArgs(v));
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
		/// Generates a walk of <paramref name="walkCount">steps</paramref>
		/// </summary>
		/// <param name="walkCount">number of steps</param>
        public void Generate(IVertex root)
        {
            Generate(root, int.MaxValue);
		}

		/// <summary>
		/// Generates a walk of <paramref name="walkCount">steps</paramref>
		/// </summary>
		/// <param name="root">root vertex</param>
		/// <param name="walkCount">number of steps</param>
		public void Generate(IVertex root,int walkCount)
		{
			int count = 0;
			IEdge e=null;
			IVertex v = root;
			
			OnStartVertex(root);
			while (count<walkCount)
			{
				e = RandomSuccessor(v);
				// if dead end stop
				if (e==null)
					break;
				// if end predicate, test
				if (this.endPredicate!=null && this.endPredicate.Test(e))
					break;
				OnTreeEdge(e);
				v = e.Target;
				// upgrade count
				++count;
			}
			OnEndVertex(v);
		}

	}
}
