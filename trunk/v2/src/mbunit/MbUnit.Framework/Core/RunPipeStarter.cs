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

using System.Threading;
using System.IO;
using System.Reflection;
using MbUnit.Core.Invokers;

namespace MbUnit.Core
{
    using System;
    using MbUnit.Core.Monitoring;
    using MbUnit.Core.Collections;
    using MbUnit.Core.Exceptions;
    using MbUnit.Core.Reports.Serialization;
    using System.Collections;

    public sealed class RunPipeStarter
    {
        private RunPipeListenerCollection listeners = new RunPipeListenerCollection();
        private RunPipe pipe = null;
        private ReportMonitor monitor = new ReportMonitor();
        private volatile object syncRoot = new object();
        private volatile bool abortPending = false;

        private ReportRun result = null;

        public RunPipeStarter(RunPipe pipe)
        {
            if (pipe == null)
                throw new ArgumentNullException("pipe");
            this.pipe = pipe;
        }

        public RunPipeListenerCollection Listeners
        {
            get
            {
                return this.listeners;
            }
        }

        public RunPipe Pipe
        {
            get
            {
                return this.pipe;
            }
        }

        public bool HasResult
        {
            get
            {
                return this.result != null;
            }
        }

        public ReportRun Result
        {
            get
            {
                return this.result;
            }
        }

        public void ClearResult()
        {
            this.result = null;
            this.abortPending = false;
        }

        public object SyncRoot
        {
            get { return this.abortPending; }
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

        public void Abort()
        {
            this.abortPending = true;
        }

        public void Run(Object fixture, bool IsExplicit)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            // create arguments...
            ArrayList args = new ArrayList();

            this.abortPending = false;
            this.monitor.Start();
            this.Start();

            Exception testException = null;
            foreach (RunInvokerVertex v in pipe.Invokers)
            {
                if (!v.HasInvoker)
                    continue;

                if (v.Invoker is ExplicitRunInvoker && !IsExplicit)
                    testException = new IgnoreRunException("Explicit selection required.");

                // if exception has already been thrown, execute non test only
                if (testException != null)
                {
                    if (!v.Invoker.Generator.IsTest)
                    {
                        // execute tear down code and drop exceptions
                        try
                        {
                            v.Invoker.Execute(fixture, args);
                        }
                        catch (Exception)
                        { }
                    }
                }
                else
                {
                    try
                    {
                        if (this.IsAbortPending)
                            continue;

                        if (v.Invoker.Generator.IsTest)
                        {
                            v.Invoker.Execute(fixture, args);
                        }
                        else
                        {
                            // can't re-use args for TearDown, or we get a parameter mismatch!
                            v.Invoker.Execute(fixture, new ArrayList());
                        }
                    }
                    catch (Exception ex)
                    {
                        testException = ex;
                    }
                }
            }

            this.monitor.Stop();

            // success
            if (testException == null)
            {
                this.result = ReportRun.Success(this.pipe,this.monitor);
                this.Success();
            }
            else
            {
                // get rid of TargetInvocationException
                if (testException.GetType() == typeof(TargetInvocationException))
                    testException = testException.InnerException;

                // check if ignoreexception
                if (testException is IgnoreRunException)
                {
                    this.result = ReportRun.Ignore(this.pipe,this.monitor);
                    this.Ignore();
                }
                // failure
                else
                {
                    this.result = ReportRun.Failure(this.pipe,this.monitor,testException);
                    this.Failure();
                }
            }
        }

        public void Fail(ReportMonitor monitor,Exception ex)
        {
            if (monitor == null)
                throw new ArgumentNullException("monitor");
            if (ex == null)
                throw new ArgumentNullException("ex");

            this.Start();
            this.result = ReportRun.Failure(this.Pipe, monitor, ex);
            this.Failure();
        }

        public void Skip(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            this.Start();
            this.result = ReportRun.Skip(this.Pipe, ex);
            this.Skip();
        }

        private void Start()
        {
            foreach (IRunPipeListener listener in this.listeners)
            {
                listener.Start(this.pipe);
            }
            MbUnit.Framework.Assert.ClearCounters();
        }

        private void Success()
        {
            foreach (IRunPipeListener listener in this.listeners)
            {
                listener.Success(this.pipe,this.result);
            }
        }

        private void Ignore()
        {
            foreach (IRunPipeListener listener in this.listeners)
            {
                listener.Ignore(this.pipe,this.result);
            }
        }

        private void Failure()
        {
            foreach (IRunPipeListener listener in this.listeners)
            {
                listener.Failure(this.pipe,this.result);
            }
        }

        private void Skip()
        {
            foreach (IRunPipeListener listener in this.listeners)
            {
                listener.Skip(this.pipe, this.result);
            }
        }
    }
}
