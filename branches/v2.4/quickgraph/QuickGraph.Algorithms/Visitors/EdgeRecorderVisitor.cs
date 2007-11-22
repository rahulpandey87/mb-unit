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

namespace QuickGraph.Algorithms.Visitors
{
	using QuickGraph.Concepts;
	using QuickGraph.Collections;

	/// <summary>
	/// A visitor that records edges.
	/// </summary>
	public class EdgeRecorderVisitor
	{
		private EdgeCollection edges;

		/// <summary>
		/// Create an empty edge visitor
		/// </summary>
		public EdgeRecorderVisitor()
		{
			this.edges = new EdgeCollection();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="edges"></param>
		public EdgeRecorderVisitor(EdgeCollection edges)
		{
			if (edges==null)
				throw new ArgumentNullException("edges");
			this.edges = edges;
		}

		/// <summary>
		/// Recorded edges
		/// </summary>
		public EdgeCollection Edges
		{
			get
			{
				return this.edges;
			}
		}

		/// <summary>
		/// Record edge handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void RecordEdge(Object sender, EdgeEventArgs args)
		{
			this.Edges.Add(args.Edge);
		}

		public void RecordSource(Object sender, EdgeEdgeEventArgs args)
		{
			this.Edges.Add(args.Edge);
		}
		public void RecordTarget(Object sender, EdgeEdgeEventArgs args)
		{
			this.Edges.Add(args.TargetEdge);
		}

	}
}
