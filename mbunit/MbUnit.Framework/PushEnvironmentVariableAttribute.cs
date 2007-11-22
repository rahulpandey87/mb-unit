using System;
using System.Collections;

using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple  = true, Inherited =true)]
    public sealed class PushEnvironmentVariableAttribute : DecoratorPatternAttribute
    {
        private string name;
        private string value;

        public PushEnvironmentVariableAttribute(
            string name,
            string value
            )
        {
            this.name = name;
            this.value = value;
        }
        public string Name
        {
            get { return this.name; }
        }
        public string Value
        {
            get { return this.value; }
        }

        public override IRunInvoker GetInvoker(IRunInvoker invoker)
        {
            return new PushEnvironmentVariableRunInvoker(invoker, this);
        }

        private sealed class PushEnvironmentVariableRunInvoker :
            DecoratorRunInvoker
        {
            private PushEnvironmentVariableAttribute attribute;
            public PushEnvironmentVariableRunInvoker(
                IRunInvoker invoker,
                PushEnvironmentVariableAttribute attribute
                )
                :base(invoker)
            {
                this.attribute = attribute;
            }

            public override Object Execute(Object o, IList args)
            {
                // store previous value
                string previousValue = Environment.GetEnvironmentVariable(
                    attribute.Name
                    );

                try
                {
                    // install new value
                    Environment.SetEnvironmentVariable(
                        attribute.Name,
                        attribute.Value
                        );

                    // run base invoker
                    return this.Invoker.Execute(o, args);
                }
                finally
                {
                    // cleaning our mess
                    Environment.SetEnvironmentVariable(
                        attribute.Name,
                        previousValue
                        );
                }
            }
        }
    }
}
