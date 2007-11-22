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
using System.Drawing;
using System.Windows.Forms;
using Refly.CodeDom;
using Refly.CodeDom.Statements;
using Refly.CodeDom.Expressions;

namespace TestFu.Gestures
{
    /// <summary>
    /// A <see cref="IMouseGesture"/> that steers the cursor to a target
    /// </summary>
    public class FixedTargetMoveMouseGesture : MoveMouseGestureBase
    {
        private Point mouseTarget = new Point();

        /// <summary>
        /// Initializes a new <see cref="FixedTargetMoveMouseGesture"/>
        /// </summary>
        public FixedTargetMoveMouseGesture()
        { }

        /// <summary>
        /// Initializes a new <see cref="FixedTargetMoveMouseGesture"/>
        /// with a target <see cref="Form"/> and a target
        /// </summary>
        /// <param name="form">
        /// Target form</param>
        /// <param name="target">
        /// Target in client coordinates
        /// </param>
        public FixedTargetMoveMouseGesture(Form form, Point target)
            :base(form)
        {
            this.mouseTarget = target;
        }

        /// <summary>
        /// Initializes a new <see cref="FixedTargetMoveMouseGesture"/>
        /// with a target <see cref="Form"/> and a target
        /// </summary>
        /// <param name="form">
        /// Target form</param>
        /// <param name="target">
        /// Target in client coordinates
        /// </param>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        public FixedTargetMoveMouseGesture(Form form, MouseButtons buttons, Point target)
            :base(form,buttons)
        {
            this.mouseTarget = target;
        }

        /// <summary>
        /// Gets the target of the movement, in client coordinates.
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> in client coordinates
        /// </value>
        /// <remarks>
        /// <para>
        /// The property value is equal to <see cref="MouseTarget"/>.
        /// </para>
        /// </remarks>
        public override Point Target
        {
            get { return this.MouseTarget; }
        }

        /// <summary>
        /// Gets or sets the target of the movement, in client coordinates
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> in client coordinates
        /// </value>
        public Point MouseTarget
        {
            get { return this.mouseTarget; }
            set { this.mouseTarget = value; }
        }

        public override Expression ToCodeDom(Refly.CodeDom.Expressions.Expression factory)
        {
            return factory.Method("MouseMove").Invoke(
                Expr.New(typeof(Point), 
                    Expr.Prim(this.Target.X),
                    Expr.Prim(this.Target.Y)
                    )
                );
        }
    }
}
