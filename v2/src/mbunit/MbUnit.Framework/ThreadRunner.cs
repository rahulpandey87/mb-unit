using System;
using System.Threading;

namespace MbUnit.Framework
{
    /// <summary>
    /// Class that runs a (test) Winforms method on a new thread.
    /// </summary>
    public sealed class ThreadRunner
    {
        private Exception catchedException;
        private Delegate start;
        private Object[] args;
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadRunner"/> class.
        /// </summary>
        /// <param name="start">A <see cref="Delegate"/> for the method to run</param>
        /// <param name="args">The arguments for the method.</param>
        public ThreadRunner(Delegate start, params object[] args)
        {
            this.start = start;
            this.args = args;
        }

        /// <summary>
        /// Returns the exception that may have occurred when the deleagte was invoked.
        /// </summary>
        /// <value>The caught exception.</value>
        public Exception CatchedException
        {
            get { return this.catchedException; }
        }

        /// <summary>
        /// Dynamically invokes the method delegate, catching the exception if one occurs.
        /// </summary>
        /// <remarks>
        /// If an exception does occur on startup, it is stored in <see cref="CatchedException"/>
        /// </remarks>
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

        /// <summary>
        /// Runs the delegates on a new thread
        /// </summary>
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
