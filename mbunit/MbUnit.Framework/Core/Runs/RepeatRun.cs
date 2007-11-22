using System;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Globalization;

using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Core.Runs
{
    internal sealed class RepeatRun : Run
    {
        public RepeatRun()
            : base("RepeatRun", true)
        { }

        public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
        {
            foreach (MethodInfo method in TypeHelper.GetAttributedMethods(t, typeof(RepeatTestAttribute)))
            {
                try
                {
                    foreach (RepeatTestAttribute rep in method.GetCustomAttributes(typeof(RepeatTestAttribute), true))
                    {
                        for (int i = 0; i < rep.Count; i++)
                        {
                            // Get invoker
                            IRunInvoker invoker = new RepeatMethodRunInvoker(this, method, i + 1);

                            // Decorate invoker
                            invoker = DecoratorPatternAttribute.DecoreInvoker(method, invoker);

                            // Add to tree
                            tree.AddChild(parent, invoker);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MethodFailedLoadingRunInvoker invoker = new MethodFailedLoadingRunInvoker(this, ex, method);
                    tree.AddChild(parent, invoker);
                }
            }
        }

        public class RepeatMethodRunInvoker : MethodRunInvoker
        {
            private int count;

            public RepeatMethodRunInvoker(IRun generator, MethodInfo method, int count)
                : base(generator, method)
            {
                this.count = count;
            }

            public override String Name
            {
                get
                {
                    return this.Method.Name + "(" + count + ")";
                }
            }
        }

    }
}
