using System;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Core.Framework;
using MbUnit.Core.Runs;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class SystemTestAttribute : TestFixturePatternAttribute
    {
		public override IRun GetRun()
		{
			SequenceRun runs = new SequenceRun();
			
			// setup
			OptionalMethodRun setup = new OptionalMethodRun(typeof(SetUpAttribute),false);
			runs.Runs.Add( setup );
			
			// tests
			StepMethodRun test = new StepMethodRun();
			runs.Runs.Add(test);
			
			// tear down
			OptionalMethodRun tearDown = new OptionalMethodRun(typeof(TearDownAttribute),false);
			runs.Runs.Add(tearDown);
			
			return runs;						
		}	

		private class StepMethodRun : Run
		{
			protected IRunInvoker InstanceStepInvoker(MethodInfo mi)
			{
				IRunInvoker invoker = new MethodRunInvoker(this,mi);						
				return DecoratorPatternAttribute.DecoreInvoker(mi,invoker);
			}

			protected IRunInvoker InstanceCriticalStepInvoker(MethodInfo mi)
			{
				IRunInvoker invoker = new MethodRunInvoker(this,mi);						
				return DecoratorPatternAttribute.DecoreInvoker(mi,invoker);
			}

			public override void Reflect(
				RunInvokerTree tree, 
				RunInvokerVertex parent, 
				Type t
				)
			{
					// populate execution tree.
					RunInvokerVertex child = parent;
					foreach(MethodInfo mi in 
						TypeHelper.GetAttributedMethods(t,typeof(StepAttribute)))
					{
//						IRunInvoker invoker = InstanceInvoker(om.Method);
//						child = tree.AddChild(child,invoker);
					}				
			}
		}
	}
}
