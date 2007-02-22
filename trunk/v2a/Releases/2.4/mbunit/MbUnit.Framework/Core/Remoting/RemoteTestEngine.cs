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
using System.IO;

using MbUnit.Core.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Reports;
using System.Windows.Forms;
using MbUnit.Core.Exceptions;
using MbUnit.Core.Monitoring;
using MbUnit.Core.Graph;

using QuickGraph.Algorithms;

namespace MbUnit.Core.Remoting
{
    using MbUnit.Core.Reports.Serialization;
    using MbUnit.Core.Filters;

    [Serializable]
    public class RemoteTestEngine : LongLivingMarshalByRefObject, IDisposable
    {
        [NonSerialized]
        private Assembly testAssembly = null;
        [NonSerialized]
        private AssemblyResolverManager resolver = null;
        [NonSerialized]
        private TopologicalSortAlgorithm topo=null;

        private ReportListener report = null;
        private FixtureExplorer explorer = null;
        private IFixtureRunner fixtureRunner = null;

        public RemoteTestEngine()
            :this(new DependencyFixtureRunner())
        { }

        public RemoteTestEngine(IFixtureRunner fixtureRunner)
        {
            this.SetFixtureRunner(fixtureRunner);
            this.report = new ReportListener();
            this.resolver = new AssemblyResolverManager();
        }

        protected void SetFixtureRunner(IFixtureRunner fixtureRunner)
        {
            if (fixtureRunner == null)
                throw new ArgumentNullException("fixtureRunner");
            this.fixtureRunner = fixtureRunner;
        }

        public virtual void Dispose()
        {
            this.explorer = null;
            this.fixtureRunner = null;
            if (this.resolver != null)
            {
                this.resolver.Dispose();
                this.resolver = null;
            }
            this.report = null;
            this.testAssembly = null;
        }

        public FixtureExplorer Explorer
        {
            get
            {
                return this.explorer;
            }
        }

        public ReportListener Report
        {
            get
            {
                return this.report;
            }
        }

        public IFixtureRunner FixtureRunner
        {
            get
            {
                return this.fixtureRunner;
            }
        }

        public Assembly TestAssembly
        {
            get
            {
                return this.testAssembly;
            }
        }

        public AssemblyResolverManager Resolver
        {
            get
            {
                return this.resolver;
            }
        }

        public void AddHintDirectory(string directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");
            this.resolver.AddHintDirectory(directory);
        }

        public virtual void SetTestFilePath(string testFilePath)
        {
            try
            {
                if (testFilePath == null)
                    throw new ArgumentNullException("testFilePath");
                string fileName = Path.GetFileNameWithoutExtension(testFilePath);

                Assembly testAssembly = Assembly.Load(
                    fileName
                    , this.GetType().Assembly.Evidence
                    );
                if (testAssembly == null)
                    throw new FileNotFoundException(testFilePath);
                SetTestAssembly(testAssembly);
            }
            catch (Exception ex)
            {
                throw new ReportExceptionException(
                    "Failed loading test assembly",
                    ReportException.FromException(ex)
                    );
            }
        }

        public void SetTestAssembly(Assembly testAssembly)
        {
            if (testAssembly == null)
                throw new ArgumentNullException("testAssembly");
            this.testAssembly = testAssembly;
            this.explorer = new FixtureExplorer(this.testAssembly);
            this.topo = new TopologicalSortAlgorithm(
                this.Explorer.FixtureGraph.Graph);
        }

        public ReportCounter GetTestCount()
        {
            if (this.Explorer == null)
                return new ReportCounter();
            else
                return this.Explorer.FixtureGraph.GetCounter();
        }

        #region Population
        public virtual void Populate()
        {
            try
            {
                this.explorer.Explore();
                this.LoadPipes();
            }
            catch (Exception ex)
            {
                ReportException rex = ReportException.FromException(ex);
                ReportExceptionException rexe = new ReportExceptionException(
                    String.Format("Error while loading tests in {0}: {1}",
                        this.testAssembly.FullName,
                        ex.Message
                            ),
                    rex);
                throw rexe;
            }
        }

        protected virtual  void LoadPipes()
        {
            try
            {
                foreach (Fixture fixture in this.explorer.FixtureGraph.Fixtures)
                {
                    try
                    {
                        fixture.Load(this.FixtureRunner.RunPipeFilter);
                        foreach (RunPipeStarter starter in fixture.Starters)
                            report.AttachToPipe(starter);
                    }
                    catch (Exception ex)
                    {
                        String message = String.Format("Fixture {0} failed loading.", fixture.Name);
                        throw new ApplicationException(message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed loading the test pipes", ex);
            }
        }

        public virtual void Clear()
        {
            if (this.Explorer!=null)
                this.Explorer.FixtureGraph.Clear();
            if (this.report!=null)
                this.report.Clear();
            this.report = null;
            this.testAssembly = null;
        }
        #endregion

        public virtual void RunPipes()
        {
            if (!(this.Explorer.Filter is AnyFixtureFilter))
            {
                this.FixtureRunner.IsExplicit = true;
            }
            this.FixtureRunner.Run(this.Explorer, this.Report);
        }

        /// <summary>
        /// Supports verbose output option of console app. 
        /// Added as part of fix to issue MBUNIT-28.
        /// </summary>
        /// <author>Marc Stober</author>
        /// <date>December 21, 2005</date>
        public void AddConsoleListener()
        {
            foreach (Fixture f in Explorer.FixtureGraph.Fixtures)
            {
                foreach (RunPipeStarter s in f.Starters)
                {
                    s.Listeners.Add(new MbUnit.Core.Cons.ConsoleRunPipeListener());
                }
            }

        }

    }
}
