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

namespace MbUnit.Core.Runs
{
	using System;
	using System.Collections.Specialized;

	using MbUnit.Core.Runs;

	using QuickGraph;
	using QuickGraph.Serialization;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Concepts.Providers;
		
	public sealed class RunVertex : Vertex, IGraphSerializable
	{
		private IRun run = null;
		
		public RunVertex(int id) 
		:base(id)
		{}
		
		public bool HasRun
		{
			get
			{
				return this.run!=null;
			}
		}
		
		public IRun Run
		{
			get
			{
				if (this.run==null)
					throw new InvalidOperationException("run is not initialized"); 
				return this.run;
			}
			set
			{
				this.run = value;
			}
		}

        internal sealed class Provider : IVertexProvider
        {
            private int nextID = 0;

            public Type VertexType
            {
                get
                {
                    return typeof(RunVertex);
                }
            }

            public IVertex ProvideVertex()
            {
                return new RunVertex(nextID++);
            }

            public void UpdateVertex(IVertex v)
            {
                nextID = v.ID + 1;
            }
        }
    }
}
