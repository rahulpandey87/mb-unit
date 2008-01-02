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
using MbUnit.Core.Runs;

namespace MbUnit.Core.Collections
{
    /// <summary>
    /// A collection of elements of type IRun
    /// </summary>
    public sealed class RunCollection: System.Collections.CollectionBase
    {
        /// <summary>
        /// Initializes a new empty instance of the RunCollection class.
        /// </summary>
        public RunCollection()
        {}

        /// <summary>
        /// Adds an instance of type IRun to the end of this RunCollection.
        /// </summary>
        /// <param name="value">
        /// The IRun to be added to the end of this RunCollection.
        /// </param>
        public void Add(IRun value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            this.List.Add(value);
        }

        /// <summary>
        /// Determines whether a specfic IRun value is in this RunCollection.
        /// </summary>
        /// <param name="value">
        /// The IRun value to locate in this RunCollection.
        /// </param>
        /// <returns>
        /// true if value is found in this RunCollection;
        /// false otherwise.
        /// </returns>
        public bool Contains(IRun value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Gets or sets the IRun at the given index in this RunCollection.
        /// </summary>
        public IRun this[int index]
        {
            get
            {
                return (IRun) this.List[index];
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific IRun from this RunCollection.
        /// </summary>
        /// <param name="value">
        /// The IRun value to remove from this RunCollection.
        /// </param>
        public void Remove(IRun value)
        {
            this.List.Remove(value);
        }

        /// <summary>
        /// Type-specific enumeration class, used by RunCollection.GetEnumerator.
        /// </summary>
        public sealed class Enumerator: System.Collections.IEnumerator
        {
            private System.Collections.IEnumerator wrapped;

            public Enumerator(RunCollection collection)
            {
                this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
            }

            public IRun Current
            {
                get
                {
                    return (IRun) (this.wrapped.Current);
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return (IRun) (this.wrapped.Current);
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
        /// Returns an enumerator that can iterate through the elements of this RunCollection.
        /// </summary>
        /// <returns>
        /// An object that implements System.Collections.IEnumerator.
        /// </returns>        
        public new RunCollection.Enumerator GetEnumerator()
        {
            return new RunCollection.Enumerator(this);
        }
    }
}

