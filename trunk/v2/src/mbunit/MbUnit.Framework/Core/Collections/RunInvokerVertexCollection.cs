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
	using MbUnit.Core.Invokers;
	
	
    /// <summary>
    /// A collection of elements of type RunInvokerVertex
    /// </summary>
    public sealed class RunInvokerVertexCollection: System.Collections.CollectionBase
    {
        /// <summary>
        /// Initializes a new empty instance of the RunInvokerVertexCollection class.
        /// </summary>
        public RunInvokerVertexCollection()
        {
            // empty
        }

        /// <summary>
        /// Adds an instance of type RunInvokerVertex to the end of this RunInvokerVertexCollection.
        /// </summary>
        /// <param name="value">
        /// The RunInvokerVertex to be added to the end of this RunInvokerVertexCollection.
        /// </param>
        public void Add(RunInvokerVertex value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
        }

        /// <summary>
        /// Determines whether a specfic RunInvokerVertex value is in this RunInvokerVertexCollection.
        /// </summary>
        /// <param name="value">
        /// The RunInvokerVertex value to locate in this RunInvokerVertexCollection.
        /// </param>
        /// <returns>
        /// true if value is found in this RunInvokerVertexCollection;
        /// false otherwise.
        /// </returns>
        public bool Contains(RunInvokerVertex value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Gets or sets the RunInvokerVertex at the given index in this RunInvokerVertexCollection.
        /// </summary>
        public RunInvokerVertex this[int index]
        {
            get
            {
                return (RunInvokerVertex) this.List[index];
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific RunInvokerVertex from this RunInvokerVertexCollection.
        /// </summary>
        /// <param name="value">
        /// The RunInvokerVertex value to remove from this RunInvokerVertexCollection.
        /// </param>
        public void Remove(RunInvokerVertex value)
        {
            this.List.Remove(value);
        }

        /// <summary>
        /// Type-specific enumeration class, used by RunInvokerVertexCollection.GetEnumerator.
        /// </summary>
        public sealed class Enumerator: System.Collections.IEnumerator
        {
            private System.Collections.IEnumerator wrapped;

            public Enumerator(RunInvokerVertexCollection collection)
            {
                this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
            }

            public RunInvokerVertex Current
            {
                get
                {
                    return (RunInvokerVertex) (this.wrapped.Current);
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return (RunInvokerVertex) (this.wrapped.Current);
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
        /// Returns an enumerator that can iterate through the elements of this RunInvokerVertexCollection.
        /// </summary>
        /// <returns>
        /// An object that implements System.Collections.IEnumerator.
        /// </returns>        
        public new RunInvokerVertexCollection.Enumerator GetEnumerator()
        {
            return new RunInvokerVertexCollection.Enumerator(this);
        }
    }

}
