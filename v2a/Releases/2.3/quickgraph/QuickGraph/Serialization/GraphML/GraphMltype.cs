namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="graphml.type")]
    [XmlRoot(ElementName="graphml", IsNullable=false, DataType="")]
    public class GraphMltype {
        
        /// <summary />
        /// <remarks />
        private KeyCollection _key = new KeyCollection();
        
        /// <summary />
        /// <remarks />
        private string _desc;
        
        /// <summary />
        /// <remarks />
        private ItemCollection _items = new ItemCollection();
        
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
        [XmlElement(ElementName="key", Type=typeof(KeyType))]
        public virtual KeyCollection Key {
            get {
                return this._key;
            }
            set {
                this._key = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlElement(ElementName="graph", Type=typeof(GraphType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        public virtual ItemCollection Items {
            get {
                return this._items;
            }
            set {
                this._items = value;
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
            public virtual void AddGraph(GraphType _graph) {
                this.List.Add(_graph);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsGraph(GraphType _graph) {
                return this.List.Contains(_graph);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveGraph(GraphType _graph) {
                this.List.Remove(_graph);
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
        }
        
        public class KeyCollection : System.Collections.CollectionBase {
            
            /// <summary />
            /// <remarks />
            public KeyCollection() {
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
            public virtual void AddKeyType(KeyType o) {
                this.List.Add(o);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsKeyType(KeyType o) {
                return this.List.Contains(o);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveKeyType(KeyType o) {
                this.List.Remove(o);
            }
        }
    }
}
