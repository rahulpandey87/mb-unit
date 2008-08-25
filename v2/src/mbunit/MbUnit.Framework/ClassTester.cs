using System;
using System.Reflection;

namespace MbUnit.Framework
{
	using MbUnit.Framework;

    /// <summary>
    /// Used in strategies for testing classes and tests for null arguments.
    /// For more info, see <a href="http://blog.dotnetwiki.org/TestingTheArgumentNullExceptionWithMbUnit.aspx">this article</a>
    /// </summary>
	public class ClassTester
	{
		private TestSuite suite;
		private Object testedInstance;

		public ClassTester(string name, Object testedInstance)
		{
			if (testedInstance==null)
				throw new ArgumentNullException("testedInstance");
			this.suite=new TestSuite(name);
			this.testedInstance=testedInstance;
		}

		public TestSuite Suite
		{
			get
			{
				return this.suite;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.suite=value;
			}
		}

		public Object TestedInstance
		{
			get
			{
				return this.testedInstance;
			}
		}

		public Type TestedType
		{
			get
			{
				return this.testedInstance.GetType();
			}
		}

		public void Add(string methodName, params Object[] parameters)
		{
			if (methodName==null)
				throw new ArgumentNullException("methodName");

			// first let's find the method
			MethodInfo mi = findMethod(methodName,parameters);

			// now let's create the test case
			MethodTester tester = new MethodTester(methodName,this.testedInstance,mi,parameters);
			tester.Suite=this.Suite;
			tester.AddAllThrowArgumentNull();
		}

		private MethodInfo findMethod(string methodName, params Object[] parameters)
		{
			Type[] types = new Type[parameters.Length];
			for(int i =0;i<parameters.Length;++i)
			{
				if (parameters[i]==null)
					throw new ArgumentNullException("parameters["+i.ToString()+"]");
				types[i]=parameters[i].GetType();
			}

			MethodInfo mi = this.TestedType.GetMethod(methodName,types);
			if (mi==null)
				throw new ArgumentException("Could not find suitable method");
			return mi;
		}
	}
}
