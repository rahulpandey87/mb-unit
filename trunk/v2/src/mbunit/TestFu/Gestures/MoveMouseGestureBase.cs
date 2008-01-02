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

namespace TestFu.Gestures
{
    /// <summary>
    /// An abstract base class for 
    /// <see cref="IMouseGesture"/> implementations 
    /// that simulates a mouse movement
    /// </summary>
    public abstract class MoveMouseGestureBase : MouseGestureBase
    {
        private Point maxVelocity = new Point(3,3);

        /// <summary>
        /// Initializes a new <see cref="MoveMouseGestureBase"/>
        /// </summary>
        public MoveMouseGestureBase()
        { }

        /// <summary>
        /// Initialiazes a new <see cref="MoveMouseGestureBase"/>
        /// with a target form and no buttons
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/>
        /// </param>
        public MoveMouseGestureBase(Form form)
            :base(form,MouseButtons.None)
        {}

        /// <summary>
        /// Initialiazes a new <see cref="MoveMouseGestureBase"/>
        /// with a target form and the buttons
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/>
        /// </param>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        public MoveMouseGestureBase(Form form, MouseButtons buttons)
            :base(form,buttons)
        {}

        /// <summary>
        /// Gets the target of the movement, in client coordinates
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> in client coordinates
        /// </value>
        public abstract Point Target {get;}

        /// <summary>
        /// Gets or sets a value indicating the maximum velocity of the
        /// cursor
        /// </summary>
        /// <value>
        /// A <see cref="Point"/> representing the maximum velocity of the cursor
        /// </value>
        public Point MaxVelocity
        {
            get
            {
                return this.maxVelocity;
            }
            set
            {
                this.maxVelocity = value;
            }
        }

        /// <summary>
        /// Steers the mouse towards the target
        /// </summary>
        public override void Start()
        {
            // get target
            Point t = this.Target;

            // get current mouse position
            Point c = this.PointToClient(Cursor.Position);

            if (this.Buttons!=MouseButtons.None)
                VirtualInput.BeginMouveMouse(this.Buttons);

            while (c != t)
            {
                int dx = Math.Min(this.MaxVelocity.X, Math.Max(-this.MaxVelocity.X, t.X - c.X));
                int dy = Math.Min(this.MaxVelocity.Y, Math.Max(-this.MaxVelocity.Y, t.Y - c.Y));
                if (Math.Abs(dx) < 2 && Math.Abs(dy) < 2)
                    return;
                VirtualInput.MouveMouse(
                    dx,
                    dy
                    );

                c = this.PointToClient(Cursor.Position);
            }

            if (this.Buttons != MouseButtons.None)
                VirtualInput.EndMouveMouse(this.Buttons);
        }
    }
}
