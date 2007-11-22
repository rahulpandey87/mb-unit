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

namespace MbUnit.Core.Remoting
{
	using MbUnit.Framework;
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Collections;

	/// <summary>
	/// Summary description for ImportanceTestTreePopulator.
	/// </summary>
	public class ImportanceTestTreePopulator : TestTreePopulator
	{
		private TestTreeNode parentNode = null;
        private TestTreeNode anonymous = null;
        private StringTestTreeNodeDictionary importanceNodes = new StringTestTreeNodeDictionary();
		private Hashtable fixtureNodes =new Hashtable();

		#region TestTreePopulator Members

		public override void Clear()
		{
			this.parentNode = null;
            this.anonymous = null;
            this.importanceNodes.Clear();
			this.fixtureNodes.Clear();

		}

		public override void Populate(
			GuidTestTreeNodeDictionary nodes, 
			TestTreeNode rootNode, 
			RunPipeStarterCollection pipes
			)
		{
			if (rootNode==null)
				throw new ArgumentNullException("rootNode");
			if (pipes==null)
				throw new ArgumentNullException("pipes");

            if (this.parentNode == null)
            {
                this.parentNode = new TestTreeNode("Importances", TestNodeType.Populator);
                rootNode.Nodes.Add(this.parentNode);
                nodes.Add(this.parentNode);
                // adding nodes
                foreach (TestImportance ti in Enum.GetValues(typeof(TestImportance)))
                {
                    this.addImportance(nodes, ti);
                }
                // add unknown Importance
                anonymous = this.addImportance(nodes, TestImportance.Default);
            }

            foreach(RunPipeStarter pipeStarter in pipes)
			{
				// get Importance attribute
				TestTreeNode node = null;
				if (TypeHelper.HasCustomAttribute(pipeStarter.Pipe.FixtureType,typeof(ImportanceAttribute)))
				{
					ImportanceAttribute Importance = (ImportanceAttribute)TypeHelper.GetFirstCustomAttribute(
						pipeStarter.Pipe.FixtureType,typeof(ImportanceAttribute));

					node = addImportance(nodes,Importance.Importance);
				}
				else
					node = anonymous;

				TestTreeNode fixtureNode = addFixtureNode(nodes,node,pipeStarter);
				CreatePipeNode(nodes,fixtureNode,pipeStarter);
			}
		}

		#endregion

		private TestTreeNode addFixtureNode(GuidTestTreeNodeDictionary nodes, 
			TestTreeNode parentNode,
			RunPipeStarter pipeStarter)
		{
			RunPipe pipe = pipeStarter.Pipe;
			string key=pipe.FixtureName+parentNode.Name;
			if (!this.fixtureNodes.Contains(key))
			{
				TestTreeNode  fixtureNode = new TestTreeNode(pipe.FixtureName,TestNodeType.Fixture);
				this.fixtureNodes.Add(key,fixtureNode);
				nodes.Add(fixtureNode);
				parentNode.Nodes.Add(fixtureNode);
			}
			return (TestTreeNode)this.fixtureNodes[key];
		}

		private TestTreeNode addImportance(
			GuidTestTreeNodeDictionary nodes, 
			TestImportance importance)
		{
			TestTreeNode node = this.importanceNodes[importance.ToString()];
			if (node==null)
			{
				node = new TestTreeNode(importance.ToString(),TestNodeType.Category);
				this.parentNode.Nodes.Add(node);
				this.importanceNodes.Add(importance.ToString(),node);
				nodes.Add(node);
			}
			return node;
		}
	}
}
