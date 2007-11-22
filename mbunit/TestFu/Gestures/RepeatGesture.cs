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
using Refly.CodeDom;
using Refly.CodeDom.Expressions;

namespace TestFu.Gestures
{
    /// <summary>
    /// A <see cref="IGesture"/> that executes a Repeat of <see cref="IGesture"/>
    /// instances.
    /// </summary>
    public class RepeatGesture : GestureBase
    {
        private IGesture gesture;
        private int repeatCount = 1;

        /// <summary>
        /// Initializes a new <see cref="RepeatGesture"/> instance
        /// </summary>
        public RepeatGesture()
        { }

        /// <summary>
        /// Initialiazes a new <see cref="RepeatGesture"/> instance
        /// with a <see cref="Form"/> instance.
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/>
        /// </param>
        /// <param name="gesture">
        /// The gesture to be repeated
        /// </param>
        /// <param name="repeatCount">
        /// The number of repetition</param>
        public RepeatGesture(Form form, IGesture gesture, int repeatCount)
            :base(form)
        {
            if (gesture == null)
                throw new ArgumentNullException("gesture");
            this.gesture = gesture;
        }

        /// <summary>
        /// Gets the collection of <see cref="IGesture"/> to execute in Repeat
        /// </summary>
        /// <value>
        /// A <see cref="IGestureCollection"/> instance
        /// </value>
        public IGesture Gesture
        {
            get
            {
                return this.gesture;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Gesture");
                this.gesture = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of gesture repetition
        /// </summary>
        /// <value>
        /// The repetition count
        /// </value>
        public int RepeatCount
        {
            get
            {
                return this.repeatCount;
            }
            set
            {
                this.repeatCount = value;
            }
        }

        /// <summary>
        /// Executes the <see cref="IGesture"/> contained in
        /// <see cref="Gestures"/> in Repeat.
        /// </summary>
        public override void Start()
        {
            for(int i= 0;i<this.repeatCount;++i)
            {
                this.Gesture.Start();
                System.Threading.Thread.Sleep(0);
            }
        }

        public override Expression ToCodeDom(Expression factory)
        {
            return factory.Method("Repeat").Invoke(
                this.Gesture.ToCodeDom(factory),
                Expr.Prim(this.RepeatCount)
                );
        }
    }
}
