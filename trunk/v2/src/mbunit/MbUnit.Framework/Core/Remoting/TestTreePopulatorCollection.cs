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
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux

using System;

namespace MbUnit.Core.Remoting
{
	/// <summary>
	/// A collection of elements of type TestTreePopulator
	/// </summary>
	public class TestTreePopulatorCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the TestTreePopulatorCollection class.
		/// </summary>
		public TestTreePopulatorCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the TestTreePopulatorCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new TestTreePopulatorCollection.
		/// </param>
		public TestTreePopulatorCollection(ITestTreePopulator[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the TestTreePopulatorCollection class, containing elements
		/// copied from another instance of TestTreePopulatorCollection
		/// </summary>
		/// <param name="items">
		/// The TestTreePopulatorCollection whose elements are to be added to the new TestTreePopulatorCollection.
		/// </param>
		public TestTreePopulatorCollection(TestTreePopulatorCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this TestTreePopulatorCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this TestTreePopulatorCollection.
		/// </param>
		public virtual void AddRange(ITestTreePopulator[] items)
		{
			foreach (TestTreePopulator item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another TestTreePopulatorCollection to the end of this TestTreePopulatorCollection.
		/// </summary>
		/// <param name="items">
		/// The TestTreePopulatorCollection whose elements are to be added to the end of this TestTreePopulatorCollection.
		/// </param>
		public virtual void AddRange(TestTreePopulatorCollection items)
		{
			foreach (TestTreePopulator item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type TestTreePopulator to the end of this TestTreePopulatorCollection.
		/// </summary>
		/// <param name="value">
		/// The TestTreePopulator to be added to the end of this TestTreePopulatorCollection.
		/// </param>
		public virtual void Add(ITestTreePopulator value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic TestTreePopulator value is in this TestTreePopulatorCollection.
		/// </summary>
		/// <param name="value">
		/// The TestTreePopulator value to locate in this TestTreePopulatorCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this TestTreePopulatorCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(ITestTreePopulator value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific TestTreePopulator from this TestTreePopulatorCollection.
		/// </summary>
		/// <param name="value">
		/// The TestTreePopulator value to remove from this TestTreePopulatorCollection.
		/// </param>
		public virtual void Remove(ITestTreePopulator value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by TestTreePopulatorCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(TestTreePopulatorCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public ITestTreePopulator Current
			{
				get
				{
					return (ITestTreePopulator) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (ITestTreePopulator) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this TestTreePopulatorCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual TestTreePopulatorCollection.Enumerator GetEnumerator()
		{
			return new TestTreePopulatorCollection.Enumerator(this);
		}
	}
}
