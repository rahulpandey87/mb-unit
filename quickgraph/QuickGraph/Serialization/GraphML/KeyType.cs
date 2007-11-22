namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="key.type")]
    [XmlRoot(ElementName="key", IsNullable=false, DataType="")]
    public class KeyType {
        
        /// <summary />
        /// <remarks />
        private DefaultType _default;
        
        /// <summary />
        /// <remarks />
        private string id;
        
        /// <summary />
        /// <remarks />
        private string _desc;
        
        /// <summary />
        /// <remarks />
        private KeyForType _for;
        
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
        [XmlElement(ElementName="default")]
        public virtual DefaultType Default {
            get {
                return this._default;
            }
            set {
                this._default = value;
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
        [XmlAttribute(AttributeName="for")]
        public virtual KeyForType For {
            get {
                return this._for;
            }
            set {
                this._for = value;
            }
        }
    }
}
