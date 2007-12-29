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


namespace MbUnit.Core.Runs
{
	using System;
	using System.Diagnostics;
	
	using MbUnit.Core.Collections;
	using MbUnit.Core.Invokers;
    using MbUnit.Core.Exceptions;
	
	using QuickGraph.Algorithms;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Serialization;
	
	/// <summary>
	/// TODO - Add class summary
	/// </summary>
	/// <remarks>
	/// 	created by - dehalleux
	/// 	created on - 29/01/2004 14:44:27
	/// </remarks>
	public sealed class ParallelRun : IRun
	{
		private RunCollection runs = new RunCollection();
        private bool allowEmpty = true;

        public ParallelRun()
		{}

        public bool AllowEmpty
        {
            get
            {
                return this.allowEmpty;
            }
            set
            {
                this.allowEmpty = value;
            }
        }

        public bool IsTest
		{
			get
			{
				return false;
			}
		}
		
		public RunCollection Runs
		{
			get
			{
				return this.runs;
			}
		}
		
		public string Name
		{
			get
			{
				return "Parallel";
			}
		}
		
		public void Reflect(
			RunInvokerTree tree, 
			RunInvokerVertex parent, 
			Type t
			)
		{
            if (this.runs.Count == 0 && !this.allowEmpty)
            {
                throw new InvalidOperationException("Parralel run is empty. Missing factory ?");
            }
            // for each leaf
			foreach(RunInvokerVertex leaf in tree.Leaves(parent))
			{
				// for each run
				foreach(IRun run in this.Runs)			
				{
					// add runs
					run.Reflect(tree,leaf,t);
				}
			}
		}
	}
}
