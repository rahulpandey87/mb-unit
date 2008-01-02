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
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Core.Collections
{
	public sealed class FixtureCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the FixtureCollection class.
		/// </summary>
		public FixtureCollection()
		{
			// empty
		}

        public ReportCounter GetCounter()
        {
            ReportCounter counter = new ReportCounter();
            foreach(Fixture fixture in this)
                counter.AddCounts(fixture.GetCounter());
            return counter;
        }
        
        /// <summary>
		/// Adds an instance of type Fixture to the end of this FixtureCollection.
		/// </summary>
		/// <param name="value">
		/// The Fixture to be added to the end of this FixtureCollection.
		/// </param>
		public void Add(Fixture value)
		{
			if (value==null)
				throw new ArgumentNullException("value");
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Fixture value is in this FixtureCollection.
		/// </summary>
		/// <param name="value">
		/// The Fixture value to locate in this FixtureCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this FixtureCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(Fixture value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific Fixture from this FixtureCollection.
		/// </summary>
		/// <param name="value">
		/// The Fixture value to remove from this FixtureCollection.
		/// </param>
		public void Remove(Fixture value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by FixtureCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(FixtureCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public Fixture Current
			{
				get
				{
					return (Fixture) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (Fixture) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this FixtureCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new FixtureCollection.Enumerator GetEnumerator()
		{
			return new FixtureCollection.Enumerator(this);
		}
	}
}
