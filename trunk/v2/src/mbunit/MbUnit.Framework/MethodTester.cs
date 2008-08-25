using System;
using System.Collections;
using System.Reflection;

namespace MbUnit.Framework
{
	using MbUnit.Core;

    /// <summary>
    /// Used in strategies for testing classes and tests for null arguments.
    /// For more info, see <a href="http://blog.dotnetwiki.org/TestingTheArgumentNullExceptionWithMbUnit.aspx">this article</a>
    /// </summary>
    public class MethodTester
    {
        private TestSuite suite;
		private Object testedInstance;
        private MethodInfo method;
        private ParameterInfo[] parameterInfos;
        private Object[] parameters;
		private NullableAttributeAttribute nullableAttribute;


        public MethodTester(string name, Delegate test, params Object[] parameters)
            :this(name,test.Target, test.Method,parameters)
        {
		}

        public MethodTester(string name, Object testedInstance, MethodInfo method, params Object[] parameters)
        {
			this.suite=new TestSuite(name);
			this.testedInstance=testedInstance;
            this.method = method;
            this.parameterInfos=method.GetParameters();
            this.parameters = parameters;
			this.nullableAttribute = NullableAttributeAttribute.GetAttribute(method.DeclaringType);
		}

		public TestSuite Suite
		{
			get
			{
				return this.suite;
			}
			set
			{
				if(value==null)
					throw new ArgumentNullException("value");
				this.suite=value;
			}
		}

        public ExpectedExceptionTestCase AddThrow(int parameterIndex, Object value, Type exceptionType)
        {
            // copy parameters
            Object[] ps = new Object[this.parameters.Length];
            this.parameters.CopyTo(ps, 0);
            ps[parameterIndex] = value;

			string pname = this.parameterInfos[parameterIndex].Name;
			pname=Char.ToUpper(pname[0])+pname.Substring(1,pname.Length-1);

            // create new name
            string caseName=String.Format("{0}With{1}NulledShouldThrow{2}",
				this.method.Name,
                pname,
                exceptionType.Name
                );

			// create type

            // create case
            MethodTestCase tc = 
                MbUnit.Framework.TestCases.Case(caseName,testedInstance, method, ps);
            ExpectedExceptionTestCase etc = 
                MbUnit.Framework.TestCases.ExpectedException(tc, exceptionType);

            this.Suite.Add(etc);

            return etc;
        }

        public ExpectedExceptionTestCase AddThrowArgumentNull(int parameterIndex)
        {
            return this.AddThrow(parameterIndex,null,typeof(ArgumentNullException));
        }

        public void AddAllThrowArgumentNull()
        {
            int i=-1;
            foreach (ParameterInfo pi in this.parameterInfos)
            {
                ++i;
                if (pi.ParameterType.IsValueType)
                    continue;
				if (this.nullableAttribute!=null)
				{
					Object attribute = TypeHelper.TryGetFirstCustomAttribute(pi,this.nullableAttribute.NullableType);
					if (attribute!=null)
						continue;
				}
                this.AddThrowArgumentNull(i);
            }
        }
    }
}
