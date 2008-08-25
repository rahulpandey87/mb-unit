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

    /// <summary>
    /// Represents a test case for a method on an object. It tests a method given a set of parameters if applicable.
    /// Can be added to a <see cref="TestSuite"/>.
    /// </summary>
    public class MethodTestCase : ITestCase
    {
        private string name;
        private string description=null;
        private Object testedInstance;
        private MethodInfo method;
        private Object[] parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodTestCase"/> class.
        /// </summary>
        /// <param name="name">The name of the test.</param>
        /// <param name="testedInstance">The instance of the object on which to call the <paramref name="method"/>.</param>
        /// <param name="method">The <see cref="MethodInfo"/> object representing the method.</param>
        /// <param name="parameters">The parameters for the method.</param>
        public MethodTestCase(string name, Object testedInstance, MethodInfo method, Object[] parameters)
        {
            this.name = name;
			this.testedInstance=testedInstance;
            this.method = method;
            this.parameters = parameters;
        }

        #region ITestCase Members
        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get 
            {
                return this.name;
            }
        }
        /// <summary>
        /// Gets or sets the description of the test.
        /// </summary>
        /// <value>The description.</value>
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

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object representing the method being tested.
        /// </summary>
        /// <value>The method.</value>
        public MethodInfo Method
        {
            get
            {
                return this.method;
            }
        }

        /// <summary>
        /// Gets the parameters for the method being tested.
        /// </summary>
        /// <returns>the parameters for the method being tested</returns>
        public Object[] GetParameters()
        {
            return this.parameters;
        }

        /// <summary>
        /// Invokes the method on the specified object <paramref name="o"/>.
        /// </summary>
        /// <param name="o">The object to invoke the method on</param>
        /// <param name="args">The arguments to send to the method.</param>
        /// <returns></returns>
        public virtual Object Invoke(Object o, IList args)
        {
            return this.method.Invoke(testedInstance,parameters);
        }
        #endregion
    }
}
