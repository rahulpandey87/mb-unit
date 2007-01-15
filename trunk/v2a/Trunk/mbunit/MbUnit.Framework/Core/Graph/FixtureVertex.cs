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
using QuickGraph;
using QuickGraph.Providers;
using MbUnit.Core.Collections;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Core.Graph
{
    public sealed class FixtureVertex : Vertex
    {
        private Type fixtureType;
        private FixtureCollection fixtures = new FixtureCollection();
        private ReportRunResult result = ReportRunResult.NotRun;

        public FixtureVertex(int id)
                :base(id)
        { }

        public Type FixtureType
        {
            get
            {
                if (this.fixtureType == null)
                    throw new InvalidOperationException("FixtureType not set");
                return this.fixtureType;
            }
            set
            {
                this.fixtureType = value;
            }
        }

        public FixtureCollection Fixtures
        {
            get
            {
                return this.fixtures;
            }
        }

        public ReportRunResult Result
        {
            get
            {
                return this.result;
            }
            set
            {
                this.result = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}({2})", base.ToString(), this.fixtureType, this.Result);
        }

        public sealed class Provider : TypedVertexProvider
        {
            public Provider()
                    :base(typeof(FixtureVertex))
            { }
        }
    }

}
