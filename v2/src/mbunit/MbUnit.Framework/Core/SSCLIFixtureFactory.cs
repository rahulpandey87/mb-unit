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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using System;
using System.Reflection;
using MbUnit.Core.Runs;
using MbUnit.Core.Invokers;
using MbUnit.Framework;
using MbUnit.Core.Collections;

namespace MbUnit.Core
{
	public sealed class SSCLIFixtureFactory : IFixtureFactory
	{
        private int successReturnCode = 100;

		public void Create(Type t, FixtureCollection fixtures)
		{
            MethodInfo mi =  this.GetMain(t,new Type[] { typeof(string[]) });
            if (mi == null)
                mi = this.GetMain(t,Type.EmptyTypes);
            if (mi == null)
                return;

            // create run
			SSCLIRun run = new SSCLIRun(mi, this.successReturnCode);
			Fixture fixture = new Fixture(t,run,null,null, false);
            fixtures.Add(fixture);
        }

        private MethodInfo GetMain(Type t, Type[] types)
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly==null)
                return null;

			// a SSCLI fixture has a static Main function
			MethodInfo mi = t.GetMethod(
                "Main",
                BindingFlags.Static
                | BindingFlags.Public
                | BindingFlags.NonPublic,
                null,
                types,
                null
                );
			if (mi==null)
				return null;
            // check that the main is not the entry point of the executing
            // assembly
            if (entryAssembly.EntryPoint == mi)
                return null;

            return mi;
        }


		public sealed class SSCLIRun : Run
		{
            private int successReturnCode;
            private MethodInfo main;
			public SSCLIRun(MethodInfo main, int successReturnCode)
				:base("SSCLI",true)
			{
				if (main==null)
					throw new ArgumentNullException("main");
				this.main=main;
                this.successReturnCode = successReturnCode;
            }

			public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
			{
				MainMethodRunInvoker invoker = new MainMethodRunInvoker(this,this.main, this.successReturnCode);
				RunInvokerVertex child = 
					tree.AddChild(parent,invoker);
			}
		}

		public class MainMethodRunInvoker : MethodRunInvoker
		{
            private int successReturnCode;
            private bool hasArguments;
            private bool returnsInt;
            public MainMethodRunInvoker(IRun generator, MethodInfo method, int successReturnCode)
				:base(generator,method)
			{
				ParameterInfo[] ps = this.Method.GetParameters();
				this.hasArguments= (ps.Length!=0);
                this.returnsInt = this.Method.ReturnType == typeof(int);
                this.successReturnCode = successReturnCode;
            }

            public override Object Execute(Object o, System.Collections.IList args)
			{
				if (this.hasArguments)
				{
					args.Add( new String[] {} );
				}

                int returnValue = 0;
                if (this.returnsInt)
                    returnValue = (int)base.Execute(o, args);
                else
                {
                    base.Execute(o, args);
                    returnValue = Environment.ExitCode;
                }
                Assert.AreEqual(this.successReturnCode,returnValue,
                    "ExitCode = {0}, expected {1} for successful run",
                    returnValue,this.successReturnCode);

				return null;
			}

		}
	}
}
