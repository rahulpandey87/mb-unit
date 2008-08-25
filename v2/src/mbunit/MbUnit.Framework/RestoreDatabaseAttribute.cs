using System;
using MbUnit.Core.Framework;
using MbUnit.Core;
using MbUnit.Core.Runs;
using MbUnit.Core.Invokers;
using TestFu.Data;

namespace MbUnit.Framework
{
    /// <summary>
    /// Tags a method to indicate that a database must be restored according to a schedule 
    /// around the execution of the test
    /// </summary>
    /// <remarks>
    /// <para>This is an abstract class. The <see cref="RestoreDatabaseFirstAttribute"/> demonstrates how to
    /// implement it.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class RestoreDatabaseAttribute : DecoratorPatternAttribute
    {
        private TestSchedule schedule;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreDatabaseAttribute"/> class.
        /// </summary>
        /// <param name="schedule"><see cref="TestSchedule"/> value defining when the restore occurs with respect to the test execution</param>
		protected RestoreDatabaseAttribute(TestSchedule schedule)
        {
            this.schedule = schedule;
        }

        /// <summary>
        /// Gets the <see cref="TestSchedule"/> value defining when the restore occurs with respect to the test execution
        /// </summary>
        /// <value>The <see cref="TestSchedule"/> value defining when the restore occurs with respect to the test execution</value>
        public TestSchedule Schedule
        {
            get
            {
                return this.schedule;
            }
        }

        /// <summary>
        /// Returns the invoker class to run the test with the scheduled database restore.
        /// </summary>
        /// <param name="invoker">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="RollBackRunInvoker"/> object wrapping <paramref name="invoker"/></returns>
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new RollBackRunInvoker(this,invoker);
		}


		private class RollBackRunInvoker : DecoratorRunInvoker
		{
            private RestoreDatabaseAttribute parent;
            public RollBackRunInvoker(RestoreDatabaseAttribute parent, IRunInvoker invoker)
				:base(invoker)
			{
                this.parent = parent;
            }

            public override object Execute(object o, System.Collections.IList args)
            {
                // get info
                DbRestoreInfoAttribute info = DbRestoreInfoAttribute.GetInfo(o.GetType());
                // create factory
                IDbFactory factory = info.CreateFactory();
                // create admin
                DbAdministratorBase admin = factory.CreateAdmin(info.ConnectionString,info.DatabaseName);

                // restore before if before
                if (this.parent.Schedule == TestSchedule.BeforeTest)
                    admin.RestoreDatabase(info.BackupDevice, info.BackupDestination);

                // do the thing
                try
                {
                    return this.Invoker.Execute(o, args);
                }
                finally
                {
                    if (this.parent.Schedule == TestSchedule.AfterTest)
                        admin.RestoreDatabase(info.BackupDevice, info.BackupDestination);
                }
            }
        }
    }
}