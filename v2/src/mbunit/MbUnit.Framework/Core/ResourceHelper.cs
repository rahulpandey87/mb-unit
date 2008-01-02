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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux


using System;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MbUnit.Core.Remoting;

namespace MbUnit.Core
{
	/// <summary>
	/// Static helper functions for retreiving resources
	/// </summary>
	public sealed class ResourceHelper
	{
		private ResourceHelper()
		{}

		public static XmlTextReader ReportSchema
		{
			get
			{
				Assembly a = typeof(ResourceHelper).Assembly;
                Stream s = a.GetManifestResourceStream("MbUnit.Framework.Core.Reports.mbunit-report.xsd");
                return new XmlTextReader(s);
			}
		}

		public static XslTransform ReportHtmlTransform
		{
			get
			{
				Assembly a = typeof(ResourceHelper).Assembly;
                Stream s = a.GetManifestResourceStream("MbUnit.Framework.Core.Reports.mbunit-report.html.xsl");
                if (s == null)
                    throw new InvalidOperationException("Bug: Could not find resource");
                XslTransform xsl = new XslTransform();				
				xsl.Load(new XmlTextReader(s),null);

				return xsl;
			}
		}

		public static XslTransform ReportTextTransform
		{
			get
			{
				Assembly a = typeof(ResourceHelper).Assembly;
                Stream s = a.GetManifestResourceStream("MbUnit.Framework.Core.Reports.mbunit-report.txt.xsl");
                XslTransform xsl = new XslTransform();			
				xsl.Load(new XmlTextReader(s),null);

				return xsl;
			}
		}


        public static XslTransform ReportCruiseControlTransform
        {
            get
            {
                Assembly a = typeof(ResourceHelper).Assembly;
                Stream s = a.GetManifestResourceStream("MbUnit.Framework.Core.Reports.mbunit-report.cruisecontrol.xsl");
                XslTransform xsl = new XslTransform();
                xsl.Load(new XmlTextReader(s), null);

                return xsl;
            }
        }
        
        public static Image LogoImage
		{
			get
			{
                return LoadImageFromResource("MbUnit.Framework.Core.Reports.mbunitlogo.png");
            }
		}

		public static Image LogoIcon
		{
			get
			{
                return LoadImageFromResource("MbUnit.Framework.Core.Reports.mbuniticon.gif");
            }
		}

        /// <summary>
        /// Creates and saves the images in the directory with the specified path.
        /// </summary>
        /// <param name="path">The directory path in which to save the images</param>
        public static void CreateImages(string path)
        {
            string directory = Path.GetFullPath(path);
            if (directory.Length > 0)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
            using (Image logoImage = LogoImage)
            {
                logoImage.Save(Path.Combine(directory, "mbunitlogo.png"), ImageFormat.Png);
            }
            using (Image logoIcon = LogoIcon)
            {
                logoIcon.Save(Path.Combine(directory, "mbuniticon.gif"), ImageFormat.Gif);
            }
            foreach (TestNodeType nodeType in Enum.GetValues(typeof(TestNodeType)))
            {
                ReflectionImageList.LoadAndSaveImage(nodeType, directory);
            }
        }

        private static Image LoadImageFromResource(string name)
		{
			return Image.FromStream(GetManifestStream(name));
		}

		private static Stream GetManifestStream(string name)
		{
			Assembly a = typeof(ResourceHelper).Assembly;
			Stream s = a.GetManifestResourceStream(name);
			if (s==null)
				throw new Exception("Could not find " + name + " resource.");
			return s;
		}
	}
}
