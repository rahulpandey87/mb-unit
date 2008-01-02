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
using MbUnit.Core.Collections;
using MbUnit.Core.Invokers;

namespace MbUnit.Core.Remoting
{	
	/// <summary>
	/// Defines a class that can populate a tree of tests
	/// </summary>
	public abstract class TestTreePopulator : ITestTreePopulator
	{		
		/// <summary>
		/// Clears the internal representation of the tree
		/// </summary>
		public abstract void Clear();
		
		/// <summary>
		/// Populates the node using the <see cref="RunPipe"/> instance
		/// contained in <paramref name="pipes"/>.
		/// </summary>
        /// <param name="nodes">Node dictionary.</param>
        /// <param name="root">The root node.</param>
        /// <param name="pipes">Collection of <see cref="RunPipeStarter"/>s</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> or <paramref name="pipes"/> is a null
		/// reference (Nothing in Visual Basic)
		/// </exception>
		public abstract void Populate(GuidTestTreeNodeDictionary nodes, TestTreeNode root, RunPipeStarterCollection pipes);

		public static TestTreeNode CreatePipeNode(
			GuidTestTreeNodeDictionary nodes, 
			TestTreeNode parentNode, RunPipeStarter starter)
		{
			TestTreeNode pipeNode = new TestTreeNode(starter.Pipe.ShortName,TestNodeType.Test);
			pipeNode.Starter = starter;
			pipeNode.State = TestState.NotRun;
			nodes.Add(pipeNode);
			parentNode.Nodes.Add(pipeNode);
/*
			foreach (RunInvokerVertex v in starter.Pipe.Invokers)
			{
				TestTreeNode invokerNode = new TestTreeNode(v.Invoker.Name,TestNodeType.Invoker);
				pipeNode.Nodes.Add(invokerNode);
			}			
*/
			return pipeNode;
		}
	}
}
