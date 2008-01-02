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
	using System.Reflection;
    using System.IO;
    using System.Collections;
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="report-exception")]
    [XmlRoot(ElementName="report-exception")]
    [Serializable]
    public sealed class ReportException {
        
        /// <summary />
        /// <remarks />
        private string _stackTrace=null;
        
        /// <summary />
        /// <remarks />
        private string _message=null;
        
        /// <summary />
        /// <remarks />
        private string _source=null;
        
        /// <summary />
        /// <remarks />
        private ReportException _exception=null;
        
        /// <summary />
        /// <remarks />
        private string _type;

        private PropertyCollection properties = new PropertyCollection();

        [XmlArray("properties")]
        [XmlArrayItem("property",Type = typeof(ReportProperty), IsNullable =false)]
        public PropertyCollection Properties
        {
            get
            {
                return this.properties;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="message")]
        public string Message {
            get {
                return this._message;
            }
            set {
                this._message = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="source")]
        public string Source {
            get {
                return this._source;
            }
            set {
                this._source = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="stack-trace")]
        public string StackTrace {
            get {
                return this._stackTrace;
            }
            set {
                this._stackTrace = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="exception")]
        public ReportException Exception {
            get {
                return this._exception;
            }
            set {
                this._exception = value;
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

		public override string ToString()
		{
			StringWriter sw =new StringWriter();
			sw.WriteLine("{0}",this.Type);
			sw.WriteLine("Message: {0}",this.Message);
			sw.WriteLine("Source: {0}",this.Source);
			sw.WriteLine("StackTrace:");
			sw.WriteLine(this.StackTrace);
			if (this.Exception!=null)
			{
				sw.WriteLine("Inner Exception");
				sw.WriteLine(this.Exception.ToString());
			}
			return sw.ToString();
		}

        
		public static ReportException FromException(Exception ex)
		{
			if (ex==null)
				throw new ArgumentNullException("ex");
			
			ReportException rex = new ReportException();
			
			if (ex.GetType() == typeof(TargetInvocationException) && ex.InnerException!=null)
				ex = ex.InnerException;

			rex.Type = ex.GetType().ToString();
			rex.Message  = String.Format("{0}",ex.Message);
			rex.Source = String.Format("{0}",ex.Source);
			rex.StackTrace = ex.StackTrace;
			if (ex.InnerException!=null)
				rex.Exception = FromException(ex.InnerException);
            foreach (PropertyInfo property in ex.GetType().GetProperties())
            {
                if (property.Name == "Message"
                    || property.Name == "Source"
                    || property.Name == "StackTrace"
                    || property.Name == "InnerException"
                    || property.Name == "Data")
                    continue;

                ReportProperty p = new ReportProperty(ex, property);
                rex.Properties.AddReportProperty(p);
            }
            
            /*
            if (ex.Data!=null)
            {
                foreach (DictionaryEntry de in ex.Data)
                {
                    ReportProperty p = new ReportProperty(de);
                    rex.Properties.Add(p);
                }
            }
            */

            return rex;
		}        

        [Serializable]
        public sealed class PropertyCollection : CollectionBase
        {
            /// <summary />
            /// <remarks />
            public PropertyCollection()
            {
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
            public void AddReportProperty(ReportProperty o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public bool ContainsReportProperty(ReportProperty o)
            {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public void RemoveReportProperty(ReportProperty o)
            {
                this.List.Remove(o);
            }
        }
    }
}
