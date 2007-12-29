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

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// A collection of elements of type Type
	/// </summary>
	public sealed class TypeCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the TypeCollection class.
		/// </summary>
		public TypeCollection()
		{}

		/// <summary>
		/// Adds an instance of type Type to the end of this TypeCollection.
		/// </summary>
		/// <param name="value">
		/// The Type to be added to the end of this TypeCollection.
		/// </param>
		public void Add(Type value)
		{
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Type value is in this TypeCollection.
		/// </summary>
		/// <param name="value">
		/// The Type value to locate in this TypeCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this TypeCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(Type value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Gets or sets the Type at the given index in this TypeCollection.
		/// </summary>
		public Type this[int index]
		{
			get
			{
				return (Type) this.List[index];
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific Type from this TypeCollection.
		/// </summary>
		/// <param name="value">
		/// The Type value to remove from this TypeCollection.
		/// </param>
		public void Remove(Type value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by TypeCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(TypeCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public Type Current
			{
				get
				{
					return (Type) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (Type) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this TypeCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new TypeCollection.Enumerator GetEnumerator()
		{
			return new TypeCollection.Enumerator(this);
		}
	}
}
