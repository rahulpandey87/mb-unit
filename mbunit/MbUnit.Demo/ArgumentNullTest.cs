using System;

using MbUnit.Framework;

[assembly: NullableAttribute(typeof(MbUnit.Demo.MyNullableAttribute))]

namespace MbUnit.Demo 
{

	[AttributeUsage(AttributeTargets.Parameter,AllowMultiple=false,Inherited=true)]
	public class MyNullableAttribute : Attribute
	{}

	public class ArgumentNullDummyClass
	{
		public object ClassicMethod(Object nullable, Object notNullable, int valueType)
		{
			if (notNullable == null)
				throw new ArgumentNullException("notNullable");
			Console.Write("test: {0}{1}{2}",nullable,notNullable,valueType);
			return String.Format("{0}{1}{2}",nullable,notNullable,valueType);
		}

	
		public object SmartMethod([MyNullable] Object nullable, Object notNullable, int valueType)
		{
			return ClassicMethod(nullable,notNullable,valueType);
		}	
	}

	[TestSuiteFixture]
	public class MethodTestSuiteDemo
	{
		public delegate object MultiArgumentDelegate(Object o,Object b, int i);

		[TestSuite]
		public ITestSuite AutomaticClassicMethodSuite()
		{
			ArgumentNullDummyClass dummy = new ArgumentNullDummyClass();
			MethodTester suite = new MethodTester(
				"ClassicMethod",
				new MultiArgumentDelegate(dummy.ClassicMethod),
				"hello",
				"world",
				1
				);
			suite.AddAllThrowArgumentNull();

			return suite.Suite;
		}

		[TestSuite]
		public ITestSuite AutomaticSmartMethodSuite()
		{
			ArgumentNullDummyClass dummy = new ArgumentNullDummyClass();
			MethodTester suite = new MethodTester(
				"SmartMethod",
				new MultiArgumentDelegate(dummy.SmartMethod),
				"hello",
				"world",
				1
				);
			suite.AddAllThrowArgumentNull();

			return suite.Suite;
		}
	}

	[TestSuiteFixture]
	public class ClassTesterDemo
	{
		[TestSuite]
		public ITestSuite AutomaticClassSuite()
		{
			ArgumentNullDummyClass dummy = new ArgumentNullDummyClass();
			ClassTester suite = new ClassTester("DummyClassTest",dummy);
			suite.Add("ClassicMethod","hello","world",1);
			suite.Add("SmartMethod","hello","world",1);

			return suite.Suite;
		}
	}
}
