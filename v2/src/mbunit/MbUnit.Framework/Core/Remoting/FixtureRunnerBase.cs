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
using System.Threading;

using MbUnit.Core.Collections;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Monitoring;
using MbUnit.Core.Filters;
using MbUnit.Core.Exceptions;
using MbUnit.Core.Remoting;

namespace MbUnit.Core.Remoting
{
    [Serializable]
    public abstract class FixtureRunnerBase : LongLivingMarshalByRefObject, IFixtureRunner
    {
        #region Filters
        [NonSerialized]
        private FixtureExplorer explorer = null;
        [NonSerialized]
        private ReportListener report = null;
        private IFixtureFilter fixtureFilter = FixtureFilters.Any;
        private IRunPipeFilter runPipeFilter = new AnyRunPipeFilter();
        private volatile object syncRoot = new object();
        private volatile bool abortPending = false;
        private volatile RunPipeStarter currentPipeStarter = null;
        private bool isexplicit;

        public bool IsExplicit
        {
            get
            {
                return this.isexplicit;
            }
            set
            {
                this.isexplicit = value;
            }
        }

        protected FixtureExplorer Explorer
        {
            get
            {
                return this.explorer;
            }
        }

        protected ReportListener Report
        {
            get
            {
                return this.report;
            }
        }

        public object SyncRoot
        {
            get { return this.syncRoot; }
        }

        public void Abort()
        {
            lock (this.SyncRoot)
            {
                this.abortPending = true;
                if (this.currentPipeStarter != null)
                    this.currentPipeStarter.Abort();
            }
        }

        public bool IsAbortPending
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this.abortPending;
                }
            }
        }

        private RunPipeStarter CurrentPipeStarter
        {
            get { lock (this.SyncRoot) { return this.currentPipeStarter; } }
            set { lock (this.SyncRoot) { this.currentPipeStarter = value; } }
        }

        public IFixtureFilter FixtureFilter
        {
            get
            {
                return fixtureFilter;
            }

            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                fixtureFilter = value;
            }
        }
        public IRunPipeFilter RunPipeFilter
        {
            get
            {
                return this.runPipeFilter;
            }
            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                this.runPipeFilter = value;
            }
        }

        protected virtual bool FilterFixture(Fixture fixture)
        {
            return this.FixtureFilter.Filter(fixture.Type);
        }

        protected virtual bool FilterRunPipe(RunPipeStarter starter)
        {
            return this.RunPipeFilter.Filter(starter.Pipe);
        }

        #endregion

        #region Events
        public event ReportSetUpAndTearDownEventHandler AssemblySetUp;
        protected virtual void OnAssemblySetUp(ReportSetUpAndTearDownEventArgs e)
        {
            if (this.AssemblySetUp != null)
                this.AssemblySetUp(this, e);
        }
        public event ReportSetUpAndTearDownEventHandler AssemblyTearDown;
        protected virtual void OnAssemblyTearDown(ReportSetUpAndTearDownEventArgs e)
        {
            if (this.AssemblyTearDown != null)
                this.AssemblyTearDown(this, e);
        }
        public event ReportSetUpAndTearDownEventHandler TestFixtureSetUp;
        protected virtual void OnTestFixtureSetUp(ReportSetUpAndTearDownEventArgs e)
        {
            if (this.TestFixtureSetUp != null)
                this.TestFixtureSetUp(this, e);
        }
        public event ReportSetUpAndTearDownEventHandler TestFixtureTearDown;
        protected virtual void OnTestFixtureTearDown(ReportSetUpAndTearDownEventArgs e)
        {
            if (this.TestFixtureTearDown != null)
                this.TestFixtureTearDown(this, e);
        }
        public event ReportRunEventHandler RunResult;
        protected virtual void OnRunResult(ReportRunEventArgs e)
        {
            if (this.RunResult != null)
                this.RunResult(this, e);
        }
        #endregion

        #region Execution
        public virtual void Run(
            FixtureExplorer explorer,
            ReportListener reportListener
            )
        {
            if (explorer == null)
                throw new ArgumentNullException("explorer");
            if (reportListener == null)
				throw new ArgumentNullException("reportListener");

            this.explorer = explorer;
            this.report = reportListener;
            this.abortPending = false;

            try
            {
                // start reporting
                this.Report.StartTests();

                if (this.IsAbortPending)
                    return;

                // assembly setup
                if (!this.RunAssemblySetUp())
                    return;

                if (this.IsAbortPending)
                    return;

                try
                {
                    RunFixtures();
                }
                finally
                {
                    this.RunAssemblyTearDown();
                }
            }
            catch (Exception ex)
            {
                ReportException rex = ReportException.FromException(ex);
                ReportExceptionException rexe = new ReportExceptionException(
                    "Internal error while running tests in " + 
                    this.Explorer.AssemblyName, rex);
                throw rexe;
            }
            finally
            {
                this.Report.FinishTests();

                this.explorer = null;
                this.report = null;
            }
        }

        protected virtual bool RunAssemblySetUp()
        {
            if (!this.Explorer.HasAssemblySetUp)
                return true;

            ReportSetUpAndTearDown setup = this.Explorer.AssemblySetUp();
            this.report.AssemblySetUp(this.Explorer.Assembly, setup);
            this.OnAssemblySetUp(new ReportSetUpAndTearDownEventArgs(setup));

            return setup.Result == ReportRunResult.Success;
        }

        protected virtual bool RunAssemblyTearDown()
        {
            if (!this.explorer.HasAssemblyTearDown)
                return true;

            ReportSetUpAndTearDown tearDown = this.explorer.AssemblyTearDown();
            this.report.AssemblyTearDown(this.Explorer.Assembly, tearDown);
            this.OnAssemblyTearDown(new ReportSetUpAndTearDownEventArgs(tearDown));

            return tearDown.Result == ReportRunResult.Success;
        }

        protected abstract void RunFixtures();

        protected virtual ReportRunResult RunFixture(Fixture fixture)
        {
            if (this.IsAbortPending)
                return ReportRunResult.Failure;

            FixtureRunnerStarter fixtureStarter = new FixtureRunnerStarter(this, fixture);
            Thread thread = null;
            try
            {
                thread = new Thread(new ThreadStart(fixtureStarter.Start));
                if (fixture.ApartmentState != ApartmentState.Unknown)
                    thread.ApartmentState = fixture.ApartmentState;
                thread.Start();
                // execute and watch for time outs
                if (!thread.Join(fixture.TimeOut))
                {
                    this.Abort();
                    Thread.Sleep(1000);
                    if (thread.ThreadState != ThreadState.Stopped)
                        thread.Abort();
                    ReportMonitor monitor = new ReportMonitor();
                    monitor.Start();
                    monitor.Stop();
                    FixtureTimedOutException ex = new FixtureTimedOutException();
                    foreach (RunPipeStarter starter in fixture.Starters)
                    {
                        starter.Fail(monitor, ex);
                        OnRunResult(new ReportRunEventArgs(starter.Result));
                    }
                    thread = null;
                    return ReportRunResult.Failure;
                }
                thread = null;

                // check if something occured duregin execution
                if (fixtureStarter.ThrowedException != null)
                    throw new FixtureExecutionException(fixture.Name, fixtureStarter.ThrowedException);

                return fixtureStarter.Result;
            }
            finally
            {
                if (thread != null)
                {
                    thread.Abort();
                    thread = null;
                }
            }
        }

        private ReportRunResult InternalRunFixture(Fixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");


            Object fixtureInstance = null;
            ReportRunResult result = ReportRunResult.Success;
            try
            {
                fixtureInstance = CreateFixtureInstance(fixture);
                if (fixtureInstance == null)
                    return ReportRunResult.Failure;

                // aborting if requested
                if (this.IsAbortPending)
                    return ReportRunResult.Failure;

                fixtureInstance = RunFixtureSetUp(fixture, fixtureInstance);
                if (fixtureInstance == null)
                    return ReportRunResult.Failure;

                // aborting if requested
                if (this.IsAbortPending)
                    return ReportRunResult.Failure;

                this.CurrentPipeStarter = null;
                foreach (RunPipeStarter starter in fixture.Starters)
                {
                    if (!this.FilterRunPipe(starter))
                        continue;
                    // aborting if requested
                    if (this.IsAbortPending)
                        return ReportRunResult.Failure;
                    this.CurrentPipeStarter = starter;
                    starter.Run(fixtureInstance, this.IsExplicit);
                    OnRunResult(new ReportRunEventArgs(starter.Result));
                    if (starter.Result.Result != ReportRunResult.Success && result != ReportRunResult.Failure)
                        result = starter.Result.Result;
                }
            }
            finally
            {
                this.CurrentPipeStarter = null;
                if (fixtureInstance != null)
                {
                    RunFixtureTearDown(fixture, fixtureInstance);
                }
            }

            return result;
        }

        protected virtual void FailStarters(
            Fixture fixture, 
            ReportMonitor monitor, 
            Exception ex)
        {
            if (monitor == null)
                throw new ArgumentNullException("monitor");
            if (ex == null)
                throw new ArgumentNullException("ex");

            foreach (RunPipeStarter starter in fixture.Starters)
            {
                starter.Fail(monitor, ex);
                OnRunResult(new ReportRunEventArgs(starter.Result));
            }
        }

        protected virtual void SkipStarters(Fixture fixture, Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            foreach (RunPipeStarter starter in fixture.Starters)
            {
                starter.Skip(ex);
                OnRunResult(new ReportRunEventArgs(starter.Result));
            }
        }

        protected virtual Object CreateFixtureInstance(Fixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            ReportMonitor monitor = new ReportMonitor();
            monitor.Start();
            try
            {
                Object fix = fixture.CreateInstance();
                return fix;
            }
            catch (Exception ex)
            {
                monitor.Stop();
                FixtureConstructorFailedException cex = new FixtureConstructorFailedException(fixture, ex);
                FailStarters(fixture, monitor, ex);
                return null;
            }
        }

        protected virtual object RunFixtureSetUp(Fixture fixture, object fixtureInstance)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            ReportMonitor monitor = new ReportMonitor();
            try
            {
                if (fixture.HasSetUp)
                {
                    monitor.Start();
                    fixture.SetUp(fixtureInstance);
                    monitor.Stop();

                    ReportSetUpAndTearDown setup = new ReportSetUpAndTearDown("TestFixtureSetUp", monitor);
                    if (this.Report!=null)
                        this.Report.TestFixtureSetUp(fixture, setup);
                    this.OnTestFixtureSetUp(new ReportSetUpAndTearDownEventArgs(setup));
                }

                return fixtureInstance;
            }
            catch (Exception ex)
            {
                monitor.Stop();
                if (fixture.HasSetUp)
                {
                    ReportSetUpAndTearDown setup = new ReportSetUpAndTearDown("TestFixtureSetUp", monitor, ex);
                    if (this.Report != null)
                        this.Report.TestFixtureSetUp(fixture, setup);
                    this.OnTestFixtureSetUp(new ReportSetUpAndTearDownEventArgs(setup));
                }

                // fail all starters
                FixtureSetUpFailedException setUpEx = new FixtureSetUpFailedException(ex);
                FailStarters(fixture, monitor, setUpEx);

                // add error message
                return null;
            }
        }

        protected virtual void RunFixtureTearDown(Fixture fixture, object fixtureInstance)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            ReportMonitor monitor = new ReportMonitor();
            try
            {
                monitor.Start();
                fixture.TearDown(fixtureInstance);
                monitor.Stop();

                if (fixture.HasTearDown)
                {
                    ReportSetUpAndTearDown tearDown = new ReportSetUpAndTearDown("TestFixtureTearDown", monitor);
                    if (this.Report != null)
                        this.Report.TestFixtureTearDown(fixture, tearDown);
                    this.OnTestFixtureTearDown(new ReportSetUpAndTearDownEventArgs(tearDown));
                }
            }
            catch (Exception ex)
            {
                monitor.Stop();
                ReportSetUpAndTearDown tearDown = new ReportSetUpAndTearDown(
                    "TestFixtureTearDown", monitor, ex);
                if (this.Report != null)
                    this.Report.TestFixtureTearDown(fixture, tearDown);
                this.OnTestFixtureTearDown(new ReportSetUpAndTearDownEventArgs(tearDown));
            }
        }

        #endregion

        #region Threading
        private sealed class FixtureRunnerStarter
        {
            private FixtureRunnerBase runner;
            private Fixture fixture;
            private Exception throwedException;
            private ReportRunResult result = ReportRunResult.NotRun;

            public FixtureRunnerStarter(
                FixtureRunnerBase runner,
                Fixture fixture)
            {
                this.runner = runner;
                this.fixture = fixture;
            }

            public void Start()
            {
                try
                {
                    this.result = this.runner.InternalRunFixture(this.fixture);
                }
                catch (Exception ex)
                {
                    this.throwedException = ex;
                }
            }

            public ReportRunResult Result
            {
                get { return this.result; }
            }

            public Exception ThrowedException
            {
                get { return this.throwedException; }
            }
        }
        #endregion
    }
}
