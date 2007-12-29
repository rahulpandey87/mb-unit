namespace MbUnit.Tests.Core.Framework
{
    using System;
	
	using MbUnit.Core.Framework;
	using MbUnit.Framework;
	using MbUnit.Core.Exceptions;
	
	/// <summary>
	/// Test fixture of the <see cref="MultipleCultureAttribute"/>.
	/// </summary>
	/// <remark>
	/// </remarks>
	[TestFixture]
	public class MultipleCultureAttributeTest
	{
		[Test]
		[ExpectedException(typeof(AssertionException))]
		[MultipleCulture("de-DE")]
		public  void TestConvertFromString()
		{
			string input="2.2";
			double output = double.Parse(input);
			Assert.AreEqual(2.2, output);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		[MultipleCulture("de-DE")]
		public  void TestConvertToString()
		{
			Console.WriteLine("CurrentCulture {0} ",System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			Console.WriteLine("CurrentUICulture {0} ",System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
			double input = 2.2;
			string output= String.Format("{0}",input);
			Assert.AreEqual("2.2",output); 
		}
	}
}

