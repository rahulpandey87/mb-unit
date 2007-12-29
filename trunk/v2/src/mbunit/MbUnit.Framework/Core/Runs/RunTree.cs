// MbUnit Test Framework
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
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux
using System;
using System.IO;
using System.Xml;

using MbUnit.Core.Collections;
using MbUnit.Core.Framework;

using QuickGraph.Serialization;
using QuickGraph.Representations;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Providers;

namespace MbUnit.Core.Runs 
{
	public sealed class RunTree 
	{
		TestFixturePatternAttribute fixturePattern = null;
		AdjacencyGraph graph = null;
		RunVertex root = null;	
		RunVertexDictionary runVertices = new RunVertexDictionary();
		
		public RunTree(TestFixturePatternAttribute fixturePattern) 
		{
			if (fixturePattern==null)
				throw new ArgumentNullException("fixturePattern");
			
			this.fixturePattern = fixturePattern;
			this.graph = new AdjacencyGraph(
				new RunVertex.Provider(),
				new EdgeProvider(),
				false);
			
			this.root = (RunVertex)this.graph.AddVertex();		
		}
		
		public static RunTree FromType(Type t)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (!TypeHelper.HasCustomAttribute(t,typeof(TestFixturePatternAttribute)))
				throw new ArgumentException("type is not tagged by TestFixturePattern", "t");
			
			TestFixturePatternAttribute pattern = 
				(TestFixturePatternAttribute)
				TypeHelper.GetFirstCustomAttribute(t,typeof(TestFixturePatternAttribute));
			
			RunTree runTree = new RunTree(pattern);
			return runTree;
		}
		                               
		
		public TestFixturePatternAttribute FixturePattern
		{
			get
			{
				return this.fixturePattern;
			}
		}

		public RunVertex Root
		{
			get
			{
				return this.root;
			}
		}
		
		public IVertexAndEdgeListGraph Graph
		{
			get
			{
				return this.graph;
			}
		}
		
		public RunVertex Add(RunVertex parent, IRun run)
		{
			if (parent==null)
				throw new ArgumentNullException("parent");
			if (run==null)
				throw new ArgumentNullException("run");
			
			RunVertex v = this.runVertices[run];
			if (v==null)
			{
				v = (RunVertex)this.graph.AddVertex();
				v.Run = run;
				this.runVertices.Add(run,v);
			}
			
			this.graph.AddEdge(parent, v);
			
			return v;
		}
		
		public void ToXml(TextWriter writer)
		{
			if (writer==null)
				throw new ArgumentNullException("writer");
			
			GraphMLGraphSerializer ser = new GraphMLGraphSerializer("");			
			XmlTextWriter xmlWriter = new XmlTextWriter(writer);			
			xmlWriter.Formatting = Formatting.Indented;
			ser.Serialize(xmlWriter,this.graph);			
		}
	}
}
