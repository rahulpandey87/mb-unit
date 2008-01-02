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
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux
//     Blog: http://blog.dotnetwiki.org
#endregion

using System;
using System.Windows.Forms;
using Refly.CodeDom;
using Refly.CodeDom.Expressions;

namespace TestFu.Gestures
{
    /// <summary>
    /// A <see cref="IMouseGesture"/> that simulates a button click
    /// </summary>
    public class ClickMouseGesture : MouseGestureBase
    {
        /// <summary>
        /// Initializes a new <see cref="ClickMouseGesture"/>
        /// </summary>
        public ClickMouseGesture()
        { }

        /// <summary>
        /// Initializes a new <see cref="ClickMouseGesture"/>
        /// with a <see cref="Form"/> instance and the left button
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/> instance
        /// </param>
        public ClickMouseGesture(Form form)
            :this(form,MouseButtons.Left)
        { }

        /// <summary>
        /// Initializes a new <see cref="ClickMouseGesture"/>
        /// with a <see cref="Form"/> instance
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/> instance
        /// </param>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        public ClickMouseGesture(Form form, MouseButtons buttons)
            :base(form,buttons)
        {
        }

        /// <summary>
        /// Executes the mouse click
        /// </summary>
        public override void Start()
        {
            VirtualInput.MouseClick(this.Buttons);
        }

        public override Expression ToCodeDom(Expression factory)
        {
            return factory.Method("MouseClick").Invoke();
        }
    }
}
