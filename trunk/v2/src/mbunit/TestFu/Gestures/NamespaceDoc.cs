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

namespace TestFu.Gestures
{
    /// <summary>
    /// <para>
    /// The <b>TestFu.Gestures</b> namespace contains classes (<see cref="IGesture"/>
    /// instances) to simulate user mouse and keyboard interactions.
    /// </para>
    /// <para>
    /// The user interaction are simulated by using native methods
    /// <c>mouse_event</c> and <c>keybd_event</c>.
    /// </para>
    /// <para>
    /// The <see cref="GestureFactory"/> can be used to rapidly generate 
    /// <see cref="IGesture"/> instances.
    /// </para>
    /// <para>
    /// The gestures should not be executed in the main thread but in a worker thread. Otherwize,
    /// you will miss message notifications. All gesture methods on <see cref="System.Windows.Forms.Control"/>
	/// and <see cref="System.Windows.Forms.Form"/> are thread safe.
    /// </para>
    /// </summary>
    internal sealed class NamespaceDoc
    {
        private NamespaceDoc()
        {}
    }
}
