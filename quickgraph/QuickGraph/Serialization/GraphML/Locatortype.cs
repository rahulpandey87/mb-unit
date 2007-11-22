namespace QuickGraph.Serialization.GraphML {
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="locator.type")]
    [XmlRoot(ElementName="locator", IsNullable=false, DataType="")]
    public class LocatorType {
    }
}
