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

namespace MbUnit.Core.Reports.Serialization
{
    [Serializable]
	public abstract class ReportVisitor
	{
		public virtual void VisitResult(ReportResult result)
		{
			foreach(ReportAssembly assembly in result.Assemblies)
				VisitAssembly(assembly);
		}

		public virtual void VisitAssembly(ReportAssembly assembly)
		{
			VisitAssemblyVersion(assembly.Version);
            if (assembly.SetUp != null)
                VisitAssemblySetUp(assembly.SetUp);
            if (assembly.TearDown != null)
                VisitAssemblyTearDown(assembly.TearDown);

            foreach(ReportNamespace ns in assembly.Namespaces)
				VisitNamespace(ns);
		}

		public virtual void VisitAssemblyVersion(ReportAssemblyVersion version)
		{ }

        public virtual void VisitAssemblySetUp(ReportSetUpAndTearDown assemblySetUp)
        { }

        public virtual void VisitAssemblyTearDown(ReportSetUpAndTearDown assemblyTearDown)
        { }

        public virtual void VisitFixtureSetUp(ReportSetUpAndTearDown fixtureSetUp)
        { }

        public virtual void VisitFixtureTearDown(ReportSetUpAndTearDown fixtureTearDown)
        { }

        public virtual void VisitNamespace(ReportNamespace ns)
		{
			foreach(ReportFixture fixture in ns.Fixtures)
				VisitFixture(fixture);

			foreach(ReportNamespace childNs in ns.Namespaces)
				VisitNamespace(childNs);
		}

		public virtual void VisitFixture(ReportFixture fixture)
		{
            if (fixture.SetUp != null)
                VisitFixtureSetUp(fixture.SetUp);
            if (fixture.TearDown != null)
                VisitFixtureTearDown(fixture.TearDown);
            foreach (ReportRun run in fixture.Runs)
                this.VisitRun(run);
        }

		public virtual void VisitRun(ReportRun run)
		{
			foreach(ReportInvoker invoker in run.Invokers)
				this.VisitInvoker(invoker);

			if (run.Exception!=null)
				this.VisitException(run.Exception);
		}

		public virtual void VisitInvoker(ReportInvoker invoker)
		{}

		public virtual void VisitException(ReportException exception)
		{
			if (exception.Exception!=null)
				VisitException(exception.Exception);
		}
	}
}
