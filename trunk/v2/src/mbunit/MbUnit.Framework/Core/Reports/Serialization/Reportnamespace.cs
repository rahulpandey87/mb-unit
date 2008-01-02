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

namespace MbUnit.Core.Reports.Serialization
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="report-namespace")]
    [XmlRoot(ElementName="report-namespace")]
    [Serializable]
    public sealed class ReportNamespace {
        
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
        private FixtureCollection _fixtures = new FixtureCollection();
        
		public void UpdateCounts()
		{
			this.Counter = new ReportCounter();
			foreach(ReportNamespace child in this.Namespaces)
			{
				child.UpdateCounts();
				this.Counter.AddCounts(child.Counter);
			}
			foreach(ReportFixture fixture in this.Fixtures)
			{
				fixture.UpdateCounts();
				this.Counter.AddCounts(fixture.Counter);
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
        [XmlArray(ElementName="fixtures")]
        [XmlArrayItem(ElementName="fixture", Type=typeof(ReportFixture), IsNullable=false)]
        public FixtureCollection Fixtures {
            get {
                return this._fixtures;
            }
            set {
                this._fixtures = value;
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
        
		[Serializable]
        public sealed class FixtureCollection : System.Collections.CollectionBase 
		{
            
            /// <summary />
            /// <remarks />
            public FixtureCollection() {
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
            public void AddReportFixture(ReportFixture o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public bool ContainsReportFixture(ReportFixture o) {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public void RemoveReportFixture(ReportFixture o) {
                this.List.Remove(o);
            }
        }
    }
}
