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

using QuickGraph.Algorithms;

using MbUnit.Core.Graph;
using MbUnit.Core.Collections;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Monitoring;
using MbUnit.Core.Exceptions;

namespace MbUnit.Core.Remoting
{
    [Serializable]
    public class DependencyFixtureRunner : FixtureRunnerBase
    {
        public DependencyFixtureRunner()
        {}

        public FixtureDependencyGraph FixtureGraph
        {
            get
            {
                return this.Explorer.FixtureGraph;
            }
        }

        protected override void RunFixtures()
        {
            // setup result
            foreach (FixtureVertex v in this.FixtureGraph.Graph.Vertices)
                v.Result = ReportRunResult.NotRun;

            // topological sort
            TopologicalSortAlgorithm topo = new TopologicalSortAlgorithm(this.FixtureGraph.Graph);
            ArrayList list = new ArrayList();
            topo.Compute(list);

            // execute pipes
            foreach (FixtureVertex v in list)
            {
                // result failed
                if (v.Result != ReportRunResult.NotRun)
                {
                    ReportMonitor monitor = new ReportMonitor();
                    ParentFixtureFailedException ex = new ParentFixtureFailedException();
                    foreach (Fixture fixture in v.Fixtures)
                    {
                        if (!this.FilterFixture(fixture))
                            continue;
                        this.SkipStarters(fixture, ex);
                    }
                }
                else
                {
                    // run fixtures
                    v.Result = ReportRunResult.Success;
                    foreach (Fixture fixture in v.Fixtures)
                    {
                        if (!this.FilterFixture(fixture))
                            continue;
                        ReportRunResult result = RunFixture(fixture);
                        if (result != ReportRunResult.Success && v.Result != ReportRunResult.Failure)
                            v.Result = result;
                    }
                }

                // update children if failed
                if (v.Result != ReportRunResult.Success)
                {
                    foreach (FixtureVertex target in this.FixtureGraph.Graph.AdjacentVertices(v))
                    {
                        target.Result = v.Result;
                    }
                }
            }
        }
    }
}
