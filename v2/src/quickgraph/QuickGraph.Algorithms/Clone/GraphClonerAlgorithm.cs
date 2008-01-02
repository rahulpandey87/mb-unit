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

namespace QuickGraph.Algorithms.Clone
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;

	/// <summary>
	/// A graph cloner algorithm
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this class to create clone of different graphs with possible different
	/// provider types.
	/// </para>
	/// <para>
	/// The <see cref="CloneVertex"/> and <see cref="CloneEdge"/> events can be 
	/// used to copy the custom properties
	/// attached to each vertex and edge.
	/// </para>
	/// </remarks>
	public class GraphClonerAlgorithm
	{
		/// <summary>
		/// Makes a copy of the source graph to the clone graph.
		/// </summary>
		/// <param name="source">source graph</param>
		/// <param name="target">clone graph</param>
		public void Clone(IVertexAndEdgeListGraph source, IEdgeMutableGraph target)
		{
			VertexVertexDictionary vertexMap = new VertexVertexDictionary();

			// copy vertices
			foreach(IVertex v in source.Vertices)
			{
				IVertex vc = target.AddVertex();
				// clone properties
				OnCloneVertex(v,vc);
				// store in table
				vertexMap[v]=vc;
			}

			// copy edges
			foreach(IEdge e in source.Edges)
			{
				IEdge ec = target.AddEdge(vertexMap[e.Source], vertexMap[e.Target]);
				// cone properties
				OnCloneEdge(e,ec);
			}
		}

		/// <summary>
		/// Clones the <paramref name="source"/> to <paramref name="target"/> and
		/// reverses the edges.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Use this class to create clone of different graphs with possible different
		/// provider types.
		/// </para>
		/// <para>
		/// The <see cref="CloneVertex"/> and <see cref="CloneEdge"/> events can be 
		/// used to copy the custom properties
		/// attached to each vertex and edge.
		/// </para>
		/// </remarks>
		public void ReversedClone(IVertexAndEdgeListGraph source, IEdgeMutableGraph target)
		{
			VertexVertexDictionary vertexMap = new VertexVertexDictionary();

			// copy vertices
			foreach(IVertex v in source.Vertices)
			{
				IVertex vc = target.AddVertex();
				// clone properties
				OnCloneVertex(v,vc);
				// store in table
				vertexMap[v]=vc;
			}

			// copy edges
			foreach(IEdge e in source.Edges)
			{
				IEdge ec = target.AddEdge(vertexMap[e.Target], vertexMap[e.Source]);
				// cone properties
				OnCloneEdge(e,ec);
			}
		}

		/// <summary>
		/// Event called on each vertex cloning
		/// </summary>
		/// <remarks>
		/// </remarks>
		public event CloneVertexEventHandler CloneVertex;

		/// <summary>
		/// Event called on each edge cloning
		/// </summary>
		/// <remarks>
		/// </remarks>
		public event CloneEdgeEventHandler CloneEdge;

		/// <summary>
		/// Triggers the CloneVertex event
		/// </summary>
		/// <param name="v"></param>
		/// <param name="vc"></param>
		protected void OnCloneVertex(IVertex v, IVertex vc)
		{
			if (CloneVertex != null)
				CloneVertex(this, new CloneVertexEventArgs(v,vc));
		}

		/// <summary>
		/// Triggers the CloneEdge event
		/// </summary>
		/// <param name="e"></param>
		/// <param name="ec"></param>
		protected void OnCloneEdge(IEdge e, IEdge ec)
		{
			if (CloneEdge != null)
				CloneEdge(this, new CloneEdgeEventArgs(e,ec));
		}
	}
}
