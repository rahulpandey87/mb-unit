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
using System.Windows.Forms;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Core.Remoting
{
	[Serializable]
	public class RemotedTestTreeNodeFacade : IRunPipeListener
	{
		public void Clear()
		{
			this.Changed=null;
		}

		public event ResultEventHandler Changed;
		protected virtual void OnChanged(Guid pipeIdentifier, TestState state)
		{
			if (this.Changed!=null)
				this.Changed(new ResultEventArgs(pipeIdentifier,state));
		}

		public void Start(RunPipe pipe)
		{
			this.OnChanged(pipe.Identifier,TestState.Running);
		}

		public void Success(RunPipe pipe, ReportRun result)
		{
			this.OnChanged(pipe.Identifier,TestState.Success);
		}

        public void Failure(RunPipe pipe, ReportRun result)
        {
			this.OnChanged(pipe.Identifier,TestState.Failure);
		}

        public void Ignore(RunPipe pipe, ReportRun result)
        {
			this.OnChanged(pipe.Identifier,TestState.Ignored);
		}
        public void Skip(RunPipe pipe, ReportRun result)
        {
            this.OnChanged(pipe.Identifier, TestState.Skip);
        }
    }
	
	[Serializable]
	public class ResultEventArgs
	{
		private Guid pipeIdentifier;
		private TestState state;
		public ResultEventArgs(Guid id,TestState state)
		{	
			this.pipeIdentifier=id;
			this.state=state;
		}

		public Guid PipeIdentifier
		{
			get
			{
				return this.pipeIdentifier;
			}
		}

		public TestState State
		{
			get
			{
				return this.state;
			}
		}
	}

	public delegate void ResultEventHandler(ResultEventArgs args);	
	public delegate UnitTreeNode CreateUnitTreeNodeInvoker();

	[Serializable]
	public class TestTreeNodeFacade : LongLivingMarshalByRefObject
	{
		private Hashtable pipeNodes = new Hashtable();

		public TestTreeNodeFacade()
		{}

		public void AddPipe(Guid pipeIdentifier)
		{
			this.pipeNodes.Add(pipeIdentifier, new ArrayList());
		}

		public void AddNode(Guid pipeIdentifier, TreeNode node)
		{
			IList list = this.pipeNodes[pipeIdentifier] as IList;
			if (list==null)
				throw new InvalidOperationException("pipe not found");
			list.Add(node);
		}

		public void Clear()
		{
			this.pipeNodes.Clear();
		}

		#region Listener Members

		public int PipeCount
		{
			get
			{
				return this.pipeNodes.Count;
			}
		}

		public event ResultEventHandler Updated;

		protected virtual void OnUpdated(ResultEventArgs args)
		{
			if (this.Updated!=null)
				this.Updated(args);
		}

		public void Changed(ResultEventArgs args)
		{
			switch(args.State)
			{
				case TestState.Running: Start(args.PipeIdentifier);break;
				case TestState.Success: Success(args.PipeIdentifier);break;
				case TestState.Failure: Failure(args.PipeIdentifier);break;
				case TestState.Ignored: Ignore(args.PipeIdentifier);break;
			}
			this.OnUpdated(args);
		}

		public void Start(Guid pipeIdentifier)
		{
			int imageIndex = ReflectionImageList.ImageIndex(TestNodeType.Test,TestState.Running);

			IEnumerable nodes = this.pipeNodes[pipeIdentifier] as IEnumerable;
			if (nodes==null)
				throw new InvalidOperationException("pipe not found");
			foreach(UnitTreeNode node in nodes)
			{
				node.TestState=TestState.NotRun;
			}
		}

		public void Success(Guid pipeIdentifier)
		{
			int imageIndex = ReflectionImageList.ImageIndex(TestNodeType.Test,TestState.Success);

			IEnumerable nodes = this.pipeNodes[pipeIdentifier] as IEnumerable;
			if (nodes==null)
				throw new InvalidOperationException("pipe not found");
			foreach(UnitTreeNode node in nodes)
			{
				node.TestState = TestState.Success;

				UnitTreeNode parent = node.Parent as UnitTreeNode;
				while (parent!=null)
				{
					if (parent.TestState!=TestState.Failure && parent.TestState!=TestState.Ignored)
						parent.TestState=TestState.Success;
					else
						break;

					parent = parent.Parent as UnitTreeNode;
				}
			}
		}

		public void Failure(Guid pipeIdentifier)
		{
			IEnumerable nodes = this.pipeNodes[pipeIdentifier] as IEnumerable;
			if (nodes==null)
				throw new InvalidOperationException("pipe not found");
			foreach(UnitTreeNode node in nodes)
			{
				node.TestState=TestState.Failure;

				UnitTreeNode parent = node.Parent as UnitTreeNode;
				while (parent!=null)
				{
					parent.TestState=TestState.Failure;
					parent = parent.Parent as UnitTreeNode;
				}
			}
		}

		public void Ignore(Guid pipeIdentifier)
		{
			int imageIndex = ReflectionImageList.ImageIndex(TestNodeType.Test,TestState.Ignored);

			IEnumerable nodes = this.pipeNodes[pipeIdentifier] as IEnumerable;
			if (nodes==null)
				throw new InvalidOperationException("pipe not found");
			foreach(UnitTreeNode node in nodes)
			{
				node.TestState=TestState.Ignored;

				UnitTreeNode parent = node.Parent as UnitTreeNode;
				while (parent!=null)
				{
					if (parent.TestState!=TestState.Failure)
						parent.TestState=TestState.Ignored;
					else
						break;					
					parent = parent.Parent as UnitTreeNode;
				}
			}
		}

		#endregion
	}
}
