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
using MbUnit.Core.Remoting;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;
using MbUnit.Core.Config;

namespace MbUnit.Core.Remoting
{
	[XmlRoot("tree-state")]
	public class UnitTreeViewState : UnitTreeNodeVisitor
	{
		private bool loading = false;
		private IDictionary states = new Hashtable();
		private TreeView tree;
		private UpdateTreeNodeDelegate updateNode;

		public delegate void UpdateTreeNodeDelegate(TreeNodeState old, UnitTreeNode node);

		public UnitTreeViewState(TreeView tree,UpdateTreeNodeDelegate updateNode)
		{
			this.tree=tree;
			this.updateNode=updateNode;
		}

		public void Save()
		{
			this.loading=false;
			this.states.Clear();
			this.VisitNodes(tree.Nodes);
		}

		public void Load(TreeViewState state)
		{
			this.states.Clear();
			foreach(TreeNodeState ns in state.NodeStates)
				this.states[ns.FullPath]=ns;
		}

		public void Load()
		{
			this.loading=true;
            if (this.states.Count == 0)
                return;
            this.VisitNodes(tree.Nodes);
		}

		public TreeViewState GetTreeViewState()
		{
			TreeViewState treeState = new TreeViewState();
			foreach(TreeNodeState state in this.states.Values)
				treeState.NodeStates.Add(state);
			return treeState;
		}

		public override void VisitNode(UnitTreeNode node)
		{
			if (this.loading)
			{
				TreeNodeState old = this.states[node.FullPath] as TreeNodeState;
				if (old!=null)
				{
					this.tree.Invoke(this.updateNode,new Object[]{old,node});
				}
			}
			else
			{
				if (node.IsExpanded || node.IsVisible)
					this.states[node.FullPath]=new TreeNodeState(node);
			}
			base.VisitNode(node);
		}
	}
}
