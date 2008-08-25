using System;
using MbUnit.Core.Runs;
namespace MbUnit.Framework {
    /// <summary>
    /// Tags classes that generate the full sequence of actions for a test method wrapped in the named <see cref="IRun"/> type
    /// </summary>
    /// <example>
    /// <para>The <see cref="TestFixtureRun"/> class neatly demonstrates the use of <see cref="RunFactoryAttribute"/></para>
    /// <code>
    /// [RunFactory(typeof(TestRun))]
    /// [RunFactory(typeof(RowRun))]
    /// [RunFactory(typeof(RepeatRun))]
    /// [RunFactory(typeof(CombinatorialRun))]
    /// public sealed class TestFixtureRun : Run
    /// {
    ///     ...
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal sealed class RunFactoryAttribute : Attribute {
        private Type runType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunFactoryAttribute"/> class.
        /// </summary>
        /// <param name="runType">Type of the run class for which the tagged class can create a sequence of actions.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="runType"/> is null</exception>
        public RunFactoryAttribute(Type runType) {
            if (runType == null)
                throw new ArgumentNullException("runType");
            this.runType = runType;
        }

        /// <summary>
        /// Gets the type of the run class for which the tagged class can create a sequence of actions.
        /// </summary>
        /// <value>The type of the run class for which the tagged class can create a sequence of actions.</value>
        public Type RunType {
            get { return this.runType; }
        }

        /// <summary>
        /// Creates the run.
        /// </summary>
        /// <returns>An <see cref="IRun"/> object of the type specified by <see cref="RunType"/></returns>
        /// <exception cref="ArgumentException">Thrown if <see cref="RunType"/> does not implement IRun</exception>
        public IRun CreateRun() {
            IRun run = Activator.CreateInstance(this.RunType) as IRun;
            if (run == null)
                throw new ArgumentException("Run type " + this.RunType + " is not assignable to IRun");
            return run;
        }
    }
}
