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
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using System.Reflection;
using MbUnit.Core.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Reports;
using System.Windows.Forms;
using System.IO;
using MbUnit.Core.Exceptions;
using MbUnit.Core.Monitoring;

namespace MbUnit.Core.Remoting
{
	using MbUnit.Core.Reports.Serialization;
	using MbUnit.Core.Filters;

	[Serializable]
    public class RemoteTestTree : RemoteTestEngine
    {
		#region Fields
		[NonSerialized]
		private GuidTestTreeNodeDictionary guidNodes = new GuidTestTreeNodeDictionary();
		[NonSerialized]
		private TestTreePopulatorCollection populators =new TestTreePopulatorCollection();
        [NonSerialized]
		private TestTreeNode parentNode = null;
        [NonSerialized]
        private Hashtable selectedFixtures = new Hashtable();
        [NonSerialized]
        private Hashtable selectedPipes = new Hashtable();
        private RemotedTestTreeNodeFacade facade= null;
        #endregion
		
		public RemoteTestTree()
		{
            this.populators.Add( new NamespaceTestTreePopulator() );
			this.populators.Add( new AuthorTestTreePopulator() );
			this.populators.Add( new FixtureCategoryTestTreePopulator());
			this.populators.Add( new ImportanceTestTreePopulator());
            this.populators.Add( new TestsOnTestTreePopulator());
            this.SetFixtureRunner(new TreeFixtureRunner(this));
        }

        public override void Dispose()
        {
            base.Dispose();

            this.facade.Clear();
            this.facade = null;
            this.guidNodes = null;
            this.populators = null;
            this.parentNode = null;
        }

		public TestTreeNode ParentNode
		{
			get
			{
				return this.parentNode;
			}
		}

		#region Population
		public void PopulateFacade(TestTreeNodeFacade rfacade)
		{
            try
            {
    			if (rfacade==null)
					throw new ArgumentNullException("rfacade");
                if (this.facade != null)
                {
                    this.facade.Changed -= new ResultEventHandler(rfacade.Changed);
                }

                this.facade=new RemotedTestTreeNodeFacade();
			    this.facade.Changed += new ResultEventHandler(rfacade.Changed);
                foreach (Fixture fixture in this.Explorer.FixtureGraph.Fixtures)
                {
                    foreach (RunPipeStarter starter in fixture.Starters)
                    {
                        starter.Listeners.Add(this.facade);
                        rfacade.AddPipe(starter.Pipe.Identifier);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportException rex = ReportException.FromException(ex);
                ReportExceptionException rexe = new ReportExceptionException(
                    "Error while populating facade of " + this.TestAssembly.FullName,
                    rex);
                throw rexe;
            }
        }

        protected override void LoadPipes()
		{
            base.LoadPipes();

            this.guidNodes.Clear();

            // create nodes
            AssemblyName assemblyName = this.TestAssembly.GetName();
            string name = String.Format("{0} {1}.{2}", assemblyName.Name, assemblyName.Version.Major, assemblyName.Version.Minor);
            this.parentNode = new TestTreeNode(name, TestNodeType.Populator);
            this.guidNodes.Add(this.parentNode);

            // populate
            foreach (TestTreePopulator pop in this.populators)
            {
                pop.Clear();
                foreach (Fixture fixture in this.Explorer.FixtureGraph.Fixtures)
                {
                    if (fixture.Starters.Count > 0)
                    {
                        pop.Populate(this.guidNodes, parentNode, fixture.Starters);
                    }
                }
            }
		}

		public override void Clear()
		{
    		this.parentNode=null;
            if (this.facade!=null)
                this.facade.Clear();
            if (this.populators!=null)
                this.populators.Clear();
            if (this.guidNodes!=null)
                this.guidNodes.Clear();
        }
		#endregion

		#region Execution
        private void GetSelectedNodes(TestTreeNode testNode)
        {
            if (testNode.Starter != null)
            {
                this.selectedPipes[testNode.Starter]=null;
                this.selectedFixtures[testNode.Starter.Pipe.Fixture] = null;
            }
            foreach (TestTreeNode child in testNode.Nodes)
            {
                GetSelectedNodes(child);
            }
        }

        public void RunPipes(UnitTreeNode node)
		{
			if (node==null)
				throw new ArgumentNullException("node");

            // get test tree node
			TestTreeNode testNode = this.guidNodes[node.TestIdentifier];

            // clear select dictionaries
            this.selectedPipes.Clear();
            this.selectedFixtures.Clear();

            // create the list of pipes to run            
            this.GetSelectedNodes(testNode);

            // run pipes
            this.RunPipes();

            // clear nodes
            this.selectedPipes.Clear();
            this.selectedFixtures.Clear();
        }
		#endregion

        [Serializable]
        internal class TreeFixtureRunner : DependencyFixtureRunner
        {
            private WeakReference tree = null;

            public TreeFixtureRunner(RemoteTestTree tree)
            {
                if (tree == null)
                    throw new ArgumentNullException("tree");
                this.tree = new WeakReference(tree);
            }

            public RemoteTestTree Tree
            {
                get
                {
                    return this.tree.Target as RemoteTestTree;
                }
            }
            protected override bool FilterRunPipe(RunPipeStarter starter)
            {
                if (!base.FilterRunPipe(starter))
                    return false;

                if (this.Tree.selectedPipes.Count == 0)
                    return true;
                return this.Tree.selectedPipes.Contains(starter);
            }

            protected override bool FilterFixture(Fixture fixture)
            {
                if (!base.FilterFixture(fixture))
                    return false;
                if (this.Tree.selectedFixtures.Count == 0)
                    return true;
                return this.Tree.selectedFixtures.Contains(fixture);
            }
        }

		#region Misc
        public ReportRun GetResult(Guid testIdentifier)
        {
			TestTreeNode testNode = this.guidNodes[testIdentifier];
			if (testNode==null)
				return null;
			if (testNode.Starter==null)
				return null;
			if (!testNode.Starter.HasResult)
				return null;
			return testNode.Starter.Result;			
		}
		#endregion
    }
}
