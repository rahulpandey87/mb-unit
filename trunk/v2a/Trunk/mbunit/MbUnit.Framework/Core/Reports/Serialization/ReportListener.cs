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
using System.Reflection;

namespace MbUnit.Core.Reports.Serialization
{
	using MbUnit.Framework;
	using MbUnit.Core.Invokers;
    using MbUnit.Core.Remoting;

	[Serializable]
    public sealed class ReportListener : LongLivingMarshalByRefObject, IRunPipeListener
    {
		private Hashtable assemblies = new Hashtable();
		private Hashtable namespaces = new Hashtable();
		private Hashtable fixtures = new Hashtable();
		private ReportResult result = null;

		public ReportListener()
		{}

		#region Pipe attach/detach
		public void AttachToPipe(RunPipeStarter starter)
		{
			starter.Listeners.Add(this);
		}
		
		public void DetachFromPipe(RunPipeStarter starter)
		{
			starter.Listeners.Remove(this);
		}
		#endregion

		public ReportResult Result
		{
			get
			{
				return this.result;
			}
		}

		public void StartTests()
		{
			this.result = new ReportResult();
			this.result.Date = DateTime.Now;
			this.result.Counter = new ReportCounter();

			this.assemblies.Clear();
			this.namespaces.Clear();
			this.fixtures.Clear();
		}

		public void FinishTests()
		{
			// update counts
			this.result.UpdateCounts();
		}

		public void Clear()
		{
			this.assemblies.Clear();
			this.fixtures.Clear();
			this.result=null;
        }

        #region Special Failures
        public void AssemblySetUp(Assembly assembly, ReportSetUpAndTearDown setUp)
        {
            ReportAssembly a = this.AddAssembly(assembly);
            a.SetUp = setUp;
        }
        public void AssemblyTearDown(Assembly assembly, ReportSetUpAndTearDown tearDown)
        {
            ReportAssembly a = this.AddAssembly(assembly);
            a.TearDown = tearDown;
        }
        public void TestFixtureSetUp(Fixture fixture, ReportSetUpAndTearDown setUp)
        {
            ReportFixture reportFixture = this.AddFixture(fixture);
            reportFixture.SetUp = setUp;
        }
        public void TestFixtureTearDown(Fixture fixture, ReportSetUpAndTearDown tearDown)
        {
            ReportFixture reportFixture = this.AddFixture(fixture);
            reportFixture.TearDown = tearDown;
        }

        #endregion

		#region Report creation
        private ReportAssembly AddAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            if (this.assemblies.Contains(assembly.FullName))
                return (ReportAssembly)this.assemblies[assembly.FullName];

			ReportAssembly ra = new ReportAssembly();
			ra.FullName = assembly.FullName;
			AssemblyName an = assembly.GetName();
			ra.Name = an.Name;
            ra.Location = assembly.CodeBase;
            ra.Version = new ReportAssemblyVersion();
			ra.Version.Major = an.Version.Major;
			ra.Version.Minor = an.Version.Minor;
			ra.Version.Build = an.Version.Build;
			ra.Version.Revision = an.Version.Revision;

			// store
            this.assemblies.Add(assembly.FullName, ra);

            // add to report
			this.result.Assemblies.AddReportAssembly(ra);

			return ra;
		}

		private ReportNamespace AddNamespace(Type fixtureType)
		{
			// get assembly
			ReportAssembly ra = AddAssembly(fixtureType.Assembly);

			// get namespace
			string ns = fixtureType.Namespace;
			if (ns==null || ns.Length==0)
				ns="";
			
			string[] partialNs = partialNs=ns.Split('.');
			
			// find root
			string rootName = partialNs[0];
			ReportNamespace parent = null;
			foreach(ReportNamespace rns in ra.Namespaces)
			{
				if (rns.Name == rootName)
				{
					parent = rns;
					break;
				}
			}

			// if not found add new
			if (parent == null)
			{
				parent = new ReportNamespace();
				parent.Name = rootName;
				ra.Namespaces.AddReportNamespace(parent);
				this.namespaces.Add(parent.Name,parent);
			}

			// add childn amespaces
			string currentNs = parent.Name;
			for(int i = 1;i<partialNs.Length;++i)
			{
				currentNs+=String.Format(".{0}",partialNs[i]);
				// if !contained, add
				if (!this.namespaces.Contains(currentNs))
				{
					ReportNamespace rns = new ReportNamespace();
					rns.Name =currentNs;

					// add in table
					this.namespaces.Add(currentNs,rns);
					parent.Namespaces.AddReportNamespace(rns);
					parent = rns;
				}
				else
				{
					// add to parent
					parent = (ReportNamespace)this.namespaces[currentNs];
				}
			}

			return (ReportNamespace)this.namespaces[ns];
		}

		private ReportFixture AddFixture(Fixture fixture)
		{
            ReportFixture reportFixture = this.fixtures[fixture] as ReportFixture;
            if (reportFixture!=null)
                return reportFixture;

			reportFixture = new ReportFixture();
            reportFixture.Name = fixture.Name;
            reportFixture.Type = fixture.Type.FullName;

            // store in table
            this.fixtures[fixture] = reportFixture;

            // add to corresponding namespace
			ReportNamespace ns = AddNamespace(fixture.Type);
			ns.Fixtures.AddReportFixture(reportFixture);

			return reportFixture;
		}

		private ReportException CreateException(Exception ex)
		{
			ReportException rex = new ReportException();
			
			if (ex.GetType() == typeof(TargetInvocationException) && ex.InnerException!=null)
				ex = ex.InnerException;

			rex.Type = ex.GetType().ToString();
			rex.Message  = ex.Message;
			rex.Source = ex.Source;
			rex.StackTrace = ex.StackTrace;
			if (ex.InnerException!=null)
				rex.Exception = CreateException(ex.InnerException);

			return rex;
		}
		#endregion

		#region IRunPipeListener
		void IRunPipeListener.Start(RunPipe pipe)
		{
			// find ficture
			ReportFixture fixture = AddFixture(pipe.Fixture);
		}

        void IRunPipeListener.Success(RunPipe pipe, ReportRun result)
        {
			ReportFixture fixture = AddFixture(pipe.Fixture);
            fixture.Runs.AddReportRun(result);
        }

        void IRunPipeListener.Failure(RunPipe pipe, ReportRun result)
        {
			ReportFixture fixture = AddFixture(pipe.Fixture);
            fixture.Runs.AddReportRun(result);
        }

        void IRunPipeListener.Ignore(RunPipe pipe, ReportRun result)
        {
			ReportFixture fixture = AddFixture(pipe.Fixture);
            fixture.Runs.AddReportRun(result);
        }

        void IRunPipeListener.Skip(RunPipe pipe, ReportRun result)
        {
            ReportFixture fixture = AddFixture(pipe.Fixture);
            fixture.Runs.AddReportRun(result);
        }
		#endregion
	}
}
