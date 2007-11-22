// MbUnit Test Framework
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux

using System;
using System.Reflection;

namespace MbUnit.Core.FrameworkBridges
{
    using MbUnit.Core.Runs;
    using MbUnit.Core.Invokers;
    using MbUnit.Framework;
    using MbUnit.Core.Exceptions;
    using MbUnit.Core.Collections;

    internal sealed class FrameworkFixtureFactory : IFixtureFactory
    {
        public FrameworkFixtureFactory()
        {}

        public void Create(Type t, FixtureCollection fixtures)
        {
            // get framework
            IFramework framework = FrameworkFactory.FromType(t);
            if (framework == null)
                return;

            // get test attribute
            Object fixtureAttribute = TypeHelper.TryGetFirstCustomAttribute(t,framework.TestFixtureAttributeType);
            if (fixtureAttribute==null)
                return;

            // create run
            bool ignored = TypeHelper.HasCustomAttribute(t, framework.IgnoreAttributeType);
            MethodInfo setUp = TypeHelper.GetAttributedMethod(t, framework.TestFixtureSetUpAttributeType);
            MethodInfo tearDown = TypeHelper.GetAttributedMethod(t, framework.TestFixtureTearDownAttributeType);

            Fixture fixture = new Fixture(t, this.CreateRun(framework),setUp,tearDown, ignored);
            fixtures.Add(fixture);
        }

        private IRun CreateRun(IFramework framework)
        {
            if (framework == null)
                throw new ArgumentNullException("framework");

            SequenceRun runs = new SequenceRun();

            // SetUp
            OptionalMethodRun setup = new OptionalMethodRun(framework.SetUpAttributeType, false);
            runs.Runs.Add(setup);

            // Test
            FrameworkMethodRun test = new FrameworkMethodRun(framework);
            runs.Runs.Add(test);

            // TearDown
            OptionalMethodRun tearDown = new OptionalMethodRun(framework.TearDownAttributeType, false);
            runs.Runs.Add(tearDown);

            return runs;
        }

        public class FrameworkMethodRun : Run
        {
            private IFramework framework;

            public FrameworkMethodRun(IFramework framework)
                :base(framework.TestAttributeType.Name,true)
            {
                this.framework = framework;
            }

            public IFramework Framework
            {
                get
                {
                    return this.framework;
                }
            }

            public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
            {
                foreach (MethodInfo mi in TypeHelper.GetAttributedMethods(t, this.framework.TestAttributeType))
                {
                    IRunInvoker invoker = this.CreateInvoker(mi);
                    tree.AddChild(parent, invoker);
                }
            }

            protected virtual IRunInvoker CreateInvoker(MethodInfo mi)
            {
                if (mi == null)
                    throw new ArgumentException("mi");

                return new FrameworkMethodRunInvoker(this,this.framework, mi);
            }
        }

        public class FrameworkMethodRunInvoker : RunInvoker
        {
            private IFramework framework;
            private MethodInfo method;
            private bool ignored = false;
            private string ignoredReason = null;
            private bool expectException = false;
            private Type expectedExceptionType = null;
            private string expectedExceptionMessage = null;

            public FrameworkMethodRunInvoker(FrameworkMethodRun run, IFramework framework, MethodInfo method)
                :base(run)
            {
                if (framework == null)
                    throw new ArgumentNullException("framework");
                if (method == null)
                    throw new ArgumentNullException("mi");
                this.framework = framework;
                this.method = method;

                this.Inspect();
            }

            public override String Name
            {
                get 
                {
                    string name = this.method.Name;
                    if (this.expectException)
                        name = String.Format("ExpectedException({0},{1})", name, this.expectedExceptionType.Name);
                    if (this.ignored)
                        name = String.Format("Ignore({0})", name);
                    return name;
                }
            }

            public override Object Execute(Object o, System.Collections.IList args)
            {
                if (this.ignored)
                {
                    Assert.Ignore(this.ignoredReason);
                    return null;
                }

                if (this.expectException)
                    return this.ExpectedExceptionInvoke(o, args);
                else 
                    return this.Invoke(o, args);
            }

            protected Object ExpectedExceptionInvoke(Object o, System.Collections.IList args)
            {
                try
                {
                    this.Invoke(o, args);
                }
                catch (Exception ex)
                {
                    // ex is TargetInvokeException
                    Exception innerException = ex.InnerException;
                    Type innerExceptionType = innerException.GetType();

                    // check if exception is the right type...
                    if (!this.expectedExceptionType.IsAssignableFrom(innerExceptionType))
                        throw new ExceptionTypeMistmachException(
                             this.expectedExceptionType,
                             this.expectedExceptionMessage,
                             ex);
                    else
                        return null;
                }
                throw new ExceptionNotThrownException(this.expectedExceptionType, this.expectedExceptionMessage);
            }

            protected object Invoke(Object o, System.Collections.IList args)
            {
                Object[] parameters = new Object[args.Count];
                args.CopyTo(parameters, 0);

                return this.method.Invoke(o, parameters);
            }

            protected void Inspect()
            {
                // check if ignored
                Object ignoreAttribute = TypeHelper.TryGetFirstCustomAttribute(
                    this.method, 
                    this.framework.IgnoreAttributeType
                    );
                if (ignoreAttribute!=null)
                {
                    this.ignored = true;
                    this.ignoredReason=this.framework.GetIgnoreAttributeReason(ignoreAttribute);
                }

                // expected exception
                Object expectedExceptionAttribute = TypeHelper.TryGetFirstCustomAttribute(
                    this.method,
                    this.framework.ExpectedExceptionAttributeType
                    );
                if (expectedExceptionAttribute != null)
                {
                    this.expectException = true;
                    this.expectedExceptionType = this.framework.GetExpectedExceptionAttributeExceptionType(expectedExceptionAttribute);
                    this.expectedExceptionMessage = this.framework.GetExpectedExceptionAttributeExpectedMessage(expectedExceptionAttribute);
                }
            }
        }
    }
}
