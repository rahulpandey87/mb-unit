namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="endpoint.type")]
    [XmlRoot(ElementName="endpoint", IsNullable=false, DataType="")]
    public class EndPointType {
        
        /// <summary />
        /// <remarks />
        private string _node;
        
        /// <summary />
        /// <remarks />
        private string id;
        
        /// <summary />
        /// <remarks />
        private string _port;
        
        /// <summary />
        /// <remarks />
        private string _desc;
        
        /// <summary />
        /// <remarks />
        private EndPointTypeType _type;
        
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
        [XmlAttribute(AttributeName="port")]
        public virtual string Port {
            get {
                return this._port;
            }
            set {
                this._port = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="node")]
        public virtual string Node {
            get {
                return this._node;
            }
            set {
                this._node = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="type")]
        public virtual EndPointTypeType Type {
            get {
                return this._type;
            }
            set {
                this._type = value;
            }
        }
    }
}
