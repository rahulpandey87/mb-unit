using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TestFu.Gestures
{
    internal sealed class Win32API
    {
        private Win32API() { }

    	public enum Messages
	    {
		    KeyDown  = 0x0100,
		    Char = 0x0102,
		    KeyUp = 0x0101,
		    MouseMove = 0x0200,
		    MouseDown = 0x0207,
		    MouseUp = 0x0208,
		    MouseDoubleClick = 0x0209,
		    MouseLeftDown = 0x0201,
		    MouseLeftUp = 0x0202,
		    MouseLeftDoubleClick = 0x0203,
		    MouseRightDown = 0x0204,
		    MouseRightUp = 0x0205,
		    MouseRightDoubleClick = 0x0206,
	    }

        #region DLL Imports
        [DllImport("user32.dll")]
        internal static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        internal static extern short VkKeyScan(char ch);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam,IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindowEx(IntPtr parent /*HWND*/, IntPtr next /*HWND*/, string sClassName, IntPtr sWindowTitle);

        [DllImport("user32")]
        internal static extern int GetDlgCtrlID(int hwnd);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr handleToWindow, StringBuilder windowText, int maxTextLength);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDlgItem(IntPtr handleToWindow, int ControlId);

        [DllImport("user32.dll")]
        internal static extern int GetClassName(IntPtr handleToWindow, StringBuilder className, int maxClassNameLength);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(int code, CBTCallback callbackFunction, IntPtr handleToInstance, int threadID);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr handleToHook);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(IntPtr handleToHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr handleToWindow, uint Message, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.Dll")]
        internal static extern IntPtr SendDlgItemMessage(IntPtr handleToWindow, int dlgItem, uint message, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal extern static int GetCursorPos(out Point lpWinPoint);

        [DllImport("user32.dll")]
        internal extern static bool ShowCursor(bool bShow);

        [DllImport("user32.dll")]
        internal extern static int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        internal extern static int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        public static extern int SystemParametersInfo(int uAction, int uParam, out int lpvParam, int fuWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool BlockInput(bool blockIt);

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        internal static extern int SendMouseInput(int cInputs, ref MOUSEINPUT pInputs, int cbSize);

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        internal static extern int SendKeyboardInput(int cInputs, ref KEYBDINPUT pInputs, int cbSize);
        #endregion
        #region Delegates

        internal delegate IntPtr CBTCallback(int code, IntPtr wParam, IntPtr lParam);
        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        #endregion
        #region Constants & Enums

        internal enum WindowMessages : uint
        {
            WM_CLOSE = 0x0010,
            WM_COMMAND = 0x0111
        }

        internal const int SPI_GETMOUSEHOVERTIME = 102;

        internal const int INPUT_KEYBOARD = 1;
        internal const int KEYEVENTF_KEYUP = 0x0002;
        internal const short VK_SHIFT = 0x10;
        internal const short VK_CONTROL = 0x11;
        internal const short VK_MENU = 0x12;

        [StructLayout(LayoutKind.Explicit, Size = 28)]
        internal struct KEYBDINPUT
        {
            [FieldOffset(0)]
            internal int type;
            [FieldOffset(4)]
            internal short wVk;
            [FieldOffset(6)]
            internal short wScan;
            [FieldOffset(8)]
            internal int dwFlags;
            [FieldOffset(12)]
            internal int time;
            [FieldOffset(16)]
            internal int dwExtraInfo;
        }

        internal const int INPUT_MOUSE = 0;
        internal const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        internal const int MOUSEEVENTF_LEFTDOWN = 0x2;
        internal const int MOUSEEVENTF_LEFTUP = 0x4;
        internal const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        internal const int MOUSEEVENTF_MIDDLEUP = 0x40;
        internal const int MOUSEEVENTF_MOVE = 0x1;
        internal const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        internal const int MOUSEEVENTF_RIGHTUP = 0x10;
        internal const int MOUSEEVENTF_WHEEL = 0x80;
        internal const int MOUSEEVENTF_XDOWN = 0x100;
        internal const int MOUSEEVENTF_XUP = 0x200;
        internal const int WHEEL_DELTA = 120;
        internal const int XBUTTON1 = 0x1;
        internal const int XBUTTON2 = 0x2;

        #endregion
        #region Structs

        internal struct MOUSEINPUT
        {
            public MOUSEINPUT(int mouseEvent)
            {
                type = INPUT_MOUSE;
                dx = 0;
                dy = 0;
                mouseData = 0;
                dwFlags = mouseEvent;
                time = 0;
                dwExtraInfo = 0;
            }

            internal int type;
            internal int dx;
            internal int dy;
            internal int mouseData;
            internal int dwFlags;
            internal int time;
            internal int dwExtraInfo;
        } ;

        internal struct Point
        {
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Point(float x, float y)
            {
                this.x = (int)x;
                this.y = (int)y;
            }

            internal int x;
            internal int y;
        }

        #endregion
    }
}