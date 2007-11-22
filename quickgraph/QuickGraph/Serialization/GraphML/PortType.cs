namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="port.type")]
    [XmlRoot(ElementName="port", IsNullable=false, DataType="")]
    public class PortType {
        
        /// <summary />
        /// <remarks />
        private ItemCollection _items = new ItemCollection();
        
        /// <summary />
        /// <remarks />
        private string _name;
        
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
        [XmlElement(ElementName="port", Type=typeof(PortType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
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
        [XmlAttribute(AttributeName="name")]
        public virtual string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
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
    }
}
