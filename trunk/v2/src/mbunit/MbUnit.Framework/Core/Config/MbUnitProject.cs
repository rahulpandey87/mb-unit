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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;

using MbUnit.Core.Remoting;
using MbUnit.Core.Reports;

namespace MbUnit.Core.Config
{
    [XmlRoot("MbUnitProject")]
    public sealed class MbUnitProject
    {
        #region fields
        private StringCollection assemblies = new StringCollection();
		private TreeViewState treeState = new TreeViewState();
        #endregion

		[XmlElement("TreeState",IsNullable=false)]
		public TreeViewState TreeState
		{
			get
			{
				return this.treeState;
			}
			set
			{
				this.treeState=value;
			}
		}

        [XmlArray("assemblies")]
        [XmlArrayItem("assembly")]
        public StringCollection Assemblies
        {
            get
            {
                return this.assemblies;
            }
        }

		public static MbUnitProject Load(string fileName)
		{
            if (fileName == null)
                throw new ArgumentException("fileName is null.");

			XmlSerializer ser = new XmlSerializer(typeof(MbUnitProject));
			using (StreamReader sr = new StreamReader(fileName))
			{
				MbUnitProject project = ser.Deserialize(sr) as MbUnitProject;
				return project;
			}
		}

		public void Save(string fileName)
		{
            if (fileName == null)
                throw new ArgumentException("fileName is null.");

            // Ensure the folder exists
            ReportBase.DirectoryCheckCreate(Path.GetDirectoryName(fileName));

            // Save serialized project
			XmlSerializer ser = new XmlSerializer(typeof(MbUnitProject));
			using (StreamWriter sw = new StreamWriter(fileName,false))
			{
				XmlTextWriter xm = new XmlTextWriter(sw);
				xm.Formatting = Formatting.Indented;
				ser.Serialize(xm,this);
				xm.Close();
			}
		}
    }
}
