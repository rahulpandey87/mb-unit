// Copyright 2007 MbUnit Project - http://www.mbunit.com/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Gallio.Model;

namespace Gallio.Model.Execution
{
    /// <summary>
    /// A test monitor tracks the execution of a single <see cref="ITest" />.  It is the mechanism
    /// used by <see cref="ITestController" /> to interface with the <see cref="ITestPlan" />
    /// and ensure that tests are executed in the desired order with all dependencies taken care of.
    /// </summary>
    /// <remarks author="jeff">
    /// At this time, use of a test monitor implies a serial order of test execution.
    /// That need not be the case.  If a test monitor has a flag to indicate whether its
    /// test could be executed in parallel with its siblings (and under what constraints)
    /// then a smart test controller could execute those tests in parallel.  If carried out
    /// recursively, a test controller could easily manage arbitrarily deeply nested
    /// parallel execution of non-interfering tests.
    /// </remarks>
    public interface ITestMonitor
    {
        /// <summary>
        /// Gets the test managed by this monitor.
        /// </summary>
        ITest Test { get; }

        /// <summary>
        /// Gets the total number of tests managed by the monitor, including itself.
        /// </summary>
        int TestCount { get; }

        /// <summary>
        /// Returns true if the test was explicitly selected by a filter.
        /// </summary>
        /// <remarks>
        /// This flag enables test controllers to distinguish the case when a particular test
        /// was explicitly selected by a user-specified filter rather than by virtue of
        /// one of its parents having been selected.
        /// </remarks>
        bool IsExplicit { get; }

        /// <summary>
        /// Gets monitors for the children of the test to run within the scope
        /// of this test in the order in which they should be executed.
        /// </summary>
        IEnumerable<ITestMonitor> Children { get; }

        /// <summary>
        /// Enumerates this monitor and all of its descendants in pre-order tree
        /// traversal.
        /// </summary>
        IEnumerable<ITestMonitor> PreOrderTraversal { get; }

        /// <summary>
        /// Gets a list consisting of this monitor and all of its descendants as
        /// enumerated by pre-order tree traversal.
        /// </summary>
        /// <returns>The list of all monitors</returns>
        IList<ITestMonitor> GetAllMonitors();

        /// <summary>
        /// Starts the root step of a new test instance and returns its step monitor.
        /// </summary>
        /// <remarks>
        /// The current thread's test context is set to a new context for the
        /// test step that is starting.  The new context will be a child of the
        /// current thread's context.
        /// </remarks>
        /// <param name="rootStep">The test root step of the test instance</param>
        /// <returns>The monitor for the root step of the test instance</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rootStep"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="rootStep"/> is not the root
        /// step of an instance of this test</exception>
        ITestStepMonitor StartTestInstance(ITestStep rootStep);

        /// <summary>
        /// Starts the root step of a new test instance as a child of the currently
        /// executing test instance and returns its step monitor
        /// using a default implementation of <see cref="ITestStep" />.
        /// </summary>
        /// <returns>The monitor for the root step of the test instance</returns>
        /// <seealso cref="StartTestInstance(ITestStep)"/>
        ITestStepMonitor StartTestInstance();
    }
}
