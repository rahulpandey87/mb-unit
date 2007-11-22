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
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace MbUnit.Core.Remoting
{
    public class SourceTestDomain : SeparateTestDomain
    {
        private string path = null;
        private string configFile = null;
        private StringCollection sources = new StringCollection();
        private StringCollection referencedAssemblies = new StringCollection();

        public SourceTestDomain()
        {
            this.path = Environment.CurrentDirectory;
            this.configFile = null;
            this.ShadowCopyFiles = false;
            this.SandBox = true;
        }

        public SourceTestDomain(string path, string configFile)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (configFile == null)
                throw new ArgumentNullException("configFile");

            this.path = path;
            this.configFile = configFile;
            this.ShadowCopyFiles = false;
            this.SandBox = true;
        }

        public StringCollection Sources
        {
            get
            {
                return this.sources;
            }
        }
        public StringCollection ReferencedAssemblies
        {
            get
            {
                return this.referencedAssemblies;
            }
        }

        public new SourceRemoteTestEngine TestEngine
        {
            get
            {
                return base.TestEngine as SourceRemoteTestEngine;
            }
        }

        protected override Type TestEngineType
        {
	        get 
            {
                return typeof(SourceRemoteTestEngine);
            }
        }

        protected override void CreateDomain()
        {
            this.MakeDomain("domain-Sources", this.path, configFile, this.path);
        }

        protected override void SetUpTestEngine()
        {
            base.SetUpTestEngine();
            this.TestEngine.AddHintDirectory(path);
            foreach (string source in sources)
            {
                this.TestEngine.AddSource(source);
            }
            foreach (string referencedAssembly in this.referencedAssemblies)
            {
                this.TestEngine.AddReferencedAssembly(referencedAssembly);
            }
            this.TestEngine.CreateAssembly();
        }
    }
}
