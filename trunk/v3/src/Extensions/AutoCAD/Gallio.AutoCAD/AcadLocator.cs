// Copyright 2005-2009 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan de Halleux
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using Microsoft.Win32;

namespace Gallio.AutoCAD
{
    /// <summary>
    /// Finds the AutoCAD install location based on registry keys.
    /// </summary>
    public class AcadLocator : IAcadLocator
    {
        private const string ExeName = "acad.exe";

        /// <inheritdoc/>
        public string GetMostRecentlyUsed()
        {
            var path = SearchRegistry();
            if (!File.Exists(path))
                throw new FileNotFoundException("Unable to find AutoCAD.");
            return path;
        }

        private static string SearchRegistry()
        {
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Autodesk\DWGCommon\shellex\Apps"))
            {
                if (regKey == null)
                    return null;

                var subKeyName = regKey.GetValue(null) as string;
                if (subKeyName == null)
                    return null;

                using (RegistryKey subKey = regKey.OpenSubKey(subKeyName))
                {
                    if (subKey == null)
                        return null;

                    var launchCommand = subKey.GetValue("OpenLaunch") as string;
                    if (launchCommand == null)
                        return null;

                    int acadIndex = launchCommand.IndexOf(ExeName, StringComparison.OrdinalIgnoreCase);
                    if (acadIndex < 0)
                        return null;

                    int startIndex = (launchCommand[0] == '"') ? 1 : 0;
                    int length = acadIndex + ExeName.Length - startIndex;

                    return launchCommand.Substring(startIndex, length);
                }
            }
        }
    }
}
