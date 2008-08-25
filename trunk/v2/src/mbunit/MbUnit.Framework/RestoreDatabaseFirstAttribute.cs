using System;
using MbUnit.Framework;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags a test method to indicate that a database must be restored before the execution of the test
    /// </summary>
    /// <remarks>
    /// The details of the database to restore and how to restore are set by tagging the class containing the test
    /// with a <see cref="SqlRestoreInfoAttribute"/> or another tag derived from <see cref="DbRestoreInfoAttribute"/>
    /// </remarks>
    /// <example>
    /// <para>The following example shows the <see cref="RestoreDatabaseFirstAttribute"/> in use with a Sql Server database</para>
    /// <code>
    /// [TestFixture]
    /// [SqlRestoreInfo("MyConnString", "MyDatabase", @"d:\data\pubs.bak")]
    /// public class PubsTest {
    /// 
    ///    [Test, RestoreDatabaseFirst]
    ///    public void TestWithRestoreFirst()
    ///    {...}
    /// 
    ///    [Test, Rollback]
    ///    public void TestWithRollback()
    ///    {...}
    /// } 
    /// </code>
    /// </example>
    /// <seealso cref="SqlRestoreInfoAttribute"/>
    /// <seealso cref="RollBackAttribute"/>
    /// <seealso cref="DbRestoreInfoAttribute"/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RestoreDatabaseFirstAttribute : RestoreDatabaseAttribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreDatabaseFirstAttribute"/> class.
        /// </summary>
        public RestoreDatabaseFirstAttribute()
            : base(TestSchedule.BeforeTest) { }
    }
}