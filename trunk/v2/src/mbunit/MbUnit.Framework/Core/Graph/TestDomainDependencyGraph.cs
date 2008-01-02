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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

using QuickGraph;
using QuickGraph.Representations;
using QuickGraph.Providers;
using QuickGraph.Algorithms;
using QuickGraph.Concepts;
using QuickGraph.Collections;

using MbUnit.Core.Cons.CommandLine;
using MbUnit.Core.Config;
using MbUnit.Core.Remoting;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Filters;

namespace MbUnit.Core.Graph
{
    public sealed class TestDomainDependencyGraph : IDisposable
    {
        private Hashtable domainVertices = new Hashtable();
        private Hashtable assemblyNameVertices = new Hashtable();
        private StringCollection assemblyPaths = new StringCollection();
        private AdjacencyGraph graph;
        private bool verbose;

        public TestDomainDependencyGraph()
        {
            graph = new AdjacencyGraph(
                new TestDomainVertex.Provider(),
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

        public StringCollection AssemblyPaths
        {
            get { return this.assemblyPaths; }
        }

        public TestDomainVertex GetVertex(TestDomainBase domain)
        {
            if (domain == null)
                throw new ArgumentNullException("domain");
            TestDomainVertex v = this.domainVertices[domain] as TestDomainVertex;
            return v;
        }

        public TestDomainVertex GetVertex(string assemblyName)
        {
            if (assemblyName == null)
                throw new ArgumentNullException("assemblyName");
            TestDomainVertex v = this.assemblyNameVertices[assemblyName] as TestDomainVertex;
            return v;
        }

        public TestDomainVertex AddDomain(TestDomainBase domain)
        {
            if (domain == null)
                throw new ArgumentNullException("domain");
            TestDomainVertex v = this.domainVertices[domain] as TestDomainVertex;
            if (v != null)
                return v;

            v = (TestDomainVertex)this.graph.AddVertex();
            v.Domain = domain;
            this.domainVertices.Add(domain, v);

            // load domain
            domain.Load();

            /*
             * MLS 12/21/05 - Tell the test engine to add a console listener, 
             * if started from console app with verbose option.
             * Note that the test engine is actually a remoting proxy to the remote test engine at this point.
             */
            if (this.verbose)
            {
                domain.TestEngine.AddConsoleListener();
            }
            

            // adding path
            foreach (string assemblyPath in this.AssemblyPaths)
                domain.TestEngine.Resolver.AddHintDirectory(assemblyPath);

            this.assemblyNameVertices.Add(domain.TestEngine.Explorer.AssemblyName, v);
            return v;
        }

        public void CreateDependencies()
        {
            foreach (TestDomainVertex target in this.Graph.Vertices)
            {
                foreach (string sourceName in target.Domain.TestEngine.Explorer.GetDependentAssemblies())
                {
                    TestDomainVertex source = this.assemblyNameVertices[sourceName] as TestDomainVertex;
                    if (source == null)
                        continue;

                    if (this.graph.ContainsEdge(source, target))
                        continue;

                    this.graph.AddEdge(source, target);
                }
            }
        }

        public void Clear()
        {
            foreach (TestDomainVertex vertex in this.graph.Vertices)
            {
                vertex.Domain.Dispose();
            }
            this.graph.Clear();
            this.domainVertices.Clear();
            this.assemblyNameVertices.Clear();
        }

        public void SetFixtureFilter(IFixtureFilter filter)
        {
            foreach (TestDomainVertex v in this.graph.Vertices)
                v.Domain.Filter = filter;
        }

        public void Dispose()
        {
            this.Clear();
        }

        public event ErrorReporter Log;
        private void OnLog(String format, params Object[] args)
        {
            if (this.Log != null)
                this.Log(String.Format(format, args));
        }

        public ReportResult RunTests()
        {
            ReportResult result = new ReportResult();
            if (graph.VerticesCount == 0)
            {
                this.OnLog("No assembly to execute");
                result.UpdateCounts();
                return result;
            }

            this.OnLog("Sorting assemblies by dependencies");
            // create topological sort
            ArrayList sortedVertices = new ArrayList();
            TopologicalSortAlgorithm topo = new TopologicalSortAlgorithm(graph);
            topo.Compute(sortedVertices);
            if (sortedVertices.Count == 0)
                throw new InvalidOperationException("Cannot be zero");

            // set vertices colors
            this.OnLog("Setting up fixture colors");
            VertexColorDictionary colors = new VertexColorDictionary();
            foreach (TestDomainVertex v in graph.Vertices)
                colors.Add(v, GraphColor.White);

            // execute each domain
            foreach (TestDomainVertex v in sortedVertices)
            {
                // if vertex color is not white, skip it
                GraphColor color = colors[v];
                if (color != GraphColor.White)
                {
                    this.OnLog("Skipping assembly {0} because dependent assembly failed", v.Domain.TestEngine.Explorer.AssemblyName);
                    // mark children
                    foreach (TestDomainVertex child in graph.AdjacentVertices(v))
                        colors[child] = GraphColor.Black;
                    continue;
                }

                this.OnLog("Loading {0}", v.Domain.TestEngine.Explorer.AssemblyName);

                ReportCounter counter = v.Domain.TestEngine.GetTestCount();
                this.OnLog("Found  {0} tests", counter.RunCount);
                this.OnLog("Running fixtures.");
                v.Domain.TestEngine.RunPipes();
                counter = v.Domain.TestEngine.GetTestCount();
                this.OnLog("Tests finished: {0} tests, {1} success, {2} failures, {3} ignored"
                    , counter.RunCount
                    , counter.SuccessCount
                    , counter.FailureCount
                    , counter.IgnoreCount
                    );

                result.Merge(v.Domain.TestEngine.Report.Result);

                if (counter.FailureCount != 0)
                {
                    // mark children as failed
                    colors[v] = GraphColor.Black;
                    foreach (TestDomainVertex child in graph.AdjacentVertices(v))
                        colors[child] = GraphColor.Black;
                }
                else
                {
                    // mark vertex as succesfull
                    colors[v] = GraphColor.Gray;
                }
            }

            result.UpdateCounts();
            MbUnit.Framework.Assert.IsNotNull(result);
            MbUnit.Framework.Assert.IsNotNull(result.Counter);

            this.OnLog("All Tests finished: {0} tests, {1} success, {2} failures, {3} ignored in {4} seconds"
                , result.Counter.RunCount
                , result.Counter.SuccessCount
                , result.Counter.FailureCount
                , result.Counter.IgnoreCount
                , result.Counter.Duration
                );

            return result;
        }

        public static TestDomainDependencyGraph BuildGraph(
            string[] testAssemblies,
            string[] assemblyPaths,
            IFixtureFilter fixtureFilter,
            bool verbose
            )
        {
            // MLS 12/21/05 - adding verbose parameter to fix MBUNIT-28 (verbose command line option not working).
            // IDEA: A more flexible solution might be to pass in a collection of IRunPipeListeners.

            return BuildGraph(testAssemblies, assemblyPaths, fixtureFilter,
                new AnyRunPipeFilter(), verbose);
        }

        public static TestDomainDependencyGraph BuildGraph(
            string[] testAssemblies, 
            string[] assemblyPaths,
            IFixtureFilter fixtureFilter,
            IRunPipeFilter runPipeFilter,
            bool verbose
            )
        {
            // MLS 12/21/05 - adding verbose parameter to fix MBUNIT-28 (verbose command line option not working).
            // IDEA: A more flexible solution might be to pass in a collection of IRunPipeListeners.

            if (testAssemblies == null)
                throw new ArgumentNullException("testAssemblies");
            if (testAssemblies.Length == 0)
                throw new ArgumentException("No assembly to test");
            if (fixtureFilter == null)
                throw new ArgumentNullException("fixtureFilter");
            if (runPipeFilter == null)
                throw new ArgumentNullException("runPipeFilter");

            Hashtable loadedAssemblies = new Hashtable();
            TestDomainDependencyGraph graph = null;
            try
            {
                graph = new TestDomainDependencyGraph();
                graph.verbose = verbose;
                if (assemblyPaths != null)
                    graph.AssemblyPaths.AddRange(assemblyPaths);
                foreach (string testAssembly in testAssemblies)
                {
                    if (testAssembly.EndsWith(".mbunit"))
                    {
                        MbUnitProject project = MbUnitProject.Load(testAssembly);
                        foreach (string assembly in project.Assemblies)
                        {
                            if (loadedAssemblies.Contains(assembly))
                                continue;

                            TestDomain domain = new TestDomain(assembly);
                            domain.Filter = fixtureFilter;
                            domain.RunPipeFilter = runPipeFilter;
                            graph.AddDomain(domain);
                            loadedAssemblies.Add(assembly, null);
                        }
                    }
                    else
                    {
                        if (loadedAssemblies.Contains(testAssembly))
                            continue;

                        TestDomain domain = new TestDomain(testAssembly);
                        domain.Filter = fixtureFilter;
                        domain.RunPipeFilter = runPipeFilter;
                        graph.AddDomain(domain);
                        loadedAssemblies.Add(testAssembly, null);
                    }
                }
                graph.CreateDependencies();

                return graph;
            }
            catch (System.Runtime.Remoting.RemotingException remote)
            {
                throw remote;
            }
            catch(Exception ex)
            {
                if (graph != null)
                {
                    graph.Dispose();
                    graph = null;
                }
                throw new ApplicationException("Failed loading assemblies", ex);
            }
        }
    }
}
