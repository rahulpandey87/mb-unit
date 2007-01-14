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
using System.Windows.Forms;

namespace MbUnit.Core.Remoting
{
    public class TreeTestDomain : TestDomain
    {
        private IUnitTreeNodeFactory factory;

        public TreeTestDomain(string testFilePath, IUnitTreeNodeFactory factory)
            :base(testFilePath)
        {
			if (factory==null)
				throw new ArgumentNullException("factory");
			this.factory=factory;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.factory = null;
        }

        protected override void CreateTestEngine()
        {
            if (this.Domain != null)
            {
               this.SetTestEngine((RemoteTestTree)
                    this.Domain.CreateInstanceAndUnwrap(
                    typeof(MbUnit.Core.Remoting.RemoteTestTree).Assembly.GetName().Name,
                    typeof(MbUnit.Core.Remoting.RemoteTestTree).FullName
                    ));
            }
            else
            {
                this.SetTestEngine(new RemoteTestTree());
            }
        }

        public RemoteTestTree TestTree
        {
            get
            {
                return this.TestEngine as RemoteTestTree;
            }
        }

        public void PopulateChildTree(UnitTreeNode root, TestTreeNodeFacade facade)
        {
            CreateTree(root, facade, this.TestTree.ParentNode);
        }

        protected void CreateTree(UnitTreeNode node, TestTreeNodeFacade facade, TestTreeNode testNode)
        {
            if (testNode.IsTest)
            {
                facade.AddNode(testNode.PipeIdentifier, node);
            }

            foreach (TestTreeNode childNode in testNode.Nodes)
            {
                UnitTreeNode cnode = this.factory.CreateUnitTreeNode(
                    childNode.Name,
                    childNode.NodeType,
                    this.Identifier,
                    childNode.Identifier
                    );
                this.factory.AddChildUnitTreeNode(node, cnode);
                this.CreateTree(cnode, facade, childNode);
            }
        }
    }
}
