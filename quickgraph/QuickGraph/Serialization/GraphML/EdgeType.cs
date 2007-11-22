namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="edge.type")]
    [XmlRoot(ElementName="edge", IsNullable=false, DataType="")]
    public class EdgeType {
        
        /// <summary />
        /// <remarks />
        private bool _directed;
        
        /// <summary />
        /// <remarks />
        private DataCollection _data = new DataCollection();
        
        /// <summary />
        /// <remarks />
        private string _source;
        
        /// <summary />
        /// <remarks />
        private string id;
        
        /// <summary />
        /// <remarks />
        private string _sourceport;
        
        /// <summary />
        /// <remarks />
        private string _targetport;
        
        /// <summary />
        /// <remarks />
        private bool _directedspecified;
        
        /// <summary />
        /// <remarks />
        private string _target;
        
        /// <summary />
        /// <remarks />
        private GraphType _graph;
        
        /// <summary />
        /// <remarks />
        private string _desc;
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="desc")]
        public virtual string Desc {
            get {
                return this._desc;
            }
            set {
                this._desc = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        public virtual DataCollection Data {
            get {
                return this._data;
            }
            set {
                this._data = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="graph")]
        public virtual GraphType Graph {
            get {
                return this._graph;
            }
            set {
                this._graph = value;
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
        [XmlAttribute(AttributeName="directed")]
        public virtual bool Directed {
            get {
                return this._directed;
            }
            set {
                this._directed = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="directedSpecified")]
        public virtual bool Directedspecified {
            get {
                return this._directedspecified;
            }
            set {
                this._directedspecified = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="source")]
        public virtual string Source {
            get {
                return this._source;
            }
            set {
                this._source = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="target")]
        public virtual string Target {
            get {
                return this._target;
            }
            set {
                this._target = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="sourceport")]
        public virtual string Sourceport {
            get {
                return this._sourceport;
            }
            set {
                this._sourceport = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="targetport")]
        public virtual string Targetport {
            get {
                return this._targetport;
            }
            set {
                this._targetport = value;
            }
        }
        
        public class DataCollection : System.Collections.CollectionBase {
            
            /// <summary />
            /// <remarks />
            public DataCollection() {
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
            public virtual void AddDataType(DataType o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsDataType(DataType o) {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveDataType(DataType o) {
                this.List.Remove(o);
            }
        }
    }
}
