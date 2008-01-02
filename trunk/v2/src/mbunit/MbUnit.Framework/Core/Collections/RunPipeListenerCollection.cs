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
	/// A collection of elements of type IRunPipeListener
	/// </summary>
	public sealed class RunPipeListenerCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the RunPipeListenerCollection class.
		/// </summary>
		public RunPipeListenerCollection()
		{}

		/// <summary>
		/// Adds an instance of type IRunPipeListener to the end of this RunPipeListenerCollection.
		/// </summary>
		/// <param name="value">
		/// The IRunPipeListener to be added to the end of this RunPipeListenerCollection.
		/// </param>
		public void Add(IRunPipeListener value)
		{
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic IRunPipeListener value is in this RunPipeListenerCollection.
		/// </summary>
		/// <param name="value">
		/// The IRunPipeListener value to locate in this RunPipeListenerCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this RunPipeListenerCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(IRunPipeListener value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific IRunPipeListener from this RunPipeListenerCollection.
		/// </summary>
		/// <param name="value">
		/// The IRunPipeListener value to remove from this RunPipeListenerCollection.
		/// </param>
		public void Remove(IRunPipeListener value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by RunPipeListenerCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(RunPipeListenerCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public IRunPipeListener Current
			{
				get
				{
					return (IRunPipeListener) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (IRunPipeListener) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this RunPipeListenerCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new RunPipeListenerCollection.Enumerator GetEnumerator()
		{
			return new RunPipeListenerCollection.Enumerator(this);
		}
	}
}
