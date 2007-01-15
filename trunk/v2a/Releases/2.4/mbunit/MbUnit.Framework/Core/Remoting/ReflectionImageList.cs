// MbUnit Test Framework
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
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing.Drawing2D;

namespace MbUnit.Core.Remoting
{
	/// <summary>
	/// Summary description for ImageListBuilder.
	/// </summary>
	public class ReflectionImageList
	{
		private ImageList list;
		private Color notRunColor = Color.WhiteSmoke;
		private Color runningColor = Color.Blue;
		private Color successColor = Color.LightGreen;
		private Color failureColor = Color.Red;
		private Color ignoredColor = Color.Orange;
        private Color skipColor = Color.BlueViolet;

        private static int testStateCount = Enum.GetNames(typeof(TestState)).Length;

		public ReflectionImageList(ImageList list)
		{
			if (list==null)
				throw new ArgumentNullException("list");

			this.list = list;

			Populate();
		}

		public static Image LoadImage(TestNodeType nodeType)
		{
			Assembly a = Assembly.GetExecutingAssembly();
			string imgName = String.Format("MbUnit.Framework.Core.{0}.png",nodeType.ToString());
			Stream s = a.GetManifestResourceStream(imgName);

			// get icon
			Image icon = Image.FromStream(s);
			return icon;
		}

		public static void LoadAndSaveImage(TestNodeType nodeType, string outputPath)
		{
			using(Image img = LoadImage(nodeType))
			{
				string path = Path.Combine(outputPath,nodeType.ToString())+".png";
				img.Save(path);
			}
		}

		public void Populate()
		{
			this.list.Images.Clear();

			// load icons
			Assembly a = Assembly.GetExecutingAssembly();
			// loading icons
			foreach(string name in Enum.GetNames(typeof(TestNodeType)))
			{
				string imgName = String.Format("MbUnit.Framework.Core.{0}.png",name);
				Stream s = a.GetManifestResourceStream(imgName);

				// get icon
				Image icon = Image.FromStream(s);

				// we are going to derive this icon in three flavors:
				// green background, success test
				this.list.Images.Add( ColoredIcon(icon, this.notRunColor) );
				this.list.Images.Add( ColoredIcon(icon, this.runningColor) );
				this.list.Images.Add( ColoredIcon(icon, this.successColor) );
				this.list.Images.Add( ColoredIcon(icon, this.failureColor) );
				this.list.Images.Add( ColoredIcon(icon, this.ignoredColor) );
                this.list.Images.Add(ColoredIcon(icon, this.skipColor));
            }	
		}

		public static int ImageIndex(TestNodeType type, TestState state)
		{
			return (int)type*testStateCount+(int)state;
		}

		public static int NotRunImageIndex(int imageIndex)
		{
			return (imageIndex/testStateCount)*testStateCount + (int)TestState.NotRun;
		}

		private Image ColoredIcon(Image icon, Color c)
		{
			Bitmap bmp = new Bitmap(icon.Width, icon.Height);

			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.FillRectangle(Brushes.White,-1,-1,bmp.Width+2,bmp.Height+2);
				Rectangle r = new Rectangle(0,0,bmp.Width,bmp.Height);
				LinearGradientBrush brush = new LinearGradientBrush(
					r,
					c,
					Color.FromArgb(200,c.R,c.G,c.B),
					45,
					true
					);
				g.FillEllipse(brush,1,1,bmp.Width-2,bmp.Height-2);
				g.DrawImage(icon,0,0);
			}

			return bmp;
		}
	}
}
