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
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new RollBackRunInvoker(invoker);
		}

		private class RollBackRunInvoker : DecoratorRunInvoker
		{
			public RollBackRunInvoker(IRunInvoker invoker)
				:base(invoker)
			{}

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
				config.TrackingAppName = "Nunit Transaction Test Case";
				config.TrackingComponentName = this.Invoker.Name;
				config.Transaction = TransactionOption.Required;                     

				return config;
			}
			#endregion
		}
	}
}
