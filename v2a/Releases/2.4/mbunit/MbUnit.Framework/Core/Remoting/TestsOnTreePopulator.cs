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
//		TestsOn: Jonathan de Halleux

using System;
using System.Collections;

namespace MbUnit.Core.Remoting
{
    using MbUnit.Framework;
    using MbUnit.Core.Invokers;

    /// <summary>
    /// Summary description for TestsOnTestTreePopulator.
    /// </summary>
    public class TestsOnTestTreePopulator : TestTreePopulator
    {
        private TestTreeNode parentNode = null;
        private TestTreeNode anonymous = null;
        private StringTestTreeNodeDictionary testedTypeNodes = new StringTestTreeNodeDictionary();
        private Hashtable fixtureNodes = new Hashtable();

        #region TestTreePopulator Members

        public override void Clear()
        {
            this.parentNode = null;
            this.anonymous = null;
            this.testedTypeNodes.Clear();
            this.fixtureNodes.Clear();
        }

        public override void Populate(
            GuidTestTreeNodeDictionary nodes,
            TestTreeNode rootNode,
            MbUnit.Core.Collections.RunPipeStarterCollection pipes
            )
        {
            if (rootNode == null)
				throw new ArgumentNullException("rootNode");
            if (pipes == null)
                throw new ArgumentNullException("pipes");

            if (parentNode == null)
            {
                this.parentNode = new TestTreeNode("TestsOns", TestNodeType.Populator);
                nodes.Add(this.parentNode);
                rootNode.Nodes.Add(this.parentNode);

                // add unknown author
                anonymous = this.addTestsOn(nodes, "Unknown");
            }

            foreach (RunPipeStarter pipeStarter in pipes)
            {
                // get author attribute
                TestTreeNode node = null;
                TestsOnAttribute testsOn = TypeHelper.TryGetFirstCustomAttribute(
                        pipeStarter.Pipe.FixtureType, typeof(TestsOnAttribute)) as TestsOnAttribute;
                if (testsOn!=null)
                    node = addTestsOn(nodes, testsOn.TestedType.FullName);
                else
                    node = anonymous;

                TestTreeNode fixtureNode = addFixtureNode(nodes, node, pipeStarter);
                CreatePipeNode(nodes, fixtureNode, pipeStarter);
            }
        }

        #endregion

        private TestTreeNode addFixtureNode(
            GuidTestTreeNodeDictionary nodes,
            TestTreeNode parentNode,
            RunPipeStarter pipeStarter)
        {
            RunPipe pipe = pipeStarter.Pipe;
            if (!this.fixtureNodes.Contains(pipe.FixtureName))
            {
                TestTreeNode fixtureNode = new TestTreeNode(pipe.FixtureName, TestNodeType.Fixture);
                this.fixtureNodes.Add(pipe.FixtureName, fixtureNode);
                nodes.Add(fixtureNode);
                parentNode.Nodes.Add(fixtureNode);
            }
            return (TestTreeNode)this.fixtureNodes[pipe.FixtureName];
        }

        private TestTreeNode addTestsOn(
            GuidTestTreeNodeDictionary nodes,
            string testedTypeFullName)
        {
            TestTreeNode node = this.testedTypeNodes[testedTypeFullName];
            if (node == null)
            {
                node = new TestTreeNode(testedTypeFullName, TestNodeType.Category);
                this.parentNode.Nodes.Add(node);
                nodes.Add(node);
                this.testedTypeNodes.Add(testedTypeFullName, node);
            }
            return node;
        }
    }
}
