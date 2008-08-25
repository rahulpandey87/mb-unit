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

using MbUnit.Core;

namespace MbUnit.Framework {
    using System;

    /// <summary>
    /// Defines an interface for iterator providers that are forward-only
    /// </summary>
    public interface IForwardIterator {
        Object Increment(Object o);
    }

    /// <summary>
    /// Defines an interface for iterator providers that are backwards-only
    /// </summary>
    public interface IBackwardIterator {
        Object Decrement(Object o);
    }

    /// <summary>
    /// Defines an interface for iterator providers that can move back and forth in their collection of values
    /// </summary>
    public interface IBidirectionalIterator :
        IForwardIterator, IBackwardIterator { }

    /// <summary>
    /// Simple class implementing <see cref="IBidirectionalIterator"/> that returns the next or previous integer in sequence
    /// </summary>
    public class IntIterator : IBidirectionalIterator {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntIterator"/> class.
        /// </summary>
        public IntIterator() { }

        /// <summary>
        /// Increments the specified integer.
        /// </summary>
        /// <param name="o">The current integer.</param>
        /// <returns>The next integer in sequence</returns>
        public int Increment(int o) {
            return o + 1;
        }
        /// <summary>
        /// Decrements the specified integer.
        /// </summary>
        /// <param name="o">The current integer.</param>
        /// <returns>The next integer in sequence</returns>
        public int Decrement(int o) {
            return o - 1;
        }
        /// <summary>
        /// Increments the specified integer.
        /// </summary>
        /// <param name="o">The current integer.</param>
        /// <returns>The next integer in sequence</returns>
        Object IForwardIterator.Increment(Object o) {
            return this.Increment((int)o);
        }
        /// <summary>
        /// Decrements the specified integer.
        /// </summary>
        /// <param name="o">The current integer.</param>
        /// <returns>The next integer in sequence</returns>
        Object IBackwardIterator.Decrement(Object o) {
            return this.Decrement((int)o);
        }
    }


    /// <summary>
    /// Tags a method that returns an object which can be iterated over using a class implementing <see cref="IBidirectionalIterator"/> 
    /// </summary>
    /// <remarks>
    /// <para>Used in conjunction with the <see cref="CollectionIndexingFixtureAttribute"/> to implement the Collection Indexing Pattern.
    /// The <see cref="IndexerProviderAttribute"/> tags a method to provide collection index type and index range.
    /// </para></remarks>
    /// <example>
    ///<para>
    ///This example checks the Collection Indexing Pattern for an array of integers
    ///</para>
    ///<code>
    ///[CollectionIndexingFixture]
    ///public class CollectionIndexingFixtureAttributeTest
    ///{		
    ///    [IndexerProvider(typeof(ArrayList), typeof(int), 0, 9, typeof(IntIterator))]
    ///    public ArrayList ProvideArrayList()
    ///    {
    ///        ArrayList list = new ArrayList();
    ///        for(int i=0;i&lt;10;++i)
    ///            list.Add(i);
    ///		 	
    ///        return list;
    ///    }
    ///}
    /// 
    /// public class IntIterator : IBidirectionalIterator 
    ///	{
    ///        public IntIterator() { }
    ///
    ///        public int Increment(int o) {
    ///            return o + 1;
    ///        }
    ///
    ///        public int Decrement(int o) {
    ///            return o - 1;
    ///        }
    ///
    ///        Object IForwardIterator.Increment(Object o) {
    ///            return this.Increment((int)o);
    ///        }
    ///
    ///        Object IBackwardIterator.Decrement(Object o) {
    ///            return this.Decrement((int)o);
    ///        }
    ///    } 
    ///</code>
    ///</example>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IndexerProviderAttribute : ProviderAttribute {
        private Type indexType;
        private Object first;
        private Object last;
        private Type iteratorType;
        private IBidirectionalIterator iterator;


        /// <summary>
        /// Initializes a new instance of the <see cref="IndexerProviderAttribute"/> class.
        /// </summary>
        /// <param name="providerType">The <see cref="Type"/> of the object to be iterated over</param>
        /// <param name="indexType">The <see cref="Type"/> of the index used by <paramref name="providerType"/>.</param>
        /// <param name="first">The first index value.</param>
        /// <param name="last">The last index value.</param>
        /// <param name="iteratorType">The <see cref="Type"/> of the iterator being used to iterate over <paramref name="providerType"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexType"/>, <paramref name="iteratorType"/>, <paramref name="first"/>,
        ///   or  <paramref name="last"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="providerType"/> does not have an index of type <paramref name="indexType"/></exception>
        /// <exception cref="ArgumentException">Thrown in <paramref name="first"/> or <paramref name="last"/> are not of type <paramref name="indexType"/></exception>
        public IndexerProviderAttribute(
            Type providerType,
            Type indexType,
            Object first,
            Object last,
            Type iteratorType
            )
            : base(providerType) {
            if (indexType == null)
                throw new ArgumentNullException("indexType");
            if (iteratorType == null)
                throw new ArgumentNullException("iteratorType");
            if (!TypeHelper.HasIndexer(this.ProviderType, indexType))
                throw new ArgumentException(
                    providerType.Name + " does not have an indexer with index of type "
                    + indexType.Name, "indexType");

            if (first == null)
                throw new ArgumentNullException("first");
            if (last == null)
                throw new ArgumentNullException("last");
            if (first.GetType() != indexType)
                throw new ArgumentException("first type does not match indexType");
            if (last.GetType() != indexType)
                throw new ArgumentException("first type does not match indexType");

            this.indexType = indexType;
            this.first = first;
            this.last = last;
            this.iteratorType = iteratorType;
            this.iterator = (IBidirectionalIterator)TypeHelper.CreateInstance(this.iteratorType);
        }

        /// <summary>
        /// Gets or sets the type of the index.
        /// </summary>
        /// <value>The type of the index.</value>
        public Type IndexType {
            get {
                return this.indexType;
            }
            set {
                this.indexType = value;
            }
        }

        /// <summary>
        /// Gets or sets the first value for the index.
        /// </summary>
        /// <value>The first value for the index.</value>
        public Object First {
            get {
                return this.first;
            }
            set {
                this.first = value;
            }
        }

        /// <summary>
        /// Gets or sets the last value for the index.
        /// </summary>
        /// <value>The last value for the index.</value>
        public Object Last {
            get {
                return this.last;
            }
            set {
                this.last = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the iterator.
        /// </summary>
        /// <value>The type of the iterator.</value>
        public Type IteratorType {
            get {
                return this.iteratorType;
            }
            set {
                this.iteratorType = value;
            }
        }

        /// <summary>
        /// Gets the iterator.
        /// </summary>
        /// <value>The iterator.</value>
        public IBidirectionalIterator Iterator {
            get {
                return this.iterator;
            }
        }
    }

    /// <summary>
    /// Class derived from <see cref="IndexerProviderAttribute"/>. Assumes that the object to be iterated over uses
    /// an integer-based index where each value in the index is one greater or lower than the next.
    /// </summary>
    /// <remarks>
    /// <para>Used in conjunction with the <see cref="CollectionIndexingFixtureAttribute"/> to implement the Collection Indexing Pattern.
    /// The <see cref="IntIndexerProviderAttribute"/> tags a method to provide collection with integer index type and indicate its index range.
    /// </para></remarks>
    /// <example>
    ///<para>
    ///This example checks the Collection Indexing Pattern for an array of integers
    ///</para>
    ///<code>
    ///[CollectionIndexingFixture]
    ///public class CollectionIndexingFixtureAttributeTest
    ///{		
    ///    [IntIndexerProvider(typeof(ArrayList), 9)]
    ///    public ArrayList ProvideArrayList()
    ///    {
    ///        ArrayList list = new ArrayList();
    ///        for(int i=0;i&lt;10;++i)
    ///            list.Add(i);
    ///		 	
    ///        return list;
    ///    }
    ///}

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IntIndexerProviderAttribute : IndexerProviderAttribute {

        /// <summary>
        /// Initializes a new instance of the <see cref="IntIndexerProviderAttribute"/> class.
        /// </summary>
        /// <param name="providerType">The <see cref="Type"/> of the object to be iterated over</param>
        /// <param name="count">The upper index value in the collection.</param>
        public IntIndexerProviderAttribute(
            Type providerType,
            Object count
            )
            : base(providerType, typeof(int), 0, count, typeof(IntIterator)) { }
    }
}
