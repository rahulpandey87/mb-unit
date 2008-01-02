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
	/// A collection of elements of type IFixtureFactory
	/// </summary>
	public sealed class FixtureFactoryCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the FixtureFactoryCollection class.
		/// </summary>
		public FixtureFactoryCollection()
		{}

		/// <summary>
		/// Adds an instance of type IFixtureFactory to the end of this FixtureFactoryCollection.
		/// </summary>
		/// <param name="value">
		/// The IFixtureFactory to be added to the end of this FixtureFactoryCollection.
		/// </param>
		public void Add(IFixtureFactory value)
		{
			if (value==null)
				throw new ArgumentNullException("value");
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic IFixtureFactory value is in this FixtureFactoryCollection.
		/// </summary>
		/// <param name="value">
		/// The IFixtureFactory value to locate in this FixtureFactoryCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this FixtureFactoryCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(IFixtureFactory value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific IFixtureFactory from this FixtureFactoryCollection.
		/// </summary>
		/// <param name="value">
		/// The IFixtureFactory value to remove from this FixtureFactoryCollection.
		/// </param>
		public void Remove(IFixtureFactory value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by FixtureFactoryCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(FixtureFactoryCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public IFixtureFactory Current
			{
				get
				{
					return (IFixtureFactory) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (IFixtureFactory) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this FixtureFactoryCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new FixtureFactoryCollection.Enumerator GetEnumerator()
		{
			return new FixtureFactoryCollection.Enumerator(this);
		}
	}
}
