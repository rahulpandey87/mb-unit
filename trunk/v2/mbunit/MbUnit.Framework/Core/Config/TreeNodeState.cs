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
using System.Xml.Serialization;
using MbUnit.Core.Remoting;

namespace MbUnit.Core.Config
{
	[XmlRoot("NodeState")]
	public sealed class TreeNodeState
	{
		private string fullPath;
		private bool isVisible;
		private bool isExpanded;
        private bool isSelected;

		public TreeNodeState()
		{}

		public TreeNodeState(UnitTreeNode node)
		{
			this.fullPath=node.FullPath;
			this.isExpanded=node.IsExpanded;
			this.isVisible=node.IsVisible;
            this.isSelected = node.IsSelected;
		}

		[XmlAttribute("FullPath")]
		public string FullPath
		{
			get
			{
				return this.fullPath;
			}
			set
			{
				this.fullPath=value;
			}
		}

		[XmlAttribute("IsVisible")]
		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			set
			{
				this.isVisible=value;
			}
		}

		[XmlAttribute("IsExpanded")]
		public bool IsExpanded
		{
			get
			{
				return this.isExpanded;
			}
			set
			{
				this.isExpanded=value;
			}
		}

        [XmlAttribute("IsSelected")]
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
            }
        }
	}
}
