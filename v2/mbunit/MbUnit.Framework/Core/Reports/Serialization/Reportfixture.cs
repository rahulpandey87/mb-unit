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
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="report-fixture")]
    [XmlRoot(ElementName="report-fixture")]
    [Serializable]
    public sealed class ReportFixture
    {

        /// <summary />
        /// <remarks />
        private string _description = "";
        
        /// <summary />
        /// <remarks />
        private string _name;

        /// <summary />
        /// <remarks />
        private string _type;
        
        /// <summary />
        /// <remarks />
        private ReportCounter _counter;
        
        /// <summary />
        /// <remarks />
        private RunCollection _runs = new RunCollection();

        private ReportSetUpAndTearDown _setUp = null;
        private ReportSetUpAndTearDown _tearDown = null;

        public void UpdateCounts()
		{
			this.Counter = new ReportCounter();
            // check setup
            if (this.SetUp != null && this.SetUp.Result != ReportRunResult.Success)
            {
                this.Counter.FailureCount = this.Runs.Count;
                this.Counter.RunCount = this.Runs.Count;
                return;
            }
            // check tearDown
            if (this.TearDown != null && this.TearDown.Result != ReportRunResult.Success)
            {
                this.Counter.FailureCount = this.Runs.Count;
                this.Counter.RunCount = this.Runs.Count;
                return;
            }

            this.Counter.RunCount = this.Runs.Count;
			foreach(ReportRun run in this.Runs)
			{
                this.Counter.Duration += run.Duration;
                this.Counter.AssertCount += run.AssertCount;
                switch(run.Result)
				{
					case ReportRunResult.Success:
						++this.Counter.SuccessCount;
						break;
					case ReportRunResult.Ignore:
						++this.Counter.IgnoreCount;
						break;
					case ReportRunResult.Failure:
						++this.Counter.FailureCount;
						break;
                    case ReportRunResult.Skip:
                        ++this.Counter.SkipCount;
                        break;
                    default:
                        throw new NotSupportedException("Unknow result type: " + run.Result.ToString());
                }
			}
		}

        /// <summary />
        /// <remarks />
        [XmlElement(ElementName = "tear-down")]
        public ReportSetUpAndTearDown TearDown
        {
            get
            {
                return this._tearDown;
            }
            set
            {
                this._tearDown = value;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlElement(ElementName = "set-up")]
        public ReportSetUpAndTearDown SetUp
        {
            get
            {
                return this._setUp;
            }
            set
            {
                this._setUp = value;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="counter")]
        public ReportCounter Counter {
            get {
                return this._counter;
            }
            set {
                this._counter = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="description")]
        public string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
				if (this._description==null)
					this._description="";
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlArray(ElementName="runs")]
        [XmlArrayItem(ElementName="run", Type=typeof(ReportRun), IsNullable=false)]
        public RunCollection Runs {
            get {
                return this._runs;
            }
            set {
                this._runs = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="name")]
        public string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="type")]
        public string Type {
            get {
                return this._type;
            }
            set {
                this._type = value;
            }
        }
        
		[Serializable]
        public sealed class RunCollection : System.Collections.CollectionBase
        {
            
            /// <summary />
            /// <remarks />
            public RunCollection() {
            }
            
            /// <summary />
            /// <remarks />
            public object this[int index] {
                get {
                    return this.List[index];
                }
                set {
                    this.List[index] = value;
                }
            }
            
            /// <summary />
            /// <remarks />
            public void Add(object o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public void AddReportRun(ReportRun o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public bool ContainsReportRun(ReportRun o) {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public void RemoveReportRun(ReportRun o) {
                this.List.Remove(o);
            }
        }
    }
}
