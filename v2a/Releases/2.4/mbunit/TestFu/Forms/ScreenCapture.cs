using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TestFu.Forms
{
    /// <summary>
    /// Provides functions to capture the entire screen, or a particular window, and save it to a file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Source code takened from 
    /// http://www.developerfusion.com/show/4630/
    /// </para>
    /// </remarks>
    public sealed class ScreenCapture
    {
        private ScreenCapture() { }
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public static Image CaptureDesktop()
        {
            return Capture(User32.GetDesktopWindow());        
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public static Image CaptureMainWindow()
        {
            IntPtr handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            return Capture(handle);
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of the <see cref="Control"/>
        /// </summary>
        public static Image Capture(Control control)
        {
            return Capture(control.Handle);
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Image Capture(IntPtr handle)
        {
            // get te hDC of the target window
            using (User32DeviceContext hdcSrc = new User32DeviceContext(handle))
            {
                Size size = GetSize(handle);

                using (GDI32DeviceContext hdcDest = new GDI32DeviceContext(hdcSrc))
                {
                    // create a bitmap we can copy it to,
                    // using GetDeviceCaps to get the width/height
                    using (GDI32HBitmap bmp = new GDI32HBitmap(hdcSrc, size))
                    {
                        bmp.Blit(hdcSrc, hdcDest);
                        return bmp.GetImage();
                    }
                }
            }
        }

        private static Size GetSize(IntPtr handle)
        {
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            return new Size(width, height);
        }

        #region Exception safe wrappers
        private class User32DeviceContext : IDisposable
        {
            IntPtr handle;
            IntPtr dc;
            public User32DeviceContext(IntPtr handle)
            {
                this.handle = handle;
                this.dc = User32.GetWindowDC(handle);
            }

            public IntPtr DC
            {
                get
                {
                    return this.dc;
                }
            }
            #region IDisposable Members
            void IDisposable.Dispose()
            {
                try
                {
                    User32.ReleaseDC(handle, dc);
                }
                catch (Exception)
                { }
            }
            #endregion
        }

        private class GDI32DeviceContext : IDisposable
        {
            User32DeviceContext hdcSrc;
            IntPtr dc;
            public GDI32DeviceContext(User32DeviceContext hdcSrc)
            {
                this.hdcSrc = hdcSrc;
                this.dc = GDI32.CreateCompatibleDC(hdcSrc.DC);
            }

            public IntPtr DC
            {
                get
                {
                    return this.dc;
                }
            }
            #region IDisposable Members
            void IDisposable.Dispose()
            {
                this.hdcSrc = null;
                try
                {
                    // clean up 
                    GDI32.DeleteDC(this.dc);
                    this.dc = new IntPtr();
                }
                catch (Exception)
                { }
            }
            #endregion
        }

        private class GDI32HBitmap : IDisposable
        {
            private User32DeviceContext hdcSrc;
            private Size size;
            private IntPtr hBitmap;

            public GDI32HBitmap(User32DeviceContext hdcSrc, Size size)
            {
                this.hdcSrc = hdcSrc;
                this.size = size;
                this.hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc.DC, size.Width, size.Height);
            }

            public void Blit(User32DeviceContext hdcSrc, GDI32DeviceContext hdcDest)
            {
                // select the bitmap object
                IntPtr hOld = new IntPtr();
                try
                {
                    hOld = GDI32.SelectObject(hdcDest.DC, hBitmap);
                    // bitblt over
                    GDI32.BitBlt(
                        hdcDest.DC,
                        0, 0, size.Width, size.Height,
                        hdcSrc.DC,
                        0, 0, GDI32.SRCCOPY
                        );
                }
                finally
                {
                    // restore selection
                    if (hOld.ToInt32()!=0)
                        GDI32.SelectObject(hdcDest.DC, hOld);
                }
            }

            public Image GetImage()
            {
                // get a .NET image object for it
                Image img = Image.FromHbitmap(hBitmap);
                return img;
            }

            #region IDisposable Members
            void IDisposable.Dispose()
            {
                this.hdcSrc = null;
                try
                {
                    // clean up 
                    GDI32.DeleteObject(this.hBitmap);
                    this.hBitmap = new IntPtr();
                }
                catch (Exception)
                { }
            }
            #endregion
        }
        #endregion

        #region Interop
        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
        #endregion
    }
}