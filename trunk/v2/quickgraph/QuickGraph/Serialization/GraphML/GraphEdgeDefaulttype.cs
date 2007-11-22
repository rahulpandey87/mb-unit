namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    public enum GraphEdgeDefaultType {
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="directed")]
        Directed,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="undirected")]
        Undirected,
    }
}
