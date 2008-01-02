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
using System.IO;
using System.Xml.Xsl;

using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Core.Reports
{
	/// <summary>
	/// Reports MbUnit result in text format.
	/// </summary>
	public sealed class TextReport : XslTransformReport
	{
		public TextReport()
		{
            Transform = ResourceHelper.ReportTextTransform;
        }

		protected override string DefaultExtension
		{
			get
            {
                return ".txt";
            }
		}

		public static string RenderToText(ReportResult result)
		{
			TextReport textReport = new TextReport();
			return textReport.Render(result);
		}

		public static string RenderToText(ReportResult result, string outputPath, string nameFormat)
		{
			return RenderToText(result, outputPath, null, nameFormat);
		}

        public static string RenderToText(ReportResult result, string outputPath, string transform, string nameFormat)
        {
            if (result == null)
                throw new ArgumentNullException("result");
            if (nameFormat == null)
                throw new ArgumentNullException("nameFormat");
            if (nameFormat.Length == 0)
                throw new ArgumentException("Length is zero", "nameFormat");

            TextReport textReport = new TextReport();
            if (transform != null)
            {
                if (!File.Exists(transform))
                    throw new ArgumentException("Transform does not exist.", "transform");
                XslTransform xsl = new XslTransform();
                xsl.Load(transform);
                textReport.Transform = xsl;
            }
            return textReport.Render(result, outputPath, nameFormat);
        }
	}
}
