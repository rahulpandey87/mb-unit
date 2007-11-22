#region Using directives

using System;
using System.Text;
using System.Reflection;
using System.Collections;

#endregion

namespace MbUnit.Framework
{
	using MbUnit.Core;
    using MbUnit.Framework;

    public class MethodTestCase : ITestCase
    {
        private string name;
        private string description=null;
        private Object testedInstance;
        private MethodInfo method;
        private Object[] parameters;

        public MethodTestCase(string name, Object testedInstance, MethodInfo method, Object[] parameters)
        {
            this.name = name;
			this.testedInstance=testedInstance;
            this.method = method;
            this.parameters = parameters;
        }

        #region ITestCase Members
        public virtual string Name
        {
            get 
            {
                return this.name;
            }
        }
        public virtual String Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public MethodInfo Method
        {
            get
            {
                return this.method;
            }
        }

        public Object[] GetParameters()
        {
            return this.parameters;
        }

        public virtual Object Invoke(Object o, IList args)
        {
            return this.method.Invoke(testedInstance,parameters);
        }
        #endregion
    }
}
