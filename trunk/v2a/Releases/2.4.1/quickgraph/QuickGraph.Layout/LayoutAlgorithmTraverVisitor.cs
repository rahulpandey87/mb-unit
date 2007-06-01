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
using System.Diagnostics;

using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Algorithms;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.Visitors;
using QuickGraph.Collections;

using System.Threading;
using Netron;

namespace QuickGraph.Layout
{
	/// <summary>
	/// 
	/// </summary>
	public class LayoutAlgorithmTraverVisitor : 
		IVertexColorizerVisitor,
		IEdgeColorizerVisitor,
		ITreeEdgeBuilderVisitor
	{
		private NetronAdaptorGraph populator = null;
		private VertexColorDictionary colors = null;
		private EdgeColorDictionary edgeColors = null;
		private int sleep = 0;

		public LayoutAlgorithmTraverVisitor(
			NetronAdaptorGraph populator)
		{
			if (populator==null)
				throw new ArgumentNullException("populator");
			this.populator = populator;
		}

		public LayoutAlgorithmTraverVisitor(
			NetronAdaptorGraph populator,
			int sleep)
		{
			if (populator==null)
				throw new ArgumentNullException("populator");
			if (sleep<0)
				throw new ArgumentException("sleep must be positive");
			this.populator = populator;
			this.sleep = sleep;
		}

		public NetronAdaptorGraph Populator
		{
			get
			{
				return this.populator;
			}
		}

		public int Sleep
		{
			get
			{
				return this.sleep;
			}
		}

		public VertexColorDictionary Colors
		{
			get
			{
				return this.colors;
			}
			set
			{
				this.colors = value;
			}
		}

		public EdgeColorDictionary EdgeColors
		{
			get
			{
				return this.edgeColors;
			}
			set
			{
				this.edgeColors = value;
			}
		}

		public event ShapeVertexEventHandler UpdateVertex;

		protected void OnUpdateVertex(Shape shape, IVertex v)
		{
			if (UpdateVertex!=null)
				UpdateVertex(this, new ShapeVertexEventArgs(shape,v));
		}

		public event ConnectionEdgeEventHandler UpdateEdge;

		protected void OnUpdateEdge(Connection conn, IEdge e)
		{
			if (UpdateEdge!=null)
				UpdateEdge(this, new ConnectionEdgeEventArgs(conn,e));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void InitializeVertex(Object sender, VertexEventArgs args)
		{
			//UpdatePanel();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void DiscoverVertex(Object sender, VertexEventArgs args)
		{
			UpdatePanel();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishVertex(Object sender, VertexEventArgs args)
		{
			UpdatePanel();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void TreeEdge(Object sender, EdgeEventArgs args)
		{
			UpdatePanel();
		}

		public void InitializeEdge(Object sender, EdgeEventArgs args)
		{
			UpdatePanel();
		}

		public void FinishEdge(Object sender, EdgeEventArgs args)
		{
			UpdatePanel();
		}

		protected void UpdatePanel()
		{
			foreach(DictionaryEntry de in this.Populator.ShapeVertices)
			{
				Shape shape = (Shape)de.Key;
				IVertex v = (IVertex)de.Value;
				OnUpdateVertex(shape,v);
			}

			foreach(DictionaryEntry de in this.Populator.ConnectionEdges)
			{
				Connection conn = (Connection)de.Key;
				IEdge e = (IEdge)de.Value;

				OnUpdateEdge(conn,e);
			}

			// wait
			Thread.Sleep(this.Sleep);

			this.populator.Panel.Invalidate();
		}
	}
}
