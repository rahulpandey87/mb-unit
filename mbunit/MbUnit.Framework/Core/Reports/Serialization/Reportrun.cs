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
    [XmlType(IncludeInSchema=true, TypeName="report-run")]
    [XmlRoot(ElementName="reportrun")]
    [Serializable]
    public sealed class ReportRun
    {        
        private long _memory;
        private string _consoleError = "";
        private string _name;
        private string _description = "";
        private string _consoleOut = "";
        private int assertCount = 0;
        private System.Double _duration;
        private ReportException _exception;
        private ReportRunResult _result;
        private InvokerCollection _invokers = new InvokerCollection();
        private WarningCollection _warnings = new WarningCollection();
        private AssertCollection _asserts = new AssertCollection();

        public static ReportRun Success(RunPipe pipe, ReportMonitor monitor)
        {
            ReportRun run = new ReportRun();
            run.ConsoleOut = monitor.Consoler.Out;
            run.ConsoleError = monitor.Consoler.Error;
            run.Result = ReportRunResult.Success;
            run.Name = pipe.Name;
            run.AssertCount = MbUnit.Framework.Assert.AssertCount;
            run.Duration = monitor.Timer.Duration;
            run.Memory = monitor.Memorizer.Usage;

            MbUnit.Framework.Assert.FlushWarnings(run);

            return run;
        }

        public static ReportRun Failure(RunPipe pipe, ReportMonitor monitor, Exception ex)
        {
            ReportRun run = new ReportRun();
            run.ConsoleOut = monitor.Consoler.Out;
            run.ConsoleError = monitor.Consoler.Error;
            run.Result = ReportRunResult.Failure;
            run.Name = pipe.Name;
            run.AssertCount = MbUnit.Framework.Assert.AssertCount;
            run.Duration = monitor.Timer.Duration;
            run.Memory = monitor.Memorizer.Usage;
            run.Exception = ReportException.FromException(ex);

            MbUnit.Framework.Assert.FlushWarnings(run);

            return run;
        }

        public static ReportRun Ignore(RunPipe pipe, ReportMonitor monitor)
        {
            ReportRun run = new ReportRun();
            run.ConsoleOut = monitor.Consoler.Out;
            run.ConsoleError = monitor.Consoler.Error;
            run.Result = ReportRunResult.Ignore;
            run.Name = pipe.Name;
            run.AssertCount = MbUnit.Framework.Assert.AssertCount;
            run.Duration = monitor.Timer.Duration;
            run.Memory = monitor.Memorizer.Usage;

            MbUnit.Framework.Assert.FlushWarnings(run);

            return run;
        }

        public static ReportRun Skip(RunPipe pipe, Exception ex)
        {
            ReportRun run = new ReportRun();
            run.Result = ReportRunResult.Skip;
            run.Name = pipe.Name;
            run.Duration = 0;
            run.Memory = 0;
            run.AssertCount = MbUnit.Framework.Assert.AssertCount;
            run.Exception = ReportException.FromException(ex);

            MbUnit.Framework.Assert.FlushWarnings(run);

            return run;
        }

        /// <summary />
        /// <remarks />
        [XmlArray(ElementName="invokers")]
        [XmlArrayItem(ElementName="invoker", Type=typeof(ReportInvoker), IsNullable=false)]
        public InvokerCollection Invokers 
        {
            get 
            {
                return this._invokers;
            }
            set 
            {
                this._invokers = value;
            }
        }

        [XmlArray(ElementName = "warnings")]
        [XmlArrayItem(ElementName = "warning", Type = typeof(ReportWarning), IsNullable = false)]
        public WarningCollection Warnings
        {
            get
            {
                return this._warnings;
            }
            set
            {
                this._warnings = value;
            }
        }

        [XmlArray(ElementName = "asserts")]
        [XmlArrayItem(ElementName = "assert", Type = typeof(ReportAssert), IsNullable = false)]
        public AssertCollection Asserts
        {
            get
            {
                return this._asserts;
            }
            set
            {
                this._asserts = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="")]
        public string Description 
        {
            get 
            {
                return this._description;
            }
            set 
            {
                this._description = value;
				if (this._description==null)
					this._description="";
			}
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="console-out")]
        public string ConsoleOut 
        {
            get 
            {
                return this._consoleOut;
            }
            set 
            {
                this._consoleOut = value;
				if (this._consoleOut==null)
					this._consoleOut="";
			}
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="console-error")]
        public string ConsoleError 
        {
            get 
            {
                return this._consoleError;
            }
            set 
            {
                this._consoleError = value;
				if (this._consoleError==null)
					this._consoleError="";
			}
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="exception")]
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
        [XmlAttribute(AttributeName="name")]
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
        [XmlAttribute(AttributeName="result")]
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

        [XmlAttribute(AttributeName = "assert-count")]
        public int AssertCount
        {
            get { return this.assertCount; }
            set { this.assertCount = value; }
        }

        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="duration")]
        public double Duration 
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
        [XmlAttribute(AttributeName="memory")]
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
        
		[Serializable]
        public sealed class InvokerCollection : System.Collections.CollectionBase 
        {
            
            /// <summary />
            /// <remarks />
            public InvokerCollection() 
            {}
            
            public object this[int index] 
            {
                get 
                {
                    return this.List[index];
                }
                set 
                {
                    this.List[index] = value;
                }
            }
            
            /// <summary />
            /// <remarks />
            public void Add(object o) 
            {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public void AddReportInvoker(ReportInvoker o) 
            {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public bool ContainsReportInvoker(ReportInvoker o) 
            {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public void RemoveReportInvoker(ReportInvoker o) 
            {
                this.List.Remove(o);
            }
        }

        [Serializable]
        public sealed class WarningCollection : System.Collections.CollectionBase
        {
            public WarningCollection()
            { }
            public object this[int index]
            {
                get
                {
                    return this.List[index];
                }
                set
                {
                    this.List[index] = value;
                }
            }
            public void Add(object o)
            {
                this.List.Add(o);
            }
            public void AddReportWarning(ReportWarning o)
            {
                this.List.Add(o);
            }
            public bool ContainsReportWarning(ReportWarning o)
            {
                return this.List.Contains(o);
            }
            public void RemoveReportWarning(ReportWarning o)
            {
                this.List.Remove(o);
            }
        }

        [Serializable]
        public sealed class AssertCollection : System.Collections.CollectionBase
        {
            public AssertCollection()
            { }
            public object this[int index]
            {
                get
                {
                    return this.List[index];
                }
                set
                {
                    this.List[index] = value;
                }
            }

            public void Add(object o)
            {
                this.List.Add(o);
            }

            public void AddReportAssert(ReportAssert o)
            {
                this.List.Add(o);
            }

            public bool ContainsReportAssert(ReportAssert o)
            {
                return this.List.Contains(o);
            }
            public void RemoveReportAssert(ReportAssert o)
            {
                this.List.Remove(o);
            }
        }
    }
}
