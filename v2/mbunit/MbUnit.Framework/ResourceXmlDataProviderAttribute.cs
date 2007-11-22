
namespace MbUnit.Framework
{
	using System;
	using System.Xml;
	using MbUnit.Core.Runs;
	using System.Reflection;
	using System.IO;

	/// <summary>
	/// A resource-based data provider
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    public sealed class ResourceXmlDataProviderAttribute : XmlDataProviderAttribute
    {
		private Type type;
		
		public ResourceXmlDataProviderAttribute(Type t, string resourceName, string xpath)
			:base(resourceName,xpath)
		{
			this.type=t;
		}

		public override StreamReader LoadResource()
		{
			return new StreamReader(this.type.Assembly.GetManifestResourceStream(this.ResourceName));
		}
	}
}
