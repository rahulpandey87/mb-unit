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
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MbUnit.Framework
{
    [Flags]
	public enum TestImportance
	{
		Critical,
		Severe,
		Serious,
		Default,
		NoOneReallyCaresAbout
	}

    /// <summary>
    /// Associates a <see cref="TestImportance" /> with a test fixture, test method, test parameter
    /// or other test component.
    /// </summary>
	/// <remarks>
	/// Fixture importance is labelled from 0, critical to higher values
	/// representing less critical tests.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class,AllowMultiple=false,Inherited=true)]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class ImportanceAttribute : InformationAttribute
	{
		private TestImportance importance;

        /// <summary>
        /// Associates a <see cref="TestImportance" />  with the test component annotated by this attribute.
        /// </summary>
        /// <param name="importance">The importance to associate</param>
		public ImportanceAttribute(TestImportance importance)
		{
			this.importance = importance;
		}

        /// <summary>
        /// Gets the importance.
        /// </summary>
        /// <value>The importance.</value>
		[Category("Data")]
		public TestImportance Importance			
		{
			get
			{
				return this.importance;
			}
		}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="TestImportance"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="TestImportance"/>.
        /// </returns>
		public override string ToString()
		{
			return String.Format("Importance: {0}",
				this.importance
				);
		}

	}
}
