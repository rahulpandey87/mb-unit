using System;
using MbUnit.Core.Runs;
namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple  = true, Inherited  = true)]
    internal sealed class RunFactoryAttribute : Attribute
    {
        private Type runType;

        public RunFactoryAttribute(Type runType)
        {
            if (runType == null)
                throw new ArgumentNullException("runType");
            this.runType = runType;
        }

        public Type RunType
        {
            get { return this.runType; }
        }

        public IRun CreateRun()
        {
            IRun run = Activator.CreateInstance(this.RunType) as IRun;
            if (run == null)
                throw new ArgumentException("Run type "+this.RunType+" is not assignable to IRun");
            return run;
        }
    }
}
