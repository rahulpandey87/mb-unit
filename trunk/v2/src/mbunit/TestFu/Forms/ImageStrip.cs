using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace TestFu.Forms
{
    public class ImageStrip : IDisposable
    {
        private float scaleFactor;
        private Hashtable images = new Hashtable();

        public ImageStrip(float scaleFactor)
        {
            this.scaleFactor = scaleFactor;
        }

        public ICollection Images
        {
            get
            {
                return this.images.Values;
            }
        }

        public Image this[Object key]
        {
            get
            {
                return (Image)this.images[key];
            }
        }

        public bool Contains(Object key)
        {
            return this.images.Contains(key);
        }

        public void Add(Object key, Image img)
        {
            Bitmap bmp = Rescale(img);
            this.images.Add(key, bmp);
        }

        public void Dispose()
        {
            foreach (Image img in this.images)
            {
                img.Dispose();
            }
            this.images.Clear();
        }

        protected Bitmap Rescale(Image img)
        {
            int width = (int)(img.Width * scaleFactor);
            int height = (int)(img.Height * scaleFactor);
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(img,
                        0, 0, bmp.Width, bmp.Height);
                }
                return bmp;
            }
            catch (Exception)
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                }
                throw;
            }
        }
    }
}
