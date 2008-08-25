using System;
using System.EnterpriseServices;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
    /// <summary>
    /// Tags a test method whose database operation must be executed within a transaction and rolled back when it has 
    /// finished executing
    /// </summary>
    /// <remarks>
    /// <para>The attribute uses EnterpriseServices to roll back the transactions done in the test case</para>
    /// </remarks>
    /// <example>
    /// <para>The following example shows the <see cref="RollBackAttribute"/> in use </para>
    /// <code>
    /// [TestFixture]
    /// public class RollbackTest {
    /// 
    ///    [Test, Rollback]
    ///    public void TestWithRollback()
    ///    {...}
    /// } 
    /// </code>
    /// </example>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]	
	public sealed class RollBackAttribute : DecoratorPatternAttribute
	{

        public int TimeOutValue = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RollBackAttribute"/> class.
        /// </summary>
        public RollBackAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RollBackAttribute"/> class.
        /// </summary>
        /// <param name="timeOutValue">The transaction timeout value in seconds</param>
        public RollBackAttribute(int timeOutValue)
        {
            TimeOutValue = timeOutValue;
        }

        /// <summary>
        /// Returns the invoker class to run the test within a transaction and roll it back once complete.
        /// </summary>
        /// <param name="invoker">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="RollBackRunInvoker"/> object wrapping <paramref name="invoker"/></returns>
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new RollBackRunInvoker(invoker, TimeOutValue);
		}

		private class RollBackRunInvoker : DecoratorRunInvoker
		{
            public int TimeOutValue = 0;

            public RollBackRunInvoker(IRunInvoker invoker, int timeOutValue) : base(invoker)
            {
                TimeOutValue = timeOutValue;
            }

			public override object Execute(object o, System.Collections.IList args)
			{
				EnterServicedDomain();

				try
				{
					Object result = this.Invoker.Execute(o,args);
					return result;
				}
				finally
				{
					AbortRunningTransaction();
					LeaveServicedDomain();
				}
			}

			#region Service stuff
			private void AbortRunningTransaction()
			{
				if(ContextUtil.IsInTransaction)
				{
					ContextUtil.SetAbort();	
				}
			}

			private void LeaveServicedDomain()
			{
			
				ServiceDomain.Leave();
			}

			private void EnterServicedDomain()
			{
				ServiceConfig config = CreateServicedConfig();
				ServiceDomain.Enter(config);
			}

			private ServiceConfig CreateServicedConfig()
			{
				ServiceConfig config = new ServiceConfig();
				config.TrackingEnabled = true;
				config.TrackingAppName = "MbUnit Transaction Test Case";
				config.TrackingComponentName = this.Invoker.Name;
				config.Transaction = TransactionOption.Required;

                if (TimeOutValue != 0)
                {
                    config.TransactionTimeout = TimeOutValue;
                }

				return config;
			}
			#endregion
		}
	}
}
