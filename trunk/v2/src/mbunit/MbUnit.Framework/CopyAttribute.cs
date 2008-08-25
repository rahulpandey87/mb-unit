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

namespace MbUnit.Framework {
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Tags a class whose assembly file and associated pdb file should be copied elsewhere
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CopyAttribute : Attribute {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAttribute"/> class.
        /// </summary>
        /// <param name="type">The type whose assembly will be copied</param>
        public CopyAttribute(Type type) {
            string assemblyFile = new Uri(type.Assembly.CodeBase).LocalPath;
            string pdbFile = Path.Combine(Path.GetDirectoryName(assemblyFile), Path.GetFileNameWithoutExtension(assemblyFile) + ".pdb");
            if (File.Exists(pdbFile)) { _files = new string[] { assemblyFile, pdbFile }; } else { _files = new string[] { assemblyFile }; }
        }

        /// <summary>
        /// Gets the files to be copied.
        /// </summary>
        /// <value>An array of the files being copied</value>
        public string[] Files {
            get { return _files; }
        }
        private string[] _files;

        /// <summary>
        /// Gets or sets the place for the files to be copied to.
        /// </summary>
        /// <value>The new path for the files</value>
        public string Destination {
            set { _destination = value; }
            get { return _destination; }
        }
        private string _destination;
    }
}
