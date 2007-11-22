using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random data generator for <see cref="Bitmap"/> binary values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="IDataGenerator"/> method generates a bitmap of size
	/// [<see cref="BinaryGeneratorBase.MaxLength"/> by <see cref="BinaryGeneratorBase.MaxLength"/>].
	/// </para>
	/// </remarks>
	public class ImageBinaryGenerator : BinaryGeneratorBase,
        IRangeDataGenerator
	{
        private PixelFormat pixelFormat = PixelFormat.Format24bppRgb;

        public ImageBinaryGenerator(DataColumn column)
		:base(column)
		{}

		/// <summary>
		/// Gets the generated type
		/// </summary>
		/// <value>
		/// Generated type.
		/// </value>
		public override Type GeneratedType 
		{
			get
			{
				return typeof(System.Byte[]);
			}
		}
		
        /// <summary>
        /// Gets or sets the pixel format
        /// </summary>
        /// <value></value>
		public PixelFormat PixelFormat
		{
			get
			{
				return this.pixelFormat;
			}
			set
			{
				this.pixelFormat = value;
			}
		}
		
		protected override void GenerateBytes(DataRow row)
		{
			int width = this.MaxLength;
			int height = this.MaxLength;

            using (Bitmap bmp = new Bitmap(width, height, this.pixelFormat))
            {
                // messing up the bmp
                PaintBitmap(bmp);

                // create stream
                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Save(stream, ImageFormat.Bmp);
                    this.FillData(row, stream.ToArray());
                }
            }
        }
		
		protected virtual void PaintBitmap(Bitmap bmp)
		{}
	}
}
