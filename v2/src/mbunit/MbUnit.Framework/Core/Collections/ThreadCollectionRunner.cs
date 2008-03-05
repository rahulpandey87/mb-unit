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
using System.Threading;

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// Summary description for ThreadCollectionRunner.
	/// </summary>
	public sealed class ThreadCollectionRunner : IDisposable
	{
		private ThreadCollection threads = new ThreadCollection();

		public ThreadCollection Threads
		{
			get
			{
				return this.threads;
			}
		}
		
		public void StartAll()
		{
			foreach(Thread thread in this.Threads)
				thread.Start();
		}

		public void SuspendAll()
		{
			foreach(Thread thread in this.Threads)
				thread.Suspend();
		}

		public void ResumeAll()
		{
			foreach(Thread thread in this.Threads)
				thread.Resume();
		}

		public void AbortAll()
		{
			foreach(Thread thread in this.Threads)
				thread.Abort();
		}

		public bool AnyAlive
		{
			get
			{
                // Fix for http://www.mertner.com/jira/browse/MBUNIT-134 by Richard Louapre
                Thread.Sleep(10);
				foreach(Thread thread in this.Threads)
				{
					if (thread.IsAlive)
						return true;
				}
				return false;
			}
		}

		public void WaitForFinishing()
		{			
			bool alive = true;
			while (alive && this.Threads.Count>0)
			{
				alive=false;
                ThreadCollection removeList = new ThreadCollection();
				foreach(Thread thread in this.Threads)
				{
                    if ( thread.IsAlive )
                    {
                        alive = true;
                    }
                    else
                    {
                        removeList.Add( thread );
                    }
				}
                foreach ( Thread thread in removeList )
                {
                    this.Threads.Remove( thread );
                }
                // Fix for http://www.mertner.com/jira/browse/MBUNIT-134 by Richard Louapre
                Thread.Sleep(10);
			}
		}
		
		public void WaitForFinishingSafe()
		{			
			bool alive = true;
			while (alive && this.Threads.Count>0)
            {
                alive = false;
                ThreadCollection removeList = new ThreadCollection();
                foreach ( Thread thread in this.Threads )
                {
                    try
                    {
                        if ( thread.IsAlive )
                        {
                            alive = true;
                        }
                        else
                        {
                            removeList.Add( thread );
                        }
                    }
                    catch ( Exception )
                    { }
                }
                foreach ( Thread thread in removeList )
                {
                    try
                    {
                        this.Threads.Remove( thread );
                    }
                    catch ( Exception )
                    { }
                }
                // Fix for http://www.mertner.com/jira/browse/MBUNIT-134 by Richard Louapre
                Thread.Sleep( 10 );
            }
		}

		public void Dispose()
		{
            if (this.threads != null)
            {
                this.AbortAll();
                this.Threads.Clear();
                this.threads = null;
            }
        }
	}
}
