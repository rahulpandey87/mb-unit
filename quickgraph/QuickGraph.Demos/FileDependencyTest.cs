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

namespace QuickGraphTest
{
	using QuickGraph.Concepts;
	using QuickGraph.Providers;
	using QuickGraph.Representations;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms.Graphviz;
	using QuickGraph.Algorithms;
	using NGraphviz;
	using NGraphviz.Helpers;

	/// <summary>
	/// Description résumée de FileDependencyTest.
	/// </summary>
	public class FileDependencyTest
	{
		private VertexStringDictionary names=new VertexStringDictionary();

		public FileDependencyTest()
		{
		}

		#region Properties

		public VertexStringDictionary Names
		{
			get
			{
				return this.names;
			}
		}
		#endregion

		public void Test()
		{
			// create a new adjacency graph
			AdjacencyGraph g = new AdjacencyGraph(false);

			// adding files and storing names
			IVertex boz_h = g.AddVertex();		Names[boz_h]="boz.h";
			IVertex zag_cpp = g.AddVertex();	Names[zag_cpp]="zag.cpp";
			IVertex yow_h = g.AddVertex();		Names[yow_h]="yow.h";
			IVertex dax_h = g.AddVertex();		Names[dax_h]="dax.h";
			IVertex bar_cpp = g.AddVertex();	Names[bar_cpp]="bar.cpp";
			IVertex zow_h = g.AddVertex();		Names[zow_h]="zow.h";
			IVertex foo_cpp = g.AddVertex();	Names[foo_cpp]="foo.cpp";

			IVertex zig_o = g.AddVertex();		Names[zig_o]="zig.o";
			IVertex zag_o = g.AddVertex();		Names[zag_o]="zago";
			IVertex bar_o = g.AddVertex();		Names[bar_o]="bar.o";
			IVertex foo_o = g.AddVertex();		Names[foo_o]="foo.o";
			IVertex libzigzag_a = g.AddVertex();Names[libzigzag_a]="libzigzig.a";
			IVertex libfoobar_a = g.AddVertex();Names[libfoobar_a]="libfoobar.a";

			IVertex killerapp = g.AddVertex();	Names[killerapp]="killerapp";
			IVertex zig_cpp = g.AddVertex();	Names[zig_cpp]="zip.cpp";

			// adding dependencies
			g.AddEdge(dax_h, foo_cpp); 
			g.AddEdge(dax_h, bar_cpp); 
			g.AddEdge(dax_h, yow_h);
			g.AddEdge(yow_h, bar_cpp); 
			g.AddEdge(yow_h, zag_cpp);
			g.AddEdge(boz_h, bar_cpp); 
			g.AddEdge(boz_h, zig_cpp); 
			g.AddEdge(boz_h, zag_cpp);
			g.AddEdge(zow_h, foo_cpp); 
			g.AddEdge(foo_cpp, foo_o);
			g.AddEdge(foo_o, libfoobar_a);
			g.AddEdge(bar_cpp, bar_o);
			g.AddEdge(bar_o, libfoobar_a);
			g.AddEdge(libfoobar_a, libzigzag_a);
			g.AddEdge(zig_cpp, zig_o);
			g.AddEdge(zig_o, libzigzag_a);
			g.AddEdge(zag_cpp, zag_o);
			g.AddEdge(zag_o, libzigzag_a);
			g.AddEdge(libzigzag_a, killerapp);

			Console.WriteLine("Leaves");
			foreach(IVertex v in AlgoUtility.Sinks(g))
			{
				Console.WriteLine(Names[v]);
			}

			Console.WriteLine("Leaves of zag_o");
			foreach(IVertex v in AlgoUtility.Sinks(g,zag_o))
			{
				Console.WriteLine(Names[v]);
			}

			// outputing graph to png
			GraphvizAlgorithm gw = new GraphvizAlgorithm(
				g,                      // graph to draw
				".",                    // output file path
				GraphvizImageType.Png   // output file type
				);
			// outputing to graph.
			gw.Write("filedependency");
			// adding custom vertex settings
			gw.FormatVertex+=new FormatVertexEventHandler(gw_FormatVertex);
			gw.Write("fp");
		}

		private void gw_FormatVertex(object sender, FormatVertexEventArgs e)
		{
			// setting the vertex name
			e.VertexFormatter.Label = Names[e.Vertex];
		}
	}
}
