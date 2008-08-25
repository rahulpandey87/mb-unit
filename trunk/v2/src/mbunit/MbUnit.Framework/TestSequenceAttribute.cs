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
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
	/// <summary>
	/// Creates .
	/// </summary>
    /// <summary>
    /// Tags a test method. Defines an an order of test execution within in the fixture
    /// The fixture must be tagged with a <see cref="ProcessTestFixtureAttribute"/> for this 
    /// execution order to be enforced
    /// </summary>
    /// <remarks>
    /// <para><em>Implements:</em> Process Test Fixture</para>
    /// <para><em>Logic:</em>
    /// <code>
    /// [SetUp]
    /// {TestSequence}
    /// [TearDown]
    /// </code>
    /// </para>
    /// <para>
    /// This fixture implements the Process Test Fixture as described in the
    /// <a href="http://www.codeproject.com/csharp/autp3.asp">CodeProject</a>
    /// article from Marc Clifton.
    /// </para>
    /// <para>
    /// In this implementation, reverse traversal is not implemented.
    /// A process can be seen as a linear graph, a very simple model. If you
    /// need more evolved models, use Model Based Testing.
    /// </para>
    /// </remarks>
    /// <para>
    /// This is the example for the <a href="http://www.codeproject.com/csharp/autp3.asp">CodeProject</a> 
    /// article adapted to MbUnit.
    /// </para>
    /// <code>
    /// <b>[ProcessTestFixture]</b>
    /// public class POSequenceTest
    /// {	
    /// 	...
    /// 	<b>[TestSequence(1)]</b>
    /// 	public void POConstructor()
    /// 	{
    /// 		po=new PurchaseOrder();
    /// 		Assert.AreEqual(po.Number,"", "Number not initialized.");
    /// 		Assert.AreEqual(po.PartCount,0, "PartCount not initialized.");
    /// 		Assert.AreEqual(po.ChargeCount,0, "ChargeCount not initialized.");
    /// 		Assert.AreEqual(po.Invoice,null, "Invoice not initialized.");
    /// 		Assert.AreEqual(po.Vendor,null, "Vendor not initialized.");
    /// 	}
    /// 
    /// 	[TestSequence(2)]
    /// 	public void VendorConstructor()
    /// 	{
    /// 		vendor=new Vendor();
    /// 		Assert.AreEqual(vendor.Name,"", "Name is not an empty string.");
    /// 		Assert.AreEqual(vendor.PartCount,0, "PartCount is not zero.");
    /// 	}
    /// 	...
    /// </code>
    /// <para>
    /// Use <see cref="ProcessTestFixtureAttribute"/> to mark a class as process test fixture and use the 
    /// <see cref="TestSequenceAttribute"/> attribute to create the order of the process. The fixture also supports
    /// SetUp and TearDown methods.
    /// </para>
    /// </example>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public sealed class TestSequenceAttribute : TestPatternAttribute
	{
		private int order;
		
		/// <summary>
        /// Initializes a new instance of <see cref="TestSequenceAttribute"/> with the given order.
		/// </summary>
		/// <param name="order">order of execution (1 = first, 2 = second etc)</param>
		public TestSequenceAttribute(int order)
		{
			this.order = order;
		}

		/// <summary>
        /// Initializes a new instance of <see cref="TestSequenceAttribute"/> with the given order
		/// and description.
		/// </summary>
        /// <param name="order">order of execution (1 = first, 2 = second etc)</param>
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

