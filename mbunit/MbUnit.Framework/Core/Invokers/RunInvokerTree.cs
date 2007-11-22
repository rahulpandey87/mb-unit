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
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using QuickGraph.Concepts;
using QuickGraph;
using QuickGraph.Representations;
using QuickGraph.Concepts.Providers;
using QuickGraph.Providers;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.Visitors;
using QuickGraph.Exceptions;
using QuickGraph.Serialization;
using QuickGraph.Collections;
using QuickGraph.Concepts.Collections;

using System.Diagnostics;

using MbUnit.Core.Collections;
using MbUnit.Core.Framework;
using MbUnit.Core.Exceptions;
using MbUnit.Core.Runs;

namespace MbUnit.Core.Invokers
{

    public sealed class RunInvokerTree
	{
		private Fixture fixture;
        private AdjacencyGraph graph = null;
		private RunInvokerVertex root = null;
        private Object[] fixtureDecorators = null;

        public RunInvokerTree(Fixture fixture) 
		{
			if (fixture==null)
				throw new ArgumentNullException("fixture");
			this.fixture = fixture;

            this.graph = new AdjacencyGraph(
				new RunInvokerVertexProvider(),
				new EdgeProvider(),
				false
				);
			
			CreateRoot();
			Reflect();
		}
		
		
		public Fixture Fixture
		{
			get
			{
				return this.fixture;
			}
		}
		
		public RunInvokerVertex Root
		{
			get
			{
				return this.root;
			}
		}
		
		public AdjacencyGraph Graph
		{
			get
			{
				return this.graph;
			}
		}
		
		public string Name
		{
			get
			{
				return this.Fixture.Type.Name;
			}
		}
		
		private void CreateRoot()
		{
			// create vertex
			this.root = (RunInvokerVertex)this.graph.AddVertex();
		}

		private void Reflect()
		{
            try
            {
                this.LoadFixtureDecorators();
                this.Fixture.Run.Reflect(this, this.Root, this.Fixture.Type);
            }
            catch (Exception ex)
            {
                this.Graph.Clear();
                this.CreateRoot();
                FailedLoadingRunInvoker invoker = new FailedLoadingRunInvoker(this.Fixture.Run,ex);
                this.AddChild(this.Root, invoker);
            }
        }

        private void LoadFixtureDecorators()
        {
            this.fixtureDecorators = this.Fixture.Type.GetCustomAttributes(
                typeof(DecoratorPatternAttribute), true);
        }

        private IRunInvoker DecorateInvokerWithFixtureDecorators(IRunInvoker invoker)
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");
            if (this.fixtureDecorators == null || this.fixtureDecorators.Length==0)
                return invoker;
            IRunInvoker current = invoker;
            foreach (DecoratorPatternAttribute decorator in this.fixtureDecorators)
            {
                current = decorator.GetInvoker(current);
            }
            return current;
        }

        public RunInvokerVertex AddChild(RunInvokerVertex parent, IRunInvoker child)
		{
			if (parent==null)
				throw new ArgumentNullException("parent");
			if (child==null)
				throw new ArgumentNullException("child");
				
			// create vertex
			RunInvokerVertex v = (RunInvokerVertex)this.graph.AddVertex();

            // decorate with fixture decorators if needed
            IRunInvoker invoker = child;
            //if (invoker.Generator.IsTest)
            invoker = this.DecorateInvokerWithFixtureDecorators(invoker);
            v.Invoker = invoker;
			
			// add edge
			this.graph.AddEdge(parent,v);
						
			return v;
		}
				
		public RunPipeCollection AllTestPipes()
		{
            if (this.graph.VerticesCount == 1)
            {
                // only the root vertex
                return new RunPipeCollection();
            }
			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(
				this.graph
				);
			
			// attach leaf recorder
			PredecessorRecorderVisitor pred = new PredecessorRecorderVisitor();
			dfs.RegisterPredecessorRecorderHandlers(pred);			
			dfs.Compute(this.Root);		

            // create pipies
			RunPipeCollection pipes = 
			    new RunPipeCollection();
			
			foreach(EdgeCollection edges in pred.AllPaths())
			{
				RunPipe pipe = new RunPipe(this.Fixture);
				
				foreach(IEdge e in edges)
				{
					pipe.Invokers.Add((RunInvokerVertex)e.Target);
				}
				
				pipes.Add(pipe);
			}
			return pipes;			
		}

		public IVertexEnumerable Leaves(RunInvokerVertex v)
		{
			return AlgoUtility.Sinks(this.graph,v);
		}
		
		public override string ToString()
		{
			return String.Format("Tree: {0} tests, {1} transtions",
			                     this.graph.VerticesCount,
			                     this.graph.EdgesCount
			                     );
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
