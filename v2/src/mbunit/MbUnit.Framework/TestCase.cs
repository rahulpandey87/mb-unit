using System;
using System.Text;
using System.Collections;
using System.Reflection;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using MbUnit.Core.Runs;

namespace MbUnit.Framework
{
    /// <summary>
    /// A single test case of a <see cref="TestSuite"/>.
    /// </summary>
    public class TestCase : ITestCase
    {
        private string name;
        private Delegate testDelegate;
        private Object[] parameters;
        private IRunInvoker invoker;
        private string description=null;

        /// <summary>
        /// Initializes a new <see cref="TestCase"/> instance
        /// with name and delegate.
        /// </summary>
        /// <param name="name">
        /// Name of the test case
        /// </param>
        /// <param name="testDelegate">
        /// Delegate called by the test case
        /// </param>
        /// <param name="parameters">
        /// Parameters of the delegate
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> or <paramref name="testDelegate"/>
        /// is a null reference (Nothing in Visual Basic)
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> is empty.
        /// </exception>
        public TestCase(string name, Delegate testDelegate, params Object[] parameters)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("name is empty");
            if (testDelegate == null)
                throw new ArgumentNullException("testDelegate");
            this.name = name;
            this.testDelegate = testDelegate;
            this.parameters = parameters;

            DelegateRunInvoker delegateInvoker = new DelegateRunInvoker(new TestCaseRun(), testDelegate, parameters);
            this.invoker = DecoratorPatternAttribute.DecoreInvoker(testDelegate.Method, delegateInvoker);
        }

        public string Description
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

        public Delegate TestDelegate
        {
            get { return this.testDelegate; }
        }

        public Object[] GetParameters()
        {
            return this.parameters;
        }

        /// <summary>
        /// Gets the name of the test case
        /// </summary>
        /// <value>
        /// The name of the test case
        /// </value>
        public virtual String Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Invokes test using the parameters returned by <see cref="GetParameters"/>.
        /// </summary>
        /// <returns></returns>
        public virtual object Invoke(Object o, IList args)
        {
            return this.invoker.Execute(o, this.parameters);
        }

        private sealed class TestCaseRun : Run
        {
            public TestCaseRun()
                :base("TestCase",true)
            { }
            public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
            {
                throw new NotSupportedException();
            }
        }
    }
}
