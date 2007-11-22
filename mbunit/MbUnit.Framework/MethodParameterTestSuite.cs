using System;
using System.Collections;
using System.Reflection;

namespace MbUnit.Framework
{
    public class MethodTestSuite : ITestSuite
    {
        private string name;
        private MethodInfo method;
        private ParameterInfo[] parameterInfos;
        private Object[] parameters;
        private IDictionary testCases = new Hashtable();

        public MethodTestSuite(string name, Delegate test, params Object[] parameters)
            :this(name,test.Method,parameters)
        {}

        public MethodTestSuite(string name, MethodInfo method, params Object[] parameters)
        {
            this.name = name;
            this.method = method;
            this.parameterInfos=method.GetParameters();
            this.parameters = parameters;
        }

        public ExpectedExceptionTestCase AddThrow(int parameterIndex, Object value, Type exceptionType)
        {
            // copy parameters
            Object[] ps = new Object[this.parameters.Length];
            this.parameters.CopyTo(ps, 0);
            ps[parameterIndex] = value;

            // create new name
            string caseName=String.Format("{0}{1}ModifiedShouldThrow{2}",
                this.name,
                this.parameterInfos[parameterIndex].Name,
                exceptionType.Name
                );

            // create case
            MethodTestCase tc = 
                MbUnit.Framework.TestCases.Case(caseName, method, ps);
            ExpectedExceptionTestCase etc = 
                MbUnit.Framework.TestCases.ExpectedException(tc, exceptionType);

            this.testCases.Add(etc.Name, etc);

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
                this.AddThrowArgumentNull(i);
            }
        }


        #region ITestSuite Members
        public string Name
        {
            get
            { 
                return this.name; 
            }
        }
        public ICollection TestCases
        {
            get 
            {
                return this.testCases.Values;
            }
        }
        #endregion
    }
}
