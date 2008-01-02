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
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Config;

namespace MbUnit.Core.Remoting
{
	public class TreeTestDomainCollection : ICollection, IDisposable
	{
        private IUnitTreeNodeFactory factory;
		private AssemblyWatcher watcher = new AssemblyWatcher();
		private IDictionary identifierDomains = new SortedList();
		private ArrayList list=new ArrayList();
        private volatile object syncRoot = new object();
        private volatile bool pendingStop = false;

        public TreeTestDomainCollection(IUnitTreeNodeFactory factory)
		{
			if (factory==null)
				throw new ArgumentNullException("factory");
			this.factory=factory;
			this.watcher.Start();
		}

        public void Stop()
        {
            lock (this.syncRoot)
            {
                this.pendingStop = true;
            }
        }

        /// <summary>
		/// Gets the assembly watcher
		/// </summary>
		public AssemblyWatcher Watcher
		{
			get
			{
				return this.watcher;
			}
		}

		public TreeTestDomain Add(string testFilePath)
		{
			if (testFilePath==null)
				throw new ArgumentNullException("testFilePath");
			if (!File.Exists(testFilePath))
				throw new FileNotFoundException("File not found",testFilePath);
			// check if already in collection
			if (this.ContainsTestAssembly(testFilePath))
				throw new ArgumentException("File "+testFilePath+" already loaded");

			TreeTestDomain domain = new TreeTestDomain(testFilePath,this.factory);
            domain.ShadowCopyFiles = true;

            this.identifierDomains.Add(domain.Identifier, domain);
			this.list.Add(domain);
			this.watcher.Add(testFilePath);

			return domain;
		}

		public void Remove(TreeTestDomain testDomain)
		{
			if (testDomain==null)
				throw new ArgumentNullException("testDomain");

			testDomain.Unload();
			this.identifierDomains.Remove(testDomain.Identifier);
			this.watcher.Remove(testDomain.TestFilePath);
			this.list.Remove(testDomain);

            testDomain.Dispose();
        }

		public bool Remove(string testFilePath)
		{
			if (testFilePath==null)
				throw new ArgumentNullException("testFilePath");
			foreach(TreeTestDomain td in this)
			{
				if (td.TestFilePath==testFilePath)
				{
					this.Remove(td);
					return true;
				}
			}
			return false;
		}

		public bool Contains(TreeTestDomain domain)
		{
			if (domain==null)
				throw new ArgumentNullException("domain");
			return this.list.Contains(domain);
		}

		public bool ContainsTestAssembly(string testFilePath)
		{
			foreach(TreeTestDomain domain in this.list)
			{
				if (domain.TestFilePath==testFilePath)
					return true;
			}
			return false;
		}

		public void Clear()
		{
			this.Unload();
			foreach(TreeTestDomain td in this)
			{
                td.Dispose();
            }

			this.Watcher.Clear();
			this.identifierDomains.Clear();
			this.list.Clear();
            this.pendingStop = false;

            // delete cache folder
            String cachePath = Environment.ExpandEnvironmentVariables("%TEMP%/Cache");
            CacheFolderHelper.DeleteDir(cachePath);
		}

		public void RunPipes()
		{
			try
			{
                this.pendingStop = false;
                this.Watcher.Stop();
                foreach(TreeTestDomain domain in this.list)
				{
                    if (this.pendingStop)
                        return;
                    domain.TestEngine.RunPipes();
                }
			}
			finally
			{
				this.Watcher.Start();
			}
		}

		public void RunPipes(UnitTreeNode node)
		{
			if (node==null)
				throw new ArgumentNullException("node");
			
			try
			{
				this.Watcher.Stop();
                TreeTestDomain domain = GetDomain(node.DomainIdentifier);
                domain.TestTree.RunPipes(node);
            }
			finally
			{
				this.Watcher.Start();
			}
		}

        public ReportRun GetResult(UnitTreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            TreeTestDomain domain = this.GetDomain(node.DomainIdentifier);
            return domain.TestTree.GetResult(node.TestIdentifier);
        }

        public ReportCounter GetTestCount()
		{
			ReportCounter count = new ReportCounter();;
			foreach(TreeTestDomain domain in this.list)
			{
                if (domain.TestEngine == null)
                    continue;
                count.AddCounts(domain.TestEngine.GetTestCount());
            }
			return count;
		}

		public StringCollection TestFilePaths
		{
			get
			{
				StringCollection paths= new StringCollection();
				foreach(TreeTestDomain domain in this)
					paths.Add(domain.TestFilePath);
				return paths;
			}
		}

		public void Reload()
		{
			foreach(TreeTestDomain td in this.list)
			{
				td.Reload();
			}
		}

		public void Unload()
		{
			foreach(TreeTestDomain td in this.list)
			{
				td.Unload();
			}
		}

		public ReportResult GetReport()
		{
			ReportResult result = new ReportResult();
			result.Date = DateTime.Now;

			foreach(TreeTestDomain td in this.list)
			{
                if (td.TestEngine == null)
                    continue;
                if (td.TestEngine.Report == null)
                    continue;
                if (td.TestEngine.Report.Result == null)
                    continue;

                result.Merge(td.TestEngine.Report.Result);
            }

			result.UpdateCounts();
			return result;
		}

		public void PopulateFacade(TestTreeNodeFacade facade)
		{
			if (facade==null)
				throw new ArgumentNullException("facade");
			foreach(TreeTestDomain td in this.list)
			{
                td.TestTree.PopulateFacade(facade);
            }
		}

		public delegate int AddNodeDelegate(System.Windows.Forms.TreeNode node);
		public void PopulateChildTree(TreeView tree,TestTreeNodeFacade facade)
		{
			foreach(TreeTestDomain td in this.list)
			{
				UnitTreeNode node = this.factory.CreateUnitTreeNode(
                    td.TestTree.ParentNode.Name,
                    TestNodeType.Populator,
                    td.Identifier,
                    td.TestTree.ParentNode.Identifier
                    );
				tree.Invoke(new AddNodeDelegate(tree.Nodes.Add),new Object[]{node});
				td.PopulateChildTree(node,facade);
			}	
		}

		protected TreeTestDomain GetDomain(Guid identifier)
		{
			TreeTestDomain domain = this.identifierDomains[identifier] as TreeTestDomain;
			if (domain==null)
				throw new InvalidOperationException("Could not find domain");
			return domain;
		}

		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		public IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		public void CopyTo(Array array, int index)
		{
			this.list.CopyTo(array,index);
		}

		public Object SyncRoot
		{
			get
			{
				return null;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}
		#region IDisposable Members

		public void Dispose()
		{
            this.Unload();
            this.Clear();
        }

		#endregion
	}
}
