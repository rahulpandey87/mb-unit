using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VMTool.Core
{
    public static class FileUtil
    {
        public static bool ParseGlob(string pathGlob, out string basePath, out string pattern)
        {
            pattern = Path.GetFileName(pathGlob);
            if (pattern != null && HasWildcard(pattern))
            {
                basePath = Path.GetDirectoryName(pathGlob);
                return true;
            }

            pattern = null;
            basePath = null;
            return false;
        }

        public static bool HasWildcard(string pathGlob)
        {
            return pathGlob.Contains("?") || pathGlob.Contains("*");
        }

        public static bool TraverseFiles(string pathGlob, bool recursive,
            Action<FileInfo, string> fileAction, Action<DirectoryInfo, string> directoryAction)
        {
            string basePath, pattern;
            if (ParseGlob(pathGlob, out basePath, out pattern))
            {
                bool foundOne = false;

                DirectoryInfo baseDirectory = new DirectoryInfo(basePath);
                if (baseDirectory.Exists)
                {
                    foreach (FileSystemInfo entry in baseDirectory.GetFileSystemInfos(pattern))
                    {
                        foundOne = true;
                        TraverseFiles(entry, null, recursive, fileAction, directoryAction);
                    }
                }

                return foundOne;
            }
            else
            {
                FileInfo file = new FileInfo(pathGlob);
                if (file.Exists)
                {
                    fileAction(file, file.Name);
                    return true;
                }

                if (recursive)
                {
                    DirectoryInfo directory = new DirectoryInfo(pathGlob);
                    if (directory.Exists)
                    {
                        TraverseFiles(directory, null, recursive, fileAction, directoryAction);
                        return true;
                    }
                }

                return false;
            }
        }

        private static void TraverseFiles(FileSystemInfo entry, string baseRelativePath, bool recursive,
            Action<FileInfo, string> fileAction, Action<DirectoryInfo, string> directoryAction)
        {
            string relativePath = baseRelativePath != null ? Path.Combine(baseRelativePath, entry.Name) : entry.Name;

            if (entry is FileInfo)
            {
                fileAction((FileInfo)entry, relativePath);
            }
            else
            {
                if (recursive)
                {
                    DirectoryInfo directory = (DirectoryInfo)entry;
                    directoryAction(directory, relativePath);

                    foreach (FileSystemInfo childEntry in directory.GetFileSystemInfos())
                    {
                        TraverseFiles(childEntry, relativePath, true, fileAction, directoryAction);
                    }
                }
            }
        }
    }
}
