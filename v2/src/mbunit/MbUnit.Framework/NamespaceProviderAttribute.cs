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

namespace MbUnit.Framework
{
	using MbUnit.Core.Runs;

    /// <summary>
    /// Tags a class as a namespace provider. For use with a <see cref="TypeFixtureAttribute"/> tagged class
    /// instead of a <see cref="ProviderFactoryAttribute"/> tagged type.
    /// </summary>
	public class NamespaceProviderAttribute : ProviderFixtureDecoratorPatternAttribute
	{
		private Type typeToAssembly;
		private Type factoredType;
		private string _namespace = "";
		private bool recurse = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceProviderAttribute"/> class.
        /// </summary>
        /// <param name="typeToAssembly">The <see cref="Type"/> of the assembly.</param>
        /// <param name="factoredType">Type of the factored.</param>
		public NamespaceProviderAttribute(
			Type typeToAssembly, 
			Type factoredType)
		{
			if (typeToAssembly==null)
				throw new ArgumentNullException("typeToAssembly");
			if (factoredType == null)
				throw new ArgumentNullException("factoredType");

			this.typeToAssembly = typeToAssembly;
			this.factoredType = factoredType;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceProviderAttribute"/> class.
        /// </summary>
        /// <param name="typeToAssembly">The <see cref="Type"/> of the assembly.</param>
        /// <param name="factoredType">Type of the factored.</param>
        /// <param name="description">A brief description of the provider for reference.</param>
		public NamespaceProviderAttribute(Type typeToAssembly, Type factoredType, string description)
			:base(description)
		{
			if (typeToAssembly==null)
				throw new ArgumentNullException("typeToAssembly");
			if (factoredType == null)
				throw new ArgumentNullException("factoredType");

			this.typeToAssembly = typeToAssembly;
			this.factoredType = factoredType;
		}

        /// <summary>
        /// Gets or sets the assembly type.
        /// </summary>
        /// <value>The assembly type.</value>
		public Type TypeToAssembly
		{
			get
			{
				return this.typeToAssembly;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.typeToAssembly = value;
			}
		}

        /// <summary>
        /// Gets or sets the factored type.
        /// </summary>
        /// <value>The factored type.</value>
		public Type FactoredType
		{
			get
			{
				return this.factoredType;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.factoredType = value;
			}
		}

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
		public String Namespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NamespaceProviderAttribute"/> is recursive.
        /// </summary>
        /// <value><c>true</c> if recursive; otherwise, <c>false</c>.</value>
		public bool Recurse
		{
			get
			{
				return this.recurse;
			}
			set
			{
				this.recurse = value;
			}
		}

        /// <summary>
        /// Gets the test runner class defining all the test to be run within the tagged fixture class.
        /// </summary>
        /// <param name="decoratedType">Type of the decorated.</param>
        /// <returns>An <see cref="AssemblyProviderRun"/> object</returns>
		public override IRun GetRun(Type decoratedType)
		{
			if (decoratedType==null)
				throw new ArgumentNullException("decoratedType");

			// get properties that return
			return new AssemblyProviderRun(
				this.typeToAssembly,
				this.factoredType,
				this.Namespace,
				this.recurse,
				decoratedType
				);
		}

	}
}
