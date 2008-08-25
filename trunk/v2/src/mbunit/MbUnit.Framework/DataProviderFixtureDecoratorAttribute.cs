
namespace MbUnit.Framework
{
	using System;
	using MbUnit.Core.Runs;
	using System.Reflection;
	using System.Xml;
	
	using MbUnit.Framework;

    /// <summary>
    /// Abstract class to derive (XML) DataProvider attribute classes from.
    /// </summary>
    /// <remarks>
    /// The <see cref="XmlDataProviderAttribute"/> is a subclass of this.
    /// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public abstract class DataProviderFixtureDecoratorAttribute : Attribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderFixtureDecoratorAttribute"/> class.
        /// </summary>
		protected DataProviderFixtureDecoratorAttribute()
		{}

        /// <summary>
        /// Gets the <see cref="XmlNodeList"/> to be used by the class tagged by this attribute.
        /// </summary>
        /// <returns>The <see cref="XmlNodeList"/> to be used by the class tagged by this attribute</returns>
		public abstract XmlNodeList GetData();
	}
}
