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
using System.Diagnostics;
using MbUnit.Core.Invokers;
using System.Reflection;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class PerfCounterAttribute : DecoratorPatternAttribute
    {
        private string categoryName;
        private string counterName;
        private string instanceName=null;
        private string machineName = null;
        private float minValue = int.MinValue;
        private float maxValue;
        private bool relative = false;

        public PerfCounterAttribute(string categoryName, string counterName, float maxValue)
        {
            if (categoryName == null)
                throw new ArgumentNullException("categoryName");
            if (counterName == null)
                throw new ArgumentNullException("counterName");
            if (!PerformanceCounterCategory.Exists(categoryName))
                throw new ArgumentException("Performance category does not exist",
                    categoryName);
            this.categoryName = categoryName;
            this.counterName = counterName;
            this.maxValue = maxValue;
        }

        public String CategoryName
        {
            get
            {
                return this.categoryName;
            }
        }

        public string CounterName
        {
            get
            {
                return this.counterName;
            }
        }

        public string InstanceName
        {
            get
            {
                return this.instanceName;
            }
            set
            {
                this.instanceName = value;
            }
        }

        public string MachineName
        {
            get
            {
                return this.machineName;
            }
            set
            {
                this.machineName = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return this.maxValue;
            }
        }

        public float MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                this.minValue = value;
            }
        }

        public bool Relative
        {
            get
            {
                return this.relative;
            }
            set
            {
                this.relative = value;
            }
        }

        public override IRunInvoker GetInvoker(IRunInvoker wrapper)
        {
            return new PerfCounterRunInvoker(wrapper, this);
        }

        #region PerfCounterRunInvoker
        public class PerfCounterRunInvoker : DecoratorRunInvoker
        {
            private PerfCounterAttribute attribute;
            private PerformanceCounter counter;
            private float value;

            public PerfCounterRunInvoker(IRunInvoker invoker, PerfCounterAttribute attribute)
            :base(invoker)
            {
                this.attribute = attribute;
                this.counter = new PerformanceCounter();
                this.counter.CategoryName = attribute.CategoryName;
                this.counter.CounterName = attribute.CounterName;

                Process currentProcesss = Process.GetCurrentProcess();
                if (attribute.InstanceName != null)
                    this.counter.InstanceName = attribute.InstanceName;
                else
                    this.counter.InstanceName = currentProcesss.ProcessName;

                if (attribute.MachineName != null)
                    this.counter.MachineName = attribute.MachineName;
                else
                    this.counter.MachineName = currentProcesss.MachineName;
            }

            public override Object Execute(object o, System.Collections.IList args)
            {
                this.value = this.counter.NextValue();
                Object result = this.Invoker.Execute(o, args);
                float finalValue = this.counter.NextValue();

                // corrected value
                float correctedValue = finalValue;
                if (attribute.Relative)
                    correctedValue -= this.value;

                Console.WriteLine("Monitoring {0},{1}, for {2},{3} : {4} -> {5} (diff {6})",
                    attribute.CategoryName,
                    attribute.CounterName,
                    this.counter.InstanceName,
                    this.counter.MachineName,
                    this.value,
                    finalValue,
                    correctedValue);

                if (correctedValue > this.attribute.MaxValue)
                {
                    Assert.Fail("PerformanceCounter {0}, {1}, {2}, {3} value ({4}) was above maxium ({5})",
                        this.attribute.CategoryName,
                        this.attribute.CounterName,
                        counter.InstanceName,
                        counter.MachineName,
                        correctedValue,
                        this.attribute.MaxValue
                        );
                }

                if (correctedValue < this.attribute.MinValue)
                {
                    Assert.Fail("PerformanceCounter {0}, {1}, {2}, {3} value ({4}) was under minimum ({5})",
                        this.attribute.CategoryName,
                        this.attribute.CounterName,
                        counter.InstanceName,
                        counter.MachineName,
                        correctedValue,
                        this.attribute.MinValue
                        );
                }

                return result;
            }
        }
        #endregion
    }
}
