using System;
using MbUnit.Core.Framework;
using MbUnit.Core;
using MbUnit.Core.Runs;
using MbUnit.Core.Invokers;
using TestFu.Data;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class RestoreDatabaseAttribute : DecoratorPatternAttribute
    {
        private TestSchedule schedule;

		protected RestoreDatabaseAttribute(TestSchedule schedule)
        {
            this.schedule = schedule;
        }

        public TestSchedule Schedule
        {
            get
            {
                return this.schedule;
            }
        }

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