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
using Refly.CodeDom.Expressions;
using Refly.CodeDom.Statements;
using Refly.CodeDom;

namespace TestFu.Gestures
{
    /// <summary>
    /// A <see cref="IMouseGesture"/> that moves the cursor to the 
    /// center of a <see cref="Control"/>.
    /// </summary>
    public class ControlMoveMouseGesture : MoveMouseGestureBase
    {
        private ControlKey controlKey=null;
        private Point offset = new Point();

        /// <summary>
        /// Initializes a new <see cref="ControlMoveMouseGesture"/>
        /// </summary>
        public ControlMoveMouseGesture()
        { }

        /// <summary>
        /// Initializes a new <see cref="ControlMoveMouseGesture"/>
        /// with a target <see cref="Form"/> and a target 
        /// <see cref="Control"/>
        /// </summary>
        /// <param name="form">
        /// Target form</param>
        /// <param name="targetControl">
        /// Target control
        /// </param>
        public ControlMoveMouseGesture(Form form, Control targetControl)
            :this(form,MouseButtons.None,targetControl)
        {}

        /// <summary>
        /// Initializes a new <see cref="ControlMoveMouseGesture"/>
        /// with a target <see cref="Form"/>, a target 
        /// <see cref="Control"/> and the buttons pushed during the move
        /// </summary>
        /// <param name="form">
        /// Target form</param>
        /// <param name="targetControl">
        /// Target control
        /// </param>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        public ControlMoveMouseGesture(Form form, MouseButtons buttons,Control targetControl)
            :base(form,buttons)
        {
            if (targetControl==null)
                throw new ArgumentNullException("targetControl");
            this.controlKey = new ControlKey(targetControl);
        }

        /// <summary>
        /// Gets or sets the target <see cref="Control"/>
        /// </summary>
        /// <value>
        /// A <see cref="Control"/> instance where the cursor has to move
        /// </value>
        public Control TargetControl
        {
            get
            {
                return this.controlKey.FindControl(this.Form);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("TargetControl");
                this.controlKey = new ControlKey(value);
            }
        }

        /// <summary>
        /// Gets the center of the target <see cref="Control"/>.
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> representing the center of the 
        /// target control in client coordiantes
        /// </value>
        public override System.Drawing.Point Target
        {
            get 
            {
                Point center = new Point(
                    this.TargetControl.Location.X + this.Offset.X + this.TargetControl.ClientSize.Width / 2,
                    this.TargetControl.Location.Y + this.Offset.Y + this.TargetControl.ClientSize.Height/2
                    );
                return center;
            }
        }

        /// <summary>
        /// Offset of the target on the <see cref="Control"/>
        /// </summary>
        /// <value></value>
        public Point Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                this.offset = value;
            }
        }

        public override Expression ToCodeDom(Expression factory)
        {
            return factory.Method("MouseMove").Invoke(
                Expr.New(typeof(ControlKey), Expr.Prim(this.controlKey.Key))
                    .Method("FindControl").Invoke(factory.Prop("Form"))
                    );
        }
    }
}
