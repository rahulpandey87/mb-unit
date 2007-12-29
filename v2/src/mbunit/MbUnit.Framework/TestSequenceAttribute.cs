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
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
	/// <summary>
	/// Creates an order of execution in the fixture.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This fixture is used to implement the Process testing advertised by
	/// Marc Clifton' 
	/// <a href="http://www.codeproject.com/csharp/autp3.asp">Code Project
	/// article</a>.
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public sealed class TestSequenceAttribute : TestPatternAttribute
	{
		private int order;
		
		/// <summary>
        /// Initializes a new instance of <see cref="TestSequenceAttribute"/> with the given order.
		/// </summary>
		/// <param name="order">order of execution</param>
		public TestSequenceAttribute(int order)
		{
			this.order = order;
		}

		/// <summary>
        /// Initializes a new instance of <see cref="TestSequenceAttribute"/> with the given order
		/// and description.
		/// </summary>
		/// <param name="order">order of execution</param>
		/// <param name="description">description of the test</param>
		public TestSequenceAttribute(int order, string description)
			:base(description)
		{
			this.order = order;
		}
		
		/// <summary>
		/// Gets or sets the order execution
		/// </summary>
		/// <value>
		/// The order of execution
		/// </value>
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		/// <summary>
		/// Returns a string that represents the instance.
		/// </summary>		
		/// <returns>
		/// String representing the object.
		/// </returns>
		public override string ToString()
		{
			return String.Format("order: {0}",this.order);
		}
	}	
}

