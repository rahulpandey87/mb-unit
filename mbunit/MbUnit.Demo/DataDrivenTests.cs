using System;
using System.Xml;
using System.Xml.Serialization;

namespace MbUnit.Demo
{

	using MbUnit.Framework;

	[XmlRoot("User")]
	public class User
	{
		private string name;
		private string lastName;
		public User()
		{}

		[XmlAttribute("Name")]
		public String Name
		{
				get{ return this.name;}
				set{ this.name = value;}
		}

		[XmlAttribute("LastName")]
		public String LastName
		{
			get{ return this.lastName;}
			set{ this.lastName = value;}
		}

		public override string ToString()
		{
			return String.Format("{0}.{1}",this.name,this.lastName);
		}

	}

	/// <summary>
	/// Summary description for DataDrivenTests.
	/// </summary>
	[DataFixture]
	[ResourceXmlDataProvider(typeof(DataDrivenTests),"MbUnit.Demo.sample.xml","//User")]
	[ResourceXmlDataProvider(typeof(DataDrivenTests),"MbUnit.Demo.sample.xml","DataFixture/Customers/User")]
	public class DataDrivenTests
	{
		[ForEachTest("//User")]
		public void ForEachTest(XmlNode node)
		{
			Assert.IsNotNull(node);
			Assert.AreEqual("User",node.Name);
			Console.WriteLine(node.OuterXml);
		}
		[ForEachTest("//User",DataType = typeof(User))]
		public void ForEachTestWithSerialization(User user)
		{
			Assert.IsNotNull(user);
			Console.WriteLine(user.ToString());
		}
	}
}
