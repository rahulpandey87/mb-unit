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
using System.Reflection;
using System.Collections;
using System.Threading;

namespace MbUnit.Core.Invokers 
{
	using MbUnit.Core.Collections;
	
	public sealed class ThreadedRepeatRunInvoker : DecoratorRunInvoker
	{
		private int count;
		
		public ThreadedRepeatRunInvoker(IRunInvoker invoker, int count) 
		:base(invoker)
		{
			if (count<1)
				throw new ArgumentException("count must be greate or equal than 1");
			this.count = count;
		}
		
		public int Count
		{
			get
			{
				return this.count;
			}
		}
		
		public override Object Execute(Object o, IList args)
		{			
			// the runner will automatically kill the thread on disposal
			using(ThreadCollectionRunner runner = new ThreadCollectionRunner())
			{			
				// create threads
				ArrayList starters = new ArrayList();
				for(int i = 0;i<this.Count;++i)
				{
					// run test
					ThreadedRunInvokerStarter starter = new ThreadedRunInvokerStarter(
						this.Invoker,
						o,
						args
						);
					starters.Add(starter);
					runner.Threads.Add(new ThreadStart(starter.Run));
				}
			
				// launch
				runner.StartAll();
				runner.WaitForFinishingSafe();

                // Now that we are done running, check to see if any exceptions were thrown.
                // NOTE: This only reports the first thread that failed.
                foreach ( ThreadedRunInvokerStarter starter in starters )
                {
                    if ( starter.HasThrown )
                    {
                        throw new Exception( "Runner throwed.", starter.Exception );
                    }
                }
			}

			return null;
        }

        #region Starter
        /// <summary>
        /// Functor class that lanches an invoker execution.
        /// </summary>
        /// <remarks>
        /// You can use this method to launch <see cref="IRunInvoker"/> execution
        /// in separate threads.
        /// </remarks>
        private sealed class ThreadedRunInvokerStarter
        {
            private IRunInvoker invoker;
            private Object o;
            private IList args;
            private Exception exception = null;

            /// <summary>
            /// Constructs a execute functor
            /// </summary>
            /// <param name="invoker">invoker to execute</param>
            /// <param name="o"><see cref="IRunInvoker"/>.Execute arguments</param>
            /// <param name="args"><see cref="IRunInvoker"/>.Execute arguments</param>
            public ThreadedRunInvokerStarter(
                IRunInvoker invoker,
                Object o,
                IList args
                )
            {
                this.invoker = invoker;
                this.o = o;
                this.args = args;
            }

            public bool HasThrown
            {
                get
                {
                    return this.exception != null;
                }
            }

            public Exception Exception
            {
                get
                {
                    return this.exception;
                }
            }

            /// <summary>Launches the invoker execution</summary>
            /// <remarks></remarks>
            public void Run()
            {
                this.exception = null;

                try
                {
                    this.invoker.Execute(this.o, this.args);
                }
                catch (Exception ex)
                {
                    this.exception = ex;
                }
                finally
                {
                    Console.Out.Flush();
                    Console.Error.Flush();
                }
            }
        }
        #endregion
    }
}
