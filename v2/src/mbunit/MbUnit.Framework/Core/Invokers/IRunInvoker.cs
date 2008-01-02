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
using System.Reflection;
using System.Collections;
using MbUnit.Core.Runs;

namespace MbUnit.Core.Invokers
{
	/// <summary>
	/// This interface defines a method invoker object.
	/// </summary>
	/// <include file="MbUnit.Core.Invokers.Doc.xml" path="doc/remarkss/remarks[@name='IRunInvoker']"/>
	public interface IRunInvoker
	{
		/// <summary>
		/// Gets a descriptive name of the <see cref="IRunInvoker"/>
		/// </summary>
		/// <value>
		/// A descriptive name of the <see cref="IRunInvoker"/>.
		/// </value>		
		String Name {get;}
		
		/// <summary>
		/// Gets a reference to the <see cref="IRun"/> instance that generated
		/// the invoker.
		/// </summary>
		/// <value>
		/// Reference to the <see cref="IRun"/> instance that generated
		/// the invoker.
		/// </value>
		IRun Generator {get;}
		
		/// <summary>
		/// Executes the wrapped method
		/// </summary>
		/// <param name="o">
		/// Test fixture instance
		/// </param>
		/// <param name="args">
		/// Method arguments
		/// </param>
		/// <returns>
		/// Return value of the invoked method. If the method returns void, null
		/// is returned.
		/// </returns>
		Object Execute(Object o, IList args);

        /// <summary>
        /// Gets a value indicating if the instance is related to
        /// <paramref name="memberInfo"/>
        /// </summary>
        /// <param name="memberInfo">
        /// A <see cref="MethodInfo"/> instance
        /// </param>
        /// <returns>
        /// true if the instance is related to the member info;
        /// otherwize false</returns>
        bool ContainsMemberInfo(MemberInfo memberInfo);
    }
}
