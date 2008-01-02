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
using System.Collections.Specialized;
using System.Diagnostics;

namespace QuickGraph.Algorithms.RandomWalks
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;

	/// <summary>
	/// Markov <see cref="IEdge"/> chain generator with the propability vector
	/// equally distributed over the out-edges. 
	/// </summary>
	/// <remarks>
	/// <value>
	/// This class can be used to generate a Markov Chain of <see cref="IEdge"/>
	/// instance. The probability vector is computed such that each
	/// out-edge has the same probability:
	/// <code>
	/// outEdgeCount = OutDegree(u)
	/// Pr[e_i] = 1/outEdgeCount
	/// </code>
	/// </value>
	/// </remarks>
	public class NormalizedMarkovEdgeChain : IMarkovEdgeChain
	{
		private Random rnd = new Random((int)DateTime.Now.Ticks);

		/// <summary>
		/// Gets or sets the random generator
		/// </summary>
		/// <value>
		/// Random number generator
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
		/// Selects the next out-<see cref="IEdge"/> in the Markov Chain.
		/// </summary>
		/// <param name="g">visted graph</param>
		/// <param name="u">source vertex</param>
		/// <returns>Random next out-edge</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="g"/> or <paramref name="u"/> is a null reference
		/// </exception>		
        public IEdge Successor(IImplicitGraph g, IVertex u)
        {
			if (g==null)
				throw new ArgumentNullException("g");
			if (u==null)
				throw new ArgumentNullException("u");

			return NextState(g.OutDegree(u), g.OutEdges(u));
		}

		internal IEdge NextState(
			int edgeCount,
			IEdgeEnumerable edges
			)
		{
            if (edgeCount == 0)
                return null;

            double r = this.rnd.NextDouble();
			int nr = (int)Math.Floor(edgeCount * r);
			if (nr==edgeCount)
				--nr;
				
			int i=0;
			foreach(IEdge e in edges)
			{
				if (i==nr)
					return e;
				++i;
			}				
			throw new InvalidOperationException("This is a bug");
		}
	}
}
