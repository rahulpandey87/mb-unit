namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="hyperedge.type")]
    [XmlRoot(ElementName="hyperedge", IsNullable=false, DataType="")]
    public class HyperEdgeType {
        
        /// <summary />
        /// <remarks />
        private ItemCollection _items = new ItemCollection();
        
        /// <summary />
        /// <remarks />
        private string _desc;
        
        /// <summary />
        /// <remarks />
        private string id;
        
        /// <summary />
        /// <remarks />
        private GraphType _graph;
        
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
        [XmlElement(ElementName="endpoint", Type=typeof(EndPointType))]
        public virtual ItemCollection Items {
            get {
                return this._items;
            }
            set {
                this._items = value;
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
        
        public class ItemCollection : System.Collections.CollectionBase {
            
            /// <summary />
            /// <remarks />
            public ItemCollection() {
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
            public virtual void AddData(DataType _data) {
                this.List.Add(_data);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsData(DataType _data) {
                return this.List.Contains(_data);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveData(DataType _data) {
                this.List.Remove(_data);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void AddEndPoint(EndPointType _endpoint) {
                this.List.Add(_endpoint);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsEndPoint(EndPointType _endpoint) {
                return this.List.Contains(_endpoint);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveEndPoint(EndPointType _endpoint) {
                this.List.Remove(_endpoint);
            }
        }
    }
}
