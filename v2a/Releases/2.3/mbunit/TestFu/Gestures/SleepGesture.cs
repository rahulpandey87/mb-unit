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
using System.Windows.Forms;
using Refly.CodeDom.Expressions;
using Refly.CodeDom;

namespace TestFu.Gestures
{
    /// <summary>
    /// A <see cref="IGesture"/> that makes the <see cref="Thread"/>
    /// sleep.
    /// </summary>
    public class SleepGesture : GestureBase
    {
        private int duration=0;

        /// <summary>
        /// Initialiazes a new <see cref="SleepGesture"/> instance.
        /// </summary>
        public SleepGesture()
        { }

        /// <summary>
        /// Initialiazes a new <see cref="SleepGesture"/> instance
        /// with a <see cref="Form"/> instance and a sleep duration
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/> instance</param>
        /// <param name="duration">
        /// Sleep duration in milliseconds
        /// </param>
        public SleepGesture(Form form, int duration)
            :base(form)
        {
            this.duration = duration;
        }

        /// <summary>
        /// Gets or sets the sleep duration (in milliseconds)
        /// </summary>
        /// <value>
        /// Number of milliseconds of sleep
        /// </value>
        public int Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
            }
        }

        /// <summary>
        /// Executes the sleep gestures
        /// </summary>
        public override void  Start()
        {
        	Thread.Sleep(this.duration);
        }

        public override Expression ToCodeDom(Expression factory)
        {
            return factory.Method("Sleep").Invoke(
                Expr.Prim(this.Duration)
                );
        }
    }
}
