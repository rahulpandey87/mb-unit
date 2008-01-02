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
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TestFu.Gestures
{
    /// <summary>
    /// Abstract base class for <see cref="IMouseGesture"/>
    /// instance.
    /// </summary>
    public abstract class MouseGestureBase : GestureBase, IMouseGesture
    {
        private MouseButtons buttons = MouseButtons.None;

        /// <summary>
        /// Initializes an new <see cref="MouseGestureBase"/>.
        /// </summary>
        public MouseGestureBase()
        { }

        /// <summary>
        /// Initializes a new <see cref="MouseGestureBase"/>
        /// with a <see cref="Form"/> instance and the buttons
        /// involved in the gesture.
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/> instance</param>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        public MouseGestureBase(Form form, MouseButtons buttons)
            :base(form)
        {
            this.buttons = buttons;
        }

        /// <summary>
        /// Gets or sets a value indicating the <see cref="MouseButtons"/>
        /// involved in the gesture.
        /// </summary>
        /// <value>
        /// A combined value of <see cref="MouseButtons"/> flags.
        /// </value>
        public MouseButtons Buttons
        {
            get
            {
                return this.buttons;
            }
            set
            {
                this.buttons = value;
            }
        }
    }
}
