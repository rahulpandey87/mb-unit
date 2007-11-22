using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections;

using MbUnit.Core.Framework;
using MbUnit.Core;
using MbUnit.Core.Runs;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =true)]
    public sealed class ComponentFixtureAttribute : TestFixturePatternAttribute
    {
        private static ComponentRun run = new ComponentRun();
        public override IRun GetRun()
        {
            return run;
        }

        public class ComponentRun : Run
        {
            public ComponentRun()
                :base("ComponentRun",false)
            { }

            public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
            {
                if (!typeof(IComponent).IsAssignableFrom(t))
                    throw new ArgumentException("Fixture type implement IComponent",t.FullName);

                // get set up or tearown
                // look
                MethodInfo setUp = TypeHelper.GetAttributedMethod(t,typeof(SetUpAttribute));
                MethodInfo tearDown = TypeHelper.GetAttributedMethod(t,typeof(TearDownAttribute));

                using (IComponent fixture = (IComponent)TypeHelper.CreateInstance(t))
                {
                    // get components field
                    FieldInfo componentsField = t.GetField("components",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    if (componentsField == null)
                        return;
                    // call InitializeMethod
                    MethodInfo initialzeComponent = t.GetMethod("InitializeComponent",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    if (initialzeComponent!=null)
                        initialzeComponent.Invoke(fixture, null);

                    IContainer components = componentsField.GetValue(fixture) as IContainer;
                    if (components == null)
                        return;

                    ArrayList suites = new ArrayList();
                    // get suites
                    foreach(IComponent component in components.Components)
                    {
                        // get component
                        ITestComponent testComponent = component as ITestComponent;
                        if (testComponent == null)
                            continue;

                        // get test suite
                        ITestSuite testSuite = testComponent.GetTests();
                        if (testSuite == null)
                            continue;
                        suites.Add(testSuite);
                    }

                    // decorate
                    foreach(IComponent component in components.Components)
                    {
                        ITestDecoratorComponent decorator = component as ITestDecoratorComponent;
                        if(decorator==null)
                            continue;

                        for(int i=0;i<suites.Count;++i)
                        {
                            suites[i] = decorator.Decorate((ITestSuite)suites[i]);
                        }
                    }

                    // add suites
                    foreach (ITestSuite testSuite in suites)
                    {
                        foreach (ITestCase testCase in testSuite.TestCases)
                        {
                            TestCaseRunInvoker invoker = new TestCaseRunInvoker(
                                this, testSuite, testCase, setUp, tearDown);
                            tree.AddChild(parent, invoker);
                        }
                    }
                }
            }
        }
    }
}
