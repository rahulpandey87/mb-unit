/*
 *	test class for CG Algorithm 
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
	public class CondensationGraphTest
	{
		/// <summary>
		/// entry point for the application.
		/// </summary>		
		public void Test(string outputImageFolder)
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
			
			CondensationGraphAlgorithm cgalgo = new CondensationGraphAlgorithm( g );
			cgalgo.InitCondensationGraphVertex += new CondensationGraphVertexEventHandler(CGVertexHandler);
				
			AdjacencyGraph cg = new AdjacencyGraph( new MyVertexProvider(), new EdgeProvider(), true);
			cgalgo.Create(cg);

			//	Render the Condensation Graph
			renderer = new GraphvizAlgorithm(cg, outputImageFolder, NGraphviz.Helpers.GraphvizImageType.Jpeg);
			renderer.FormatVertex += new FormatVertexEventHandler(FormatGraph);
			renderer.Write("CG.jpeg");
		}

		public void CGVertexHandler(object sender, CondensationGraphVertexEventArgs arg)	
		{
			IVertex vCG = arg.CondensationGraphVertex;			
			Console.WriteLine("Added CG vertex <{0}> with connected component vertices -", 
				vCG.ID);
			foreach( IVertex v in arg.StronglyConnectedVertices )	
			{
				((MyVertex)vCG).Name += (v is MyVertex)? ((MyVertex)v).Name + "  " : v.ID.ToString() + "  ";
				Console.Write("{0}  ",(v is MyVertex)?((MyVertex)v).Name : v.ID.ToString());
			}
			Console.WriteLine();
		}

		public void FormatGraph(object sender, FormatVertexEventArgs args)	
		{
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
