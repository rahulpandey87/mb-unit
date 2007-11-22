using System;
using System.Reflection;
using System.Collections;
using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Core.Runs;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using TestFu.Operations;
using System.IO;

namespace MbUnit.Core.Runs
{
    internal sealed class CombinatorialRun : Run
    {
        public CombinatorialRun()
                :base("Combinatorial",true)
        {
        }

        public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
        {
            object fixture = null;
            try
            {
                // Check if fixture is ignored or explicit
                IgnoreAttribute ignore = null;
                if (TypeHelper.HasCustomAttribute(t, typeof(IgnoreAttribute)))
                {
                    ignore = TypeHelper.GetFirstCustomAttribute(t, typeof(IgnoreAttribute)) as IgnoreAttribute;
                }
                ExplicitAttribute expl = null;
                if (TypeHelper.HasCustomAttribute(t, typeof(ExplicitAttribute)))
                {
                    expl = TypeHelper.GetFirstCustomAttribute(t, typeof(ExplicitAttribute)) as ExplicitAttribute;
                }

                foreach (MethodInfo method in TypeHelper.GetAttributedMethods(t, typeof(CombinatorialTestAttribute)))
                {
                    if (fixture == null)
                        fixture = TypeHelper.CreateInstance(t);

                    this.ReflectTestMethod(tree, parent, fixture, method, ignore, expl);
                }
            }
            finally
            {
                IDisposable disposable = fixture as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }

        private void ReflectTestMethod(
            RunInvokerTree tree, 
            RunInvokerVertex parent, 
            object fixture, 
            MethodInfo method,
            IgnoreAttribute ignore,
            ExplicitAttribute expl)
        {
            // Check if fixture/method is ignored/explicit
            if (ignore == null && TypeHelper.HasCustomAttribute(method, typeof(IgnoreAttribute)))
            {
                ignore = TypeHelper.GetFirstCustomAttribute(method, typeof(IgnoreAttribute)) as IgnoreAttribute;
            }
            if (expl == null && TypeHelper.HasCustomAttribute(method, typeof(ExplicitAttribute)))
            {
                expl = TypeHelper.GetFirstCustomAttribute(method, typeof(ExplicitAttribute)) as ExplicitAttribute;
            }

            if (ignore != null)
            {
                // Do not generate unnecessary test cases
                IgnoredLoadingRunInvoker invoker = new IgnoredLoadingRunInvoker(this, method, ignore.Description);
                tree.AddChild(parent, invoker);
            }
            else if (expl != null)
            {
                // Do not generate unnecessary test cases
                IgnoredLoadingRunInvoker invoker = new IgnoredLoadingRunInvoker(this, method, expl.Description);
                tree.AddChild(parent, invoker);
            }
            else
            {
                CombinatorialTestAttribute testAttribute = TypeHelper.GetFirstCustomAttribute(method, typeof(CombinatorialTestAttribute))
                    as CombinatorialTestAttribute;

                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 0)
                {
                    Exception ex = new Exception("No parameters");
                    MethodFailedLoadingRunInvoker invoker = new MethodFailedLoadingRunInvoker(this, ex, method);
                    tree.AddChild(parent, invoker);
                    return;
                }

                // create the models
                DomainCollection domains = new DomainCollection();
                Type[] parameterTypes = new Type[parameters.Length];
                int index = 0;
                foreach (ParameterInfo parameter in parameters)
                {
                    parameterTypes[index] = parameter.ParameterType;

                    DomainCollection pdomains = new DomainCollection();
                    foreach (UsingBaseAttribute usingAttribute in parameter.GetCustomAttributes(typeof(UsingBaseAttribute), true))
                    {
                        try
                        {
                            usingAttribute.GetDomains(pdomains, parameter, fixture);
                        }
                        catch (Exception ex)
                        {
                            Exception pex = new Exception("Failed while loading domains from parameter " + parameter.Name,
                                ex);
                            MethodFailedLoadingRunInvoker invoker = new MethodFailedLoadingRunInvoker(this, pex, method);
                            tree.AddChild(parent, invoker);
                        }
                    }
                    if (pdomains.Count == 0)
                    {
                        Exception ex = new Exception("Could not find domain for argument " + parameter.Name);
                        MethodFailedLoadingRunInvoker invoker = new MethodFailedLoadingRunInvoker(this, ex, method);
                        tree.AddChild(parent, invoker);
                        return;
                    }
                    domains.Add(Domains.ToDomain(pdomains));

                    index++;
                }

                // get the validator method if any
                MethodInfo validator = null;
                if (testAttribute.TupleValidatorMethod != null)
                {
                    validator = fixture.GetType().GetMethod(testAttribute.TupleValidatorMethod, parameterTypes);
                    if (validator == null)
                    {
                        Exception ex = new Exception("Could not find validator method " + testAttribute.TupleValidatorMethod);
                        MethodFailedLoadingRunInvoker invoker = new MethodFailedLoadingRunInvoker(this, ex, method);
                        tree.AddChild(parent, invoker);
                        return;
                    }
                }

                // we make a cartesian product of all those
                foreach (ITuple tuple in Products.Cartesian(domains))
                {
                    // create data domains
                    DomainCollection tdomains = new DomainCollection();
                    for (int i = 0; i < tuple.Count; ++i)
                    {
                        IDomain dm = (IDomain)tuple[i];
                        tdomains.Add(dm);
                    }

                    // computing the pairwize product
                    foreach (ITuple ptuple in testAttribute.GetProduct(tdomains))
                    {
                        if (validator != null)
                        {
                            bool isValid = (bool)validator.Invoke(fixture, ptuple.ToObjectArray());
                            if (!isValid)
                                continue;
                        }

                        TupleRunInvoker invoker = new TupleRunInvoker(this, method, tuple, ptuple);
                        IRunInvoker dinvoker = DecoratorPatternAttribute.DecoreInvoker(method, invoker);
                        tree.AddChild(parent, dinvoker);
                    }
                }
            }
        }

        private class TupleRunInvoker : RunInvoker
        {
            private MethodInfo method;
            private ITuple tupleDomains;
            private ITuple tuple;

            public TupleRunInvoker(
                IRun generator, 
                MethodInfo method, 
                ITuple tupleDomains, 
                ITuple tuple)
                    :base(generator)
            {
                this.method = method;
                this.tupleDomains = tupleDomains;
                this.tuple = tuple;
            }

            public override String Name
            {
                get
                {
                    StringWriter sw = new StringWriter();
                    for (int i = 0; i < this.tupleDomains.Count; ++i)
                    {
                        IDomain dm = (IDomain)this.tupleDomains[i];
                        if (dm.Name != null)
                            sw.Write("{0}({1}),", dm.Name, this.tuple[i]);
                        else
                            sw.Write("{0},", this.tuple[i]);
                    }
                    return String.Format("{0}({1})", this.method.Name, sw.ToString().TrimEnd(','));
                }
            }

            public override Object Execute(Object o, IList args)
            {
                foreach (Object item in this.tuple)
                    args.Add(item);

                object[] arguments = new object[args.Count];
                args.CopyTo(arguments, 0);
                try
                {
                    this.method.Invoke(o, arguments);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    args.Clear();
                }
                return null;
            }

            public override bool ContainsMemberInfo(MemberInfo memberInfo)
            {
                return this.method == memberInfo;
            }
        }
    }
}
