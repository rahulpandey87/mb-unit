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
    [XmlType(IncludeInSchema=true, TypeName="report-result")]
    [XmlRoot(ElementName="report-result", IsNullable=false, DataType="")]
    [Serializable]
    public sealed class ReportResult
    {

        /// <summary />
        /// <remarks />
        private System.DateTime _date = DateTime.Now;
        
        /// <summary />
        /// <remarks />
        private ReportCounter _counter;
        
        /// <summary />
        /// <remarks />
        private Assembliecollection _assemblies = new Assembliecollection();
        
		#region Count update
		public void UpdateCounts()
		{
			// go down the tree counting...
			this.Counter = new ReportCounter();

			foreach(ReportAssembly a in this.Assemblies)
			{
				a.UpdateCounts();
				this.Counter.AddCounts(a.Counter);
			}
		}
		#endregion

		public void Merge(ReportResult result)
		{
			Merge(result,true);
		}

		public void Merge(ReportResult result, bool updateCounts)
		{
			foreach(ReportAssembly assembly in result.Assemblies)
			{
				this.Assemblies.AddReportAssembly(assembly);
			}
			if (updateCounts)
				this.UpdateCounts();
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
        [XmlArray(ElementName="assemblies")]
        [XmlArrayItem(ElementName="assembly", Type=typeof(ReportAssembly), IsNullable=false)]
        public Assembliecollection Assemblies {
            get {
                return this._assemblies;
            }
            set {
                this._assemblies = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="date")]
        public System.DateTime Date {
            get {
                return this._date;
            }
            set {
                this._date = value;
            }
        }
        
		[Serializable]
        public sealed class Assembliecollection : System.Collections.CollectionBase
        {
            
            /// <summary />
            /// <remarks />
            public Assembliecollection() {
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
            public void AddReportAssembly(ReportAssembly o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public bool ContainsReportAssembly(ReportAssembly o) {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public void RemoveReportAssembly(ReportAssembly o) {
                this.List.Remove(o);
            }
        }
    }
}
