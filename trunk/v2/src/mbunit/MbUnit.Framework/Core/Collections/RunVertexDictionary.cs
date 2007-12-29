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

namespace MbUnit.Core.Collections
{
	using System;
	using MbUnit.Core.Runs;
	
    /// <summary>
    /// A dictionary with keys of type IRun and values of type RunVertex
    /// </summary>
    public sealed class RunVertexDictionary: System.Collections.DictionaryBase
    {
        /// <summary>
        /// Initializes a new empty instance of the RunVertexDictionary class
        /// </summary>
        public RunVertexDictionary()
        {
            // empty
        }

        /// <summary>
        /// Gets or sets the RunVertex associated with the given IRun
        /// </summary>
        /// <param name="key">
        /// The IRun whose value to get or set.
        /// </param>
        public RunVertex this[IRun key]
        {
            get
            {
                return (RunVertex) this.Dictionary[key];
            }
        }

        /// <summary>
        /// Adds an element with the specified key and value to this RunVertexDictionary.
        /// </summary>
        /// <param name="key">
        /// The IRun key of the element to add.
        /// </param>
        /// <param name="value">
        /// The RunVertex value of the element to add.
        /// </param>
        public void Add(IRun key, RunVertex value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");
            this.Dictionary.Add(key, value);
        }

        /// <summary>
        /// Determines whether this RunVertexDictionary contains a specific key.
        /// </summary>
        /// <param name="key">
        /// The IRun key to locate in this RunVertexDictionary.
        /// </param>
        /// <returns>
        /// true if this RunVertexDictionary contains an element with the specified key;
        /// otherwise, false.
        /// </returns>
        public bool Contains(IRun key)
        {
            return this.Dictionary.Contains(key);
        }

        /// <summary>
        /// Determines whether this RunVertexDictionary contains a specific value.
        /// </summary>
        /// <param name="value">
        /// The RunVertex value to locate in this RunVertexDictionary.
        /// </param>
        /// <returns>
        /// true if this RunVertexDictionary contains an element with the specified value;
        /// otherwise, false.
        /// </returns>
        public bool ContainsValue(RunVertex value)
        {
            foreach (RunVertex item in this.Dictionary.Values)
            {
                if (item == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the element with the specified key from this RunVertexDictionary.
        /// </summary>
        /// <param name="key">
        /// The IRun key of the element to remove.
        /// </param>
        public void Remove(IRun key)
        {
            this.Dictionary.Remove(key);
        }

        /// <summary>
        /// Gets a collection containing the keys in this RunVertexDictionary.
        /// </summary>
        public System.Collections.ICollection Keys
        {
            get
            {
                return this.Dictionary.Keys;
            }
        }

        /// <summary>
        /// Gets a collection containing the values in this RunVertexDictionary.
        /// </summary>
        public System.Collections.ICollection Values
        {
            get
            {
                return this.Dictionary.Values;
            }
        }
    }

}
