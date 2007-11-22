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
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MbUnit.Core.Reports
{
	using MbUnit.Core.Reports.Serialization;
	/// <summary>
	/// Summary description for XmlReport.
	/// </summary>
	public sealed class XmlReport : ReportBase
	{
		private static XmlSerializer serializer = new XmlSerializer(typeof(ReportResult));
	
		public XmlReport()
		{
		}

		protected override string DefaultExtension
		{
			get { return ".xml"; }
		}

		public override void Render(ReportResult result, TextWriter writer)
		{
            XmlTextWriter xmlWriter = new XmlTextWriter(writer);
				xmlWriter.Formatting = Formatting.Indented;
				serializer.Serialize(xmlWriter, result);
				xmlWriter.Flush();
				xmlWriter.Close();
		}

		public static string RenderToXml(ReportResult result)
		{
			XmlReport xmlReport = new XmlReport();
			return xmlReport.Render(result);
		}
        public static void RenderToXml(ReportResult result, TextWriter writer)
        {
            XmlReport xmlReport = new XmlReport();
            xmlReport.Render(result,writer);
        }
        public static string RenderToXml(ReportResult result, string outputPath, string nameFormat)
		{
			XmlReport xmlReport = new XmlReport();
			return xmlReport.Render(result, outputPath, nameFormat);
		}


	}
}
