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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TestFu.Gestures
{
    /// <summary>
    /// A static helper for artificially generationg mouse
    /// and keyboard input.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class uses <c>mouse_event</c> and <c>keybd_event</c> native
    /// methods (through interop) to simulate user mouse or keyboard input.
    /// </para>
    /// </remarks>
    public sealed class VirtualInput
    {
        private static IntPtr zeroVal = new IntPtr(0);
        private VirtualInput()
        { }

        #region Flags
        /// <summary>
        /// Mouse even type enumeration
        /// </summary>
        [Flags]
        public enum MouseEventType
        {
            /// <summary>
            /// No event
            /// </summary>
            None = 0,
            /// <summary>
            /// Mouse move where dx,dy are in absolute coordinate
            /// </summary>
            Absolute = 0x8000,
            /// <summary>
            /// Left button bown
            /// </summary>
            LeftDown = 0x2,
            /// <summary>
            /// Left button up
            /// </summary>
            LeftUp = 0x4 ,
            /// <summary>
            /// Middle button down
            /// </summary>
            MiddleDown = 0x20,
            /// <summary>
            /// middle button up
            /// </summary>
            MiddleUp = 0x40,
            /// <summary>
            /// Mouse moved
            /// </summary>
            Move = 0x1 ,
            /// <summary>
            /// Right button down
            /// </summary>
            RightDown = 0x8 ,
            /// <summary>
            /// Right button up
            /// </summary>
            RightUp = 0x10 ,
            /// <summary>
            /// Mouse wheel movement
            /// </summary>
            Wheel = 0x80 ,
            /// <summary>
            /// Additional button down
            /// </summary>
            XDown = 0x100,
            /// <summary>
            /// Additional button up
            /// </summary>
            Xup = 0x200 ,
        }
        #endregion

        #region Mouse events
        /// <summary>
        /// Generates a mouse event
        /// </summary>
        /// <param name="mouseEventType">
        /// Combined flag describing the mouse event
        /// </param>
        public static void MouseEvent(MouseEventType mouseEventType)
        {
            MouseEvent(mouseEventType, 0, 0, 0);
        }

        /// <summary>
        /// Mouse event with additional data
        /// </summary>
        /// <param name="mouseEventType">
        /// Combined flag describing the mouse event
        /// </param>
        /// <param name="dx">
        /// Relative horizontal movement of the cursor
        /// </param>
        /// <param name="dy">
        /// Relative vertical movement of the cursor</param>
        /// <param name="dwData">
        /// Additional data
        /// </param>
        public static void MouseEvent(
            MouseEventType mouseEventType,
            int dx, int dy, int dwData)
        {
            Win32API.mouse_event(
                (uint)mouseEventType,
                dx, dy, dwData, new UIntPtr()
                );
        }

        /// <summary>
        /// Move mouse of units
        /// </summary>
        /// <param name="dx">
        /// horizontal movement</param>
        /// <param name="dy">
        /// vertical movement
        /// </param>
        public static void MouveMouse(int dx, int dy)
        {
            MouseEvent(MouseEventType.Move,dx,dy,0);
        }

        /// <summary>
        /// Notfies that a mouse movement is starting
        /// with the buttons settings
        /// </summary>
        /// <param name="buttons">
        /// Combined flag describing the current button
        /// state
        /// </param>
        public static void BeginMouveMouse(MouseButtons buttons)
        {
            MouseEventType type = GetDownType(buttons);
            type |= MouseEventType.Move;
            MouseEvent(type, 0, 0,0);
        }

        /// <summary>
        /// Notfies that a mouse movement is finishing
        /// with the buttons settings
        /// </summary>
        /// <param name="buttons">
        /// Combined flag describing the current button
        /// state
        /// </param>
        public static void EndMouveMouse(MouseButtons buttons)
        {
            MouseEventType type = GetUpType(buttons);
            type |= MouseEventType.Move;
            MouseEvent(type, 0, 0,0);
        }

        /// <summary>
        /// Mouse click using button state
        /// </summary>
        /// <param name="buttons">
        /// Combined flag describing the current button
        /// state
        /// </param>
        public static void MouseClick(MouseButtons buttons)
        {
            MouseDown(buttons);
            MouseUp(buttons);
        }

        /// <summary>
        /// Mouse down event
        /// </summary>
        /// <param name="buttons"></param>
        public static void MouseDown(MouseButtons buttons)
        {
            MouseEventType downType = GetDownType(buttons);
            MouseEvent(downType);
        }

        /// <summary>
        /// Mouse up event
        /// </summary>
        /// <param name="buttons"></param>
        public static void MouseUp(MouseButtons buttons)
        {
            MouseEventType upType = GetUpType(buttons);
            MouseEvent(upType);
        }

        /// <summary>
        /// Mouse wheel event
        /// </summary>
        /// <param name="value">
        /// Wheel movement</param>
        public static void MouseWheel(int value)
        {
            MouseEvent(MouseEventType.Wheel,
                0,
                0,
                value
                );
        }

        private static MouseEventType GetDownType(MouseButtons buttons)
        {
            MouseEventType type = MouseEventType.None;

            if ((buttons & MouseButtons.Left) != 0)
                type |= MouseEventType.LeftDown;
            if ((buttons & MouseButtons.Middle) != 0)
                type |= MouseEventType.MiddleDown;
            if ((buttons & MouseButtons.Right) != 0)
                type |= MouseEventType.RightDown;
            if ((buttons & MouseButtons.XButton1) != 0)
                type |= MouseEventType.XDown;
            if ((buttons & MouseButtons.XButton2) != 0)
                type |= MouseEventType.XDown;
            return type;
        }
        private static MouseEventType GetUpType(MouseButtons buttons)
        {
            MouseEventType type = MouseEventType.None;

            if ((buttons & MouseButtons.Left) != 0)
                type |= MouseEventType.LeftUp;
            if ((buttons & MouseButtons.Middle) != 0)
                type |= MouseEventType.MiddleUp;
            if ((buttons & MouseButtons.Right) != 0)
                type |= MouseEventType.RightUp;
            if ((buttons & MouseButtons.XButton1) != 0)
                type |= MouseEventType.Xup;
            if ((buttons & MouseButtons.XButton2) != 0)
                type |= MouseEventType.Xup;
            return type;
        }
        #endregion

        #region Keyboard events
        public static void KeyDown(IntPtr hwnd,char character)
        {
            Win32API.SendMessage(hwnd, (uint)Win32API.Messages.KeyDown, (IntPtr)character, zeroVal);
            Win32API.SendMessage(hwnd, (uint)Win32API.Messages.Char, (IntPtr)character, zeroVal);
        }
        public static void KeyUp(IntPtr hwnd, char character)
        {
            Win32API.SendMessage(hwnd, (uint)Win32API.Messages.Char, (IntPtr)character, zeroVal);
            Win32API.SendMessage(hwnd, (uint)Win32API.Messages.KeyUp, (IntPtr)character, zeroVal);
        }
        /// <summary>
        /// Simulates a Key action (KeyDown, Key, KeyUp message sequence)
        /// </summary>
        /// <param name="character">character pressed</param>
        /// <param name="hwnd">handle of control to receive the event</param>
        public static void PressKey(IntPtr hwnd, char character)
        {
            Win32API.SendMessage(hwnd, (uint)Win32API.Messages.KeyDown, (IntPtr)character, zeroVal);
            Win32API.SendMessage(hwnd, (uint)Win32API.Messages.Char, (IntPtr)character, zeroVal);
            Win32API.SendMessage(hwnd, (uint)Win32API.Messages.KeyUp, (IntPtr)character, zeroVal);
        }

        /// <summary>
        /// Simulates a Backspace
        /// </summary>
        /// <param name="hwnd">handle of control to receive the event</param>
        public static void PressBackspace(IntPtr hwnd)
        {
            PressKey(hwnd,(char)0x08);
        }

        /// <summary>
        /// Simulates a user typing text
        /// </summary>
        /// <param name="text">text to enter</param>
        /// <param name="hwnd">handle of control to receive the event</param>
        public void Type(IntPtr hwnd, string text)
        {
            char[] chars = text.ToCharArray();
            foreach (char c in chars)
                PressKey(hwnd,c);
        }
        #endregion
    }
}
