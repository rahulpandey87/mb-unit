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
using System.Reflection;

using MbUnit.Core.Remoting;

using QuickGraph;
using QuickGraph.Providers;

namespace MbUnit.Core.Graph
{
    public sealed class TestDomainVertex : Vertex
    {
        private TestDomainBase domain;

        public TestDomainVertex(int id)
            :base(id)
        {}

        public TestDomainBase Domain
        {
            get
            {
                if (this.domain == null)
                    throw new InvalidOperationException("Domain not set");
                return this.domain;
            }
            set
            {
                this.domain = value;
            }
        }

        public override string ToString()
        {
            return this.Domain.ToString();
        }

        public sealed class Provider : TypedVertexProvider
        {
            public Provider()
                    :base(typeof(TestDomainVertex))
            { }
        }
    }
}
