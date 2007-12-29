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

namespace MbUnit.Core.Reports
{
	using MbUnit.Core.Reports.Serialization;
    using System.IO.IsolatedStorage;

	public abstract class ReportBase
	{
        protected const string AppPathRootName = "MbUnit";
        protected const string AppPathReportsName = "Reports";
		protected ReportBase()
		{
		}

		protected abstract string DefaultExtension {get;}

		protected virtual string DefaultNameFormat
		{
			get
			{
				return "mbunit-result-{0}{1}";
			}
		}

		protected virtual string GetFileName(
            ReportResult result, 
            string outputPath, 
            string nameFormat, 
            string extension) 
        {
            if (result == null)
                throw new ArgumentNullException("result");
            if (nameFormat == null)
                throw new ArgumentNullException("nameFormat");
            if (nameFormat.Length==0)
                throw new ArgumentException("Length is 0","nameFormat");
            if (extension == null)
                throw new ArgumentNullException("extension");
            if (extension.Length == 0)
                throw new ArgumentException("Length is 0", "extension");

            string outputFileName = String.Format(nameFormat + extension
				, result.Date.ToShortDateString()
				, result.Date.ToLongTimeString()
				);

            outputFileName = outputFileName
                                .Replace('/', '_')
                                .Replace('\\', '_')
                                .Replace(':', '_')
                                .Replace(' ', '_');

            outputPath = GetAppDataPath(outputPath);

            DirectoryCheckCreate(outputPath);

            outputFileName = Path.Combine(outputPath, outputFileName);

            return outputFileName;
		}

        public static string GetAppDataPath(string outputPath)
        {
            if (outputPath == null || outputPath.Length == 0)
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                outputPath = Path.Combine(appDataPath, AppPathRootName + @"\" + AppPathReportsName);
            }
            return outputPath;
        }

        public static void DirectoryCheckCreate(string outputPath)
        {
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
        }

		/// <summary>
		/// Render the report result to the specified writer
		/// </summary>
		/// <param name="result">Result from the test</param>
		/// <param name="writer">Writer to write result output to</param>
		public abstract void Render(ReportResult result, TextWriter writer);

		/// <summary>
		/// Render the report result to a file 
		/// </summary>
		/// <param name="result">Result from the test</param>
		/// <param name="fileName">Report output file name </param>
		public virtual void Render(ReportResult result, string fileName)
		{
              bool isProfiling = Environment.GetEnvironmentVariable("cor_enable_profiling") == "1";
              if (!isProfiling)
              {
                  if (result == null)
                      throw new ArgumentNullException("result");
                  if (fileName == null)
                      throw new ArgumentNullException("fileName");
                  if (fileName.Length == 0)
                      throw new ArgumentException("Length is 0", "fileName");

                  using (StreamWriter writer = new StreamWriter(fileName))
                  {
                      // this will create a UTF-8 format with no preamble. 
                      // We might need to change that if it create a problem with internationalization
                      Render(result, writer);
                  }
              }
		}

		/// <summary>
		/// Render the report result to a file 
		/// </summary>
		/// <param name="result">Result from the test</param>
		/// <param name="outputPath">Output directory</param>
		/// <param name="nameFormat">Default format name</param>
		/// <param name="extension">Extension of the file</param>
		/// <returns>File name of the report</returns>
		public virtual string Render(ReportResult result, string outputPath, string nameFormat, string extension)
		{
             bool isProfiling = Environment.GetEnvironmentVariable("cor_enable_profiling") == "1";
             if (!isProfiling)
             {
                 if (result == null)
                     throw new ArgumentNullException("result");
                 if (nameFormat == null)
                     throw new ArgumentNullException("nameFormat");
                 if (nameFormat.Length == 0)
                     throw new ArgumentException("Length is 0", "nameFormat");
                 if (extension == null)
                     throw new ArgumentNullException("extension");
                 if (extension.Length == 0)
                     throw new ArgumentException("Length is 0", "extension");

                 string fileName = GetFileName(result, outputPath, nameFormat, extension);
                 Render(result, fileName);
                 return fileName;
             }
             else
             {
                 return "";
             }

		}

		/// <summary>
		/// Render the report result to a file 
		/// </summary>
		/// <param name="result">Result from the test</param>
		/// <param name="outputPath">Output directory</param>
		/// <param name="nameFormat">Default format name. If null, the default name will be used</param>
		/// <returns>File name of the report</returns>
		public virtual string Render(ReportResult result, string outputPath, string nameFormat)
		{
            bool isProfiling = Environment.GetEnvironmentVariable("cor_enable_profiling") == "1";
            if (!isProfiling)
            {
                if (result == null)
                    throw new ArgumentNullException("result");

                if (nameFormat == null)
                    nameFormat = DefaultNameFormat;
                return Render(result, outputPath, nameFormat, DefaultExtension);
            }
            else
            {
                return "";
            }
		}

        public virtual string Render(ReportResult result)
        {
            bool isProfiling = Environment.GetEnvironmentVariable("cor_enable_profiling") == "1";
            if (!isProfiling)
            {
                if (result == null)
                    throw new ArgumentNullException("result");
                return Render(result, "", null);
            }
            else
            {
                return "";
            }
        }

	}
}
