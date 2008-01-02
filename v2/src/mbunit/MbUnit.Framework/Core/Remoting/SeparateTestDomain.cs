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
using System.Reflection;
using System.Collections;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Lifetime;
using System.Diagnostics;
using System.Configuration;
using System.Windows.Forms;
using MbUnit.Core.Config;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;

namespace MbUnit.Core.Remoting
{
    [Serializable]
    public abstract class SeparateTestDomain : TestDomainBase
    {
        private AppDomain domain = null;
        private bool shadowCopyFiles = false;
        private string cachePath = null;
        private bool sandBox = false;

        protected SeparateTestDomain()
        {}

        public override AppDomain Domain
        {
            get { return this.domain; }
        }

        public override bool SeparateAppDomain
        {
            get { return true; }
        }

        public bool SandBox
        {
            get
            {
                return this.sandBox;
            }
            set
            {
                this.sandBox = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the assemblies have
        /// to be shadow copied
        /// </summary>
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

        public string CachePath
        {
            get
            {
                return this.cachePath;
            }
        }

        protected override void CreateTestEngine()
        {
            if (this.domain == null)
                throw new InvalidOperationException("AppDomain is a null reference");
            try
            {
                RemoteTestEngine te = null;
                try
                {
                    te = (RemoteTestEngine)
                                this.domain.CreateInstanceAndUnwrap(
                                this.TestEngineType.Assembly.GetName().Name,
                                this.TestEngineType.FullName
                                );
                }
                catch (Exception)
                {
                    te = (RemoteTestEngine)
                                this.domain.CreateInstanceFromAndUnwrap(
                                this.TestEngineType.Assembly.GetName().Name+".dll",
                                this.TestEngineType.FullName
                                );
                }
                this.SetTestEngine(te);
                this.TestEngine.AddHintDirectory(Path.GetDirectoryName(typeof(SeparateTestDomain).Assembly.Location));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed creating the TestEngine instance", ex);
            }
        }

        private void CopyMbUnitAssemblies()
        {
            CopyFileToTestFolder(typeof(SeparateTestDomain).Assembly.Location);
            CopyFileToTestFolder(typeof(TestFu.Data.SqlClient.SqlAdministrator).Assembly.Location);
            CopyFileToTestFolder(typeof(QuickGraph.Vertex).Assembly.Location);
            CopyFileToTestFolder(typeof(QuickGraph.Algorithms.AlgoUtility).Assembly.Location);
        }

        private void CopyFileToTestFolder(string file)
        {
            string destFile = Path.GetFullPath(Path.Combine(this.domain.BaseDirectory,
                Path.GetFileName(file)));
            try
            {
                File.Copy(file, destFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed Copying " + file);
                Console.WriteLine(ex.ToString());
            }
        }

        public override void  Unload()
        {
            try
            {
                if (this.domain == null)
                    return;

                if (this.TestEngine != null)
                    this.TestEngine.Clear();
                this.SetTestEngine(null);

                AppDomain.Unload(this.domain);
                this.domain = null;
                if (this.ShadowCopyFiles &&this.CachePath!=null && Directory.Exists(this.CachePath))
                    CacheFolderHelper.DeleteDir(new DirectoryInfo(this.CachePath));
                this.OnUnLoaded();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed unloading domain", ex);
            }
        }

        /// <summary>
        /// Set the location for caching and delete any old cache info
        /// </summary>
		/// <param name="setup">Our domain</param>
        protected void ConfigureCachePath(AppDomainSetup setup)
        {
            try
            {
                this.cachePath = String.Format(@"%TEMP%\Cache\{0:00000000}", DateTime.Now.Ticks);
                this.cachePath = Environment.ExpandEnvironmentVariables(cachePath);
                this.cachePath = Path.GetFullPath(this.cachePath);

                DirectoryInfo dir = new DirectoryInfo(this.cachePath);
                if (dir.Exists)
                    CacheFolderHelper.DeleteDir(dir);
                dir.Create();
                setup.CachePath = cachePath;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed configurating the Shadow copy path", ex);
            }
        }

        /// <summary> 
        /// This method is used to provide assembly location resolver. It is called on event as needed by the CLR. 
        /// Refer to document related to AppDomain.CurrentDomain.AssemblyResolve 
        /// </summary> 
        private Assembly AssemblyResolveHandler(object sender, ResolveEventArgs e)
        {
            try
            {
                string[] assemblyDetail = e.Name.Split(',');
                string assemblyBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Assembly assembly = Assembly.LoadFrom(assemblyBasePath + @"\" + assemblyDetail[0] + ".dll");
                return assembly;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed resolving assembly", ex);
            }
        } 

        /// <summary>
        /// Creates an AppDomain for the Test Assembly
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="appBase"></param>
        /// <param name="configFile"></param>
        /// <param name="binPath"></param>
        /// <returns></returns>
        protected void MakeDomain(string domainName, string appBase, string configFile, string binPath)
        {
            try
            {
                //define an assembly resolver routine in case the CLR cannot find our assemblies. 
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveHandler);

                Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
                Evidence evidence = new Evidence(baseEvidence);

                AppDomainSetup setup = new AppDomainSetup();

                // We always use the same application name
                setup.ApplicationName = "Tests";

                // Note that we do NOT
                // set ShadowCopyDirectories because we  rely on the default
                // setting of ApplicationBase plus PrivateBinPath
                if (this.ShadowCopyFiles)
                {
                    setup.ShadowCopyFiles = "true";
                    setup.ShadowCopyDirectories = appBase;
                }
                else
                {
                    setup.ShadowCopyFiles = "false";
                }

                setup.ApplicationBase = appBase;
                setup.ConfigurationFile = configFile;
                setup.PrivateBinPath = binPath;
                if (this.ShadowCopyFiles)
                    ConfigureCachePath(setup);
                this.domain = AppDomain.CreateDomain(domainName, evidence, setup);

                if (this.SandBox)
                    this.SetSandBoxPolicy();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed creating new AppDomain", ex);
            }
        }

        private void SetSandBoxPolicy()
        {
            if (!this.SandBox)
                throw new InvalidOperationException("SandBox property is not set to true");
            // http://www.dotnetthis.com/Articles/DynamicSandboxing.htm

            // Now we need to set the appdomain policy, 
            // and to do that we will need to create a Policy Level. 
            // A Policy Level is a tree-like structure that has Code Groups as its nodes. 
            // Each code group consists of a Membership Condition (something that 
            // defines if an assembly in question belongs to the code group) and 
            // a Permission Set that is granted to the assembly if it does. 
            PolicyLevel domainPolicy = PolicyLevel.CreateAppDomainLevel();

            // Let's create a code group that gives Internet permission set 
            // to all code. 
            // First, let's create a membership condition that accepts all code. 
            AllMembershipCondition allCodeMC = new AllMembershipCondition();

            // If you were to build a more complex policy (giving different permissions 
            // to different assemblies) you could use other membership conditions, 
            // such as ZoneMembershipCondition, StrongNameMembershipCondition, etc. 

            // Now let's create a policy statement that represents Internet permissions. 
            // Here we just grab named permission set called "Internet" from the default policy, 
            // but you could also create your own permission set with whatever permissions 
            // you want in there. 
            PermissionSet internetPermissionSet = domainPolicy.GetNamedPermissionSet("Internet");
            PolicyStatement internetPolicyStatement = new PolicyStatement(internetPermissionSet);

            // We are ready to create a code group that maps all code to Internet permissions 
            CodeGroup allCodeInternetCG = new UnionCodeGroup(allCodeMC, internetPolicyStatement);

            // We have used a UnionCodeGroup here. It does not make much difference for 
            // a simple policy like ours here, but if you were to set up a more complex one 
            // you would probably add some child code groups and then the type of the parent 
            // code group would matter. UnionCodeGroup unions all permissions granted by its 
            // child code groups (as opposed to FirstMatchCodeGroup that only takes one child 
            // code group into effect). 
            // Once we have the CodeGroup set up we can add it to our Policy Level. 
            domainPolicy.RootCodeGroup = allCodeInternetCG;

            // If our root code group had any children the whole tree would be added 
            // to the appdomain security policy now. 
            // Imagine you wanted to modify our policy so that your strongname signed 
            // assemblies would get FullTrust and all other assemblies would get Internet 
            // permissions. Do accomplish that you would create a new UnionCodeGroup, 
            // whose membership condition would be a StrongNameMembershipCondition 
            // specifying your public key, and its permission set would be a "FullTrust" 
            // or just a "new PermissionSet(PermissionState.Unrestricted)". 
            // Then you would add that code group as a child to our allCodeInternetCG by 
            // calling its AddChild method. Whenever you then loaded a correct strong 
            // name signed assembly into your appdomain it would get Internet from the 
            // root code group and FullTrust from the child code group, and the effective 
            // permissions would be a union of the two, which is FullTrust. 
            // and our final policy related step is setting the AppDomain policy 
            this.Domain.SetAppDomainPolicy(domainPolicy);
        }
    }
}
