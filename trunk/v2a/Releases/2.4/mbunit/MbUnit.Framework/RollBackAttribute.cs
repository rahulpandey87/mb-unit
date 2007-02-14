using System;
using System.EnterpriseServices;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
    /// <summary>
	/// Tags methods to execute database operation in its own database
	/// transaction.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute was invented by <b>Roy Osherove</b> (
	/// http://weblogs.asp.net/rosherove/).
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]	
	public sealed class RollBackAttribute : DecoratorPatternAttribute
	{

        public int TimeOutValue = 0;

        public void RollBackAttribute()
        {

        }

        public void RollBackAttribute(int timeOutValue)
        {
            TimeOutValue = timeOutValue;
        }

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
