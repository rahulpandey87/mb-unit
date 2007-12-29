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
    [XmlType(IncludeInSchema=true, TypeName="report-assembly")]
    [XmlRoot(ElementName="report-assembly")]
    [Serializable]
    public sealed class ReportAssembly 
    {
        
        /// <summary />
        /// <remarks />
        private ReportAssemblyVersion _version;
        
        /// <summary />
        /// <remarks />
        private string _name;
        
        /// <summary />
        /// <remarks />
        private NamespaceCollection _nameSpaces = new NamespaceCollection();
        
        /// <summary />
        /// <remarks />
        private ReportCounter _counter;

        /// <summary />
        /// <remarks />
        private string _fullName;

        private string location;

        private ReportSetUpAndTearDown _setUp = null;
        private ReportSetUpAndTearDown _tearDown = null;

        public void UpdateCounts()
		{
			this.Counter =new ReportCounter();

			foreach(ReportNamespace ns in this.Namespaces)
			{
				ns.UpdateCounts();
				this.Counter.AddCounts(ns.Counter);
			}

            // check setup
            if (this.SetUp != null && this.SetUp.Result != ReportRunResult.Success)
            {
                this.Counter.FailureCount++;
                return;
            }
            // check tearDown
            if (this.TearDown != null && this.TearDown.Result != ReportRunResult.Success)
            {
                this.Counter.FailureCount++;
                return;
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
        [XmlElement(ElementName="version")]
        public ReportAssemblyVersion Version {
            get {
                return this._version;
            }
            set {
                this._version = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlArray(ElementName="namespaces")]
        [XmlArrayItem(ElementName="namespace", Type=typeof(ReportNamespace), IsNullable=false)]
        public NamespaceCollection Namespaces {
            get {
                return this._nameSpaces;
            }
            set {
                this._nameSpaces = value;
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

        [XmlAttribute(AttributeName ="location")]
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="full-name")]
        public string FullName {
            get {
                return this._fullName;
            }
            set {
                this._fullName = value;
            }
        }
        
		[Serializable]
        public sealed class NamespaceCollection : System.Collections.CollectionBase 
		{
            
            /// <summary />
            /// <remarks />
            public NamespaceCollection() {
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
            public void AddReportNamespace(ReportNamespace o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public bool ContainsReportNamespace(ReportNamespace o) {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public void RemoveReportNamespace(ReportNamespace o) {
                this.List.Remove(o);
            }
        }
    }
}
