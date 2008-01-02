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

namespace MbUnit.Core.Collections
{
    using System;
    /// <summary>
    /// A collection of elements of type RunInvokerVertexCollection
    /// </summary>
    public sealed class RunInvokerVertexCollectionCollection: System.Collections.CollectionBase
    {
        /// <summary>
        /// Initializes a new empty instance of the RunInvokerVertexCollectionCollection class.
        /// </summary>
        public RunInvokerVertexCollectionCollection()
        {}

        /// <summary>
        /// Adds an instance of type RunInvokerVertexCollection to the end of this RunInvokerVertexCollectionCollection.
        /// </summary>
        /// <param name="value">
        /// The RunInvokerVertexCollection to be added to the end of this RunInvokerVertexCollectionCollection.
        /// </param>
        public void Add(RunInvokerVertexCollection value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
        }

        /// <summary>
        /// Determines whether a specfic RunInvokerVertexCollection value is in this RunInvokerVertexCollectionCollection.
        /// </summary>
        /// <param name="value">
        /// The RunInvokerVertexCollection value to locate in this RunInvokerVertexCollectionCollection.
        /// </param>
        /// <returns>
        /// true if value is found in this RunInvokerVertexCollectionCollection;
        /// false otherwise.
        /// </returns>
        public bool Contains(RunInvokerVertexCollection value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Gets or sets the RunInvokerVertexCollection at the given index in this RunInvokerVertexCollectionCollection.
        /// </summary>
        public RunInvokerVertexCollection this[int index]
        {
            get
            {
                return (RunInvokerVertexCollection) this.List[index];
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific RunInvokerVertexCollection from this RunInvokerVertexCollectionCollection.
        /// </summary>
        /// <param name="value">
        /// The RunInvokerVertexCollection value to remove from this RunInvokerVertexCollectionCollection.
        /// </param>
        public void Remove(RunInvokerVertexCollection value)
        {
            this.List.Remove(value);
        }

        /// <summary>
        /// Type-specific enumeration class, used by RunInvokerVertexCollectionCollection.GetEnumerator.
        /// </summary>
        public sealed class Enumerator: System.Collections.IEnumerator
        {
            private System.Collections.IEnumerator wrapped;

            public Enumerator(RunInvokerVertexCollectionCollection collection)
            {
                this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
            }

            public RunInvokerVertexCollection Current
            {
                get
                {
                    return (RunInvokerVertexCollection) (this.wrapped.Current);
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return (RunInvokerVertexCollection) (this.wrapped.Current);
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
        /// Returns an enumerator that can iterate through the elements of this RunInvokerVertexCollectionCollection.
        /// </summary>
        /// <returns>
        /// An object that implements System.Collections.IEnumerator.
        /// </returns>        
        public new RunInvokerVertexCollectionCollection.Enumerator GetEnumerator()
        {
            return new RunInvokerVertexCollectionCollection.Enumerator(this);
        }
    }
	
}
