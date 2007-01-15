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
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Lifetime;
using System.Diagnostics;
using System.IO;
using System.Security.Policy;
using System.Configuration;
using System.Windows.Forms;
using MbUnit.Core.Filters;
using MbUnit.Core.Config;

namespace MbUnit.Core.Remoting
{
	[Serializable]
	public class TestDomain :  SeparateTestDomain
	{
        private string testFilePath = null;

		public TestDomain(string testFilePath)		
		{
			if (testFilePath==null)
				throw new ArgumentNullException("testFilePath");
			this.testFilePath=testFilePath;
		}

		/// <summary>
		/// Gets the testFilePath
		/// </summary>
		public string TestFilePath
		{
			get
			{
				return this.testFilePath;
			}
		}

        protected virtual void CheckTestFile()
        {
            if (!File.Exists(this.testFilePath))
            {
                string fullPath = Path.GetFullPath(this.testFilePath);
                throw new FileNotFoundException("Test file " + this.testFilePath + " not found (full path: " + fullPath+")");
            }
        }

        protected override void CreateDomain()
        {
            this.CheckTestFile();
            FileInfo testInfo = new FileInfo(this.testFilePath);
            string appBase = testInfo.DirectoryName;
            string domainName = string.Format("domain-{0}", testInfo.Name);
            string configFile = testInfo.FullName + ".config";
            string binPath = testInfo.DirectoryName;
            this.MakeDomain(domainName, appBase, configFile, binPath);
        }

        protected override void SetUpTestEngine()
        {
            base.SetUpTestEngine();
            this.TestEngine.AddHintDirectory(this.testFilePath);
            this.TestEngine.SetTestFilePath(this.testFilePath);
        }
    }
}
