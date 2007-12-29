using System;
using System.Data;
using System.Data.OracleClient;

namespace TestFu.Data.OracleClient
{
    /// <summary>
    /// Abstract  base class for MSOracle server database testing.
    /// </summary>
    /// <include 
    ///		file='TestFu/Data/TestFu.Data.Doc.xml' 
    ///		path='//example[contains(descendant-or-self::*,"OracleFixture")]'
    ///		/>
    public abstract class OracleFixture : DbFixture
    {
        /// <summary>
        /// Initializes a <see cref="DbFixture"/> with a connection string.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string for accessing the test database.
        /// </param>
        /// <param name="database">
        /// database name
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionString"/> is a null reference
        /// (Nothing in Visual Basic)
        /// </exception>
        public OracleFixture(string connectionString, string database)
			:base(connectionString,database,new OracleFactory())
        { }

        public override string DatabaseConnectionString
        {
            get
            {
                return String.Format(
                    "{0};Initial Catalog={1}",
                    this.ConnectionString,
                    this.DatabaseName
                    );
            }
        }



        /// <summary>
        /// Gets the current connection instance.
        /// </summary>
        /// <value>
        /// <see cref="OracleConnection"/> instance.
        /// </value>
        public new OracleConnection Connection
        {
            get
            {
                return (OracleConnection)base.Connection;
            }
        }

        /// <summary>
        /// Gets the current transaction.
        /// </summary>
        /// <value>
        /// A <see cref="OracleTransaction" instance if <see cref="BeginTransaction"/> was called
        /// and the connection not closed; otherwise, a null reference (Nothing in Visual Basic)
        /// </value>
        public new OracleTransaction Transaction
        {
            get
            {
                return (OracleTransaction)base.Transaction;
            }
        }
    }
}
