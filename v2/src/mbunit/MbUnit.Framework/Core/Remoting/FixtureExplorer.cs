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
using System.Collections.Specialized;

using MbUnit.Core.Filters;
using MbUnit.Core.Collections;
using MbUnit.Core.FrameworkBridges;
using MbUnit.Framework;
using MbUnit.Core.Config;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Monitoring;
using MbUnit.Core.Graph;


namespace MbUnit.Core.Remoting
{

	[Serializable]
	public class FixtureExplorer : LongLivingMarshalByRefObject
	{
		private Assembly assembly;
        private ITypeFilter typeFilter = TypeFilters.Any;
        private IFixtureFilter filter = FixtureFilters.Any;
        [NonSerialized]
        private FixtureFactoryCollection fixtureFactories = new FixtureFactoryCollection();
        [NonSerialized]
        private MethodInfo assemblySetUp = null;
        [NonSerialized]
        private MethodInfo assemblyTearDown = null;
        [NonSerialized]
        private FixtureDependencyGraph fixtureGraph = new FixtureDependencyGraph();

        public FixtureExplorer(Assembly assembly)
		{
			if (assembly==null)
				throw new ArgumentNullException("assembly");
			this.assembly=assembly;

			this.FixtureFactories.Add( new AttributeFixtureFactory() );
            this.FixtureFactories.Add( new FrameworkFixtureFactory() );
            //this.FixtureFactories.Add( new NakedFixtureFactory() );
            //this.FixtureFactories.Add( new SSCLIFixtureFactory() );
        }

        public FixtureFactoryCollection FixtureFactories
		{
			get
			{
				return this.fixtureFactories;
			}
		}

        public ITypeFilter TypeFilter
        {
            get
            {
                return this.typeFilter;
            }
            set
            {
                this.typeFilter = value;
            }
        }

        public IFixtureFilter Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter=value;
			}
		}

        public string AssemblyName
        {
            get
            {
                return this.assembly.GetName().Name;
            }
        }

        public Assembly Assembly
        {
            get
            {
                return this.assembly;
            }
        }

        public StringCollection GetDependentAssemblies()
        {
            StringCollection dependencies = new StringCollection();
            foreach (AssemblyDependsOnAttribute attribute in this.assembly.GetCustomAttributes(
                typeof(AssemblyDependsOnAttribute), true))
            {
                dependencies.Add(attribute.AssemblyName);
            }
            return dependencies;
        }

        public FixtureDependencyGraph FixtureGraph
		{
			get
			{
				return this.fixtureGraph;
			}
		}

        public void Explore()
		{
            ExploreAssemblyCleanUpAndTearDown();
            foreach(Type type in this.TypeFilter.GetTypes(this.assembly))
			{
                ExploreType(type);
            }

            this.fixtureGraph.CreateDependencies();
        }

        private void ExploreAssemblyCleanUpAndTearDown()
        {
            // look if AssemblyCleanUpAttribute is present
            AssemblyCleanupAttribute assemblyCleanUp =
                    TypeHelper.TryGetFirstCustomAttribute(this.assembly,
                        typeof(AssemblyCleanupAttribute)) as AssemblyCleanupAttribute;
            if (assemblyCleanUp == null)
            {
                this.assemblySetUp = null;
                this.assemblyTearDown = null;
                return;
            }

            // try getting methods
            this.assemblySetUp =
                    TypeHelper.GetAttributedMethod(assemblyCleanUp.TargetType, typeof(SetUpAttribute), BindingFlags.Static | BindingFlags.Public);
            this.assemblyTearDown =
                    TypeHelper.GetAttributedMethod(assemblyCleanUp.TargetType, typeof(TearDownAttribute), BindingFlags.Static | BindingFlags.Public);
        }

        protected void ExploreType(Type type)
        {
            // it is a class
            // it is not an interface
            // it is not abstract
            if (!type.IsClass || type.IsInterface || type.IsAbstract)
                return;

            // check if filter ok
            if (!this.Filter.Filter(type))
                return;

            // look if any of the fixture attribute can return a run
            FixtureCollection fixs = new FixtureCollection();
            foreach (IFixtureFactory factory in this.FixtureFactories)
            {
                fixs.Clear();
                factory.Create(type, fixs);
                if (fixs.Count != 0)
                {
                    this.fixtureGraph.AddFixtureRange(fixs);
                    break;
                }
            }
        }

        public bool HasAssemblySetUp
        {
            get { return this.assemblySetUp != null; }
        }
        public bool HasAssemblyTearDown
        {
            get { return this.assemblyTearDown != null; }
        }

        public ReportSetUpAndTearDown AssemblySetUp()
        {
            if (this.assemblySetUp == null)
                throw new InvalidOperationException("AssemblySetUp is null");

            return ExecuteAndMonitor(this.assemblySetUp);
        }
        public ReportSetUpAndTearDown AssemblyTearDown()
        {
            if (this.assemblyTearDown == null)
                throw new InvalidOperationException("AssemblyTearDown is null");

            return ExecuteAndMonitor(this.assemblyTearDown);
        }
        private static ReportSetUpAndTearDown ExecuteAndMonitor(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            string name = String.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
            ReportMonitor monitor = new ReportMonitor();
            try
            {
                monitor.Start();
                method.Invoke(null, null);
                monitor.Stop();

                ReportSetUpAndTearDown setup = new ReportSetUpAndTearDown(name,monitor);
                return setup;
            }
            catch (Exception ex)
            {
                monitor.Stop();

                ReportSetUpAndTearDown setup = new ReportSetUpAndTearDown(name, monitor,ex);
                return setup;
            }
        }
    }
}
