using System;
using System.Diagnostics;
using MbUnit.Core.Exceptions;

namespace MbUnit.Core.Monitoring
{
    public sealed class DebugMonitor : IMonitor
    {
        private DefaultTraceListener defaultListener;
        private DebugMonitorTraceListener listener = new DebugMonitorTraceListener();

        public void Start()
        {
            // find default listnerer
            foreach (TraceListener tc in Debug.Listeners)
            {
                this.defaultListener = tc as DefaultTraceListener;
                if (defaultListener != null)
                    break;
            }

            // remove default listener
            if (this.defaultListener != null)
                Debug.Listeners.Remove(this.defaultListener);

            // adding custom
            if (Debug.Listeners.Count > 0)
            {
                if (Debug.Listeners[0].GetType() != typeof(DebugMonitorTraceListener))
                {
                    Debug.Listeners.Add(this.listener);
                }
            }
            else
            {
                Debug.Listeners.Add(this.listener);
            }
        }

        public void Stop()
        {
            if (this.defaultListener != null)
            {
                Debug.Listeners.Add(this.defaultListener);
                this.defaultListener = null;
            }
            Debug.Listeners.Remove(this.listener);
        }

        private sealed class DebugMonitorTraceListener : TraceListener
        {
            private DefaultTraceListener defaultListener;

            public DebugMonitorTraceListener()
            {
                foreach (TraceListener tc in Debug.Listeners)
                {
                    this.defaultListener = tc as DefaultTraceListener;
                    if (defaultListener != null)
                        break;
                }
            }

            private void WriteLine(string format, params object[] args)
            {
                Console.Error.WriteLine(format, args);
                if (this.defaultListener != null)
                    this.defaultListener.WriteLine(String.Format(format, args));
            }

            public override void Fail(string message)
            {
                this.WriteLine("Failure", message);
                throw new DebugFailException(message);
            }

            public override void Fail(string message, string messageDetail)
            {
                this.WriteLine("Failure", "{0}\n{1}",message,messageDetail);
                throw new DebugFailException(message, messageDetail);
            }

            public override void Write(string message)
            {
                Console.Error.Write(message);
                if (this.defaultListener != null)
                    this.defaultListener.Write(message);
            }

            public override void WriteLine(string message)
            {
                Console.Error.WriteLine(message);
                if (this.defaultListener != null)
                    this.defaultListener.WriteLine(message);
            }

            public override void Write(string message, string category)
            {
                Console.Error.Write("[{0}] {1}", category,message);
                if (this.defaultListener != null)
                    this.defaultListener.Write(message,category);
            }

            public override void WriteLine(string message, string category)
            {
                Console.Error.WriteLine("[{0}] {1}", category, message);
                if (this.defaultListener != null)
                    this.defaultListener.WriteLine(message, category);
            }
        }
    }
}
