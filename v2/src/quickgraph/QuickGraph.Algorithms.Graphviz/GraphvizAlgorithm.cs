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
	using System.IO;
	using System.Text;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;
	using NGraphviz;
	using NGraphviz.Helpers;

	/// <summary>
	/// A graphviz writer.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This algorithms "renders" the graph to the dot format. dot is an open
	/// source application for drawing graphs available at  
	/// http://www.research.att.com/sw/tools/graphviz/.
	/// </para>
	/// <para>
	/// Further output customization can be achieved by using visitors and
	/// events. The events are
	/// <list>
	/// <para>The GraphViz .Net wrapping was found written by Leppied and can be found 
	/// at http://www.codeproject.com/dotnet/dfamachine.asp</para>
	/// <listheader>
	///		<term>Event</term>
	///		<description>Description</description>
	/// </listheader>
	/// <item>
	///		<term>WriteGraph</term>
	///		<description>Add graph and global customization code here.</description>
	/// </item>
	/// <item>
	///		<term>WriteVertex</term>
	///		<description>Add vertex appearance customization code here.</description>
	/// </item>
	/// <item>
	///		<term>WriteEdge</term>
	///		<description>Add edge appearance customization code here.</description>
	/// </item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <example>
	/// This basic samples outputs the graph to a file:
	/// <code>				
	/// FileStream file = new FileStream(...,FileMode.Create, FileAccess.Write);
	/// GraphvizWriterAlgorithm gw = new GraphvizWriterAlgorithm(g,file);
	/// gw.Write();
	/// </code>
	/// </example>
	public class GraphvizAlgorithm
	{
		private IVertexAndEdgeListGraph m_VisitedGraph;
		private StringWriter m_StringWriter;
		private Dot m_Dot;
		private GraphvizImageType m_ImageType;

		private int clusterCount;
		private GraphvizGraph m_GraphFormat;
		private GraphvizVertex m_CommonVertexFormat;
		private GraphvizVertex m_VertexFormat;
		private GraphvizEdge m_CommonEdgeFormat;
		private GraphvizEdge m_EdgeFormat;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		public GraphvizAlgorithm(IVertexAndEdgeListGraph g)
		{
			if (g == null)
				throw new ArgumentNullException("g");
			m_VisitedGraph = g;
			m_StringWriter = null;
			m_Dot = new Dot();
			m_ImageType = GraphvizImageType.Png;

			m_GraphFormat = new GraphvizGraph();

			m_CommonVertexFormat = new GraphvizVertex();
			m_CommonEdgeFormat = new GraphvizEdge();

			m_VertexFormat = new GraphvizVertex();
			m_EdgeFormat = new GraphvizEdge();
		}

		/// <summary>
		/// Builds a new Graphviz writer of the graph g using the Stream s.
		/// </summary>
		/// <param name="g">Graph to visit.</param>
		/// <param name="path">Path where files are to be created</param>
		/// <param name="imageType">output image type</param>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		/// <exception cref="ArgumentNullException">path is a null reference</exception>
		public GraphvizAlgorithm(
			IVertexAndEdgeListGraph g, 
			String path,
			GraphvizImageType imageType
			)
		{
			if (g == null)
				throw new ArgumentNullException("g");
			if (path==null)
				throw new ArgumentNullException("path");
			m_VisitedGraph = g;
			m_StringWriter = null;
			m_Dot = new Dot(path);
			m_ImageType = imageType;

			clusterCount = 0;
			m_GraphFormat = new GraphvizGraph();

			m_CommonVertexFormat = new GraphvizVertex();
			m_CommonEdgeFormat = new GraphvizEdge();

			m_VertexFormat = new GraphvizVertex();
			m_EdgeFormat = new GraphvizEdge();
		}

		/// <summary>
		/// Graph formatter property
		/// </summary>
		public GraphvizGraph GraphFormat
		{
			get
			{
				return m_GraphFormat;
			}
		}

		/// <summary>
		/// Common vertex format
		/// </summary>
		public GraphvizVertex CommonVertexFormat
		{
			get
			{
				return m_CommonVertexFormat;
			}
		}

		/// <summary>
		/// Common edge format
		/// </summary>
		public GraphvizEdge CommonEdgeFormat
		{
			get
			{
				return m_CommonEdgeFormat;
			}
		}

		/// <summary>
		/// Common vertex format
		/// </summary>
		public GraphvizVertex VertexFormat
		{
			get
			{
				return m_VertexFormat;
			}
		}

		/// <summary>
		/// Common edge format
		/// </summary>
		public GraphvizEdge EdgeFormat
		{
			get
			{
				return m_EdgeFormat;
			}
		}

		/// <summary>
		/// Visited graph
		/// </summary>
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return m_VisitedGraph;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("graph");
				m_VisitedGraph = value;
			}
		}

		/// <summary>
		/// Renderer
		/// </summary>
		public Dot Renderer
		{
			get
			{
				return m_Dot;
			}
		}

		/// <summary>
		/// Dot output stream.
		/// </summary>
		public StringWriter Output
		{
			get
			{
				return m_StringWriter;
			}
		}

		/// <summary>
		/// Current image output type
		/// </summary>
		public GraphvizImageType ImageType
		{
			get
			{
				return m_ImageType;
			}
			set
			{
				m_ImageType = value;
			}
		}

		/// <summary>
		/// Internal cluster count
		/// </summary>
		internal int ClusterCount
		{
			get
			{
				return clusterCount;
			}
			set
			{
				clusterCount = value;
			}
		}

		#region Events
		/// <summary>
		/// Output the graph properties
		/// </summary>
		public event EventHandler WriteGraph;

		/// <summary>
		/// Triggers the write graph event
		/// </summary>
		protected virtual void OnWriteGraph()
		{
			if (WriteGraph != null)
			{
				WriteGraph(this, new EventArgs());
				Output.WriteLine();
			}
		}

		/// <summary>
		/// Event raised while drawing a cluster
		/// </summary>
		public event FormatClusterEventHandler FormatCluster;

		/// <summary>
		/// Raises the <see cref="FormatCluster"/> event.
		/// </summary>
		/// <param name="cluster"></param>
		protected virtual void OnFormatCluster(IVertexAndEdgeListGraph cluster)
		{
			if (FormatCluster!=null)
			{
				FormatClusterEventArgs args = 
					new FormatClusterEventArgs(cluster, new GraphvizGraph());
				FormatCluster(this,args);
				string s=args.GraphFormat.ToDot();
				if (s.Length!=0)
					Output.WriteLine(s);
			}
		}

		/// <summary>
		/// Outputs the vertex property
		/// </summary>
		public event VertexEventHandler WriteVertex;

		/// <summary>
		/// Triggers the WriteVertex event
		/// </summary>
		/// <param name="v"></param>
		protected virtual void OnWriteVertex(IVertex v)
		{
			Output.Write("{0} ",v.ID);

			if (WriteVertex != null)
			{
				Output.Write("[");
				WriteVertex(this, new VertexEventArgs(v));
				Output.Write("]");
			}
			Output.WriteLine(";");
		}

		/// <summary>
		/// Event called while outputing edge format string
		/// </summary>
		public event EdgeEventHandler WriteEdge;

		/// <summary>
		/// Triggers the write edge event
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWriteEdge(IEdge e)
		{
			if (WriteEdge != null)
			{
				WriteEdge(this, new EdgeEventArgs(e));
			}
		}

		/// <summary>
		/// Called for formatting the vertex output
		/// </summary>
		public event FormatVertexEventHandler FormatVertex;

		/// <summary>
		/// Triggers the FormatVertex event
		/// </summary>
		/// <param name="v"></param>
		protected virtual void OnFormatVertex(IVertex v)
		{
			Output.Write("{0} ",v.ID);
			if (FormatVertex != null)
			{
				GraphvizVertex gv = new GraphvizVertex();
				FormatVertex(this, new FormatVertexEventArgs(gv,v));

				string s = gv.ToDot();
				if (s.Length!=0)
					Output.Write("[{0}]",s);
			}
			Output.WriteLine(";");
		}

		/// <summary>
		/// Called for formatting the edge output
		/// </summary>
		public event FormatEdgeEventHandler FormatEdge;

		/// <summary>
		/// Triggers the FormatEdge event
		/// </summary>
		/// <param name="ge"></param>
		/// <param name="e"></param>
		protected virtual void OnFormatEdge(GraphvizEdge ge, IEdge e)
		{
			if (FormatEdge != null)
			{
				FormatEdge(this, new FormatEdgeEventArgs(ge,e));
				Output.Write(" {0}",EdgeFormat.ToDot());
			}
		}
		#endregion Events

		/// <summary>
		/// Generates the dot code to be rendered with GraphViz
		/// </summary>
		/// <param name="outputFileName">output file name</param>
		/// <returns>corrected output file name</returns>
		/// <remarks>
		/// The output filename extension is automatically matched with the
		/// output file type.
		/// </remarks>
		public string Write(string outputFileName)
		{
			if (outputFileName == null)
				throw new ArgumentNullException("outputFileName");

			ClusterCount=0;
			m_StringWriter = new StringWriter();

			Output.WriteLine("digraph G {");

			String gf = GraphFormat.ToDot();
			if (gf.Length > 0)
				Output.WriteLine(gf);
			String vf = CommonVertexFormat.ToDot();
			if (vf.Length > 0)
				Output.WriteLine("node [{0}];",vf);
			String ef = CommonEdgeFormat.ToDot();
			if (ef.Length > 0)
				Output.WriteLine("edge [{0}];",ef);

			OnWriteGraph();

			// initialize vertex map
			VertexColorDictionary colors = new VertexColorDictionary();
			foreach(IVertex v in VisitedGraph.Vertices)
				colors[v]=GraphColor.White;
			EdgeColorDictionary edgeColors = new EdgeColorDictionary();
			foreach(IEdge e in VisitedGraph.Edges)
				edgeColors[e]=GraphColor.White;

			// write
			if (VisitedGraph is IClusteredGraph)
				WriteClusters(colors,edgeColors,VisitedGraph as IClusteredGraph);

			WriteVertices(colors,VisitedGraph.Vertices);
			WriteEdges(edgeColors,VisitedGraph.Edges);

			Output.WriteLine("}");

			return m_Dot.Run(ImageType,Output.ToString(), outputFileName);
		}

		internal void WriteClusters(
			VertexColorDictionary colors,
			EdgeColorDictionary edgeColors,
			IClusteredGraph parent
			)
		{
			++ClusterCount;
			foreach(IVertexAndEdgeListGraph g in parent.Clusters)
			{
				Output.Write("subgraph cluster{0}",ClusterCount.ToString());
				Output.WriteLine(" {");

				OnFormatCluster(g);

				if (g is IClusteredGraph)
					WriteClusters(colors,edgeColors, g as IClusteredGraph);

				if (parent.Colapsed)
				{
					// draw cluster
					// put vertices as black
					foreach(IVertex v in g.Vertices)
					{
						colors[v]=GraphColor.Black;

					}
					foreach(IEdge e in g.Edges)
						edgeColors[e]=GraphColor.Black;

					// add fake vertex
					
				}
				else
				{
					WriteVertices(colors,g.Vertices);
					WriteEdges(edgeColors,g.Edges);
				}

				Output.WriteLine("}");
			}
		}

		internal void WriteVertices(VertexColorDictionary colors,
			IVertexEnumerable vertices)
		{
			foreach(IVertex v in vertices)
			{
				if(colors[v]==GraphColor.White)
				{
					OnWriteVertex(v);
					OnFormatVertex(v);
					colors[v]=GraphColor.Black;
				}
			}
		}

		internal void WriteEdges(EdgeColorDictionary edgeColors,
			IEdgeEnumerable edges)
		{
			foreach(IEdge e in edges)
			{
				if (edgeColors[e]!=GraphColor.White)
					continue;

				Output.Write("{0} -> {1} [",
					e.Source.ID,
					e.Target.ID
					);

				OnWriteEdge(e);
				OnFormatEdge(EdgeFormat,e);
				Output.WriteLine("];");

				edgeColors[e]=GraphColor.Black;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vis"></param>
		public void RegisterVisitor(IGraphvizVisitor vis)
		{
			WriteGraph += new EventHandler(vis.WriteGraph);
			WriteVertex += new VertexEventHandler(vis.WriteVertex);
			WriteEdge += new EdgeEventHandler(vis.WriteEdge);
		}
	}
}
