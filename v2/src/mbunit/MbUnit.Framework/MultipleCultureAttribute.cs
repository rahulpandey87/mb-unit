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
using System.Collections.Specialized;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;


namespace MbUnit.Framework 
{


    /// <summary>
	/// Used to tag any test method that needs to be run under a number of different cultures.
	/// </summary>
	/// <remarks>Takes a comma-delimited list of cultures to run this test under.</remarks>
	/// <example>
	/// <para>The following example demonstrates a simple test run under multiple cultures. It will pass under en-US culture but fail under en-GB</para>
	/// <code>
	/// [Test]
	/// [MultipleCulture("en-US, eb-GB")]
	/// public void CheckCurrencySymbol()
	/// {
	///     Assert.AreEqual("$4.50", String.Format("{0:C}", 4.5d);
	/// }
	/// </code>
	/// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class MultipleCultureAttribute : DecoratorPatternAttribute
    {
		private String cultureString ;
		private StringCollection cultures = new StringCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleCultureAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// <para>A full list of culture strings can be found <a href="http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx">here</a>.</para>
        /// </remarks>
        /// <param name="cultures">A comma-delimited list of cultures to be used in the test.</param>
		public MultipleCultureAttribute(string cultures)
		{
			this.cultureString = cultures;
			this.cultures.AddRange(cultures.Split(',',';'));
		}

        /// <summary>
        /// Gets the list of cultures to be used in the test as a StringCollection
        /// </summary>
        /// <value>The list of cultures to be used in the test as a StringCollection</value>
		public StringCollection Cultures
		{
			get
			{
				return this.cultures;
			}
		}

        /// <summary>
        /// Gets the list of cultures to be used in the test as a comma-delimited string
        /// </summary>
        /// <value>The list of cultures to be used in the test as a comma-delimited string</value>
		public String CultureString
		{
			get
			{
				return this.cultureString;
			}
		}

        /// <summary>
        /// Returns the invoker class to run the test with the given duration parameters.
        /// </summary>
        /// <param name="invoker">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="MultipleCultureRunInvoker"/> object wrapping <paramref name="invoker"/></returns>
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new MultipleCultureRunInvoker(invoker, this);
		}

	}
}
