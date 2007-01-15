
namespace MbUnit.Framework
{
	using System;
	using MbUnit.Core.Runs;
	using System.Reflection;
	using System.Xml;
	
	using MbUnit.Framework;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public abstract class DataProviderFixtureDecoratorAttribute : Attribute
	{
		protected DataProviderFixtureDecoratorAttribute()
		{}
		
		public abstract XmlNodeList GetData();
	}
}
