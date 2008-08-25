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

namespace MbUnit.Framework {
    /// <summary>
    /// Tags a test fixture class or test method with implicit assertions about the values of a performance 
    /// counter on your machine. 
    /// </summary>
    /// <remarks>
    /// Use multiple PerfCounter attributes on a class or method to make assertions about several performance counters at 
    /// a time. Use the <see cref="PerfCounterInfo"/> classes to reference the required performance counter correctly.
    /// </remarks>
    /// <example>
    /// <code>
    /// [Test]
    /// [PerfCounter("Process", "% Processor Time", 10)]
    /// public void MonitorProcess()
    /// {
    ///     this.CreateArray();
    /// }
    ///
    /// [Test]
    /// [PerfCounter(".NET CLR Loading", "% Time Loading", 10)]
    /// [PerfCounter(".NET CLR Security", "% Time in RT checks", 10000)]
    /// [PerfCounter(".NET CLR Security", "% Time Sig. Authenticating", 10)]
    /// [PerfCounter(".NET CLR Memory", "# Bytes in all Heaps", 5000000, Relative =true)]
    /// [PerfCounter(".NET CLR Jit", "% Time in Jit", 10)]
    /// public void MonitorMultipleCounters()
    /// {
    ///     this.CreateArray();
    /// }
    ///    </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class PerfCounterAttribute : DecoratorPatternAttribute {
        private string categoryName;
        private string counterName;
        private string instanceName = null;
        private string machineName = null;
        private float minValue = int.MinValue;
        private float maxValue;
        private bool relative = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerfCounterAttribute"/> class.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="maxValue">The max value of the counter.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="categoryName"/> or 
        /// <paramref name="counterName"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="categoryName"/> references a category
        /// that does not exist</exception>
        public PerfCounterAttribute(string categoryName, string counterName, float maxValue) {
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

        /// <summary>
        /// Gets the name of the performance category.
        /// </summary>
        /// <value>The name of the category.</value>
        public String CategoryName {
            get {
                return this.categoryName;
            }
        }

        /// <summary>
        /// Gets the name of the performance counter being checked.
        /// </summary>
        /// <value>The name of the performance counter being checked.</value>
        public string CounterName {
            get {
                return this.counterName;
            }
        }

        /// <summary>
        /// Gets or sets the instance name of the performance counter
        /// </summary>
        /// <value>The instance name of the performance counter</value>
        public string InstanceName {
            get {
                return this.instanceName;
            }
            set {
                this.instanceName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the machine the counter is running on.
        /// Default value is the local machine
        /// </summary>
        /// <value>The name of the machine.</value>
        public string MachineName {
            get {
                return this.machineName;
            }
            set {
                this.machineName = value;
            }
        }

        /// <summary>
        /// Gets the maximum value of the counter.
        /// </summary>
        /// <value>The maximum value of the counter.</value>
        public float MaxValue {
            get {
                return this.maxValue;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the counter.
        /// </summary>
        /// <value>The minimum value of the counter.</value>
        public float MinValue {
            get {
                return this.minValue;
            }
            set {
                this.minValue = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the counter measurement 
        /// should be calculated relative to its original value.
        /// </summary>
        /// <value><c>true</c> if relative; otherwise, <c>false</c>.</value>
        public bool Relative {
            get {
                return this.relative;
            }
            set {
                this.relative = value;
            }
        }

        /// <summary>
        /// Returns the invoker class to run the test with the given duration parameters.
        /// </summary>
        /// <param name="wrapper">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="PerfCounterRunInvoker"/> object wrapping <paramref name="wrapper"/></returns>
        public override IRunInvoker GetInvoker(IRunInvoker wrapper) {
            return new PerfCounterRunInvoker(wrapper, this);
        }

        #region PerfCounterRunInvoker
        /// <summary>
        /// Class that understands how to invoke a test given and monitor a performance counter
        /// nominated by a <see cref="PerfCounterAttribute"/> tagging the test method
        /// </summary>
        public class PerfCounterRunInvoker : DecoratorRunInvoker {
            private PerfCounterAttribute attribute;
            private PerformanceCounter counter;
            private float value;

            /// <summary>
            /// Initializes a new instance of the <see cref="PerfCounterRunInvoker"/> class.
            /// </summary>
            /// <param name="invoker">The test invoker.</param>
            /// <param name="attribute">The <see cref="PerfCounterAttribute"/> tagging the test method.</param>
            public PerfCounterRunInvoker(IRunInvoker invoker, PerfCounterAttribute attribute)
                : base(invoker) {
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

            /// <summary>
            /// Executes the specified test.
            /// </summary>
            /// <param name="o">The test.</param>
            /// <param name="args">The arguments for the test.</param>
            /// <returns></returns>
            public override Object Execute(object o, System.Collections.IList args) {
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

                if (correctedValue > this.attribute.MaxValue) {
                    Assert.Fail("PerformanceCounter {0}, {1}, {2}, {3} value ({4}) was above maxium ({5})",
                        this.attribute.CategoryName,
                        this.attribute.CounterName,
                        counter.InstanceName,
                        counter.MachineName,
                        correctedValue,
                        this.attribute.MaxValue
                        );
                }

                if (correctedValue < this.attribute.MinValue) {
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
