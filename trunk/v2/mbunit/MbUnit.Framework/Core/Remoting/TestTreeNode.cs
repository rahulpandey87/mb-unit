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
using System.Text;
using System.Windows.Forms;

namespace MbUnit.Core.Remoting
{
	[Serializable]
	public class TestTreeNode
	{
		private Guid guid = Guid.NewGuid();
		private string name;
		private TestNodeType nodeType;
		private TestState state = TestState.NotRun;
		private TestTreeNodeCollection nodes;
		[NonSerialized]
		private TestTreeNode parent = null;
		[NonSerialized]
		private RunPipeStarter starter=null;
		private Guid pipeIdentifier = Guid.NewGuid();

		public TestTreeNode(string name, TestNodeType nodeType)
		{
			this.name=name;
			this.nodeType = nodeType;
			this.state = TestState.NotRun;
			this.nodes = new TestTreeNodeCollection(this);
		}

		public Guid Identifier
		{
			get
			{
				return this.guid;
			}
		}

		public bool IsTest
		{
			get
			{
				return this.NodeType==TestNodeType.Test;
			}
		}

		public Guid PipeIdentifier
		{
			get
			{
				if (!this.IsTest)
					throw new InvalidOperationException("no a test");
				return this.pipeIdentifier;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

        public string FullPath
        {
            get
            {
                StringBuilder path = new StringBuilder();
                GetFullPath(path, "\\");
                return path.ToString();
            }
        }

        private void GetFullPath(StringBuilder path, string pathSeparator)
        {
            if (this.parent != null)
            {
                this.parent.GetFullPath(path, pathSeparator);
                if (this.parent.parent != null)
                {
                    path.Append(pathSeparator);
                }
                path.Append(this.name);
            }
        }

		public TestNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
			set
			{
				this.nodeType = value;
				this.OnChanged();
			}
		}

		public TestState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				this.OnChanged();
			}
		}

		public TestTreeNodeCollection Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		public TestTreeNode Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		public RunPipeStarter Starter
		{
			get
			{
				return this.starter;
			}
			set
			{
				this.starter = value;
				if (this.starter!=null)
					this.pipeIdentifier=this.starter.Pipe.Identifier;
				else
					this.pipeIdentifier=Guid.NewGuid();
			}
		}

		public event EventHandler Changed;

		protected virtual void OnChanged()
		{
			if (this.Changed!=null)
				this.Changed(this,new EventArgs());
		}

		public int ImageIndex
		{
			get
			{
				return ReflectionImageList.ImageIndex(this.nodeType,this.state);
			}
		}

		public int SelectedImageIndex
		{
			get
			{
				return ReflectionImageList.ImageIndex(this.nodeType,this.state);
			}
		}
	}
}
