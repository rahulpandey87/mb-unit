using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Text.RegularExpressions;


namespace MbUnit.EditAssemblyInfo
{
    //msbuild xml
 // <UsingTask AssemblyFile="..\EditAssemblyInfo\bin\Debug\EditAssemblyInfo.dll" TaskName="MbUnit.EditAssemblyInfo.UpdateAssemblyTitleTask" />


 // <Target Name="BeforeBuild">	  
 //       <UpdateAssemblyTitleTask FilePath="E:\My Documents\Visual Studio 2005\Projects\EditAssemblyInfo\Debugger\Properties" VersionMajor="2" VersionMinor="1" VersionBuild="3" />
 //</Target>


    public class UpdateAssemblyTitleTask : Task
    {
        public UpdateAssemblyTitleTask()
        {

        }

        private string filePath = string.Empty;
        [Required]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private string versionMajor = "0";
        [Required]
        public string VersionMajor
        {
            get { return versionMajor; }
            set { versionMajor = value; }
        }

        private string versionMinor = "0";
        [Required]
        public string VersionMinor
        {
            get { return versionMinor; }
            set { versionMinor = value; }
        }

        private string versionBuild = "0";
        [Required]
        public string VersionBuild
        {
            get { return versionBuild; }
            set { versionBuild = value; }
        }



        public override bool Execute()
        {
            DirectoryInfo directoryRoot = new DirectoryInfo(FilePath);

            foreach (DirectoryInfo dir in directoryRoot.GetDirectories())
            {
                string dirName = dir.Name;
                string pathFileName = Path.Combine(dirName, @"AssemblyInfoAdditional.cs");
                pathFileName = Path.Combine(filePath, pathFileName);

                if (File.Exists(pathFileName))
                {
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

                    string regexPattern = "AssemblyTitle\\(\"([^\"]*)\"\\)";
                    Regex assemblyTitle = new Regex(regexPattern);

                    string result = Regex.Replace(assemblyInfo, regexPattern, string.Format("AssemblyTitle(\"{3} {0}.{1}.{2}\")", VersionMajor, VersionMinor, VersionBuild, dirName));

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
            }

            return true;
        }

    }
}
