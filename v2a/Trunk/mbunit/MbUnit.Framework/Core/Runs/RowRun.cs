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
    internal sealed class RowRun : Run
    {
        public RowRun()
            :base("RowRun",true)
        {}

        public override void Reflect(
            RunInvokerTree tree, 
            RunInvokerVertex parent, 
            Type t)
        {
            foreach (MethodInfo method in TypeHelper.GetAttributedMethods(t, typeof(RowTestAttribute)))
            {
                try
                {
                    foreach (RowAttribute row in method.GetCustomAttributes(typeof(RowAttribute), true))
                    {
                        // get invoker
                        IRunInvoker invoker = new RowMethodRunInvoker(
                        this,
                            method,
                            row
                            );
                        if (row.ExpectedException != null)
                        {
                            invoker = new ExpectedExceptionRunInvoker(
                                invoker, row.ExpectedException, row.Description
                                    );
                        }
                        // decore invoker
                        invoker = DecoratorPatternAttribute.DecoreInvoker(method, invoker);

                        tree.AddChild(parent, invoker);
                    }
                }
                catch (Exception ex)
                {
                    MethodFailedLoadingRunInvoker invoker = new MethodFailedLoadingRunInvoker(this, ex, method);
                    tree.AddChild(parent, invoker);
                }
            }
        }

        public class RowMethodRunInvoker : RunInvoker
        {
            private MethodInfo method;
            private RowAttribute row;
            public RowMethodRunInvoker(IRun generator, MethodInfo method, RowAttribute row)
                :base(generator)
            {
                this.method = method;
                this.row = row;
            }

            public override String Name
            {
                get 
                {
                    StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
                    sw.Write("{0}(", this.method.Name);
                    Object[] data = this.row.GetRow();
                    for(int i =0;i<data.Length;++i)
                    {
                        if (i != 0)
                            sw.Write(",{0}", data[i]);
                        else
                            sw.Write("{0}", data[i]);
                    }
                    sw.Write(")");
                    return sw.ToString();
                }
            }

            public override bool ContainsMemberInfo(MemberInfo memberInfo)
            {
                return this.method == memberInfo;
            }

            public override Object Execute(Object o, IList args)
            {
                return this.method.Invoke(o, this.row.GetRow());
            }
        }
    }
}
