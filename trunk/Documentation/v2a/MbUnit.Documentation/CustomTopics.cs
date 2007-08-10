using System;
using System.IO;
using System.Text;
using DaveSexton.DocProject;
using System.Xml;

namespace MbUnit.Documentation
{
    public class CustomTopics
    {
        public static int Import(BuildContext context, string target)
        {
            int count;
            string rootFolder = context.ProjectDirectory;
            string sourceFolder = Path.Combine(rootFolder, "Custom Topics");
            string targetFolder = Path.Combine(rootFolder, target);

            count = CopyAllFilesInSourceToTarget(sourceFolder, targetFolder);

            HHCFile.MergeTOC(rootFolder, sourceFolder);
            HHKFile.MergeTOC(rootFolder, sourceFolder);
            return count;
        }

       


        private static string GetSandcastleTOC(string tocPath)
        {
            StreamReader sr = new StreamReader(tocPath);
            string contents = sr.ReadToEnd();
            sr.Close();
            System.Diagnostics.Debug.Write(">>>GetSandcastleTOC>>>");
            return contents;
        }

        private static int CopyAllFilesInSourceToTarget(string sourceFolder, string targetFolder)
        {
            int count = 0;
            System.Diagnostics.Debug.Write("---CopyAllFilesInSourceToTarget---");
            System.Diagnostics.Debug.Write("Target Folder:" + targetFolder);

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            string[] files = Directory.GetFileSystemEntries(sourceFolder);

            foreach (string source in files)
            {
                string target = Path.Combine(targetFolder, Path.GetFileName(source));

                if (Directory.Exists(source)) //Must be a directory
                    count += CopyAllFilesInSourceToTarget(source, target);
                else //Must be a file
                {
                    File.Copy(source, target, true);
                    count++;
                }
            }

            return count;
        }

        internal static int ImportHelp2(BuildContext context, string target)
        {
            //Content into Html2 folder
            //Table of Contents into .HxT

            throw new Exception("The method or operation is not implemented.");
        }
    }
}