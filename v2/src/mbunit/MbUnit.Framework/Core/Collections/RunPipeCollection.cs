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

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// A collection of elements of type RunPipe
	/// </summary>
	public sealed class RunPipeCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the RunPipeCollection class.
		/// </summary>
		public RunPipeCollection()
		{}

		/// <summary>
		/// Adds an instance of type RunPipe to the end of this RunPipeCollection.
		/// </summary>
		/// <param name="value">
		/// The RunPipe to be added to the end of this RunPipeCollection.
		/// </param>
		public void Add(RunPipe value)
		{
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic RunPipe value is in this RunPipeCollection.
		/// </summary>
		/// <param name="value">
		/// The RunPipe value to locate in this RunPipeCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this RunPipeCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(RunPipe value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Gets or sets the RunPipe at the given index in this RunPipeCollection.
		/// </summary>
		public RunPipe this[int index]
		{
			get
			{
				return (RunPipe) this.List[index];
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific RunPipe from this RunPipeCollection.
		/// </summary>
		/// <param name="value">
		/// The RunPipe value to remove from this RunPipeCollection.
		/// </param>
		public void Remove(RunPipe value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by RunPipeCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(RunPipeCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public RunPipe Current
			{
				get
				{
					return (RunPipe) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (RunPipe) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this RunPipeCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new RunPipeCollection.Enumerator GetEnumerator()
		{
			return new RunPipeCollection.Enumerator(this);
		}
	}

}
