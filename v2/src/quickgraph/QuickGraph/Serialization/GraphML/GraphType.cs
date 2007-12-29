namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="graph.type")]
    [XmlRoot(ElementName="graph", IsNullable=false, DataType="")]
    public class GraphType {
        
        /// <summary />
        /// <remarks />
        private ItemCollection _items = new ItemCollection();
        
        /// <summary />
        /// <remarks />
        private GraphEdgeDefaultType _edgeDefault;
        
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
        [XmlElement(ElementName="edge", Type=typeof(EdgeType))]
        [XmlElement(ElementName="node", Type=typeof(NodeType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        [XmlElement(ElementName="hyperedge", Type=typeof(HyperEdgeType))]
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
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="edgedefault")]
        public virtual GraphEdgeDefaultType EdgeDefault {
            get {
                return this._edgeDefault;
            }
            set {
                this._edgeDefault = value;
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
            public virtual void AddEdge(EdgeType _edge) {
                this.List.Add(_edge);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsEdge(EdgeType _edge) {
                return this.List.Contains(_edge);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveEdge(EdgeType _edge) {
                this.List.Remove(_edge);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void AddNode(NodeType _node) {
                this.List.Add(_node);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsNode(NodeType _node) {
                return this.List.Contains(_node);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveNode(NodeType _node) {
                this.List.Remove(_node);
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
            public virtual void AddHyperEdge(HyperEdgeType _hyperedge) {
                this.List.Add(_hyperedge);
            }
            
            /// <summary />
            /// <remarks />
            public virtual bool ContainsHyperEdge(HyperEdgeType _hyperedge) {
                return this.List.Contains(_hyperedge);
            }
            
            /// <summary />
            /// <remarks />
            public virtual void RemoveHyperEdge(HyperEdgeType _hyperedge) {
                this.List.Remove(_hyperedge);
            }
        }
    }
}
