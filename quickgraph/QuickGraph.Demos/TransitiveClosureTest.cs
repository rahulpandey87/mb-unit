/*
 *	test class for TransitiveClosureAlgorithm 
 *	Output graphs are rendered as Jpeg in the outputImageFolder
 * 
 *	Author - Rohit Gadagkar
 */

using System;
using QuickGraph;
using QuickGraph.Providers;	
using QuickGraph.Representations;
using QuickGraph.Algorithms.Graphviz;
using QuickGraph.Algorithms;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Providers;
using System.Collections;

namespace QuickGraphTest
{
	/// <summary>
	/// Test Stub for CondensationGraphAlgorithm
	/// </summary>
	class TransitiveClosureTest
	{
		/// <summary>
		/// Entry point for the application.
		/// </summary>		
		public static void Test(string outputImageFolder)
		{
			AdjacencyGraph g = new AdjacencyGraph( new MyVertexProvider(), new EdgeProvider(), true);
			
			MyVertex[] v = new MyVertex[12];
			for( int i=0; i<12;i++)	{
				v[i] = (MyVertex) g.AddVertex();
				v[i].Name = "Vertex " + i;
				v[i].Value = i;
			}			
			g.AddEdge(v[0], v[1]);
			g.AddEdge(v[1], v[2]);
			g.AddEdge(v[2], v[3]);
			g.AddEdge(v[3], v[0]);
			g.AddEdge(v[2], v[0]);
			g.AddEdge(v[4], v[5]);
			g.AddEdge(v[5], v[6]);
			g.AddEdge(v[6], v[7]);
			g.AddEdge(v[7], v[4]);
			g.AddEdge(v[2], v[5]);
			g.AddEdge(v[8], v[9]);
			g.AddEdge(v[9], v[8]);
			g.AddEdge(v[10], v[11]);
			g.AddEdge(v[11], v[10]);
			g.AddEdge(v[2], v[9]);
			g.AddEdge(v[9], v[10]);
			
			GraphvizAlgorithm renderer = new GraphvizAlgorithm(g,outputImageFolder, NGraphviz.Helpers.GraphvizImageType.Jpeg);			
			renderer.FormatVertex += new FormatVertexEventHandler(FormatGraph);
			renderer.Write("Original_Graph.jpeg");
			
			TransitiveClosureAlgorithm tcalgo = new TransitiveClosureAlgorithm(g);
			AdjacencyGraph tc = new AdjacencyGraph(new MyVertexProvider(), new EdgeProvider(), true);
			tcalgo.InitTransitiveClosureVertex += new TransitiveClosureVertexEventHandler(MapTCVertex);
			tcalgo.ExamineEdge += new EdgeEventHandler(InitEdge);
			tcalgo.Create(tc);

			renderer = new GraphvizAlgorithm(tc, outputImageFolder, NGraphviz.Helpers.GraphvizImageType.Jpeg);
			renderer.FormatVertex += new FormatVertexEventHandler(FormatGraph);
			renderer.Write("TC.jpeg");
		}

		public static void MapTCVertex( object sender, TransitiveClosureVertexEventArgs arg )	
		{
			if(arg.VertexInTransformationGraph is MyVertex)
				((MyVertex)arg.VertexInTransformationGraph).Name = ((MyVertex)arg.VertexInOriginalGraph).Name;
		}

		public static void InitEdge(object sender, EdgeEventArgs arg)	
		{
			string s = (arg.Edge.Source is MyVertex)? ((MyVertex)arg.Edge.Source).Name : 
				arg.Edge.Source.ID.ToString();
			string t = (arg.Edge.Target is MyVertex)? ((MyVertex)arg.Edge.Target).Name : 
				arg.Edge.Target.ID.ToString();
			Console.WriteLine("Edge from {0} to {1}",s,t);
		}

		public static void FormatGraph(object sender, QuickGraph.Algorithms.Graphviz.FormatVertexEventArgs args)	{
			MyVertex o = (MyVertex) args.Vertex;
			args.VertexFormatter.Label = o.Name;
			args.VertexFormatter.Shape = NGraphviz.Helpers.GraphvizVertexShape.Ellipse;
		}

		class MyVertex : QuickGraph.NamedVertex
		{
			private object value;
		
			public MyVertex(int id) : base(id) {}
		
			public object Value	
			{
				get	
				{
					return this.value;
				}
				set	
				{
					this.value = value;
				}
			}
		}

		class MyVertexProvider : QuickGraph.Providers.TypedVertexProvider
		{
			public MyVertexProvider() : base(typeof(MyVertex)) {}
		}
	}
}
