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

using MbUnit.Core.Runs;
using MbUnit.Core.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Core
{
    public class NakedFixtureFactory : IFixtureFactory
    {
        private IRun run = null;

        private string fixtureNameSuffix = "Fixture";
        private string testFixtureSetUpName = "TestFixtureSetUp";
        private string testFixtureTearDownName = "TestFixtureTearDown";
        private string setUpName = "SetUp";
        private string tearDownName = "TearDown";
        private string testNameSuffix = "Test";

        public NakedFixtureFactory()
        {
            this.CreateRun();
        }

        #region Properties
        public string FixtureNameSuffix
        {
            get
            {
                return this.fixtureNameSuffix;
            }
            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                this.fixtureNameSuffix = value;
            }
        }
        public string TestFixtureSetUpName
        {
            get
            {
                return testFixtureSetUpName;
            }

            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                testFixtureSetUpName = value;
            }
        }
        public string TestFixtureTearDownName
        {
            get
            {
                return testFixtureTearDownName;
            }

            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                testFixtureTearDownName = value;
            }
        }
        public string SetUpName
        {
            get
            {
                return setUpName;
            }

            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                setUpName = value;
            }
        }
        public string TearDownName
        {
            get
            {
                return this.tearDownName;
            }

            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                tearDownName = value;
            }
        }
        public string TestNameSuffix
        {
            get
            {
                return this.testNameSuffix;
            }

            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                testNameSuffix = value;
            }
        }
        #endregion

        public void Create(Type t, FixtureCollection fixtures)
        {
            if (!t.IsClass)
                return;
            if (t.IsAbstract)
                return;
            if (!t.Name.EndsWith(this.fixtureNameSuffix))
                return;


            MethodInfo setup = t.GetMethod(this.testFixtureSetUpName, Type.EmptyTypes);
            MethodInfo tearDown = t.GetMethod(this.testFixtureTearDownName, Type.EmptyTypes);

            Fixture fixture = new Fixture(t, this.run, setup, tearDown, false);
            fixtures.Add(fixture);
        }

        private void CreateRun()
        {
            SequenceRun runs = new SequenceRun();

            OptionalNakedMethodRun setUp = new OptionalNakedMethodRun(this.setUpName);
            runs.Runs.Add(setUp);

            NakedMethodRun tests = new NakedMethodRun(this.testNameSuffix);
            runs.Runs.Add(tests);

            OptionalNakedMethodRun tearDown = new OptionalNakedMethodRun(this.tearDownName);
            runs.Runs.Add(tearDown);

            this.run = runs;
        }

        internal class OptionalNakedMethodRun : Run
        {
            private string methodName;
            public OptionalNakedMethodRun(string methodName)
                :base("OptionalNakedMethod",false)
            {
                if (methodName == null)
                    throw new ArgumentNullException("methodName");
                this.methodName = methodName;
            }

            public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
            {
                MethodInfo method = t.GetMethod(this.methodName, Type.EmptyTypes);
                if (method == null)
                    return;
                MethodRunInvoker invoker = new MethodRunInvoker(this, method);
                IRunInvoker decoratedInvoker = DecoratorPatternAttribute.DecoreInvoker(method, invoker);
                tree.AddChild(parent, decoratedInvoker);
            }
        }

        internal class NakedMethodRun : Run
        {
            private string methodNameSuffix;

            public NakedMethodRun(string methodNameSuffix)
                :base("NakedMethod",true)
            {
                if (methodNameSuffix == null)
                    throw new ArgumentNullException("methodName");
                this.methodNameSuffix = methodNameSuffix;
            }

            public override void Reflect(
                RunInvokerTree tree, 
                RunInvokerVertex parent, 
                Type t)
            {
                foreach (MethodInfo method in t.GetMethods())
                {
                    if (method.IsSpecialName)
                        continue;
                    if (!method.Name.EndsWith(this.methodNameSuffix))
                        continue;

                    MethodRunInvoker invoker = new MethodRunInvoker(this, method);
                    IRunInvoker decoratedInvoker = DecoratorPatternAttribute.DecoreInvoker(method, invoker);
                    tree.AddChild(parent, decoratedInvoker);
                }
            }
        }
    }
}
