#region TestFu Library License, Copyright (c) 2004 Jonathan de Halleux
// TestFu Library License
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
//		QuickGraph Library HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux
//     Blog: http://blog.dotnetwiki.org
#endregion


using System;
using System.Windows.Forms;
using Refly.CodeDom.Expressions;

namespace TestFu.Gestures
{
    /// <summary>
    /// A user gesture.
    /// </summary>
    /// <remarks>
    /// <para>
    /// User gesture can be a combination of keyboard or mouse
    /// interactions.
    /// </para>
    /// </remarks>
    public interface IGesture
    {
        /// <summary>
        /// Gets the <see cref="Form"/> that is targeted
        /// by the gesture
        /// </summary>
        /// <value></value>
        Form Form { get;set;}

        /// <summary>
        /// Executes the gesture
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method should be executed on separate thread
        /// from the main thread, otherwize event will not be
        /// fired correctly.
        /// </para>
        /// </remarks>
        void Start();

        /// <summary>
        /// Gets the CodeDom statement creating this gesture
        /// </summary>
        /// <returns></returns>
        Expression ToCodeDom(Expression factory);
    }
}
