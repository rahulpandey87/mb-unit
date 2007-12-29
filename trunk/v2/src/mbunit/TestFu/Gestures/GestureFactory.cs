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
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace TestFu.Gestures
{
    /// <summary>
    /// A helper factory of <see cref="IGesture"/> instance.
    /// </summary>
    public class GestureFactory
    {
        #region fields
		private Form form;
    	#endregion        
        
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="GestureFactory"/>
        /// with a <see cref="Form"/> instance
        /// </summary>
        /// <param name="form">
        /// Target form</param>
        public GestureFactory(Form form)
        {
            if (form == null)
                throw new ArgumentNullException("form");
            this.form=form;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the target <see cref="Form"/> instance
        /// </summary>
        /// <value>
        /// A <see cref="Form"/> instance that is targetted by the gestures
        /// </value>
        public Form Form
        {
            get
            {
                return this.form;
            }
        }
        #endregion

        #region Static helper methods
        /// <summary>
        /// Creates a <see cref="Thread"/> for the 
        /// <see cref="IGesture.Start"/> method and starts it.
        /// </summary>
        /// <param name="gesture">
        /// The <see cref="IGesture"/> to execute
        /// </param>
        /// <returns>
        /// The started <see cref="Thread"/> instance
        /// </returns>
        public static Thread Start(IGesture gesture)
        {
            ThreadStart start = new ThreadStart(gesture.Start);
            Thread thread = new Thread(start);
            thread.Start();
            return thread;
        }
        #endregion

        #region Misc. gestures
        /// <summary>
        /// Creates a <see cref="SequenceGesture"/> instance
        /// with a variable list of <see cref="IGesture"/> instances.
        /// </summary>
        /// <param name="gestures">
        /// gestures to execute in sequence.
        /// </param>
        /// <returns>
        /// A <see cref="SequenceGesture"/> instance</returns>
        public SequenceGesture Sequence(params IGesture[] gestures)
        {
            if (gestures.Length == 0)
                throw new ArgumentException("Length is 0", "gestures");

            SequenceGesture seq = new SequenceGesture(this.form);
            foreach (GestureBase g in gestures)
            {
                seq.Gestures.Add(g);
            }
            return seq;
        }

        /// <summary>
        /// Creates a <see cref="SleepGesture"/> that makes the
        /// thread sleep a given number of milliseconds
        /// </summary>
        /// <param name="duration">
        /// Duration in milliseconds of the sleep
        /// </param>
        /// <returns>
        /// A <see cref="SleepGesture"/> instance
        /// </returns>
        public SleepGesture Sleep(int duration)
        {
            return new SleepGesture(this.form, duration);
        }

        /// <summary>
        /// Creates a new <see cref="RepeatGesture"/> with
        /// the gesture and the repeat count
        /// </summary>
        /// <param name="gesture">
        /// Target <see cref="IGesture"/> instance
        /// </param>
        /// <param name="repeatCount">
        /// Number of repetition
        /// </param>
        /// <returns>
        /// A <see cref="RepeatGesture"/> instance
        /// </returns>
        public RepeatGesture Repeat(IGesture gesture, int repeatCount)
        {
            if (gesture == null)
                throw new ArgumentNullException("gesture");
            return new RepeatGesture(this.Form, gesture, repeatCount);

        }
        #endregion

        #region Mouse gesture
        #region Clicks
        /// <summary>
        /// Creates a <see cref="ClickMouseGesture"/>
        /// that simulates a left click of the mouse
        /// </summary>
        /// <returns>
        /// A <see cref="ClickMouseGesture"/> instance
        /// </returns>
        public ClickMouseGesture MouseClick()
        {
            return new ClickMouseGesture(this.form);
        }

        /// <summary>
        /// Creates a <see cref="ClickMouseGesture"/>
        /// that simulates a left click of the mouse
        /// </summary>
        /// <returns>
        /// A <see cref="ClickMouseGesture"/> instance
        /// </returns>
        /// <param name="control">
        /// Control to click
        /// </param>
        public SequenceGesture MouseClick(Control control)
        {
            return Sequence(
                MouseMove(control),
                MouseClick()
                );
        }

        /// <summary>
        /// Creates a <see cref="ClickMouseGesture"/>
        /// that simulates a left click of the mouse
        /// </summary>
        /// <returns>
        /// A <see cref="ClickMouseGesture"/> instance
        /// </returns>
        /// <param name="control">The control to click.</param>
        /// <param name="buttons">Which button(s) to use.</param>
        public SequenceGesture MouseClick(Control control, MouseButtons buttons)
        {
            return Sequence(
                MouseMove(control),
                MouseClick(buttons)
                );
        }

        /// <summary>
        /// Creates a <see cref="ClickMouseGesture"/>
        /// that simulates a left click of the mouse
        /// </summary>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        /// <returns>
        /// A <see cref="ClickMouseGesture"/> instance
        /// </returns>
        public ClickMouseGesture MouseClick(MouseButtons buttons)
        {
            return new ClickMouseGesture(this.form,buttons);
        }

        /// <summary>
        /// Creates a new <see cref="ButtonDownMouseGesture"/> instance
        /// that simulates a Mouse down event (left click)
        /// </summary>
        /// <returns>
        /// A <see cref="ButtonDownMouseGesture"/> instance
        /// </returns>
        public ButtonDownMouseGesture MouseDown()
        {
            return new ButtonDownMouseGesture(this.Form);
        }

        /// <summary>
        /// Creates a new <see cref="ButtonDownMouseGesture"/> instance
        /// that simulates a Mouse down event with the buttons
        /// </summary>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        /// <returns>
        /// A <see cref="ButtonDownMouseGesture"/> instance
        /// </returns>
        public ButtonDownMouseGesture MouseDown(MouseButtons buttons)
        {
            return new ButtonDownMouseGesture(this.Form, buttons);
        }

        /// <summary>
        /// Creates a new <see cref="ButtonUpMouseGesture"/> instance
        /// that simulates a Mouse up event (left click)
        /// </summary>
        /// <returns>
        /// A <see cref="ButtonUpMouseGesture"/> instance
        /// </returns>
        public ButtonUpMouseGesture MouseUp()
        {
            return new ButtonUpMouseGesture(this.Form);
        }

        /// <summary>
        /// Creates a new <see cref="ButtonUpMouseGesture"/> instance
        /// that simulates a Mouse up event with the buttons
        /// </summary>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        /// <returns>
        /// A <see cref="ButtonUpMouseGesture"/> instance
        /// </returns>
        public ButtonUpMouseGesture MouseUp(MouseButtons buttons)
        {
            return new ButtonUpMouseGesture(this.Form, buttons);
        }
        #endregion

        #region Movement
        /// <summary>
        /// Creates a <see cref="FixedTargetMoveMouseGesture"/>
        /// that simulates the movement of the mouse to the target
        /// </summary>
        /// <param name="target">
        /// Target client coordinate
        /// </param>
        /// <returns>
        /// A <see cref="FixedTargetMoveMouseGesture"/> instance
        /// </returns>
        public FixedTargetMoveMouseGesture MouseMove(Point target)
        {
            return new FixedTargetMoveMouseGesture(this.form, target);
        }

        /// <summary>
        /// Creates a <see cref="FixedTargetMoveMouseGesture"/>
        /// that simulates the movement of the mouse to the target
        /// and the buttons down
        /// </summary>
        /// <param name="target">
        /// Target client coordinate
        /// </param>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        /// <returns>
        /// A <see cref="FixedTargetMoveMouseGesture"/> instance
        /// </returns>
        public FixedTargetMoveMouseGesture MouseMove(Point target, MouseButtons buttons)
        {
            return new FixedTargetMoveMouseGesture(this.form, buttons, target);
        }

        /// <summary>
        /// Creates a <see cref="ControlMoveMouseGesture"/>
        /// that simulates the movement of the mouse to the center
        /// of the <see cref="Control"/>
        /// </summary>
        /// <param name="control">
        /// Target <see cref="Control"/> instance
        /// </param>
        /// <returns>
        /// A <see cref="ControlMoveMouseGesture"/> instance
        /// </returns>
        public ControlMoveMouseGesture MouseMove(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            return new ControlMoveMouseGesture(this.form, control);
        }

        /// <summary>
        /// Creates a <see cref="ControlMoveMouseGesture"/>
        /// that simulates the movement of the mouse to the center
        /// of the <see cref="Control"/> with the buttons down
        /// </summary>
        /// <param name="control">
        /// Target <see cref="Control"/> instance
        /// </param>
        /// <param name="buttons">
        /// value representing the <see cref="MouseButtons"/>
        /// involved in the gesture
        /// </param>
        /// <returns>
        /// A <see cref="ControlMoveMouseGesture"/> instance
        /// </returns>
        public ControlMoveMouseGesture MouseMove(Control control,MouseButtons buttons)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            return new ControlMoveMouseGesture(this.form, buttons, control);
        }
        #endregion

        #region Drag and drop
        /// <summary>
        /// Creates a <see cref="IGesture"/> instance that simulates
        /// a drag and drop between <paramref name="source"/>
        /// and <paramref name="target"/>
        /// </summary>
        /// <param name="source">
        /// Source client coordinate</param>
        /// <param name="target">
        /// Target client coordinate
        /// </param>
        /// <returns>
        /// A <see cref="IGesture"/> instance that simulates the drag and drop
        /// </returns>
        public SequenceGesture MouseDragAndDrop(Point source, Point target)
        {
            return Sequence(
                MouseMove(source),
                MouseDown(),
                MouseMove(target, MouseButtons.Left),
                MouseUp()
                );
        }

        /// <summary>
        /// Creates a <see cref="IGesture"/> instance that simulates
        /// a drag and drop between <paramref name="source"/>
        /// and <paramref name="target"/>
        /// </summary>
        /// <param name="source">
        /// Source client coordinate</param>
        /// <param name="target">
        /// Target <see cref="Control"/>
        /// </param>
        /// <returns>
        /// A <see cref="IGesture"/> instance that simulates the drag and drop
        /// </returns>
        public SequenceGesture MouseDragAndDrop(Point source, Control target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            return Sequence(
                MouseMove(source),
                MouseDown(),
                MouseMove(target, MouseButtons.Left),
                MouseUp()
                );
        }

        /// <summary>
        /// Creates a <see cref="IGesture"/> instance that simulates
        /// a drag and drop between <paramref name="source"/>
        /// and <paramref name="target"/>
        /// </summary>
        /// <param name="source">
        /// Source <see cref="Control"/>
        /// </param>
        /// <param name="target">
        /// Target client coordinate
        /// </param>
        /// <returns>
        /// A <see cref="IGesture"/> instance that simulates the drag and drop
        /// </returns>
        public SequenceGesture MouseDragAndDrop(Control source, Point target)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return Sequence(
                MouseMove(source),
                MouseDown(),
                MouseMove(target, MouseButtons.Left),
                MouseUp()
                );
        }
        public SequenceGesture MouseDragAndDrop(Control source, Control target, MouseButtons buttons)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return Sequence(
                MouseMove(source),
                MouseDown(buttons),
                MouseMove(target, buttons),
                MouseUp(buttons)
                );
        }

        /// <summary>
        /// Creates a <see cref="IGesture"/> instance that simulates
        /// a drag and drop between <paramref name="source"/>
        /// and <paramref name="target"/>
        /// </summary>
        /// <param name="source">
        /// Source <see cref="Control"/>
        /// </param>
        /// <param name="target">
        /// Target <see cref="Control"/>
        /// </param>
        /// <returns>
        /// A <see cref="IGesture"/> instance that simulates the drag and drop
        /// </returns>
        public SequenceGesture MouseDragAndDrop(Control source, Control target)
        {
            if (source == null)
                throw new ArgumentNullException("control");
            if (target == null)
                throw new ArgumentNullException("target");

            return Sequence(
                MouseMove(source),
                MouseDown(),
                MouseMove(target, MouseButtons.Left),
                MouseUp()
                );
        }

        /// <summary>
        /// Creates a <see cref="IGesture"/> instance that simulates
        /// a drag and drop between <paramref name="source"/>
        /// and <paramref name="target"/>
        /// </summary>
        /// <param name="source">
        /// Source <see cref="ListViewItem"/>
        /// </param>
        /// <param name="target">
        /// Target client coordinate
        /// </param>
        /// <returns>
        /// A <see cref="IGesture"/> instance that simulates the drag and drop
        /// </returns>
        public SequenceGesture MouseDragAndDrop(ListViewItem source, Control target)
        {
            if (source == null)
                throw new ArgumentNullException("item");
            if (target == null)
                throw new ArgumentNullException("target");
            throw new NotSupportedException("Not supported under 1.0");
/*           
            Point p = new Point(
                source.ListView.Location.X + source.Position.X + 5,
                source.ListView.Location.Y + source.Position.Y + 5
                );
            return this.MouseDragAndDrop(
                p, target);
*/
        }
        #endregion
        #endregion

        #region Keyboard gesture
        public PressKeyGesture PressKey(char c)
        {
            return new PressKeyGesture(this.form, c);       
        }
        public KeyDownGesture KeyDown(char c)
        {
            return new KeyDownGesture(this.form, c);
        }
        public KeyUpGesture KeyUp(char c)
        {
            return new KeyUpGesture(this.form, c);
        }
        #endregion
    }
}
