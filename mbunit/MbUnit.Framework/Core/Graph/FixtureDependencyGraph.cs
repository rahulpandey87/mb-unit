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
using System.Collections;

using QuickGraph.Concepts;
using QuickGraph;
using QuickGraph.Providers;
using QuickGraph.Representations;
using QuickGraph.Concepts.Collections;

using MbUnit.Core.Collections;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Framework;

namespace MbUnit.Core.Graph
{
    public sealed class FixtureDependencyGraph
    {
        private AdjacencyGraph graph;
        private Hashtable typeVertices = new Hashtable();

        public FixtureDependencyGraph()
        {
            this.graph = new AdjacencyGraph(
                new FixtureVertex.Provider(),
                new EdgeProvider(),
                false
                );
        }

        public AdjacencyGraph Graph
        {
            get
            {
                return this.graph;
            }
        }

        public void AddFixtureRange(FixtureCollection fixtures)
        {
            if (fixtures == null)
                throw new ArgumentNullException("fixtures");
            foreach (Fixture fixture in fixtures)
                AddFixture(fixture);
        }
        public void AddFixture(Fixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            FixtureVertex v = this.AddFixtureVertex(fixture.Type);
            v.Fixtures.Add(fixture);
        }
        public void Clear()
        {
            this.graph.Clear();
            this.typeVertices.Clear();
        }

        public void CreateDependencies()
        {
            foreach (FixtureVertex v in this.graph.Vertices)
            {
                FixtureVertex target = this.typeVertices[v.FixtureType] as FixtureVertex;
                if (target==null)
                    throw new InvalidOperationException("Could not find vertex for fixture " + v.FixtureType.FullName);

                // get DependsOn attributes
                foreach (DependsOnAttribute dependsOn in v.FixtureType.GetCustomAttributes(typeof(DependsOnAttribute), true))
                {
                    FixtureVertex source = this.typeVertices[dependsOn.ParentFixtureType] as FixtureVertex;
                    if (source == null)
                        continue;

                    if (this.graph.ContainsEdge(source, target))
                        continue;

                    this.graph.AddEdge(source, target);
                }
            }
        }

        public ReportCounter GetCounter()
        {
            ReportCounter counter = new ReportCounter();
            foreach (FixtureVertex v in this.graph.Vertices)
            {
                counter.AddCounts(v.Fixtures.GetCounter());
            }
            return counter;
        }

        private FixtureVertex AddFixtureVertex(Type fixtureType)
        {
            FixtureVertex v = this.typeVertices[fixtureType] as FixtureVertex;
            if (v != null)
                return v;

            v = (FixtureVertex)this.graph.AddVertex();
            v.FixtureType = fixtureType;
            this.typeVertices.Add(fixtureType, v);
            return v;
        }

        public IEnumerable Fixtures
        {
            get
            {
                return new FixtureEnumerable(this);
            }
        }

        public class FixtureEnumerable : IEnumerable
        {
            private FixtureDependencyGraph graph;
            public FixtureEnumerable(FixtureDependencyGraph graph)
            {
                this.graph = graph;
            }

            public FixtureEnumerator GetEnumerator()
            {
                return new FixtureEnumerator(this.graph);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public class FixtureEnumerator : IEnumerator
            {
                private IVertexEnumerator vertexEn;
                private IEnumerator fixtureEn;

                public FixtureEnumerator(FixtureDependencyGraph graph)
                {
                    this.vertexEn = graph.Graph.Vertices.GetEnumerator();
                    this.fixtureEn = null;
                }

                public Fixture Current
                {
                    get { return this.fixtureEn.Current as Fixture; }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return this.Current;
                    }
                }

                public bool MoveNext()
                {
                    if (fixtureEn==null)
                    {
                        if (!vertexEn.MoveNext())
                            return false;
                        FixtureVertex v = vertexEn.Current as FixtureVertex;
                        this.fixtureEn = v.Fixtures.GetEnumerator();
                    }

                    while(!this.fixtureEn.MoveNext())
                    {
                        if (!vertexEn.MoveNext())
                            return false;
                        FixtureVertex v = vertexEn.Current as FixtureVertex;
                        this.fixtureEn = v.Fixtures.GetEnumerator();                        
                    }

                    return true;
                }

                public void Reset()
                {
                    this.vertexEn.Reset();
                    this.fixtureEn = null;
                }
            }
        }
    }
}
