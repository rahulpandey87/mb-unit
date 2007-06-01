namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    public enum KeyForType {
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="all")]
        All,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="graph")]
        Graph,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="node")]
        Node,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="edge")]
        Edge,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="hyperedge")]
        HyperEdge,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="port")]
        Port,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="endpoint")]
        EndPoint,
    }
}
