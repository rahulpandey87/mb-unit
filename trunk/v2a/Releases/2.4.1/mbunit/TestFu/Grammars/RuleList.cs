using System;
using System.Collections;

namespace TestFu.Grammars
{
    /// <summary>
    /// A collection of elements of type IRule
    /// </summary>
    public class RuleList: System.Collections.CollectionBase, IRuleList
    {
        /// <summary>
        /// Initializes a new empty instance of the RuleList class.
        /// </summary>
        public RuleList()
        {
        }

        /// <summary>
        /// Adds an instance of type IRule to the end of this RuleList.
        /// </summary>
        /// <param name="value">
        /// The IRule to be added to the end of this RuleList.
        /// </param>
        public virtual void Add(IRule value)
        {
            this.List.Add(value);
        }

        /// <summary>
        /// Determines whether a specfic IRule value is in this RuleList.
        /// </summary>
        /// <param name="value">
        /// The IRule value to locate in this RuleList.
        /// </param>
        /// <returns>
        /// true if value is found in this RuleList;
        /// false otherwise.
        /// </returns>
        public virtual bool Contains(IRule value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Inserts an element into the RuleList at the specified index
        /// </summary>
        /// <param name="index">
        /// The index at which the IRule is to be inserted.
        /// </param>
        /// <param name="value">
        /// The IRule to insert.
        /// </param>
        public virtual void Insert(int index, IRule value)
        {
            this.List.Insert(index, value);
        }

        /// <summary>
        /// Gets or sets the IRule at the given index in this RuleList.
        /// </summary>
        public virtual IRule this[int index]
        {
            get
            {
                return (IRule) this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific IRule from this RuleList.
        /// </summary>
        /// <param name="value">
        /// The IRule value to remove from this RuleList.
        /// </param>
        public virtual void Remove(IRule value)
        {
            this.List.Remove(value);
        }

        /// <summary>
        /// Type-specific enumeration class, used by RuleList.GetEnumerator.
        /// </summary>
        public class Enumerator: IRuleEnumerator
        {
            private System.Collections.IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
            public Enumerator(RuleList collection)
            {
                this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
            }

			/// <summary>
			/// 
			/// </summary>
            public IRule Current
            {
                get
                {
                    return (IRule) (this.wrapped.Current);
                }
            }

			/// <summary>
			/// 
			/// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
            public bool MoveNext()
            {
                return this.wrapped.MoveNext();
            }

			/// <summary>
			/// 
			/// </summary>
            public void Reset()
            {
                this.wrapped.Reset();
            }
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the elements of this RuleList.
        /// </summary>
        /// <returns>
        /// An object that implements System.Collections.IEnumerator.
        /// </returns>        
        public new virtual RuleList.Enumerator GetEnumerator()
        {
            return new RuleList.Enumerator(this);
        }
        
        IRuleEnumerator IRuleCollection.GetEnumerator()
        {
        	return this.GetEnumerator();
        }
    }
}
