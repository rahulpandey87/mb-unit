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
using System.Collections;

namespace TestFu.Gestures
{
    /// <summary>
    /// A <see cref="IGestureCollection"/> implementation.
    /// </summary>
    internal class GestureCollection : CollectionBase, IGestureCollection
    {
        public IGesture this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (GestureBase)value;
            }
        }

        public void Add(IGesture gesture)
        {
            if (gesture == null)
                throw new ArgumentNullException("gesture");
            this.List.Add(gesture);
        }

        public new IEnumerator GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }
    }
}
