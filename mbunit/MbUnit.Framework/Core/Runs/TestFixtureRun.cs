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
using MbUnit.Core.Invokers;
using MbUnit.Framework;

namespace MbUnit.Core.Runs
{
	/// <summary>
	/// Test fixture run with support for decoration by
    /// <see cref="TestFixtureExtensionAttribute" />.
	/// </summary>
    [RunFactory(typeof(TestRun))]
    [RunFactory(typeof(RowRun))]
    [RunFactory(typeof(CombinatorialRun))]
    public sealed class TestFixtureRun : Run
	{
        private static readonly object syncRoot = new object();
        private static IRun setupRun;
        private static IRun tearDownRun;
        private static IRun[] testRuns;

        public TestFixtureRun()
            : base("TestFixture", false)
        {
        }
		
        /// <summary>
        /// Builds the test run invoker tree.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="parent"></param>
        /// <param name="t"></param>
		public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
		{
            // Cache these runs because we reuse them for every test fixture.
            lock (syncRoot)
            {
                if (tearDownRun == null)
                {
                    setupRun = new OptionalMethodRun(typeof(SetUpAttribute), false);

                    object[] factories = GetType().GetCustomAttributes(typeof(RunFactoryAttribute), true);
                    testRuns = new IRun[factories.Length];
                    for (int i = 0; i < factories.Length; i++)
                        testRuns[i] = ((RunFactoryAttribute) factories[i]).CreateRun();

                    tearDownRun = new OptionalMethodRun(typeof(TearDownAttribute), false);
                }
            }

            // Build the sequence including any extensions and apply it.
            CreateSequence(t).Reflect(tree, parent, t);
        }

        private IRun CreateSequence(Type t)
        {
            SequenceRun seq = new SequenceRun();
            object[] extensions = t.GetCustomAttributes(typeof(TestFixtureExtensionAttribute), true);

            // Before Setup (extensions)
            foreach (TestFixtureExtensionAttribute extension in extensions)
                extension.AddBeforeSetupRuns(seq.Runs);

            // Setup
            seq.Runs.Add(setupRun);

            // Tests
            ParallelRun tests = new ParallelRun();
            seq.Runs.Add(tests);

            foreach (IRun testRun in testRuns)
                tests.Runs.Add(testRun);

            // Tests (extensions)
            foreach (TestFixtureExtensionAttribute extension in extensions)
                extension.AddTestRuns(tests.Runs);

            // TearDown
            seq.Runs.Add(tearDownRun);

            // After TearDown (extensions)
            foreach (TestFixtureExtensionAttribute extension in extensions)
                extension.AddAfterTearDownRuns(seq.Runs);

            return seq;
		}
	}
}
