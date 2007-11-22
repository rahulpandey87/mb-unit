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
    public abstract class TestDomainBase : LongLivingMarshalByRefObject, IDisposable
    {
        private RemoteTestEngine testEngine = null;

        private Guid identifier = Guid.NewGuid();
        private ITypeFilter typeFilter = TypeFilters.Any;
        private IFixtureFilter filter = FixtureFilters.Any;
        private IRunPipeFilter runPipeFilter = new AnyRunPipeFilter();

        protected TestDomainBase()
        {}

        public virtual void Dispose()
        {
            this.Unload();
        }

        public abstract AppDomain Domain {get;}
        public abstract bool SeparateAppDomain {get;}

        protected virtual Type TestEngineType
        {
            get { return typeof(RemoteTestEngine); }
        }
        public virtual RemoteTestEngine TestEngine
        {
            get
            {
                return this.testEngine;
            }
        }
        protected virtual void SetTestEngine(RemoteTestEngine testEngine)
        {
            this.testEngine = testEngine;
        }

        /// <summary>
        /// Gets a <see cref="Guid"/> identifying the <see cref="TestDomain"/>
        /// </summary>
        public Guid Identifier
        {
            get
            {
                return this.identifier;
            }
        }

        #region Filters
        public ITypeFilter TypeFilter
        {
            get
            {
                return this.typeFilter;
            }
            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
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
                if (value == null)
					throw new ArgumentNullException("value");
                this.filter = value;
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

        protected virtual void SetFilters()
        {
            if (this.typeFilter != null)
                this.TestEngine.Explorer.TypeFilter = this.typeFilter;
            if (this.filter != null)
                this.TestEngine.Explorer.Filter = this.filter;
            if (this.RunPipeFilter != null)
                this.TestEngine.FixtureRunner.RunPipeFilter = this.RunPipeFilter;
        }
        #endregion

        #region Events
        public event EventHandler Loaded;

        /// <summary>
        /// Raises the <see cref="Loaded"/> event.
        /// </summary>
        protected virtual void OnLoaded()
        {
            if (this.Loaded != null)
                this.Loaded(this, EventArgs.Empty);
        }
        public event EventHandler ReLoaded;

        /// <summary>
        /// Raises the <see cref="ReLoaded"/> event.
        /// </summary>
        protected virtual void OnReLoaded()
        {
            if (this.ReLoaded != null)
                this.ReLoaded(this, EventArgs.Empty);
        }

        public event EventHandler UnLoaded;

        /// <summary>
        /// Raises the <see cref="UnLoaded"/> event.
        /// </summary>
        protected virtual void OnUnLoaded()
        {
            if (this.UnLoaded != null)
                this.UnLoaded(this, EventArgs.Empty);
        }
        #endregion

        protected abstract void CreateTestEngine();

        protected abstract void CreateDomain();

        protected virtual void SetUpTestEngine()
        {
            this.TestEngine.AddHintDirectory(Path.GetDirectoryName(typeof(QuickGraph.Concepts.IVertex).Assembly.Location));
            this.TestEngine.AddHintDirectory(Path.GetDirectoryName(typeof(QuickGraph.Algorithms.AlgoUtility).Assembly.Location));
            this.TestEngine.AddHintDirectory(Path.GetDirectoryName(typeof(TestDomain).Assembly.Location));
        }

        public virtual void InitializeEngine()
        {
            this.Unload();
            try
            {
                this.CreateDomain();
                this.CreateTestEngine();
                this.SetUpTestEngine();
                this.SetFilters();
            }
            catch (System.Threading.ThreadAbortException tae)
            {
                throw tae;
            }
            catch (Exception ex)
            {
                this.Unload();
                throw new Exception("Failed loading assembly", ex);
            }
        }

        public virtual void PopulateEngine()
        {
            try
            {
                this.TestEngine.Populate();
                this.OnLoaded();
            }
            catch (System.Threading.ThreadAbortException tae)
            {
                throw tae;
            }
            catch (Exception ex)
            {
                this.Unload();
                throw new Exception("Failed populating fixtures", ex);
            }
        }

        /// <summary>
        /// Loads domain and test assembly
        /// </summary>
        public virtual void Load()
        {
            try
            {
                this.InitializeEngine();
                this.PopulateEngine();
            }
            catch (System.Threading.ThreadAbortException tae)
            {
                throw tae;
            }
            catch (Exception ex)
            {
                this.Unload();
                throw new ApplicationException("Failed loading TestDomain", ex);
            }

        }

        /// <summary>
        /// Unload domain
        /// </summary>
        public abstract void Unload();

        /// <summary>
        /// Unload and reload test domain
        /// </summary>
        public virtual void Reload()
        {
            try
            {
                this.Unload();
                this.Load();
                this.OnReLoaded();
            }
            catch (System.Threading.ThreadAbortException tae)
            {
                throw tae;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed Reloading domain", ex);
            }
        }
    }
}
