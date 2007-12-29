
namespace MbUnit.Framework
{
	using System;
	using System.Xml;
	using MbUnit.Core.Runs;
	using System.Reflection;
	using System.IO;

	/// <summary>
	/// A file-based data provider
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
	public class XmlDataProviderAttribute : DataProviderFixtureDecoratorAttribute
	{
		private string xpath;
		private string resourceName;
		
		public XmlDataProviderAttribute(string resourceName, string xpath)
			:base()
		{
			this.ResourceName = resourceName;
			this.XPath = xpath;
		}
		
		public String XPath
		{
			get
			{
				return this.xpath;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.xpath = value;
			}
		}
		
		public String ResourceName
		{
			get
			{
				return this.resourceName;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.resourceName = value;
			}
		}

		public virtual StreamReader LoadResource()
		{
			return new StreamReader(this.resourceName);
		}
		
		public override XmlNodeList GetData()
		{
			// load document
			XmlDocument doc = new XmlDocument();
			using (StreamReader stream = this.LoadResource())
			{
				doc.Load(stream);
			}
			
			// return queried xpath
			return doc.SelectNodes(this.XPath);
		}
	}
}
