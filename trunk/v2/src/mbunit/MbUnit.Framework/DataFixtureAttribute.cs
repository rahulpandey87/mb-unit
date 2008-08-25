using System;
using MbUnit.Core;
using MbUnit.Core.Framework;
using System.Diagnostics;

namespace MbUnit.Framework {
    using MbUnit.Core.Runs;

    /// <summary>
    /// The DataFixture tags a test class that will be using provider classes to push values to the tests it contains.
    /// </summary>
    /// <example>
    /// <para>In this example a test class uses an XML file as its source of data. The file looks like this</para>
    /// <code>
    /// &lt;DataFixture&gt;
    ///   &lt;Employees&gt;
    ///     &lt;User Name="Mickey" LastName="Mouse" /&gt;
    ///   &lt;/Employees&gt;
    ///   &lt;Customers&gt;
    ///     &lt;User Name="Jonathan" LastName="de Halleux" /&gt;
    ///     &lt;User Name="Voldo" LastName="Unkown" /&gt;
    ///   &lt;/Customers&gt;
    /// &lt;/DataFixture&gt;
    /// </code>
    /// <para>The class which maps the data in the XML file to a .NET class looks like this</para>
    /// <code>
    ///    [XmlRoot("User")]
    ///    public class User {
    /// 
    ///        private string name;
    ///        private string lastName;
    /// 
    ///        public User() { }
    ///        [XmlAttribute("Name")]
    ///        public String Name {
    ///            get { return this.name; }
    ///            set { this.name = value; }
    ///        }
    /// 
    ///        [XmlAttribute("LastName")]
    ///        public String LastName {
    ///            get { return this.lastName; }
    ///            set { this.lastName = value; }
    ///        }
    ///    }
    /// </code>
    /// <para>The test class adorned with the DataFixture attribute looks like this</para>
    /// <code>
    ///    [DataFixture]
    ///    [XmlDataProvider("sample.xml", "//User")]
    ///    [XmlDataProvider("sample.xml", "DataFixture/Customers")]
    ///    public class DataDrivenTests {
    /// 
    ///        [ForEachTest("User")]
    ///        public void ForEachTest(XmlNode node) {
    ///            Assert.IsNotNull(node);
    ///            Assert.AreEqual("User", node.Name);
    ///            Console.WriteLine(node.OuterXml);
    ///        }
    ///
    ///        [ForEachTest("User", DataType = typeof(User))]
    ///        public void ForEachTestWithSerialization(User user) {
    ///            Assert.IsNotNull(user);
    ///            Console.WriteLine(user.ToString());
    ///        }
    ///    }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DataFixtureAttribute : TestFixturePatternAttribute {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// </remarks>
        public DataFixtureAttribute()
            : base() { }

        /// <summary>
        /// Constructor with a fixture description
        /// </summary>
        /// <param name="description">fixture description</param>
        public DataFixtureAttribute(string description)
            : base(description) { }

        /// <summary>
        /// Creates the execution logic
        /// </summary>
        /// <remarks>
        /// See summary.
        /// </remarks>
        /// <returns>A <see cref="IRun"/> instance that represent the type
        /// test logic.
        /// </returns>
        public override IRun GetRun() {
            SequenceRun runs = new SequenceRun();

            // setup
            OptionalMethodRun setup = new OptionalMethodRun(typeof(SetUpAttribute), false);
            runs.Runs.Add(setup);

            // the tests
            DataFixtureRun tests = new DataFixtureRun();
            runs.Runs.Add(tests);

            // tear down
            OptionalMethodRun tearDown = new OptionalMethodRun(typeof(TearDownAttribute), false);
            runs.Runs.Add(tearDown);

            return runs;
        }


    }
}
