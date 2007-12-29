namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="node.type")]
    [XmlRoot(ElementName="node", IsNullable=false, DataType="")]
    public class NodeType {
        
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
        [XmlElement(ElementName="locator", Type=typeof(LocatorType))]
        [XmlElement(ElementName="graph", Type=typeof(GraphType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        [XmlElement(ElementName="port", Type=typeof(PortType))]
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
            public virtual void AddLocator(LocatorType _locator) {
                this.List.Add(_locator);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsLocator(LocatorType _locator) {
                return this.List.Contains(_locator);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveLocator(LocatorType _locator) {
                this.List.Remove(_locator);
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
            
            /// <summary />
            /// <remarks />
            public virtual void AddPort(PortType _port) {
                this.List.Add(_port);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsPort(PortType _port) {
                return this.List.Contains(_port);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemovePort(PortType _port) {
                this.List.Remove(_port);
            }
        }
    }
}
