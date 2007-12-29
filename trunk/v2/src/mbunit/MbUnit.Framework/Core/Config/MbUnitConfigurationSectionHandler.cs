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
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace MbUnit.Core.Config
{
    [XmlRoot("mbunit")]
    public sealed class MbUnitConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region fields
        private bool shadowCopyFiles = true;
        private string cachePath = "Cache";
        private StringCollection fixtureFactories = new StringCollection();
        private StringCollection hintDirectories = new StringCollection();
        #endregion

        #region Properties
		[XmlIgnore]
		public DirectoryInfo CachePathInfo
		{
			get
			{
				return new DirectoryInfo(this.cachePath);
			}
		}

        [XmlAttribute("shadow-copy-files")]
        public bool ShadowCopyFiles
        {
            get
            {
                return this.shadowCopyFiles;
            }
            set
            {
                this.shadowCopyFiles = value;
            }
        }

        [XmlElement("cachePath",IsNullable = true)]
        public string CachePath
        {
            get
            {
                return this.cachePath;
            }
            set
            {
                this.cachePath = value;
            }
        }

        [XmlArray("fixtureFactories")]
        [XmlArrayItem("factory")]
        public StringCollection FixtureFactories
        {
            get
            {
                return this.fixtureFactories;
            }
        }

        [XmlArray("hintDirectories")]
        [XmlArrayItem("hintDirectory")]
        public StringCollection HintDirectories
        {
            get
            {
                return this.hintDirectories;
            }
        }
        #endregion

        #region Static helper methods
        public static MbUnitConfigurationSectionHandler GetConfig()
        {
            MbUnitConfigurationSectionHandler handler =
                ConfigurationSettings.GetConfig("mbunit") as MbUnitConfigurationSectionHandler;
            if (handler == null)
            {
                // return default
                handler = new MbUnitConfigurationSectionHandler();
                handler.FixtureFactories.Add("MbUnit.Core.ArgumentFixtureFactory, MbUnit.Framework");
                handler.FixtureFactories.Add("MbUnit.Core.FrameworkBridges.FrameworkFixtureFactory, MbUnit.Framework");
            }
            return handler;
        }
        #endregion

        #region IConfigurationSectionHandler Members
        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            XmlSerializer ser = new XmlSerializer(typeof(MbUnitConfigurationSectionHandler));
            return ser.Deserialize(new XmlNodeReader(section));
        }
        #endregion
    }
}
