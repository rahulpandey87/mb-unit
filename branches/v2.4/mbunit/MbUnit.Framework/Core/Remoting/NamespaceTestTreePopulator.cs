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
using MbUnit.Core.Collections;
using MbUnit.Core.Invokers;

namespace MbUnit.Core.Remoting
{
	public class NamespaceTestTreePopulator : TestTreePopulator
	{				
		private TestTreeNode parentNode = null;
		private StringTestTreeNodeDictionary namespaceNodes = new StringTestTreeNodeDictionary();
		private Hashtable fixtureNodes =new Hashtable();
		
		/// <summary>
		/// Clears the internal representation of the tree
		/// </summary>
		public override void Clear()
		{
			this.parentNode = null;
			this.namespaceNodes.Clear();	
			this.fixtureNodes.Clear();
		}
		
		/// <summary>
		/// Populates the node using the <see cref="RunPipe"/> instance
		/// contained in <paramref name="pipes"/>.
		/// </summary>
		public override void Populate(
			GuidTestTreeNodeDictionary nodes, 
			TestTreeNode rootNode, RunPipeStarterCollection pipes)
		{
			if (rootNode==null)
				throw new ArgumentNullException("rootNode");
			if (pipes==null)
				throw new ArgumentNullException("pipes");

            if (this.parentNode == null)
            {
                this.parentNode = new TestTreeNode("Namespaces", TestNodeType.Populator);
                rootNode.Nodes.Add(this.parentNode);
                nodes.Add(this.parentNode);
            }

            foreach(RunPipeStarter pipeStarter in pipes)
			{
				TestTreeNode namespaceNode = this.addNamespaceNodes(nodes,pipeStarter);

				// get fixture name
				TestTreeNode fixtureNode = this.addFixtureNode(nodes,namespaceNode,pipeStarter);

				CreatePipeNode(nodes,fixtureNode,pipeStarter);
			}
		}

		private TestTreeNode addFixtureNode(GuidTestTreeNodeDictionary nodes, 
			TestTreeNode parentNode,
			RunPipeStarter pipeStarter)
		{
			RunPipe pipe = pipeStarter.Pipe;
            TestTreeNode fixtureNode = this.fixtureNodes[pipe.Fixture.Type] as TestTreeNode;
            if (fixtureNode != null)
                return fixtureNode;

            fixtureNode = new TestTreeNode(pipe.FixtureName,TestNodeType.Fixture);
			this.fixtureNodes.Add(pipe.Fixture.Type,fixtureNode);
			nodes.Add(fixtureNode);
			parentNode.Nodes.Add(fixtureNode);

            return fixtureNode;
        }

		private TestTreeNode addNamespaceNodes(GuidTestTreeNodeDictionary nodes, RunPipeStarter pipeStarter)
		{
			RunPipe pipe = pipeStarter.Pipe;
			string ns = "";
			
			TestTreeNode parent = this.parentNode;
			string namespace_ = pipe.FixtureType.Namespace;
			if (namespace_==null ||namespace_.Length==0)
				namespace_="";
			foreach(string name in namespace_.Split('.'))
			{
				if (ns.Length==0)
					ns+=name;
				else
					ns+="."+name;

				if (!this.namespaceNodes.Contains(ns))
				{
					TestTreeNode  node = new TestTreeNode(name,TestNodeType.Category);

					this.namespaceNodes.Add(ns,node);
					parent.Nodes.Add(node);
					nodes.Add(node);
					parent = node;
				}
				else
					parent = (TestTreeNode)this.namespaceNodes[ns];	
			}
			
			return parent;
		}	
	}
}
