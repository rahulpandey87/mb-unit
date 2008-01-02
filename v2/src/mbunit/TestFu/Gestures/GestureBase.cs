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

namespace TestFu.Gestures
{
    /// <summary>
    /// Abstract base class for <see cref="IGesture"/>
    /// implementation.
    /// </summary>
    public abstract class GestureBase : IGesture
    {
        #region Fields
        private Form form;
        private delegate Point PointConversionDelegate(Point p);
        private PointConversionDelegate pointToScreen;
        private PointConversionDelegate pointToClient; 
        #endregion

        #region Contructors
        /// <summary>
        /// Initializes an empty <see cref="IGesture"/>.
        /// </summary>
        public GestureBase()
        { }

        /// <summary>
        /// Initializes a new <see cref="IGesture"/>
        /// instance with a form
        /// </summary>
        /// <param name="form"></param>
        public GestureBase(Form form)
        {
            this.Form = form;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="Form"/> instance targeted
        /// by the <see cref="IGesture"/>
        /// </summary>
        /// <value>
        /// A <see cref="Form"/> instance
        /// </value>
        public virtual Form Form
        {
            get
            {
                if (this.form == null)
                    throw new InvalidOperationException("Form must be setup first");
                return this.form;
            }
            set
            {
                if (this.form != null)
                {
                    this.form = null;
                    this.pointToScreen = null;
                    this.pointToClient = null;
                }
                this.form = value;
                if (this.form != null)
                {
                    this.pointToScreen = new PointConversionDelegate(this.Form.PointToScreen);
                    this.pointToClient = new PointConversionDelegate(this.Form.PointToClient);
                }
                this.OnFormChanged(EventArgs.Empty);
            }
        } 
        #endregion  

        #region Events
        /// <summary>
        /// Raised when the target <see cref="Form"/> is changed
        /// </summary>
        public event EventHandler FormChanged;

        /// <summary>
        /// Raises the <see cref="FormChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFormChanged(EventArgs e)
        {
            if (this.FormChanged != null)
                this.FormChanged(this, e);
        } 
        #endregion

        #region Form utilities
        /// <summary>
        /// Converts the target from client to
        /// screen coordinates
        /// </summary>
        /// <param name="target">
        /// Position in client coordinates</param>
        /// <returns>
        /// Position converted into screen coordinates
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method makes a thread safe invokation of the
        /// <c>Form.PointToScreen</c> method.
        /// </para>
        /// </remarks>
        protected Point PointToScreen(Point target)
        {
            Point p = (Point)this.Form.Invoke(
                this.pointToScreen, new object[] { target });
            return p;
        }

        /// <summary>
        /// Converts the target from screen to
        /// client coordinates
        /// </summary>
        /// <param name="target">
        /// Position in screen coordinates</param>
        /// <returns>
        /// Position converted into client coordinates
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method makes a thread safe invokation of the
        /// <c>Form.PointToClient</c> method.
        /// </para>
        /// </remarks>
        protected Point PointToClient(Point target)
        {
            Point p = (Point)this.Form.Invoke(
                this.pointToClient, new object[] { target });
            return p;
        } 
        #endregion

        #region Overridable members
        /// <summary>
        /// Executes the gesture.
        /// </summary>
        public abstract void Start();

        public abstract Expression ToCodeDom(Expression factory);
        #endregion
    }
}
