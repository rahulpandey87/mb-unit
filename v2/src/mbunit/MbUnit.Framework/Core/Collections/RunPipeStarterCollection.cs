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
using System.Collections;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Invokers;

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// A collection of elements of type RunPipeStarter
	/// </summary>
	public sealed class RunPipeStarterCollection: System.Collections.CollectionBase
	{
        private WeakReference fixture;

        /// <summary>
        /// Initializes a new empty instance of the RunPipeStarterCollection class.
		/// </summary>
        public RunPipeStarterCollection(Fixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            this.fixture = new WeakReference(fixture);
        }

        public Fixture Fixture
        {
            get
            {
                return this.fixture.Target as Fixture;
            }
        }

		/// <summary>
		/// Adds an instance of type RunPipeStarter to the end of this RunPipeStarterCollection.
		/// </summary>
		/// <param name="value">
		/// The RunPipeStarter to be added to the end of this RunPipeStarterCollection.
		/// </param>
		public void Add(RunPipeStarter value)
		{
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic RunPipeStarter value is in this RunPipeStarterCollection.
		/// </summary>
		/// <param name="value">
		/// The RunPipeStarter value to locate in this RunPipeStarterCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this RunPipeStarterCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(RunPipeStarter value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific RunPipeStarter from this RunPipeStarterCollection.
		/// </summary>
		/// <param name="value">
		/// The RunPipeStarter value to remove from this RunPipeStarterCollection.
		/// </param>
		public void Remove(RunPipeStarter value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by RunPipeStarterCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(RunPipeStarterCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public RunPipeStarter Current
			{
				get
				{
					return (RunPipeStarter) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (RunPipeStarter) (this.wrapped.Current);
				}
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this RunPipeStarterCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new RunPipeStarterCollection.Enumerator GetEnumerator()
		{
			return new RunPipeStarterCollection.Enumerator(this);
		}
	}
}
