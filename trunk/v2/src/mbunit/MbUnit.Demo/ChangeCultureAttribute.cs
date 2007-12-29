using System;
using System.Collections;
using System.Threading;
using System.Globalization;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Demo
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false,Inherited = true)]
    public class ChangeCultureAttribute : DecoratorPatternAttribute
    {
        private string name;
        public ChangeCultureAttribute(string name)
        {
            if (name == null)
                throw new ArgumentNullException("null");
            this.name = name;
        }

        public override IRunInvoker GetInvoker(IRunInvoker wrapped)
        {
            ChangeCultureRunInvoker invoker = 
                new ChangeCultureRunInvoker(this.name,wrapped);
            return invoker;
        }

        internal class ChangeCultureRunInvoker : DecoratorRunInvoker
        {
            private string cultureName;
            public ChangeCultureRunInvoker(string cultureName, IRunInvoker invoker)
                :base(invoker)
            {
                this.cultureName = cultureName;
            }

            public override Object Execute(
                object o, 
                IList args)
            {
                CultureInfo culture = null;

                try
                {
                    culture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);

                    return this.Invoker.Execute(o, args);
                }
                finally
                {
                    if (culture!=null)
                        Thread.CurrentThread.CurrentCulture = culture;
                }
            }
        }
    }
}
