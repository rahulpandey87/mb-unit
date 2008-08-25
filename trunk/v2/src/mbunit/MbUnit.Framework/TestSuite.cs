using System;
using System.Collections;
using MbUnit.Framework;
using MbUnit.Core;

namespace MbUnit.Framework
{
    /// <summary>
    /// A named collection of uniquely named <see cref="TestCase"/>.
    /// </summary>
	public class TestSuite : ITestSuite
	{
		private ArrayList testCases = new ArrayList();
		private string name;

        /// <summary>
        /// Initializes a <see cref="TestSuite"/> instance
        /// with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">name of the suite</param>
        /// <exception cref="ArgumentNullException">
        /// 	<paramref name="name"/> is a null reference
        /// (Nothing in Visual Basic)
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 	<paramref name="name"/> is empty.
        /// </exception>
        public TestSuite(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("name is empty");
            this.name = name;
        }

        /// <summary>
        /// Gets the <see cref="TestSuite"/> name.        /// </summary>
        /// <value>
        /// The <see cref="TestSuite"/> name.
        /// </value>
        public virtual String Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="TestCase"/>.
        /// </summary>
        /// <value>
        /// A collection of <see cref="TestCase"/>.
        /// </value>
        public ICollection TestCases
        {
            get
            {
                return this.testCases;
            }
        }

        #region Add/Remove Test cases
        /// <summary>
        /// Adds the test case to the suite
        /// </summary>
        /// <param name="testCase"><see cref="TestCase"/> instance to add.</param>
        /// <exception cref="InvalidOperationException">
        /// The suite already contains a test case with the same name as <paramref name="testCase"/>.
        /// </exception>
        public void Add(ITestCase testCase)
        {
            if (testCase == null)
                throw new ArgumentNullException("testCase");
            if (this.testCases.Contains(testCase.Name))
                throw new InvalidOperationException("TestCase " + testCase.Name + " already in collection");
            this.testCases.Add(testCase);
        }

        /// <summary>
        /// Removes the test case from the suite
        /// </summary>
        /// <param name="testCase">Test case to remove</param>
        /// <exception cref="ArgumentNullException">
        /// 	<paramref name="testCase"/> is a null reference
        /// (Nothing in Visual Basic)
        /// </exception>
        public void Remove(TestCase testCase)
        {
            if (testCase == null)
                throw new ArgumentNullException("testCase");
            this.testCases.Remove(testCase);
        }
        #endregion

        #region Specialized Add
        /// <summary>
        /// Adds a new <see cref="TestCase"/> to the suite.
        /// </summary>
        /// <param name="name">Name of the new test case</param>
        /// <param name="test"><see cref="Delegate"/> invoked by the test case</param>
        /// <param name="parameters">parameters sent to <paramref name="test"/> when invoked</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// 	<paramref name="name"/> is a null reference
        /// (Nothing in Visual Basic)
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 	<paramref name="name"/> is empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The suite already contains a test case named <paramref name="name"/>.
        /// </exception>
        public TestCase Add(string name, Delegate test, params object[] parameters)
        {
            TestCase tc = MbUnit.Framework.TestCases.Case(name, test, parameters);
            this.Add(tc);
            return tc;
        }
        #endregion
    }
}
