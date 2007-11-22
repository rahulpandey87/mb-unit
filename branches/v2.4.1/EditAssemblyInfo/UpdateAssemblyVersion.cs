using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace MbUnit.EditAssemblyInfo
{
    // msbuild xml
    // <UsingTask AssemblyFile="$(OutputPath)\EditAssemblyInfo\EditAssemblyInfo.dll" TaskName="MbUnit.EditAssemblyInfo.UpdateAssemblyVersionTask" />

	// <Target Name="BeforeBuild">	  
	//      <MSBuild Projects="EditAssemblyInfo\EditAssemblyInfo.sln" Targets="Clean;Rebuild" Properties="OutputPath=$(OutputPath)\EditAssemblyInfo" />
	//      <UpdateAssemblyVersionTask FilePath="$(MSBuildProjectDirectory)\MbUnit\" VersionMajor="$(VersionMajor)" VersionMinor="$(VersionMinor)" VersionBuild="$(VersionBuild)" />
 	// </Target>

    public class UpdateAssemblyVersionTask : Task
    {
        public UpdateAssemblyVersionTask()
        { }

        private string filePath = string.Empty;
        private string versionMajor = "0";
        private string versionMinor = "0";
        private string versionBuild = "0";

        [Required]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        
        [Required]
        public string VersionMajor
        {
            get { return versionMajor; }
            set { versionMajor = value; }
        }

        [Required]
        public string VersionMinor
        {
            get { return versionMinor; }
            set { versionMinor = value; }
        }

        [Required]
        public string VersionBuild
        {
            get { return versionBuild; }
            set { versionBuild = value; }
        }

        public override bool Execute()
        {
            // Update AssemblyVersion in shared AssemblyInfo.cs
            string pathFileName = Path.Combine(filePath, "AssemblyInfo.cs");
            if (File.Exists(pathFileName))
            {
                // Read in whole file
                string assemblyInfo;
                try
                {
                    StreamReader sr = new StreamReader(pathFileName);
                    assemblyInfo = sr.ReadToEnd();
                    sr.Close();
                }
                catch
                {
                    return false;
                }

                // Find and replace AssemblyVersion attribute
                string regexPattern = "AssemblyVersion\\(\"([^\"]*)\"\\)";
                string result = Regex.Replace(assemblyInfo, regexPattern, string.Format("AssemblyVersion(\"{0}.{1}.{2}\")",
                    VersionMajor, VersionMinor, VersionBuild));

                // Write updated file
                try
                {
                    StreamWriter sw = new StreamWriter(pathFileName, false);
                    sw.Write(result);
                    sw.Close();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}