namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="data.type")]
    [XmlRoot(ElementName="data", IsNullable=false, DataType="")]
    public class DataType {
        
        /// <summary />
        /// <remarks />
        private Textcollection _text = new Textcollection();
        
        /// <summary />
        /// <remarks />
        private string _key;
        
        /// <summary />
        /// <remarks />
        private string id;
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="key")]
        public virtual string Key {
            get {
                return this._key;
            }
            set {
                this._key = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="id")]
        public virtual string ID {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlText(Type=typeof(string))]
        public virtual Textcollection Text {
            get {
                return this._text;
            }
            set {
                this._text = value;
            }
        }
        
        public class Textcollection : System.Collections.CollectionBase {
            
            /// <summary />
            /// <remarks />
            public Textcollection() {
            }
            
            /// <summary />
            /// <remarks />
            public virtual object this[int index] {
                get {
                    return this.List[index];
                }
                set {
                    this.List[index] = value;
                }
            }
            
            /// <summary />
            /// <remarks />
            public virtual void Add(object o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void AddString(string o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsString(string o) {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveString(string o) {
                this.List.Remove(o);
            }
            
            /// <summary />
            /// <remarks />
            public override string ToString() {
                System.IO.StringWriter sw = new System.IO.StringWriter();
                // <foreach>
                // This loop mimics a foreach call. See C# programming language, pg 247
                // Here, the enumerator is seale and does not implement IDisposable
                System.Collections.IEnumerator enumerator = this.List.GetEnumerator();
                for (
                ; enumerator.MoveNext(); 
                ) {
                    string s = ((string)(enumerator.Current));
                    // foreach body
                    sw.Write(s);
                }
                // </foreach>
                return sw.ToString();
            }
        }
    }
}
