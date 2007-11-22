using System;
using System.Xml;
using System.IO;

namespace QuickGraphTest
{
	/// <summary>
	/// Summary description for GraphMLUpdater.
	/// </summary>
	public class GraphMLUpdater
	{
		public static void Update(string path, string searchPattern, string xsdPath)
		{
			// <graphml xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://graphml.graphdrawing.org/xmlns/1.0rc">
			foreach(string f in Directory.GetFiles(path,searchPattern))
			{
				try
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(f);

					XmlElement graphml = null;
					XmlNode docType = null;
					foreach(XmlNode node in doc.ChildNodes)
					{
						if (node is XmlDocumentType)
							docType=node;
						
						if (node is XmlElement)
						{
							graphml = (XmlElement)node;
							break;
						}
					}
					if (docType!=null)
						doc.RemoveChild(docType);

					XmlAttribute  attr = (XmlAttribute)graphml.Attributes.GetNamedItem("xmlns");
					if (attr==null)
					{
						attr=doc.CreateAttribute("xmlns");
						attr.Value = xsdPath;
						graphml.Attributes.Append(attr);
					}

					attr = (XmlAttribute)graphml.Attributes.GetNamedItem("xmlns:xsd");
					if (attr==null)
					{
						attr=doc.CreateAttribute("xmlns:xsd");
						attr.Value = "http://www.w3.org/2001/XMLSchema";
						graphml.Attributes.Append(attr);
					}

					attr = (XmlAttribute)graphml.Attributes.GetNamedItem("xmlns:xsi");
					if (attr==null)
					{
						attr=doc.CreateAttribute("xmlns:xsi");
						attr.Value = "http://www.w3.org/2001/XMLSchema-instance";
						graphml.Attributes.Append(attr);
					}

					using (StreamWriter sw = new StreamWriter(f))
					{
						XmlTextWriter writer = new XmlTextWriter(sw);
						writer.Formatting = Formatting.Indented;
						doc.Save(writer);
					}
				}
				catch(Exception)
				{
					Console.WriteLine(f+" process failed");
				}
			}
		}
	}
}
