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
using System.Reflection;
using System.IO;
using MbUnit.Core.Reports;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Cons.CommandLine;
using MbUnit.Core.Remoting;

namespace MbUnit.Core
{

    public sealed class AutoRunner : IDisposable
    {
        private AssemblyResolverManager resolver = null;
        private AssemblyTestDomain domain = null;
        private ReportResult result = null;
        private bool verbose = true;

        public AutoRunner()
            :this(Assembly.GetEntryAssembly())
        {
            this.Log += new ErrorReporter(Console.Out.WriteLine);
        }

        public AutoRunner(Assembly testAssembly)
        {
            TypeHelper.InvokeFutureStaticMethod(
                typeof(System.Windows.Forms.Application), "EnableVisualStyles");

            this.resolver = new AssemblyResolverManager();
            this.resolver.AddMbUnitDirectories();
            this.domain = new AssemblyTestDomain(testAssembly);
        }

        public int ExitCode
        {
            get
            {
                return (this.IsSuccess) ? 0 : -1;
            }
        }

        public bool IsSuccess
        {
            get
            {
                if (this.result == null)
                    return true;
                return this.result.Counter.FailureCount == 0;
            }
        }

        public bool Verbose
        {
            get
            {
                return this.verbose;
            }
            set
            {
                this.verbose = value;
            }
        }

        public event ErrorReporter Log;

        private void OnLog(String format, params Object[] args)
        {
            if (this.Log != null)
                this.Log(String.Format(format, args));
        }

        public AssemblyTestDomain Domain
        {
            get
            {
                return this.domain;
            }
        }

        public void Load()
        {
            try
            {
                AddLog("MbUnit AutorRunner");
                AddLog("Loading test domain");
                // load domain
                this.domain.Load();
            }
            catch (Exception ex)
            {
                AddLog("Exception occured while loading");
                AddLog(ex.ToString());
                if (this.domain != null)
                {
                    this.domain.Dispose();
                    this.domain = null;
                }
                throw new Exception("Exception while loading assemblies", ex);
            }
        }

        public void Run()
        {
            this.Load();

            try
            {
                AddLog("Found  {0} tests, running...", this.domain.TestEngine.GetTestCount().RunCount);
                // attach listeners
                if (verbose)
                {
                    this.domain.TestEngine.FixtureRunner.AssemblySetUp += new ReportSetUpAndTearDownEventHandler(TestEngine_AssemblySetUp);
                    this.domain.TestEngine.FixtureRunner.AssemblyTearDown += new ReportSetUpAndTearDownEventHandler(TestEngine_AssemblyTearDown);
                    this.domain.TestEngine.FixtureRunner.TestFixtureSetUp += new ReportSetUpAndTearDownEventHandler(TestEngine_TestFixtureSetUp);
                    this.domain.TestEngine.FixtureRunner.TestFixtureTearDown += new ReportSetUpAndTearDownEventHandler(TestEngine_TestFixtureTearDown);
                    this.domain.TestEngine.FixtureRunner.RunResult += new ReportRunEventHandler(TestEngine_RunResult);
                }

                // run tests
                this.domain.TestEngine.RunPipes();

                // store result
                this.result = this.domain.TestEngine.Report.Result;
                AddLog("Test finished: {0} success, {1} ignored, {2} failures, {3:0.00}s",
                    this.result.Counter.SuccessCount,
                    this.result.Counter.IgnoreCount,
                    this.result.Counter.FailureCount,
                    this.result.Counter.Duration
                    );
            }
            catch (Exception ex)
            {
                AddLog("[error] Unexpected exception occured in MbUnit");
                AddLog(ex.ToString());
            }
            finally
            {
                AddLog("[cleaning] unloading domain");
                if (this.domain != null)
                {
                    if (this.domain.TestEngine != null && this.verbose)
                    {
                        this.domain.TestEngine.FixtureRunner.AssemblySetUp -= new ReportSetUpAndTearDownEventHandler(TestEngine_AssemblySetUp);
                        this.domain.TestEngine.FixtureRunner.AssemblyTearDown -= new ReportSetUpAndTearDownEventHandler(TestEngine_AssemblyTearDown);
                        this.domain.TestEngine.FixtureRunner.TestFixtureSetUp -= new ReportSetUpAndTearDownEventHandler(TestEngine_TestFixtureSetUp);
                        this.domain.TestEngine.FixtureRunner.TestFixtureTearDown -= new ReportSetUpAndTearDownEventHandler(TestEngine_TestFixtureTearDown);
                        this.domain.TestEngine.FixtureRunner.RunResult -= new ReportRunEventHandler(TestEngine_RunResult);
                    }
                    this.domain.Unload();
                }
                AddLog("[cleaning] domain unloaded");
            }
        }

        public ReportResult Result
        {
            get
            {
                return this.result;
            }
        }

        public void Dispose()
        {
            if (this.resolver != null)
            {
                this.resolver.Dispose();
                this.resolver = null;
            }

            if (this.Domain != null)
            {
                this.Domain.Dispose();
                this.domain = null;
            }
        }

        private void AddLog(string format, params Object[] args)
        {
            if (this.Log == null)
                return;
            OnLog(format, args);
        }

        public string GetReportName()
        {
            string fileName = String.Format("{0}.Tests",Assembly.GetEntryAssembly().GetName().Name);
            return fileName;
        }

        public void ReportToHtml()
        {
            if (this.result == null)
            {
                AddLog("Result is a null reference. Make sure tests were executed succesfully");
                return;
            }
            AddLog("Generating HTML report");
            System.Diagnostics.Process.Start(HtmlReport.RenderToHtml(this.result,"",GetReportName()));
        }

        public void ReportToXml()
        {
            this.ReportToXml(true);
        }

        public void ReportToXml(bool display)
        {
            if (this.result == null)
            {
                AddLog("Result is a null reference. Make sure tests were executed succesfully");
                return;
            }
            AddLog("Generating XML report");
            string output = XmlReport.RenderToXml(this.result, "", GetReportName());
            if (display)
                System.Diagnostics.Process.Start(output);
        }

        public void ReportToText()
        {
            if (this.result == null)
            {
                AddLog("Result is a null reference. Make sure tests were executed succesfully");
                return;
            }
            AddLog("Generating Text report");
            System.Diagnostics.Process.Start(TextReport.RenderToText(this.result, "", GetReportName()));
        }
        public void ReportToDox()
        {
            if (this.result == null)
            {
                AddLog("Result is a null reference. Make sure tests were executed succesfully");
                return;
            }
            AddLog("Generating Dox report");
            System.Diagnostics.Process.Start(DoxReport.RenderToDox(this.result, "", GetReportName()));
        }

        void TestEngine_AssemblySetUp(object sender, ReportSetUpAndTearDownEventArgs e)
        {
            AddLog("[assembly-setup][{0}] {1}", e.SetUpAndTearDown.Result, e.SetUpAndTearDown.Name);
        }

        void TestEngine_AssemblyTearDown(object sender, ReportSetUpAndTearDownEventArgs e)
        {
            AddLog("[assembly-teardown][{0}] {1}", e.SetUpAndTearDown.Result, e.SetUpAndTearDown.Name);
        }

        void TestEngine_TestFixtureSetUp(object sender, ReportSetUpAndTearDownEventArgs e)
        {
            AddLog("[fixture-setup][{0}] {1}", e.SetUpAndTearDown.Result, e.SetUpAndTearDown.Name);
        }

        void TestEngine_TestFixtureTearDown(object sender, ReportSetUpAndTearDownEventArgs e)
        {
            AddLog("[fixture-teardown][{0}] {1}", e.SetUpAndTearDown.Result, e.SetUpAndTearDown.Name);
        }

        void TestEngine_RunResult(object sender, ReportRunEventArgs e)
        {
            switch (e.Run.Result)
            {
                case ReportRunResult.Success:
                    AddLog("[success] {0}", e.Run.Name);
                    break;
                case ReportRunResult.Failure:
                    AddLog("[failure] {0}: {1}", e.Run.Name, e.Run.Exception.Message);
                    break;
                case ReportRunResult.Ignore:
                    AddLog("[ignored] {0}", e.Run.Name);
                    break;
                case ReportRunResult.NotRun:
                    AddLog("[not-run] {0}", e.Run.Name);
                    break;
                case ReportRunResult.Skip:
                    AddLog("[skipped] {0}", e.Run.Name);
                    break;
            }
        }
    }
}
