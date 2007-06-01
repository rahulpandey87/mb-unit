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
using System.Drawing;
using System.Xml;

namespace QuickGraphTest
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Serialization;

	using QuickGraph;
	using QuickGraph.Providers;
	using QuickGraph.Serialization;
	using QuickGraph.Predicates;
	using QuickGraph.Collections;
	using QuickGraph.Representations;
	using QuickGraph.Collections.Filtered;

	using QuickGraph.Algorithms.Graphviz;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Algorithms.Travelling;
	using QuickGraph.Algorithms.Visitors;
	using NGraphviz;
	using NGraphviz.Helpers;

	/// <summary>
	/// An vertex predicate that filters out black vertices
	/// </summary>
	public class NoBlackVertexPredicate : IVertexPredicate
	{
		private VertexColorDictionary m_VertexColors;

		/// <summary>
		/// Construct the edge predicate with the edge color map
		/// </summary>
		/// <param name="edgeColors">edge color map</param>
		/// <remarks>
		/// The vertex color map must be initialize for the graph edges.
		/// </remarks>
		public NoBlackVertexPredicate(VertexColorDictionary vertexColors)
		{
			if (vertexColors == null)
				throw new ArgumentNullException("vertexColors");
			m_VertexColors = vertexColors;
		}

		/// <summary>
		/// Edge color map
		/// </summary>
		public VertexColorDictionary VertexColors
		{
			get
			{
				return m_VertexColors;
			}
		}

		/// <summary>
		/// Test wheter edge is black or not.
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>true if black</returns>
		public bool Test(IVertex e)
		{
			return VertexColors[e]!=GraphColor.Black;
		}
	}

	/// <summary>
	/// Description résumée de FileDependencyTest.
	/// </summary>
	public class EdgeDfsTest
	{
		private GraphvizRecordCell m_Cell;
		private GraphvizVertex m_Vertex;
		private GraphvizEdge m_Edge;
		private EdgeCollection m_CurrentEdgePath;

		/// <summary>
		/// Emptry constructor
		/// </summary>
		public EdgeDfsTest()
		{
			m_Vertex = new GraphvizVertex();			
			m_Edge = new GraphvizEdge();

			Vertex.Style = GraphvizVertexStyle.Filled;
			Vertex.FillColor = Color.LightSkyBlue;

			GraphvizRecordCell cell = new GraphvizRecordCell();
			Vertex.Record.Cells.Add(cell);

			GraphvizRecordCell child = new GraphvizRecordCell();
			child.Text = "State";
			cell.Cells.Add(child);

			m_Cell = new GraphvizRecordCell();
			cell.Cells.Add(m_Cell);
		}

		#region Properties

		public GraphvizVertex Vertex
		{
			get
			{
				return m_Vertex;
			}
		}
		public GraphvizEdge Edge
		{
			get
			{
				return m_Edge;
			}
		}

		/// <summary>
		/// Name to vertex map
		/// </summary>
		public EdgeCollection CurrentEdgePath
		{
			get
			{
				return m_CurrentEdgePath;
			}
			set
			{
				m_CurrentEdgePath = value;
			}
		}
		#endregion

		#region Event Handlers
		/// <summary>
		/// Tree edge event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void TreeEdge(Object sender, EdgeEventArgs args)
		{
			NamedEdge e = (NamedEdge)args.Edge;
			NamedVertex v = (NamedVertex)args.Edge.Target;
			Console.WriteLine("{0}, {1}",
				e.Name,
				v.Name 
				);
		}

		public void FinishEdge(Object sender, EdgeEventArgs args)
		{
			NamedEdge e = (NamedEdge)args.Edge;
			NamedVertex v = (NamedVertex)args.Edge.Target;
			Console.WriteLine("-- {0}, {1}",
				e.Name,
				v.Name 
				);
		}

		/// <summary>
		/// Format vertex event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FormatVertex(Object sender, FormatVertexEventArgs args)
		{
			args.VertexFormatter.Label = ((NamedVertex)args.Vertex).Name;
		}

		/// <summary>
		/// Format edge event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FormatEdge(Object sender, FormatEdgeEventArgs args)
		{
			NamedEdge e = (NamedEdge)args.Edge;

			args.EdgeFormatter.Label.Value = ((NamedEdge)args.Edge).Name;

			// setting the vertex name
			if (CurrentEdgePath!=null)
			{
				if (CurrentEdgePath.Contains(e))
				{
					args.EdgeFormatter.Label.Value = String.Format("{0} ({1})",
						e.Name,
						m_CurrentEdgePath.IndexOf(e)
						);
					args.EdgeFormatter.Style = GraphvizEdgeStyle.Bold;
					args.EdgeFormatter.Label.FontColor = Color.DarkGreen;
					args.EdgeFormatter.StrokeColor = Color.DarkGreen;
				}
				else
				{
					args.EdgeFormatter.Label.FontColor = Color.Gray;
					args.EdgeFormatter.StrokeColor = Color.Gray;
					args.EdgeFormatter.Style = GraphvizEdgeStyle.Unspecified;
				}
			}
		}


		#endregion


		/// <summary>
		/// Tests all actions for adjacencygraph and filtered graph.
		/// </summary>
		public void Test()
		{

			// The all action algorithms
			AdjacencyGraph g = GraphProvider.Fsm();
			Console.WriteLine("Graph: {0} vertices, {1} edges, {2} eulerian trails",
				g.VerticesCount,
				g.EdgesCount,
				EulerianTrailAlgorithm.ComputeEulerianPathCount(g)
				);

			// do layout
			EulerianTrailAlgorithm euler = CreateEulerianTrail(g);
			EdgeCollection temps = euler.AddTemporaryEdges(g);
			Console.WriteLine("Added {0} temporary edges",temps.Count);
			foreach(NamedEdge ne in temps)
				ne.Name = "temporary";
			euler.Compute();
			euler.RemoveTemporaryEdges(g);
			
			Console.WriteLine("Circuit: {0} edges",
				euler.Circuit.Count);
			foreach(NamedEdge e in euler.Circuit)
			{
				Console.WriteLine("{0}->{1} ({2})",
					(NamedVertex)e.Source,
					(NamedVertex)e.Target,
					e.Name
					);
			}
			
			
			Console.WriteLine("Trails:");
			foreach(EdgeCollection ec in euler.Trails(
				Traversal.FirstVertexIf(g.Vertices,new NameEqualPredicate("S0")))
				)
			{
				foreach(IEdge edge in ec)
				{
					Console.WriteLine("{0}->{1}, {2}",
						((NamedVertex)edge.Source).Name,
						((NamedVertex)edge.Target).Name,
						((NamedEdge)edge).Name
						);
				}
				Console.WriteLine();
			}
			

			Console.WriteLine("Testing AdjacencyGraph");
			TestAllActions(GraphProvider.Fsm(), @"../EdgeDfs");

			// testing on filtered graph
			Console.WriteLine("Testing FilteredVertexAndEdgeListGraph");
			TestAllActions(GraphProvider.FilteredFsm(), @"../FilteredEdgeDfs");
		}

		/// <summary>
		/// Output graph to xml
		/// </summary>
		/// <param name="g"></param>
		public void WriteToXml(ISerializableVertexAndEdgeListGraph g)
		{
			// output to xml
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;
			XmlGraphSerializer ser = new XmlGraphSerializer(g);
			ser.Serialize(writer);
		}

		/// <summary>
		/// Renders action graph
		/// </summary>
		/// <param name="g"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public GraphvizAlgorithm RenderActions(IVertexAndEdgeListGraph g, string path)
		{
			// outputing graph to png
			GraphvizAlgorithm gw = new GraphvizAlgorithm(
				g,                      // graph to draw
				path,                    // output file path
				GraphvizImageType.Png  // output file type
				);

			gw.FormatVertex += new FormatVertexEventHandler(this.FormatVertex);
			gw.FormatEdge += new FormatEdgeEventHandler(this.FormatEdge);
			gw.Write("actions");

			return gw;
		}

		/// <summary>
		/// Creates a new edge dfs algorithm
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public EdgeDepthFirstSearchAlgorithm CreateEdgeDfs(IEdgeListAndIncidenceGraph g)
		{
			EdgeDepthFirstSearchAlgorithm edfs = new EdgeDepthFirstSearchAlgorithm(
				g
				);
			edfs.TreeEdge += new EdgeEventHandler(this.TreeEdge);
			edfs.FinishEdge += new EdgeEventHandler(this.FinishEdge);

			return edfs;
		}

		public EulerianTrailAlgorithm CreateEulerianTrail(IFilteredVertexAndEdgeListGraph g)
		{
			return new EulerianTrailAlgorithm(g);
		}

		/// <summary>
		/// Adds an algorithm tracer to the edge dfs algorithm
		/// </summary>
		/// <param name="edfs"></param>
		/// <param name="path"></param>
		/// <returns>tracer</returns>
		public AlgorithmTracerVisitor AddTracer(EdgeDepthFirstSearchAlgorithm edfs, string path)
		{
			// The visitor that draws the graph at each iteration
			AlgorithmTracerVisitor vis = new AlgorithmTracerVisitor(
				(IVertexAndEdgeListGraph)edfs.VisitedGraph,
				"tr",
				path,
				GraphvizImageType.Png);

			// setting some vertex,edge style options
			vis.Algo.VertexFormat.Style = GraphvizVertexStyle.Filled;
			vis.Algo.VertexFormat.FillColor = Color.LightSkyBlue;
			
			vis.Algo.FormatVertex += new FormatVertexEventHandler(this.FormatVertex);
			vis.Algo.FormatEdge += new FormatEdgeEventHandler(this.FormatEdge);
			edfs.RegisterTreeEdgeBuilderHandlers(vis);
			edfs.FinishEdge += new EdgeEventHandler(vis.FinishEdge);

			return vis;
		}

		/// <summary>
		/// Add a predecessor recorder to the edge dfs algorithm.
		/// </summary>
		/// <param name="edfs"></param>
		/// <returns>predecessor recorder</returns>
		public EdgePredecessorRecorderVisitor AddPredecessorRecorder(EdgeDepthFirstSearchAlgorithm edfs)
		{
			// The visitor that will record actions path for us
			EdgePredecessorRecorderVisitor erec = new EdgePredecessorRecorderVisitor();
			edfs.DiscoverTreeEdge += new EdgeEdgeEventHandler(erec.DiscoverTreeEdge);
			edfs.FinishEdge += new EdgeEventHandler(erec.FinishEdge);

			return erec;
		}


		/// <summary>
		/// Executes edge dfs
		/// </summary>
		/// <param name="g"></param>
		public void TestAllActions(IVertexAndEdgeListGraph g, string path)
		{
			// Testing dfs all apth
			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);
			PredecessorRecorderVisitor visv = new PredecessorRecorderVisitor();
			dfs.RegisterPredecessorRecorderHandlers(visv);
			IVertex s0 = Traversal.FirstVertexIf(g.Vertices, new NameEqualPredicate("S0"));
			dfs.Compute(s0);
			Console.WriteLine("vertex paths");
			foreach(EdgeCollection ec in visv.AllPaths())
			{
				foreach(IEdge edge in ec)
				{
					Console.WriteLine("{0}->{1}, {2}",
						((NamedVertex)edge.Source).Name,
						((NamedVertex)edge.Target).Name,
						((NamedEdge)edge).Name
						);
				}
				Console.WriteLine();
			}
			// end of dfs test

			GraphvizAlgorithm gw = RenderActions(g,path);

			// The all action algorithms
			EdgeDepthFirstSearchAlgorithm edfs = CreateEdgeDfs(g);
			// add tracer
			AlgorithmTracerVisitor vis = AddTracer(edfs,path);
			// add predecessor recorder
			EdgePredecessorRecorderVisitor erec = AddPredecessorRecorder(edfs);

			// computing all actions
			edfs.Compute(s0);

			// display end path edges
			OutputEndPathEdges(erec);

			// display all actions path
			OutputAllActions(erec,gw,path);
		}

		private void GraphSize(Object sender, SizeEventArgs args)
		{
			Console.WriteLine(args.Size.ToString());
		}

		private void LayVertex(Object sender, VertexPositionEventArgs args)
		{
			Console.WriteLine("{0} {1}",args.Vertex.ID,args.Position.ToString());
		}


		private void LayEdge(Object sender, EdgeKeysEventArgs args)
		{
			Console.Write("{0} ({1}->{2}) ",
				((NamedEdge)args.Edge).Name,
				((NamedVertex)args.Edge.Source).Name,
				((NamedVertex)args.Edge.Target).Name
				);
			foreach(PointF p in args.Keys)
			{
				Console.Write(" {0}",p.ToString());
			}
			Console.WriteLine();
		}

		/// <summary>
		/// Output end of path edges
		/// </summary>
		/// <param name="erec"></param>
		public void OutputEndPathEdges(EdgePredecessorRecorderVisitor erec)
		{
			// these are the edges that are terminal.
			Console.WriteLine("End path edges:");
			foreach(IEdge se in erec.EndPathEdges)
			{
				Console.WriteLine("\t{0}->{1}, {2}",
					((NamedVertex)se.Source).Name,
					((NamedVertex)se.Target).Name,
					((NamedEdge)se).Name
					);
			}
		}

		/// <summary>
		/// Output all actions paths
		/// </summary>
		/// <param name="erec"></param>
		/// <param name="gw"></param>
		/// <param name="path"></param>
		public void OutputAllActions(
			EdgePredecessorRecorderVisitor erec,
			GraphvizAlgorithm gw,
			string path
			)
		{
			int i=1000;

			Console.WriteLine("All paths (non merged):");
			foreach(EdgeCollection ec in erec.AllPaths())
			{
				CurrentEdgePath = ec;
				gw.Write(String.Format("path{1}",path,i));
				foreach(IEdge edge in ec)
				{
					Console.WriteLine("{0}->{1}, {2}",
						((NamedVertex)edge.Source).Name,
						((NamedVertex)edge.Target).Name,
						((NamedEdge)edge).Name
						);
				}
				++i;
				Console.WriteLine();
			}
			CurrentEdgePath=null;
		}

		public void OutputAllMergedActions(
			EdgePredecessorRecorderVisitor erec,
			GraphvizAlgorithm gw,
			string path)
		{
			int i=1000;

			Console.WriteLine("All paths (merged):");
			foreach(EdgeCollection ec in erec.AllMergedPaths())
			{
				CurrentEdgePath = ec;
				gw.Write(String.Format("path-merged{1}",path,i));
				foreach(IEdge edge in ec)
				{
					Console.WriteLine("{0}->{1}, {2}",
						((NamedVertex)edge.Source).Name,
						((NamedVertex)edge.Target).Name,
						((NamedEdge)edge).Name
						);
				}
				++i;
				Console.WriteLine();
			}
			CurrentEdgePath=null;
		}
	}
}
