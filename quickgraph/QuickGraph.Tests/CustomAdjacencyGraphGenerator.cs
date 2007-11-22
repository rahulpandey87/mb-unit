// Automatically generated code, dot edit!

namespace QuickGraph.UnitTests
{
	using System;
	using System.Collections;
	using System.Diagnostics;

	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.MutableTraversals;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Providers;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Collections;
	using QuickGraph.Exceptions;
	using QuickGraph.Predicates;

	using QuickGraph;
	using QuickGraph.Providers;

	public class CustomAdjacencyGraphGenerator
	{
		public CustomAdjacencyGraph EmptyGraph
		{
			get
			{
				return new CustomAdjacencyGraph(true);
			}
		}

		public CustomAdjacencyGraph Tree
		{
			get
			{
				CustomAdjacencyGraph g =  new CustomAdjacencyGraph(true);
				Vertex u = g.AddVertex();
				Vertex v = g.AddVertex();
				Vertex w = g.AddVertex();
				g.AddEdge(u,v);
				g.AddEdge(v,w);
				g.AddEdge(u,w);

				return g;
			}
		}

		public CustomAdjacencyGraph CyclicGraph
		{
			get
			{
				CustomAdjacencyGraph g =  new CustomAdjacencyGraph(true);
				Vertex u = g.AddVertex();
				Vertex v = g.AddVertex();
				Vertex w = g.AddVertex();
				g.AddEdge(u,v);
				g.AddEdge(v,w);
				g.AddEdge(w,u);

				return g;
			}
		}
	}
}

