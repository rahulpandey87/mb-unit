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
using System.Collections;
using MbUnit.Core.Invokers;

namespace MbUnit.Core.Collections 
{	
    /// <summary>
    /// A collection of elements of type IRunInvoker
    /// </summary>
    public sealed class RunInvokerCollection: System.Collections.CollectionBase
    {
        /// <summary>
        /// Initializes a new empty instance of the IRunInvokerCollection class.
        /// </summary>
        public RunInvokerCollection()
        {}

        /// <summary>
        /// Adds an instance of type IRunInvoker to the end of this IRunInvokerCollection.
        /// </summary>
        /// <param name="value">
        /// The IRunInvoker to be added to the end of this IRunInvokerCollection.
        /// </param>
        public void Add(IRunInvoker value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
        }

        /// <summary>
        /// Determines whether a specfic IRunInvoker value is in this IRunInvokerCollection.
        /// </summary>
        /// <param name="value">
        /// The IRunInvoker value to locate in this IRunInvokerCollection.
        /// </param>
        /// <returns>
        /// true if value is found in this IRunInvokerCollection;
        /// false otherwise.
        /// </returns>
        public bool Contains(IRunInvoker value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Gets or sets the IRunInvoker at the given index in this IRunInvokerCollection.
        /// </summary>
        public IRunInvoker this[int index]
        {
            get
            {
                return (IRunInvoker) this.List[index];
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific IRunInvoker from this IRunInvokerCollection.
        /// </summary>
        /// <param name="value">
        /// The IRunInvoker value to remove from this IRunInvokerCollection.
        /// </param>
        public void Remove(IRunInvoker value)
        {
            this.List.Remove(value);
        }

        /// <summary>
        /// Type-specific enumeration class, used by IRunInvokerCollection.GetEnumerator.
        /// </summary>
        public sealed class Enumerator: System.Collections.IEnumerator
        {
            private System.Collections.IEnumerator wrapped;

            public Enumerator(RunInvokerCollection collection)
            {
                this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
            }

            public IRunInvoker Current
            {
                get
                {
                    return (IRunInvoker) (this.wrapped.Current);
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return (IRunInvoker) (this.wrapped.Current);
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
        /// Returns an enumerator that can iterate through the elements of this IRunInvokerCollection.
        /// </summary>
        /// <returns>
        /// An object that implements System.Collections.IEnumerator.
        /// </returns>        
        public new RunInvokerCollection.Enumerator GetEnumerator()
        {
            return new RunInvokerCollection.Enumerator(this);
        }
    }

}

