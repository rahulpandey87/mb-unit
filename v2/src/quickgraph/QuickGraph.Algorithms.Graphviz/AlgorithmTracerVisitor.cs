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


namespace QuickGraph.Algorithms.Graphviz
{
	using System;
	using System.Drawing;
	using System.Collections;

	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Visitors;

	using NGraphviz.Helpers;

	/// <summary>
	/// A visitor that output the graph color state at each change
	/// </summary>
	public class AlgorithmTracerVisitor :
		IVertexColorizerVisitor,
		ITreeEdgeBuilderVisitor,
		IGraphvizVisitor
	{
		private GraphvizAlgorithm m_Algo;
		private String m_FileName;
		private int m_CurrentFile;
		private IDictionary m_Colors;
		private IDictionary m_EdgeColors;
		private IDictionary m_VertexLabels;
		private IDictionary m_EdgeLabels;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g">graph to plot</param>
		/// <param name="prefix">prefix of generated images</param>
		/// <param name="outputPath">output path for image files</param>
		/// <param name="imageType">ouput type</param>
		public AlgorithmTracerVisitor(
			IVertexAndEdgeListGraph g,
			String prefix,
			String outputPath,
			GraphvizImageType imageType
			)
		{
			if (prefix==null)
				throw new ArgumentNullException("prefix");
			if (outputPath==null)
				throw new ArgumentNullException("tempPath");

			m_VertexLabels =null;
			m_EdgeLabels = null;

			m_Colors = null;
			m_EdgeColors = new EdgeColorDictionary();

			m_FileName = prefix;
			m_CurrentFile = 0;

			m_Algo = new GraphvizAlgorithm(g,outputPath,imageType);
			m_Algo.RegisterVisitor(this);
		}

		/// <summary>
		/// Graphviz algorithm
		/// </summary>
		public GraphvizAlgorithm Algo
		{
			get
			{
				return m_Algo;
			}
		}

		/// <summary>
		/// Vertex name map
		/// </summary>
		public IDictionary VertexLabels
		{
			get
			{
				return m_VertexLabels;
			}
			set
			{
				m_VertexLabels = value;
			}
		}

		/// <summary>
		/// Edge name map
		/// </summary>
		public IDictionary EdgeLabels
		{
			get
			{
				return m_EdgeLabels;
			}
			set
			{
				m_EdgeLabels = value;
			}
		}

		/// <summary>
		/// Graphviz vertex format
		/// </summary>
		public GraphvizVertex VertexFormatter
		{
			get
			{
				return Algo.VertexFormat;
			}
		}

		/// <summary>
		/// Graphviz edge format
		/// </summary>
		public GraphvizEdge EdgeFormatter
		{
			get
			{
				return Algo.EdgeFormat;
			}
		}

		/// <summary>
		/// Increment file name count
		/// </summary>
		protected void UpdateFileName()
		{
			++m_CurrentFile;
		}

		/// <summary>
		/// Current outputed file name
		/// </summary>
		protected String CurrentFileName
		{
			get
			{
				return String.Format("{0}{1}", m_FileName,1000+m_CurrentFile);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void WriteGraph(Object sender, EventArgs args)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void WriteVertex(Object sender, VertexEventArgs args)
		{
			if (m_Colors!=null &&  m_Colors.Contains(args.Vertex))
			{
				GraphColor c = (GraphColor)m_Colors[args.Vertex];
				VertexFormatter.FillColor = GraphColorConverter.Convert(c);
				if (c == GraphColor.Black)
					VertexFormatter.FontColor = Color.White;
				else
					VertexFormatter.FontColor = Color.Black;
			}
			else
				VertexFormatter.StrokeColor = Color.White;

			if (m_VertexLabels != null)
				VertexFormatter.Label = m_VertexLabels[args.Vertex].ToString();

			((GraphvizAlgorithm)sender).Output.Write(VertexFormatter.ToDot());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void WriteEdge(Object sender, EdgeEventArgs args)
		{
			if (m_EdgeColors != null && m_EdgeColors.Contains(args.Edge))
			{
				GraphColor c = (GraphColor)m_EdgeColors[args.Edge]; 
				switch(c)
				{
					case GraphColor.White:
						EdgeFormatter.Style = GraphvizEdgeStyle.Unspecified;
						EdgeFormatter.StrokeColor = Color.Black;
						break;
					case GraphColor.Gray:
						EdgeFormatter.Style = GraphvizEdgeStyle.Bold;
						EdgeFormatter.StrokeColor = Color.Gray;
						break;
					case GraphColor.Black:
						EdgeFormatter.Style = GraphvizEdgeStyle.Bold;
						EdgeFormatter.StrokeColor = Color.Black;
						break;
				}
			}
			else
				EdgeFormatter.Style = GraphvizEdgeStyle.Unspecified;

			if (EdgeLabels != null)
				EdgeFormatter.Label.Value = EdgeLabels[args.Edge].ToString();

			((GraphvizAlgorithm)sender).Output.Write(EdgeFormatter.ToDot());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void InitializeVertex(Object sender, VertexEventArgs args)
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void DiscoverVertex(Object sender, VertexEventArgs args)
		{
			OutputGraph(sender,args);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishVertex(Object sender, VertexEventArgs args)
		{
			OutputGraph(sender,args);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected void OutputGraph(Object sender, VertexEventArgs args)
		{
			IVertexColorizerAlgorithm algo = (IVertexColorizerAlgorithm)sender;
			m_Colors = algo.Colors;

			UpdateFileName();
			m_Algo.Write(CurrentFileName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void TreeEdge(Object sender, EdgeEventArgs args)
		{
			GetColors(sender,args);

			UpdateFileName();
			m_Algo.Write(CurrentFileName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishEdge(Object sender, EdgeEventArgs args)
		{
			GetColors(sender,args);

			UpdateFileName();
			m_Algo.Write(CurrentFileName);
		}

		/// <summary>
		/// Extracts the color tables from the calling algorithm
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected void GetColors(Object sender, EdgeEventArgs args)
		{
			if (sender is IVertexColorizerAlgorithm)
			{
				IVertexColorizerAlgorithm algo = (IVertexColorizerAlgorithm)sender;
				m_Colors = algo.Colors;
			}
			if (sender is IEdgeColorizerAlgorithm)
			{
				IEdgeColorizerAlgorithm algo = (IEdgeColorizerAlgorithm)sender;
				m_EdgeColors = algo.EdgeColors;
			}
			else if (m_EdgeColors != null)
				m_EdgeColors[args.Edge]=GraphColor.Black;
		}
	}
}
