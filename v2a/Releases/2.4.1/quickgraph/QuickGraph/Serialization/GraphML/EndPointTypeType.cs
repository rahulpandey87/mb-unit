namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    public enum EndPointTypeType {
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="in")]
        IN,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="out")]
        Out,
        
        /// <summary />
        /// <remarks />
        [XmlEnum(Name="undir")]
        Undir,
    }
}
