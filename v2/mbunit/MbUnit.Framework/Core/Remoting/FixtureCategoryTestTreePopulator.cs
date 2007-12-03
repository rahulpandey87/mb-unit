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
using MbUnit.Framework;

namespace MbUnit.Core.Remoting
{

	/// <summary>
	/// Summary description for FixtureCategoryTestTreePopulator.
	/// </summary>
	public class FixtureCategoryTestTreePopulator : TestTreePopulator
	{
		private TestTreeNode parentNode = null;
		private TestTreeNode miscNode = null;
		private StringTestTreeNodeDictionary categoryNodes = new StringTestTreeNodeDictionary();
		private Hashtable fixtureNodes =new Hashtable();

		public FixtureCategoryTestTreePopulator()
		{}
		#region ITestTreePopulator Members

		public override void Clear()
		{
			this.categoryNodes.Clear();
			this.parentNode=null;
			this.miscNode = null;
			this.fixtureNodes.Clear();
		}

		public override void Populate(
			GuidTestTreeNodeDictionary nodes, 
			TestTreeNode root, 
			RunPipeStarterCollection pipes)
		{
            if (this.parentNode == null)
            {
                this.parentNode = new TestTreeNode("Categories", TestNodeType.Populator);
                root.Nodes.Add(this.parentNode);
                nodes.Add(this.parentNode);
                this.miscNode = this.addCategoryNodes(nodes, "Misc");
            }

            foreach(RunPipeStarter starter in pipes)
			{
				if (TypeHelper.HasCustomAttribute(
					starter.Pipe.FixtureType,
					typeof(FixtureCategoryAttribute)
					))
				{
					foreach(TestTreeNode categoryNode in 
						this.categoryNodesFrom(nodes,starter.Pipe.FixtureType))
					{
						TestTreeNode fixtureNode = addFixtureNode(nodes,categoryNode,starter);
						CreatePipeNode(nodes,fixtureNode,starter);
					}
				}
				else
				{
					TestTreeNode fixtureNode = addFixtureNode(nodes,this.miscNode,starter);
					CreatePipeNode(nodes,fixtureNode,starter);
				}
			}
		}
		#endregion

		private TestTreeNode addFixtureNode(GuidTestTreeNodeDictionary nodes, 
			TestTreeNode parentNode,
			RunPipeStarter pipeStarter)
		{
			RunPipe pipe = pipeStarter.Pipe;
			string key = pipe.FixtureName + parentNode.FullPath;
			if (!this.fixtureNodes.Contains(key))
			{
				TestTreeNode  fixtureNode = new TestTreeNode(pipe.FixtureName,TestNodeType.Fixture);
				this.fixtureNodes.Add(key,fixtureNode);
				nodes.Add(fixtureNode);
				parentNode.Nodes.Add(fixtureNode);
			}
			return (TestTreeNode)this.fixtureNodes[key];
		}

		private TestTreeNode addCategoryNodes(GuidTestTreeNodeDictionary nodes, string category)
		{
			string ns = "";
			
			TestTreeNode parent = this.parentNode;
			foreach(string name in category.Split('.'))
			{
				if (ns.Length==0)
					ns+=name;
				else
					ns+="."+name;

				if (!this.categoryNodes.Contains(ns))
				{
					TestTreeNode  node = new TestTreeNode(name,TestNodeType.Category);

					nodes.Add(node);
					this.categoryNodes.Add(ns,node);
					parent.Nodes.Add(node);
					parent = node;
				}
				else
					parent = (TestTreeNode)this.categoryNodes[ns];	
			}
			
			return parent;
		}	

		private IEnumerable categoryNodesFrom(GuidTestTreeNodeDictionary nodes, Type fixtureType)
		{
			ArrayList catNodes = new ArrayList();

			foreach(FixtureCategoryAttribute cat in 
				fixtureType.GetCustomAttributes(typeof(FixtureCategoryAttribute),true))
			{
				// get parent
				TestTreeNode catNode = this.addCategoryNodes(nodes,cat.Category);
				catNodes.Add(catNode);
			}		
	
			return catNodes;
		}
	}
}
