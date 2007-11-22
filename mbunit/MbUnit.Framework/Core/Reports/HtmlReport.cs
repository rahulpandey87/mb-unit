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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MbUnit.Core.Remoting;

namespace MbUnit.Core.Reports
{
	using MbUnit.Core.Reports.Serialization;

	public sealed class HtmlReport : XslTransformReport
	{
		public HtmlReport()
		{
		}

		protected override string DefaultExtension
		{
			get { return ".html"; }
		}

		public override void Render(ReportResult result, TextWriter writer)
		{
			base.Transform = ResourceHelper.ReportHtmlTransform;
			base.Render(result, writer);
		}

		public static string RenderToHtml(ReportResult result)
		{
            if (result == null)
                throw new ArgumentNullException("result");

            HtmlReport htmlReport = new HtmlReport();
            ResourceHelper.CreateImages(GetAppDataPath(""));
            return htmlReport.Render(result);
		}

		public static string RenderToHtml(ReportResult result, string outputPath, string nameFormat)
		{
            if (result == null)
                throw new ArgumentNullException("result");
            if (nameFormat == null)
                throw new ArgumentNullException("nameFormat");
            if (nameFormat.Length == 0)
                throw new ArgumentException("Length is zero", "nameFormat");

            HtmlReport htmlReport = new HtmlReport();

            outputPath = GetAppDataPath(outputPath);
            
            ResourceHelper.CreateImages(outputPath);

            return htmlReport.Render(result, outputPath, nameFormat);
        }
	}
}
