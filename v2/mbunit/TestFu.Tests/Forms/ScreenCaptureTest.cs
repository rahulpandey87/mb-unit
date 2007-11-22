using System;
using System.Drawing;
using System.Drawing.Imaging;
using MbUnit.Core.Framework;
using MbUnit.Framework;

using TestFu.Forms;

namespace TestFu.Tests.Forms
{
    [TestFixture]
    public class ScreenCaptureTest
    {
        [Test]
        public void CaptureDesktop()
        {
            using (Image img = ScreenCapture.CaptureDesktop())
            {
                RenderImage(img,"Desktop.png");
            }
        }

        [Test]
        public void CaptureCurrentWindow()
        {
            using (Image img = ScreenCapture.CaptureMainWindow())
            {
                RenderImage(img, "Window.png");
            }
        }

        protected void RenderImage(Image img, string name)
        {
            using (Bitmap bmp = new Bitmap(img.Width / 2, img.Height / 2))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(img,
                        0, 0, bmp.Width, bmp.Height);
                }
                bmp.Save(name, ImageFormat.Png);
              //  System.Diagnostics.Process.Start(name);
            }
        }
    }
}
