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

namespace MbUnit.Core.Reports.Serialization
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using MbUnit.Core.Monitoring;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="report-setup-teardown")]
    [XmlRoot(ElementName="report-setup-teardown")]
    [Serializable]
    public sealed class ReportSetUpAndTearDown
    {
        /// <summary />
        /// <remarks />
        private ReportRunResult _result;

        /// <summary />
        /// <remarks />
        private long _memory;

        /// <summary />
        /// <remarks />
        private string _consoleError = "";

        /// <summary />
        /// <remarks />
        private string _name;

        /// <summary />
        /// <remarks />
        private string _consoleOut = "";

        /// <summary />
        /// <remarks />
        private System.Double _duration;

        /// <summary />
        /// <remarks />
        private ReportException _exception;

        public ReportSetUpAndTearDown()
        { }

        public ReportSetUpAndTearDown(string name, ReportMonitor monitor)
        {
            this.Name = name;
            this.ConsoleOut = monitor.Consoler.Out;
            this.ConsoleError = monitor.Consoler.Error;
            this.Duration = monitor.Timer.Duration;
            this.Memory = monitor.Memorizer.Usage;
            this.Result = ReportRunResult.Success;
        }

        public ReportSetUpAndTearDown(string name, ReportMonitor monitor, Exception ex)
            :this(name,monitor)
        {
            this.Result = ReportRunResult.Failure;
            this.Exception = ReportException.FromException(ex);
        }

        /// <summary />
        /// <remarks />
        [XmlElement(ElementName = "console-out")]
        public string ConsoleOut
        {
            get
            {
                return this._consoleOut;
            }
            set
            {
                this._consoleOut = value;
                if (this._consoleOut == null)
                    this._consoleOut = "";
            }
        }

        /// <summary />
        /// <remarks />
        [XmlElement(ElementName = "console-error")]
        public string ConsoleError
        {
            get
            {
                return this._consoleError;
            }
            set
            {
                this._consoleError = value;
                if (this._consoleError == null)
                    this._consoleError = "";
            }
        }

        /// <summary />
        /// <remarks />
        [XmlElement(ElementName = "exception")]
        public ReportException Exception
        {
            get
            {
                return this._exception;
            }
            set
            {
                this._exception = value;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName = "name")]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName = "result")]
        public ReportRunResult Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName = "duration")]
        public System.Double Duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName = "memory")]
        public long Memory
        {
            get
            {
                return this._memory;
            }
            set
            {
                this._memory = value;
            }
        }
    }
}
