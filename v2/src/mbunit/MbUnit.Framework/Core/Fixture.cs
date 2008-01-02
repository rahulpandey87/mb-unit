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
using System.Reflection;
using System.Threading;

namespace MbUnit.Core
{
	using MbUnit.Core.Runs;
    using MbUnit.Framework;
	using MbUnit.Core.Exceptions;
    using MbUnit.Core.Collections;
    using MbUnit.Core.Reports.Serialization;
    using MbUnit.Core.Invokers;
    using MbUnit.Core.Filters;

	public class Fixture
	{
		private Type type;
		private IRun run;
        private MethodInfo setUp = null;
        private MethodInfo tearDown = null;
        private bool ignored;
        private RunPipeStarterCollection starters;
        private TimeSpan timeOut = new TimeSpan(0, 5, 0);
        private ApartmentState apartmentState = ApartmentState.Unknown;

        public Fixture(Type type, IRun run, MethodInfo setUp, MethodInfo tearDown, bool ignored)
		{
			if (type==null)
				throw new ArgumentNullException("type");
			if (run==null)
				throw new ArgumentNullException("run");
			this.type=type;
            ReflectionAssert.HasDefaultConstructor(this.type);

            this.run = run;
            this.setUp = setUp;
            this.tearDown = tearDown;
            this.ignored = ignored;
            this.starters = new RunPipeStarterCollection(this);
        }

        public string Name
        {
            get
            {
                if (this.Type.DeclaringType != null)
                {
                    string name = this.Type.FullName;
                    return name.Substring(
                        this.Type.Namespace.Length+1,
                        name.Length - this.Type.Namespace.Length-1
                        );
                }
                else
                    return this.Type.Name;
            }
        }

        public Type Type
		{
			get
			{
				return this.type;
			}
		}

		public IRun Run
		{
			get
			{
				return this.run;
			}
        }

        public RunPipeStarterCollection Starters
        {
            get
            {
                return this.starters;
            }
        }

        public MethodInfo SetUpMethod
        {
            get
            {
                return this.setUp;
            }
        }
        public MethodInfo TearDownMethod
        {
            get
            {
                return this.tearDown;
            }
        }

        /// <summary>
        /// Returns true if the entire test fixture is ignored.
        /// </summary>
        public bool IsIgnored
        {
            get
            {
                return this.ignored;
            }
        }

        public TimeSpan TimeOut
        {
            get { return this.timeOut; }
            set { this.timeOut = value; }
        }

        public ApartmentState ApartmentState
        {
            get { return this.apartmentState; }
            set { this.apartmentState = value; }
        }

        public bool HasSetUp
        {
            get
            {
                return this.setUp != null;
            }
        }
        public bool HasTearDown
        {
            get
            {
                return this.tearDown!=null;
            }
        }

        public void Load(IRunPipeFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            this.starters.Clear();
            try
            {
                RunInvokerTree tree = new RunInvokerTree(this);

                foreach (RunPipe pipe in tree.AllTestPipes())
                {
                    if (!filter.Filter(pipe))
                        continue;
    
                    RunPipeStarter starter = new RunPipeStarter(pipe);
                    this.Starters.Add(starter);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while create the invoker tree", ex);
            }
        }

        public ReportCounter GetCounter()
        {
            ReportCounter counter = new ReportCounter();
            counter.RunCount = this.Starters.Count;
            foreach (RunPipeStarter starter in this.Starters)
            {
                if (starter.HasResult)
                {
                    switch (starter.Result.Result)
                    {
                        case ReportRunResult.Success:
                            ++counter.SuccessCount;
                            break;
                        case ReportRunResult.Failure:
                            ++counter.FailureCount;
                            break;
                        case ReportRunResult.Ignore:
                            ++counter.IgnoreCount;
                            break;
                    }
                }
            }
            return counter;
        }

        #region SetUp and TearDown
        public Object CreateInstance()
        {
            Object fixture = TypeHelper.CreateInstance(this.type); ;
            return fixture;
        }

        public void SetUp(object fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (this.setUp!=null && ! ignored)
                this.setUp.Invoke(fixture,null);
        }

        public void TearDown(object fixture)
        {
            if (fixture==null)
                throw new ArgumentNullException("fixture");
            if (this.tearDown!=null && ! ignored)
                this.tearDown.Invoke(fixture,null);
            IDisposable disposable = fixture as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}
