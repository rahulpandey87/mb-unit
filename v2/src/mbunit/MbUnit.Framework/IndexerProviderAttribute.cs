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

namespace MbUnit.Framework 
{
	using System;	

	public interface IForwardIterator
	{
		Object Increment(Object o);
	}
	
	public interface IBackwardIterator
	{
		Object Decrement(Object o);
	}
	
	public interface IBidirectionalIterator :
		IForwardIterator, IBackwardIterator
	{}
	
	public class IntIterator : IBidirectionalIterator
	{
		public IntIterator()
		{}
		
		public int Increment(int o)
		{
			return o+1;
		}
		public int Decrement(int o)
		{
			return o-1;			
		}
		Object IForwardIterator.Increment(Object o)
		{
			return this.Increment((int)o);
		}
		Object IBackwardIterator.Decrement(Object o)
		{
			return this.Decrement((int)o);
		}		
	}
	
	/// <summary>
	/// Tag method that provider a collection, an inde
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='IndexerProviderAttribute']"/>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public class IndexerProviderAttribute : ProviderAttribute 
	{
		private Type indexType;
		private Object first;
		private Object last;
		private Type iteratorType;
		private IBidirectionalIterator iterator;

		
		/// <summary>
		/// Default constructor - initializes all fields to default values
		/// </summary>
		public IndexerProviderAttribute(
		   	Type providerType,
			Type indexType,
			Object first,
			Object last,
			Type iteratorType
			) 
			:base(providerType)
		{
			if (indexType==null)
				throw new ArgumentNullException("indexType");
			if (iteratorType==null)
				throw new ArgumentNullException("iteratorType");
			if (!TypeHelper.HasIndexer(this.ProviderType,indexType))
				throw new ArgumentException(
					providerType.Name + " does not have an indexer with index of type " 
					+ indexType.Name, "indexType");
			
			if (first==null)
				throw new ArgumentNullException("first");
			if (last==null)
				throw new ArgumentNullException("last");
			if (first.GetType()!=indexType)
				throw new ArgumentException("first type does not match indexType");
			if (last.GetType()!=indexType)
				throw new ArgumentException("first type does not match indexType");
			
			this.indexType = indexType;
			this.first = first;
			this.last = last;
			this.iteratorType = iteratorType;
			this.iterator = (IBidirectionalIterator)TypeHelper.CreateInstance(this.iteratorType);
		}
		
		public Type IndexType
		{
			get
			{
				return this.indexType;
			}
			set
			{
				this.indexType = value;
			}
		}
		
		public Object First
		{
			get
			{
				return this.first;
			}
			set
			{
				this.first = value;
			}
		}
		
		public Object Last
		{
			get
			{
				return this.last;
			}
			set
			{
				this.last = value;
			}
		}	
		
		public Type IteratorType
		{
			get
			{
				return this.iteratorType;
			}
			set
			{
				this.iteratorType = value;
			}
		}
		
		public IBidirectionalIterator Iterator
		{
			get
			{
				return this.iterator;
			}
		}
	}
	
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public class IntIndexerProviderAttribute : IndexerProviderAttribute
	{
		/// <summary>
		/// Default constructor - initializes all fields to default values
		/// and int iterator
		/// </summary>
		public IntIndexerProviderAttribute(
		   	Type providerType,
			Object count
			) 
			:base(providerType,typeof(int),0,count,typeof(IntIterator))
		{}
	}
}
