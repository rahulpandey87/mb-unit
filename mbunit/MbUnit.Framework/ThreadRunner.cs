using System;
using System.Threading;

namespace MbUnit.Framework
{
    public sealed class ThreadRunner
    {
        private Exception catchedException;
        private Delegate start;
        private Object[] args;
        public ThreadRunner(Delegate start, params object[] args)
        {
            this.start = start;
            this.args = args;
        }

        public Exception CatchedException
        {
            get { return this.catchedException; }
        }

        private void GuardedStart()
        {
            try
            {
                this.start.DynamicInvoke(this.args);
            }
            catch (Exception ex)
            {
                this.catchedException = ex;
            }
        }

        public void Run()
        {
            Thread thread = new Thread(new ThreadStart(this.GuardedStart));
            thread.Start();
            while (thread.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(0);
                System.Windows.Forms.Application.DoEvents();
            }
        }
    }
}
