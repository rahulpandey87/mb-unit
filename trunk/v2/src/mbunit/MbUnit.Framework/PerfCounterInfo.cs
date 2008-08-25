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

namespace MbUnit.Framework
{
    /// <summary>
    /// Class containing static helper methods for retrieving performance counters.
    /// For use in conjunction with <see cref="PerfCounterAttribute"/>
    /// </summary>
    public class PerfCounterInfo
    {
        #region Constructors
        /// <summary>
        /// Private constructor for a new instance of the <see cref="PerfCounterInfo"/> class.
        /// </summary>
        private PerfCounterInfo() { }
        #endregion

        #region Categories
        #region .NET CLR Exceptions
        /// <summary>
        /// Runtime statistics on CLR exception handling.
        /// </summary>
        public sealed class NetClrExceptions
        {
            const string categoryName = @".NET CLR Exceptions";
            #region Constructors
            private NetClrExceptions() { }
            #endregion

            #region # of Exceps Thrown
            /// <summary>
            /// This counter displays the total number of exceptions thrown since the start of the application. These include both .NET exceptions and unmanaged exceptions that get converted into .NET exceptions e.g. null pointer reference exception in unmanaged code would get re-thrown in managed code as a .NET System.NullReferenceException; this counter includes both handled and unhandled exceptions. Exceptions that are re-thrown would get counted again. Exceptions should only occur in rare situations and not in the normal control flow of the program.
            /// </summary>			
            public sealed class NbofExcepsThrown
            {
                const string counterName = @"# of Exceps Thrown";
                private NbofExcepsThrown() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrExceptions.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of Exceps Thrown / sec
            /// <summary>
            /// This counter displays the number of exceptions thrown per second. These include both .NET exceptions and unmanaged exceptions that get converted into .NET exceptions e.g. null pointer reference exception in unmanaged code would get re-thrown in managed code as a .NET System.NullReferenceException; this counter includes both handled and unhandled exceptions. Exceptions should only occur in rare situations and not in the normal control flow of the program; this counter was designed as an indicator of potential performance problems due to large (>100s) rate of exceptions thrown. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class NbofExcepsThrownsec
            {
                const string counterName = @"# of Exceps Thrown / sec";
                private NbofExcepsThrownsec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrExceptions.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of Filters / sec
            /// <summary>
            /// This counter displays the number of .NET exception filters executed per second. An exception filter evaluates whether an exception should be handled or not. This counter tracks the rate of exception filters evaluated; irrespective of whether the exception was handled or not. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class NbofFilterssec
            {
                const string counterName = @"# of Filters / sec";
                private NbofFilterssec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrExceptions.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of Finallys / sec
            /// <summary>
            /// This counter displays the number of finally blocks executed per second. A finally block is guaranteed to be executed regardless of how the try block was exited. Only the finally blocks that are executed for an exception are counted; finally blocks on normal code paths are not counted by this counter. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class NbofFinallyssec
            {
                const string counterName = @"# of Finallys / sec";
                private NbofFinallyssec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrExceptions.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Throw To Catch Depth / sec
            /// <summary>
            /// This counter displays the number of stack frames traversed from the frame that threw the .NET exception to the frame that handled the exception per second. This counter resets to 0 when an exception handler is entered; so nested exceptions would show the handler to handler stack depth. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class ThrowToCatchDepthsec
            {
                const string counterName = @"Throw To Catch Depth / sec";
                private ThrowToCatchDepthsec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrExceptions.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Remoting
        /// <summary>
        /// Stats for CLR Remoting.
        /// </summary>
        public sealed class NetClrRemoting
        {
            const string categoryName = @".NET CLR Remoting";
            #region Constructors
            private NetClrRemoting() { }
            #endregion

            #region Remote Calls/sec
            /// <summary>
            /// This counter displays the number of remote procedure calls invoked per second. A remote procedure call is a call on any object outside the caller;s AppDomain. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class RemoteCallssec
            {
                const string counterName = @"Remote Calls/sec";
                private RemoteCallssec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrRemoting.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Total Remote Calls
            /// <summary>
            /// This counter displays the total number of remote procedure calls invoked since the start of this application. A remote procedure call is a call on any object outside the caller;s AppDomain.
            /// </summary>			
            public sealed class TotalRemoteCalls
            {
                const string counterName = @"Total Remote Calls";
                private TotalRemoteCalls() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrRemoting.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Channels
            /// <summary>
            /// This counter displays the total number of remoting channels registered across all AppDomains since the start of the application. Channels are used to transport messages to and from remote objects.
            /// </summary>			
            public sealed class Channels
            {
                const string counterName = @"Channels";
                private Channels() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrRemoting.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Context Proxies
            /// <summary>
            /// This counter displays the total number of remoting proxy objects created in this process since the start of the process. Proxy object acts as a representative of the remote objects and ensures that all calls made on the proxy are forwarded to the correct remote object instance.
            /// </summary>			
            public sealed class ContextProxies
            {
                const string counterName = @"Context Proxies";
                private ContextProxies() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrRemoting.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Context-Bound Classes Loaded
            /// <summary>
            /// This counter displays the current number of context-bound classes loaded. Classes that can be bound to a context are called context-bound classes; context-bound classes are marked with Context Attributes which provide usage rules for synchronization; thread affinity; transactions etc.
            /// </summary>			
            public sealed class ContextBoundClassesLoaded
            {
                const string counterName = @"Context-Bound Classes Loaded";
                private ContextBoundClassesLoaded() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrRemoting.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Context-Bound Objects Alloc / sec
            /// <summary>
            /// This counter displays the number of context-bound objects allocated per second. Instances of classes that can be bound to a context are called context-bound objects; context-bound classes are marked with Context Attributes which provide usage rules for synchronization; thread affinity; transactions etc. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class ContextBoundObjectsAllocsec
            {
                const string counterName = @"Context-Bound Objects Alloc / sec";
                private ContextBoundObjectsAllocsec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrRemoting.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Contexts
            /// <summary>
            /// This counter displays the current number of remoting contexts in the application. A context is a boundary containing a collection of objects with the same usage rules like synchronization; thread affinity; transactions etc.
            /// </summary>			
            public sealed class Contexts
            {
                const string counterName = @"Contexts";
                private Contexts() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }

                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrRemoting.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Networking
        /// <summary>
        /// Help not available.
        /// </summary>
        public sealed class NetClrNetworking
        {
            const string categoryName = @".NET CLR Networking";
            #region Constructors
            private NetClrNetworking() { }
            #endregion

            #region Connections Established
            /// <summary>
            /// The cumulative total number of socket connections established for this process since the process was started.
            /// </summary>			
            public sealed class ConnectionsEstablished
            {
                const string counterName = @"Connections Established";
                private ConnectionsEstablished() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrNetworking.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Bytes Received
            /// <summary>
            /// The cumulative total number of bytes received over all open socket connections since the process was started. This number includes data and any protocol information that is not defined by the TCP/IP protocol.
            /// </summary>			
            public sealed class BytesReceived
            {
                const string counterName = @"Bytes Received";
                private BytesReceived() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrNetworking.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Bytes Sent
            /// <summary>
            /// The cumulative total number of bytes sent over all open socket connections since the process was started. This number includes data and any protocol information that is not defined by the TCP/IP protocol.
            /// </summary>			
            public sealed class BytesSent
            {
                const string counterName = @"Bytes Sent";
                private BytesSent() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrNetworking.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Datagrams Received
            /// <summary>
            /// The cumulative total number of datagram packets received since the process was started.
            /// </summary>			
            public sealed class DatagramsReceived
            {
                const string counterName = @"Datagrams Received";
                private DatagramsReceived() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrNetworking.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Datagrams Sent
            /// <summary>
            /// The cumulative total number of datagram packets sent since the process was started.
            /// </summary>			
            public sealed class DatagramsSent
            {
                const string counterName = @"Datagrams Sent";
                private DatagramsSent() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrNetworking.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Memory
        /// <summary>
        /// Counters for CLR Garbage Collected heap.
        /// </summary>
        public sealed class NetClrMemory
        {
            const string categoryName = @".NET CLR Memory";
            #region Constructors
            private NetClrMemory() { }
            #endregion

            #region # Gen 0 Collections
            /// <summary>
            /// This counter displays the number of times the generation 0 objects (youngest; most recently allocated) are garbage collected (Gen 0 GC) since the start of the application. Gen 0 GC occurs when the available memory in generation 0 is not sufficient to satisfy an allocation request. This counter is incremented at the end of a Gen 0 GC. Higher generation GCs include all lower generation GCs. This counter is explicitly incremented when a higher generation (Gen 1 or Gen 2) GC occurs. _Global_ counter value is not accurate and should be ignored. This counter displays the last observed value.
            /// </summary>			
            public sealed class NbGen0Collections
            {
                const string counterName = @"# Gen 0 Collections";
                private NbGen0Collections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # Gen 1 Collections
            /// <summary>
            /// This counter displays the number of times the generation 1 objects are garbage collected since the start of the application. The counter is incremented at the end of a Gen 1 GC. Higher generation GCs include all lower generation GCs. This counter is explicitly incremented when a higher generation (Gen 2) GC occurs. _Global_ counter value is not accurate and should be ignored. This counter displays the last observed value.
            /// </summary>			
            public sealed class NbGen1Collections
            {
                const string counterName = @"# Gen 1 Collections";
                private NbGen1Collections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # Gen 2 Collections
            /// <summary>
            /// This counter displays the number of times the generation 2 objects (older) are garbage collected since the start of the application. The counter is incremented at the end of a Gen 2 GC (also called full GC). _Global_ counter value is not accurate and should be ignored. This counter displays the last observed value.
            /// </summary>			
            public sealed class NbGen2Collections
            {
                const string counterName = @"# Gen 2 Collections";
                private NbGen2Collections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Promoted Memory from Gen 0
            /// <summary>
            /// This counter displays the bytes of memory that survive garbage collection (GC) and are promoted from generation 0 to generation 1; objects that are promoted just because they are waiting to be finalized are not included in this counter. This counter displays the value observed at the end of the last GC; its not a cumulative counter.
            /// </summary>			
            public sealed class PromotedMemoryfromGen0
            {
                const string counterName = @"Promoted Memory from Gen 0";
                private PromotedMemoryfromGen0() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Promoted Memory from Gen 1
            /// <summary>
            /// This counter displays the bytes of memory that survive garbage collection (GC) and are promoted from generation 1 to generation 2; objects that are promoted just because they are waiting to be finalized are not included in this counter. This counter displays the value observed at the end of the last GC; its not a cumulative counter. This counter is reset to 0 if the last GC was a Gen 0 GC only.
            /// </summary>			
            public sealed class PromotedMemoryfromGen1
            {
                const string counterName = @"Promoted Memory from Gen 1";
                private PromotedMemoryfromGen1() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Gen 0 Promoted Bytes/Sec
            /// <summary>
            /// This counter displays the bytes per second that are promoted from generation 0 (youngest) to generation 1; objects that are promoted just because they are waiting to be finalized are not included in this counter. Memory is promoted when it survives a garbage collection. This counter was designed as an indicator of relatively long-lived objects being created per sec. This counter displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class Gen0PromotedBytesSec
            {
                const string counterName = @"Gen 0 Promoted Bytes/Sec";
                private Gen0PromotedBytesSec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Gen 1 Promoted Bytes/Sec
            /// <summary>
            /// This counter displays the bytes per second that are promoted from generation 1 to generation 2 (oldest); objects that are promoted just because they are waiting to be finalized are not included in this counter. Memory is promoted when it survives a garbage collection. Nothing is promoted from generation 2 since it is the oldest. This counter was designed as an indicator of very long-lived objects being created per sec. This counter displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class Gen1PromotedBytesSec
            {
                const string counterName = @"Gen 1 Promoted Bytes/Sec";
                private Gen1PromotedBytesSec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Promoted Finalization-Memory from Gen 0
            /// <summary>
            /// This counter displays the bytes of memory that are promoted from generation 0 to generation 1 just because they are waiting to be finalized. This counter displays the value observed at the end of the last GC; its not a cumulative counter.
            /// </summary>			
            public sealed class PromotedFinalizationMemoryfromGen0
            {
                const string counterName = @"Promoted Finalization-Memory from Gen 0";
                private PromotedFinalizationMemoryfromGen0() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Promoted Finalization-Memory from Gen 1
            /// <summary>
            /// This counter displays the bytes of memory that are promoted from generation 1 to generation 2 just because they are waiting to be finalized. This counter displays the value observed at the end of the last GC; its not a cumulative counter. This counter is reset to 0 if the last GC was a Gen 0 GC only.
            /// </summary>			
            public sealed class PromotedFinalizationMemoryfromGen1
            {
                const string counterName = @"Promoted Finalization-Memory from Gen 1";
                private PromotedFinalizationMemoryfromGen1() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Gen 0 heap size
            /// <summary>
            /// This counter displays the maximum bytes that can be allocated in generation 0 (Gen 0); its does not indicate the current number of bytes allocated in Gen 0. A Gen 0 GC is triggered when the allocations since the last GC exceed this size. The Gen 0 size is tuned by the Garbage Collector and can change during the execution of the application. At the end of a Gen 0 collection the size of the Gen 0 heap is infact 0 bytes; this counter displays the size (in bytes) of allocations that would trigger the next Gen 0 GC. This counter is updated at the end of a GC; its not updated on every allocation.
            /// </summary>			
            public sealed class Gen0heapsize
            {
                const string counterName = @"Gen 0 heap size";
                private Gen0heapsize() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Gen 1 heap size
            /// <summary>
            /// This counter displays the current number of bytes in generation 1 (Gen 1); this counter does not display the maximum size of Gen 1. Objects are not directly allocated in this generation; they are promoted from previous Gen 0 GCs. This counter is updated at the end of a GC; its not updated on every allocation.
            /// </summary>			
            public sealed class Gen1heapsize
            {
                const string counterName = @"Gen 1 heap size";
                private Gen1heapsize() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Gen 2 heap size
            /// <summary>
            /// This counter displays the current number of bytes in generation 2 (Gen 2). Objects are not directly allocated in this generation; they are promoted from Gen 1 during previous Gen 1 GCs. This counter is updated at the end of a GC; its not updated on every allocation.
            /// </summary>			
            public sealed class Gen2heapsize
            {
                const string counterName = @"Gen 2 heap size";
                private Gen2heapsize() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Large Object Heap size
            /// <summary>
            /// This counter displays the current size of the Large Object Heap in bytes. Objects greater than 20 KBytes are treated as large objects by the Garbage Collector and are directly allocated in a special heap; they are not promoted through the generations. This counter is updated at the end of a GC; its not updated on every allocation.
            /// </summary>			
            public sealed class LargeObjectHeapsize
            {
                const string counterName = @"Large Object Heap size";
                private LargeObjectHeapsize() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Finalization Survivors
            /// <summary>
            /// This counter displays the number of garbage collected objects that survive a collection because they are waiting to be finalized. If these objects hold references to other objects then those objects also survive but are not counted by this counter; the "Promoted Finalization-Memory from Gen 0" and "Promoted Finalization-Memory from Gen 1" counters represent all the memory that survived due to finalization. This counter is not a cumulative counter; its updated at the end of every GC with count of the survivors during that particular GC only. This counter was designed to indicate the extra overhead that the application might incur because of finalization.
            /// </summary>			
            public sealed class FinalizationSurvivors
            {
                const string counterName = @"Finalization Survivors";
                private FinalizationSurvivors() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # GC Handles
            /// <summary>
            /// This counter displays the current number of GC Handles in use. GCHandles are handles to resources external to the CLR and the managed environment. Handles occupy small amounts of memory in the GCHeap but potentially expensive unmanaged resources.
            /// </summary>			
            public sealed class NbGcHandles
            {
                const string counterName = @"# GC Handles";
                private NbGcHandles() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Allocated Bytes/sec
            /// <summary>
            /// This counter displays the rate of bytes per second allocated on the GC Heap. This counter is updated at the end of every GC; not at each allocation. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class AllocatedBytessec
            {
                const string counterName = @"Allocated Bytes/sec";
                private AllocatedBytessec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # Induced GC
            /// <summary>
            /// This counter displays the peak number of times a garbage collection was performed because of an explicit call to GC.Collect. Its a good practice to let the GC tune the frequency of its collections.
            /// </summary>			
            public sealed class NbInducedGc
            {
                const string counterName = @"# Induced GC";
                private NbInducedGc() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region % Time in GC
            /// <summary>
            /// % Time in GC is the percentage of elapsed time that was spent in performing a garbage collection (GC) since the last GC cycle. This counter is usually an indicator of the work done by the Garbage Collector on behalf of the application to collect and compact memory. This counter is updated only at the end of every GC and the counter value reflects the last observed value; its not an average.
            /// </summary>			
            public sealed class TimeinGc
            {
                const string counterName = @"% Time in GC";
                private TimeinGc() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Not Displayed
            /// <summary>
            /// Not Displayed.
            /// </summary>			
            public sealed class NotDisplayed
            {
                const string counterName = @"Not Displayed";
                private NotDisplayed() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # Bytes in all Heaps
            /// <summary>
            /// This counter is the sum of four other counters; Gen 0 Heap Size; Gen 1 Heap Size; Gen 2 Heap Size and the Large Object Heap Size. This counter indicates the current memory allocated in bytes on the GC Heaps.
            /// </summary>			
            public sealed class NbBytesinallHeaps
            {
                const string counterName = @"# Bytes in all Heaps";
                private NbBytesinallHeaps() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # Total committed Bytes
            /// <summary>
            /// This counter displays the amount of virtual memory (in bytes) currently committed by the Garbage Collector. (Committed memory is the physical memory for which space has been reserved on the disk paging file).
            /// </summary>			
            public sealed class NbTotalcommittedBytes
            {
                const string counterName = @"# Total committed Bytes";
                private NbTotalcommittedBytes() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # Total reserved Bytes
            /// <summary>
            /// This counter displays the amount of virtual memory (in bytes) currently reserved by the Garbage Collector. (Reserved memory is the virtual memory space reserved for the application but no disk or main memory pages have been used.)
            /// </summary>			
            public sealed class NbTotalreservedBytes
            {
                const string counterName = @"# Total reserved Bytes";
                private NbTotalreservedBytes() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of Pinned Objects
            /// <summary>
            /// This counter displays the number of pinned objects encountered in the last GC. This counter tracks the pinned objects only in the heaps that were garbage collected e.g. a Gen 0 GC would cause enumeration of pinned objects in the generation 0 heap only. A pinned object is one that the Garbage Collector cannot move in memory.
            /// </summary>			
            public sealed class NbofPinnedObjects
            {
                const string counterName = @"# of Pinned Objects";
                private NbofPinnedObjects() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of Sink Blocks in use
            /// <summary>
            /// This counter displays the current number of sync blocks in use. Sync blocks are per-object data structures allocated for storing synchronization information. Sync blocks hold weak references to managed objects and need to be scanned by the Garbage Collector. Sync blocks are not limited to storing synchronization information and can also store COM interop metadata. This counter was designed to indicate performance problems with heavy use of synchronization primitives.
            /// </summary>			
            public sealed class NbofSinkBlocksinuse
            {
                const string counterName = @"# of Sink Blocks in use";
                private NbofSinkBlocksinuse() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrMemory.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Interop
        /// <summary>
        /// Stats for CLR interop.
        /// </summary>
        public sealed class NetClrInterop
        {
            const string categoryName = @".NET CLR Interop";
            #region Constructors
            private NetClrInterop() { }
            #endregion

            #region # of CCWs
            /// <summary>
            /// This counter displays the current number of Com-Callable-Wrappers (CCWs). A CCW is a proxy for the .NET managed object being referenced from unmanaged COM client(s). This counter was designed to indicate the number of managed objects being referenced by unmanaged COM code.
            /// </summary>			
            public sealed class NbofCcws
            {
                const string counterName = @"# of CCWs";
                private NbofCcws() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrInterop.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of Stubs
            /// <summary>
            /// This counter displays the current number of stubs created by the CLR. Stubs are responsible for marshalling arguments and return values from managed to unmanaged code and vice versa; during a COM Interop call or PInvoke call.
            /// </summary>			
            public sealed class NbofStubs
            {
                const string counterName = @"# of Stubs";
                private NbofStubs() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrInterop.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of marshalling
            /// <summary>
            /// This counter displays the total number of times arguments and return values have been marshaled from managed to unmanaged code and vice versa since the start of the application. This counter is not incremented if the stubs are inlined. (Stubs are responsible for marshalling arguments and return values). Stubs usually get inlined if the marshalling overhead is small.
            /// </summary>			
            public sealed class Nbofmarshalling
            {
                const string counterName = @"# of marshalling";
                private Nbofmarshalling() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrInterop.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of TLB imports / sec
            /// <summary>
            /// Reserved for future use.
            /// </summary>			
            public sealed class NbofTlbimportssec
            {
                const string counterName = @"# of TLB imports / sec";
                private NbofTlbimportssec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrInterop.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of TLB exports / sec
            /// <summary>
            /// Reserved for future use.
            /// </summary>			
            public sealed class NbofTlbexportssec
            {
                const string counterName = @"# of TLB exports / sec";
                private NbofTlbexportssec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrInterop.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET Data Provider for SqlServer
        /// <summary>
        /// Counters for System.Data.SqlClient
        /// </summary>
        public sealed class NetDataProviderforSqlServer
        {
            const string categoryName = @".NET Data Provider for SqlServer";
            #region Constructors
            private NetDataProviderforSqlServer() { }
            #endregion

            #region HardConnectsPerSecond
            /// <summary>
            /// The number of actual connections per second that are being made to servers
            /// </summary>			
            public sealed class HardConnectsPerSecond
            {
                const string counterName = @"HardConnectsPerSecond";
                private HardConnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region HardDisconnectsPerSecond
            /// <summary>
            /// The number of actual disconnects per second that are being made to servers
            /// </summary>			
            public sealed class HardDisconnectsPerSecond
            {
                const string counterName = @"HardDisconnectsPerSecond";
                private HardDisconnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SoftConnectsPerSecond
            /// <summary>
            /// The number of connections we get from the pool per second
            /// </summary>			
            public sealed class SoftConnectsPerSecond
            {
                const string counterName = @"SoftConnectsPerSecond";
                private SoftConnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SoftDisconnectsPerSecond
            /// <summary>
            /// The number of connections we return to the pool per second
            /// </summary>			
            public sealed class SoftDisconnectsPerSecond
            {
                const string counterName = @"SoftDisconnectsPerSecond";
                private SoftDisconnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfNonPooledConnections
            /// <summary>
            /// The number of connections that are not using connection pooling
            /// </summary>			
            public sealed class NumberOfNonPooledConnections
            {
                const string counterName = @"NumberOfNonPooledConnections";
                private NumberOfNonPooledConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfPooledConnections
            /// <summary>
            /// The number of connections that are managed by the connection pooler
            /// </summary>			
            public sealed class NumberOfPooledConnections
            {
                const string counterName = @"NumberOfPooledConnections";
                private NumberOfPooledConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfActiveConnectionPoolGroups
            /// <summary>
            /// The number of unique connection strings
            /// </summary>			
            public sealed class NumberOfActiveConnectionPoolGroups
            {
                const string counterName = @"NumberOfActiveConnectionPoolGroups";
                private NumberOfActiveConnectionPoolGroups() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfInactiveConnectionPoolGroups
            /// <summary>
            /// The number of unique connection strings waiting for pruning
            /// </summary>			
            public sealed class NumberOfInactiveConnectionPoolGroups
            {
                const string counterName = @"NumberOfInactiveConnectionPoolGroups";
                private NumberOfInactiveConnectionPoolGroups() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfActiveConnectionPools
            /// <summary>
            /// The number of connection pools
            /// </summary>			
            public sealed class NumberOfActiveConnectionPools
            {
                const string counterName = @"NumberOfActiveConnectionPools";
                private NumberOfActiveConnectionPools() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfInactiveConnectionPools
            /// <summary>
            /// The number of connection pools
            /// </summary>			
            public sealed class NumberOfInactiveConnectionPools
            {
                const string counterName = @"NumberOfInactiveConnectionPools";
                private NumberOfInactiveConnectionPools() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfActiveConnections
            /// <summary>
            /// The number of connections currently in-use
            /// </summary>			
            public sealed class NumberOfActiveConnections
            {
                const string counterName = @"NumberOfActiveConnections";
                private NumberOfActiveConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfFreeConnections
            /// <summary>
            /// The number of connections currently available for use
            /// </summary>			
            public sealed class NumberOfFreeConnections
            {
                const string counterName = @"NumberOfFreeConnections";
                private NumberOfFreeConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfStasisConnections
            /// <summary>
            /// The number of connections currently waiting to be made ready for use
            /// </summary>			
            public sealed class NumberOfStasisConnections
            {
                const string counterName = @"NumberOfStasisConnections";
                private NumberOfStasisConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfReclaimedConnections
            /// <summary>
            /// The number of connections we reclaim from GCed from external connections
            /// </summary>			
            public sealed class NumberOfReclaimedConnections
            {
                const string counterName = @"NumberOfReclaimedConnections";
                private NumberOfReclaimedConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforSqlServer.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Data
        /// <summary>
        /// .Net CLR Data
        /// </summary>
        public sealed class NetClrData
        {
            const string categoryName = @".NET CLR Data";
            #region Constructors
            private NetClrData() { }
            #endregion

            #region SqlClient: Current # pooled and nonpooled connections
            /// <summary>
            /// Current number of connections, pooled or not.
            /// </summary>			
            public sealed class SqlClientCurrentNbpooledandnonpooledconnections
            {
                const string counterName = @"SqlClient: Current # pooled and nonpooled connections";
                private SqlClientCurrentNbpooledandnonpooledconnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetClrData.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SqlClient: Current # pooled connections
            /// <summary>
            /// Current number of connections in all pools associated with the process.
            /// </summary>			
            public sealed class SqlClientCurrentNbpooledconnections
            {
                const string counterName = @"SqlClient: Current # pooled connections";
                private SqlClientCurrentNbpooledconnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetClrData.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SqlClient: Current # connection pools
            /// <summary>
            /// Current number of pools associated with the process.
            /// </summary>			
            public sealed class SqlClientCurrentNbconnectionpools
            {
                const string counterName = @"SqlClient: Current # connection pools";
                private SqlClientCurrentNbconnectionpools() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetClrData.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SqlClient: Peak # pooled connections
            /// <summary>
            /// The highest number of connections in all pools since the process started.
            /// </summary>			
            public sealed class SqlClientPeakNbpooledconnections
            {
                const string counterName = @"SqlClient: Peak # pooled connections";
                private SqlClientPeakNbpooledconnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetClrData.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SqlClient: Total # failed connects
            /// <summary>
            /// The total number of connection open attempts that have failed for any reason.
            /// </summary>			
            public sealed class SqlClientTotalNbfailedconnects
            {
                const string counterName = @"SqlClient: Total # failed connects";
                private SqlClientTotalNbfailedconnects() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetClrData.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SqlClient: Total # failed commands
            /// <summary>
            /// The total number of command executes that have failed for any reason.
            /// </summary>			
            public sealed class SqlClientTotalNbfailedcommands
            {
                const string counterName = @"SqlClient: Total # failed commands";
                private SqlClientTotalNbfailedcommands() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetClrData.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Loading
        /// <summary>
        /// Statistics for CLR Class Loader.
        /// </summary>
        public sealed class NetClrLoading
        {
            const string categoryName = @".NET CLR Loading";
            #region Constructors
            private NetClrLoading() { }
            #endregion

            #region Current Classes Loaded
            /// <summary>
            /// This counter displays the current number of classes loaded in all Assemblies.
            /// </summary>			
            public sealed class CurrentClassesLoaded
            {
                const string counterName = @"Current Classes Loaded";
                private CurrentClassesLoaded() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Total Classes Loaded
            /// <summary>
            /// This counter displays the cumulative number of classes loaded in all Assemblies since the start of this application.
            /// </summary>			
            public sealed class TotalClassesLoaded
            {
                const string counterName = @"Total Classes Loaded";
                private TotalClassesLoaded() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Rate of Classes Loaded
            /// <summary>
            /// This counter displays the number of classes loaded per second in all Assemblies. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class RateofClassesLoaded
            {
                const string counterName = @"Rate of Classes Loaded";
                private RateofClassesLoaded() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Current appdomains
            /// <summary>
            /// This counter displays the current number of AppDomains loaded in this application. AppDomains (application domains) provide a secure and versatile unit of processing that the CLR can use to provide isolation between applications running in the same process.
            /// </summary>			
            public sealed class Currentappdomains
            {
                const string counterName = @"Current appdomains";
                private Currentappdomains() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Total Appdomains
            /// <summary>
            /// This counter displays the peak number of AppDomains loaded since the start of this application. AppDomains (application domains) provide a secure and versatile unit of processing that the CLR can use to provide isolation between applications running in the same process.
            /// </summary>			
            public sealed class TotalAppdomains
            {
                const string counterName = @"Total Appdomains";
                private TotalAppdomains() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Rate of appdomains
            /// <summary>
            /// This counter displays the number of AppDomains loaded per second. AppDomains (application domains) provide a secure and versatile unit of processing that the CLR can use to provide isolation between applications running in the same process. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class Rateofappdomains
            {
                const string counterName = @"Rate of appdomains";
                private Rateofappdomains() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Current Assemblies
            /// <summary>
            /// This counter displays the current number of Assemblies loaded across all AppDomains in this application. If the Assembly is loaded as domain-neutral from multiple AppDomains then this counter is incremented once only. Assemblies can be loaded as domain-neutral when their code can be shared by all AppDomains or they can be loaded as domain-specific when their code is private to the AppDomain.
            /// </summary>			
            public sealed class CurrentAssemblies
            {
                const string counterName = @"Current Assemblies";
                private CurrentAssemblies() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Total Assemblies
            /// <summary>
            /// This counter displays the total number of Assemblies loaded since the start of this application. If the Assembly is loaded as domain-neutral from multiple AppDomains then this counter is incremented once only. Assemblies can be loaded as domain-neutral when their code can be shared by all AppDomains or they can be loaded as domain-specific when their code is private to the AppDomain.
            /// </summary>			
            public sealed class TotalAssemblies
            {
                const string counterName = @"Total Assemblies";
                private TotalAssemblies() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Rate of Assemblies
            /// <summary>
            /// This counter displays the number of Assemblies loaded across all AppDomains per second. If the Assembly is loaded as domain-neutral from multiple AppDomains then this counter is incremented once only. Assemblies can be loaded as domain-neutral when their code can be shared by all AppDomains or they can be loaded as domain-specific when their code is private to the AppDomain. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class RateofAssemblies
            {
                const string counterName = @"Rate of Assemblies";
                private RateofAssemblies() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region % Time Loading
            /// <summary>
            /// Reserved for future use.
            /// </summary>			
            public sealed class TimeLoading
            {
                const string counterName = @"% Time Loading";
                private TimeLoading() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Assembly Search Length
            /// <summary>
            /// Reserved for future use.
            /// </summary>			
            public sealed class AssemblySearchLength
            {
                const string counterName = @"Assembly Search Length";
                private AssemblySearchLength() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Total # of Load Failures
            /// <summary>
            /// This counter displays the peak number of classes that have failed to load since the start of the application. These load failures could be due to many reasons like inadequate security or illegal format. Full details can be found in the profiling services help.
            /// </summary>			
            public sealed class TotalNbofLoadFailures
            {
                const string counterName = @"Total # of Load Failures";
                private TotalNbofLoadFailures() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Rate of Load Failures
            /// <summary>
            /// This counter displays the number of classes that failed to load per second. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval. These load failures could be due to many reasons like inadequate security or illegal format. Full details can be found in the profiling services help.
            /// </summary>			
            public sealed class RateofLoadFailures
            {
                const string counterName = @"Rate of Load Failures";
                private RateofLoadFailures() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Bytes in Loader Heap
            /// <summary>
            /// This counter displays the current size (in bytes) of the memory committed by the class loader across all AppDomains. (Committed memory is the physical memory for which space has been reserved on the disk paging file.)
            /// </summary>			
            public sealed class BytesinLoaderHeap
            {
                const string counterName = @"Bytes in Loader Heap";
                private BytesinLoaderHeap() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Total appdomains unloaded
            /// <summary>
            /// This counter displays the total number of AppDomains unloaded since the start of the application. If an AppDomain is loaded and unloaded multiple times this counter would count each of those unloads as separate.
            /// </summary>			
            public sealed class Totalappdomainsunloaded
            {
                const string counterName = @"Total appdomains unloaded";
                private Totalappdomainsunloaded() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Rate of appdomains unloaded
            /// <summary>
            /// This counter displays the number of AppDomains unloaded per second. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class Rateofappdomainsunloaded
            {
                const string counterName = @"Rate of appdomains unloaded";
                private Rateofappdomainsunloaded() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLoading.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Security
        /// <summary>
        /// Stats for CLR Security.
        /// </summary>
        public sealed class NetClrSecurity
        {
            const string categoryName = @".NET CLR Security";
            #region Constructors
            private NetClrSecurity() { }
            #endregion

            #region Total Runtime Checks
            /// <summary>
            /// This counter displays the total number of runtime Code Access Security (CAS) checks performed since the start of the application. Runtime CAS checks are performed when a caller makes a call to a callee demanding a particular permission; the runtime check is made on every call by the caller; the check is done by examining the current thread stack of the caller. This counter used together with "Stack Walk Depth" is indicative of performance penalty for security checks.
            /// </summary>			
            public sealed class TotalRuntimeChecks
            {
                const string counterName = @"Total Runtime Checks";
                private TotalRuntimeChecks() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrSecurity.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region % Time Sig. Authenticating
            /// <summary>
            /// Reserved for future use.
            /// </summary>			
            public sealed class TimeSigAuthenticating
            {
                const string counterName = @"% Time Sig. Authenticating";
                private TimeSigAuthenticating() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrSecurity.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # Link Time Checks
            /// <summary>
            /// This counter displays the total number of linktime Code Access Security (CAS) checks since the start of the application. Linktime CAS checks are performed when a caller makes a call to a callee demanding a particular permission at JIT compile time; linktime check is performed once per caller. This count is not indicative of serious performance issues; its indicative of the security system activity.
            /// </summary>			
            public sealed class NbLinkTimeChecks
            {
                const string counterName = @"# Link Time Checks";
                private NbLinkTimeChecks() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrSecurity.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region % Time in RT checks
            /// <summary>
            /// This counter displays the percentage of elapsed time spent in performing runtime Code Access Security (CAS) checks since the last such check. CAS allows code to be trusted to varying degrees and enforces these varying levels of trust depending on code identity. This counter is updated at the end of a runtime security check; it represents the last observed value; its not an average.
            /// </summary>			
            public sealed class TimeinRtchecks
            {
                const string counterName = @"% Time in RT checks";
                private TimeinRtchecks() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrSecurity.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Not Displayed
            /// <summary>
            /// Not Displayed.
            /// </summary>			
            public sealed class NotDisplayed
            {
                const string counterName = @"Not Displayed";
                private NotDisplayed() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrSecurity.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Stack Walk Depth
            /// <summary>
            /// This counter displays the depth of the stack during that last runtime Code Access Security check. Runtime Code Access Security check is performed by crawling the stack. This counter is not an average; it just displays the last observed value.
            /// </summary>			
            public sealed class StackWalkDepth
            {
                const string counterName = @"Stack Walk Depth";
                private StackWalkDepth() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrSecurity.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR Jit
        /// <summary>
        /// Stats for CLR Jit.
        /// </summary>
        public sealed class NetClrJit
        {
            const string categoryName = @".NET CLR Jit";
            #region Constructors
            private NetClrJit() { }
            #endregion

            #region # of Methods Jitted
            /// <summary>
            /// This counter displays the total number of methods compiled Just-In-Time (JIT) by the CLR JIT compiler since the start of the application. This counter does not include the pre-jitted methods.
            /// </summary>			
            public sealed class NbofMethodsJitted
            {
                const string counterName = @"# of Methods Jitted";
                private NbofMethodsJitted() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrJit.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of IL Bytes Jitted
            /// <summary>
            /// This counter displays the total IL bytes jitted since the start of the application. This counter is exactly equivalent to the "Total # of IL Bytes Jitted" counter.
            /// </summary>			
            public sealed class NbofIlBytesJitted
            {
                const string counterName = @"# of IL Bytes Jitted";
                private NbofIlBytesJitted() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrJit.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Total # of IL Bytes Jitted
            /// <summary>
            /// This counter displays the total IL bytes jitted since the start of the application. This counter is exactly equivalent to the "# of IL Bytes Jitted" counter.
            /// </summary>			
            public sealed class TotalNbofIlBytesJitted
            {
                const string counterName = @"Total # of IL Bytes Jitted";
                private TotalNbofIlBytesJitted() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrJit.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region IL Bytes Jitted / sec
            /// <summary>
            /// This counter displays the rate at which IL bytes are jitted per second. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class IlBytesJittedsec
            {
                const string counterName = @"IL Bytes Jitted / sec";
                private IlBytesJittedsec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrJit.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Standard Jit Failures
            /// <summary>
            /// This counter displays the peak number of methods the JIT compiler has failed to JIT since the start of the application. This failure can occur if the IL cannot be verified or if there was an internal error in the JIT compiler.
            /// </summary>			
            public sealed class StandardJitFailures
            {
                const string counterName = @"Standard Jit Failures";
                private StandardJitFailures() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrJit.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region % Time in Jit
            /// <summary>
            /// This counter displays the percentage of elapsed time spent in JIT compilation since the last JIT compilation phase. This counter is updated at the end of every JIT compilation phase. A JIT compilation phase is the phase when a method and its dependencies are being compiled.
            /// </summary>			
            public sealed class TimeinJit
            {
                const string counterName = @"% Time in Jit";
                private TimeinJit() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrJit.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Not Displayed
            /// <summary>
            /// Not Displayed.
            /// </summary>			
            public sealed class NotDisplayed
            {
                const string counterName = @"Not Displayed";
                private NotDisplayed() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrJit.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET CLR LocksAndThreads
        /// <summary>
        /// Stats for CLR Locks and Threads.
        /// </summary>
        public sealed class NetClrLocksAndThreads
        {
            const string categoryName = @".NET CLR LocksAndThreads";
            #region Constructors
            private NetClrLocksAndThreads() { }
            #endregion

            #region Total # of Contentions
            /// <summary>
            /// This counter displays the total number of times threads in the CLR have attempted to acquire a managed lock unsuccessfully. Managed locks can be acquired in many ways; by the "lock" statement in C# or by calling System.Monitor.Enter or by using MethodImplOptions.Synchronized custom attribute.
            /// </summary>			
            public sealed class TotalNbofContentions
            {
                const string counterName = @"Total # of Contentions";
                private TotalNbofContentions() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Contention Rate / sec
            /// <summary>
            /// Rate at which threads in the runtime attempt to acquire a managed lock unsuccessfully. Managed locks can be acquired in many ways; by the "lock" statement in C# or by calling System.Monitor.Enter or by using MethodImplOptions.Synchronized custom attribute.
            /// </summary>			
            public sealed class ContentionRatesec
            {
                const string counterName = @"Contention Rate / sec";
                private ContentionRatesec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Current Queue Length
            /// <summary>
            /// This counter displays the total number of threads currently waiting to acquire some managed lock in the application. This counter is not an average over time; it displays the last observed value.
            /// </summary>			
            public sealed class CurrentQueueLength
            {
                const string counterName = @"Current Queue Length";
                private CurrentQueueLength() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Queue Length Peak
            /// <summary>
            /// This counter displays the total number of threads that waited to acquire some managed lock since the start of the application.
            /// </summary>			
            public sealed class QueueLengthPeak
            {
                const string counterName = @"Queue Length Peak";
                private QueueLengthPeak() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region Queue Length / sec
            /// <summary>
            /// This counter displays the number of threads per second waiting to acquire some lock in the application. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class QueueLengthsec
            {
                const string counterName = @"Queue Length / sec";
                private QueueLengthsec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of current logical Threads
            /// <summary>
            /// This counter displays the number of current .NET thread objects in the application. A .NET thread object is created either by new System.Threading.Thread or when an unmanaged thread enters the managed environment. This counters maintains the count of both running and stopped threads. This counter is not an average over time; it just displays the last observed value.
            /// </summary>			
            public sealed class NbofcurrentlogicalThreads
            {
                const string counterName = @"# of current logical Threads";
                private NbofcurrentlogicalThreads() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of current physical Threads
            /// <summary>
            /// This counter displays the number of native OS threads created and owned by the CLR to act as underlying threads for .NET thread objects. This counters value does not include the threads used by the CLR in its internal operations; it is a subset of the threads in the OS process.
            /// </summary>			
            public sealed class NbofcurrentphysicalThreads
            {
                const string counterName = @"# of current physical Threads";
                private NbofcurrentphysicalThreads() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of current recognized threads
            /// <summary>
            /// This counter displays the number of threads that are currently recognized by the CLR; they have a corresponding .NET thread object associated with them. These threads are not created by the CLR; they are created outside the CLR but have since run inside the CLR at least once. Only unique threads are tracked; threads with same thread ID re-entering the CLR or recreated after thread exit are not counted twice.
            /// </summary>			
            public sealed class Nbofcurrentrecognizedthreads
            {
                const string counterName = @"# of current recognized threads";
                private Nbofcurrentrecognizedthreads() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region # of total recognized threads
            /// <summary>
            /// This counter displays the total number of threads that have been recognized by the CLR since the start of this application; these threads have a corresponding .NET thread object associated with them. These threads are not created by the CLR; they are created outside the CLR but have since run inside the CLR at least once. Only unique threads are tracked; threads with same thread ID re-entering the CLR or recreated after thread exit are not counted twice.
            /// </summary>			
            public sealed class Nboftotalrecognizedthreads
            {
                const string counterName = @"# of total recognized threads";
                private Nboftotalrecognizedthreads() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
            #region rate of recognized threads / sec
            /// <summary>
            /// This counter displays the number of threads per second that have been recognized by the CLR; these threads have a corresponding .NET thread object associated with them. These threads are not created by the CLR; they are created outside the CLR but have since run inside the CLR at least once. Only unique threads are tracked; threads with same thread ID re-entering the CLR or recreated after thread exit are not counted twice. This counter is not an average over time; it displays the difference between the values observed in the last two samples divided by the duration of the sample interval.
            /// </summary>			
            public sealed class rateofrecognizedthreadssec
            {
                const string counterName = @"rate of recognized threads / sec";
                private rateofrecognizedthreadssec() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the current instance.
                /// </returns>
                public static float NextValue()
                {
                    return NextValue(Process.GetCurrentProcess().ProcessName);
                }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <param name="instanceName">Name of the counter instance to query.</param>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>
                /// for the named instance.
                /// </returns>
                public static float NextValue(string instanceName)
                {
                    return PerfCounterInfo.NextValue(
                        NetClrLocksAndThreads.categoryName,
                        counterName,
                        instanceName
                        );
                }
            }
            #endregion
        }
        #endregion


        #region .NET Data Provider for Oracle
        /// <summary>
        /// Counters for System.Data.OracleClient
        /// </summary>
        public sealed class NetDataProviderforOracle
        {
            const string categoryName = @".NET Data Provider for Oracle";
            #region Constructors
            private NetDataProviderforOracle() { }
            #endregion

            #region HardConnectsPerSecond
            /// <summary>
            /// The number of actual connections per second that are being made to servers
            /// </summary>			
            public sealed class HardConnectsPerSecond
            {
                const string counterName = @"HardConnectsPerSecond";
                private HardConnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region HardDisconnectsPerSecond
            /// <summary>
            /// The number of actual disconnects per second that are being made to servers
            /// </summary>			
            public sealed class HardDisconnectsPerSecond
            {
                const string counterName = @"HardDisconnectsPerSecond";
                private HardDisconnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SoftConnectsPerSecond
            /// <summary>
            /// The number of connections we get from the pool per second
            /// </summary>			
            public sealed class SoftConnectsPerSecond
            {
                const string counterName = @"SoftConnectsPerSecond";
                private SoftConnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region SoftDisconnectsPerSecond
            /// <summary>
            /// The number of connections we return to the pool per second
            /// </summary>			
            public sealed class SoftDisconnectsPerSecond
            {
                const string counterName = @"SoftDisconnectsPerSecond";
                private SoftDisconnectsPerSecond() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfNonPooledConnections
            /// <summary>
            /// The number of connections that are not using connection pooling
            /// </summary>			
            public sealed class NumberOfNonPooledConnections
            {
                const string counterName = @"NumberOfNonPooledConnections";
                private NumberOfNonPooledConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfPooledConnections
            /// <summary>
            /// The number of connections that are managed by the connection pooler
            /// </summary>			
            public sealed class NumberOfPooledConnections
            {
                const string counterName = @"NumberOfPooledConnections";
                private NumberOfPooledConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfActiveConnectionPoolGroups
            /// <summary>
            /// The number of unique connection strings
            /// </summary>			
            public sealed class NumberOfActiveConnectionPoolGroups
            {
                const string counterName = @"NumberOfActiveConnectionPoolGroups";
                private NumberOfActiveConnectionPoolGroups() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfInactiveConnectionPoolGroups
            /// <summary>
            /// The number of unique connection strings waiting for pruning
            /// </summary>			
            public sealed class NumberOfInactiveConnectionPoolGroups
            {
                const string counterName = @"NumberOfInactiveConnectionPoolGroups";
                private NumberOfInactiveConnectionPoolGroups() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfActiveConnectionPools
            /// <summary>
            /// The number of connection pools
            /// </summary>			
            public sealed class NumberOfActiveConnectionPools
            {
                const string counterName = @"NumberOfActiveConnectionPools";
                private NumberOfActiveConnectionPools() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfInactiveConnectionPools
            /// <summary>
            /// The number of connection pools
            /// </summary>			
            public sealed class NumberOfInactiveConnectionPools
            {
                const string counterName = @"NumberOfInactiveConnectionPools";
                private NumberOfInactiveConnectionPools() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfActiveConnections
            /// <summary>
            /// The number of connections currently in-use
            /// </summary>			
            public sealed class NumberOfActiveConnections
            {
                const string counterName = @"NumberOfActiveConnections";
                private NumberOfActiveConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfFreeConnections
            /// <summary>
            /// The number of connections currently available for use
            /// </summary>			
            public sealed class NumberOfFreeConnections
            {
                const string counterName = @"NumberOfFreeConnections";
                private NumberOfFreeConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfStasisConnections
            /// <summary>
            /// The number of connections currently waiting to be made ready for use
            /// </summary>			
            public sealed class NumberOfStasisConnections
            {
                const string counterName = @"NumberOfStasisConnections";
                private NumberOfStasisConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
            #region NumberOfReclaimedConnections
            /// <summary>
            /// The number of connections we reclaim from GCed from external connections
            /// </summary>			
            public sealed class NumberOfReclaimedConnections
            {
                const string counterName = @"NumberOfReclaimedConnections";
                private NumberOfReclaimedConnections() { }
                /// <summary>
                /// Gets the value of the <see cref="PerformanceCounter"/>.
                /// </summary>
                /// <returns>
                /// Value returned by <see cref="PerformanceCounter.NextValue"/>.
                /// </returns>
                public static float NextValue()
                {
                    return PerfCounterInfo.NextValue(
                            NetDataProviderforOracle.categoryName,
                            counterName,
                            null
                            );
                }
            }
            #endregion
        }
        #endregion


        #endregion

        /// <summary>
        /// Gets the value of the <see cref="PerformanceCounter"/>.
        /// </summary>
        /// <param name="categoryName">Name of the performance counter category.</param>
        /// <param name="counterName">Name of the performance counter to query.</param>
        /// <param name="instanceName">Name of the counter instance to query.</param>
        /// <returns>
        /// Value returned by <see cref="PerformanceCounter.NextValue"/>
        /// for the named instance.
        /// </returns>
        public static float NextValue(
            string categoryName,
            string counterName,
            string instanceName
            )
        {
            using (PerformanceCounter counter = new PerformanceCounter(
                categoryName,
                counterName,
                instanceName,
                true)
                )
            {
                return counter.NextValue();
            }
        }
    }
}

