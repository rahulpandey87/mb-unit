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
using Refly.CodeDom;
using Refly.CodeDom.Statements;

namespace TestFu.Gestures
{
    /// <summary>
    /// A <see cref="IGesture"/> that executes a sequence of <see cref="IGesture"/>
    /// instances.
    /// </summary>
    public class SequenceGesture : GestureBase
    {
        private IGestureCollection gestures = new GestureCollection();

        /// <summary>
        /// Initializes a new <see cref="SequenceGesture"/> instance
        /// </summary>
        public SequenceGesture()
        { }

        /// <summary>
        /// Initialiazes a new <see cref="SequenceGesture"/> instance
        /// with a <see cref="Form"/> instance.
        /// </summary>
        /// <param name="form">
        /// Target <see cref="Form"/>
        /// </param>
        public SequenceGesture(Form form)
            :base(form)
        { }

        /// <summary>
        /// Gets the collection of <see cref="IGesture"/> to execute in sequence
        /// </summary>
        /// <value>
        /// A <see cref="IGestureCollection"/> instance
        /// </value>
        public IGestureCollection Gestures
        {
            get
            {
                return this.gestures;
            }
        }

        /// <summary>
        /// Executes the <see cref="IGesture"/> contained in
        /// <see cref="Gestures"/> in sequence.
        /// </summary>
        public override void Start()
        {
            foreach (IGesture gesture in this.Gestures)
            {
                gesture.Start();
                System.Threading.Thread.Sleep(0);
            }
        }

        public override Form Form
        {
            get
            {
                return base.Form;
            }
            set
            {
                base.Form = value;
                foreach (IGesture gesture in this.Gestures)
                    gesture.Form = value;
            }
        }

        public override Expression ToCodeDom(Expression factory)
        {
            Expression[] expressions = new Expression[this.Gestures.Count];
            int index = 0;
            foreach (IGesture gesture in this.Gestures)
            {
                expressions[index] = gesture.ToCodeDom(factory);
                index++;
            }
            return factory.Method("Sequence").Invoke(expressions);
        }
    }
}
