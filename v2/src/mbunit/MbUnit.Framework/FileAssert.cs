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
using MbUnit.Core.Exceptions;

namespace MbUnit.Framework
{
    /// <summary>
    /// Class containing generic assert methods for the comparison of <see cref="FileInfo">files</see> and <see cref="Stream">streams</see>.
    /// </summary>
    public sealed class FileAssert
    {
        /// <summary>
        /// Private constructor so you can't create an instance of the FileAssert class
        /// </summary>
        private FileAssert()
        {}


        /// <summary>
        /// Verifies that the <paramref name="actualPath"/> to a file is the same as the <paramref name="expectedPath"/>
        /// </summary>
        /// <param name="expectedPath">The expected path.</param>
        /// <param name="actualPath">The actual path.</param>
        public static void AreEqual(string expectedPath, string actualPath)
        {
            FileInfo expected = new FileInfo(expectedPath);
            AreEqual(expected, actualPath);
       }

        /// <summary>
        /// Verifies that the path to the <paramref name="expected"/> file identified as a <see cref="FileInfo"/> is the 
        /// same as the actual <paramref name="filePath"/>
        /// </summary>
        /// <param name="expected">A <see cref="FileInfo"/> object representing the expected file</param>
        /// <param name="filePath">The actual file path in the test as a string.</param>
        public static void AreEqual(FileInfo expected, string filePath)
        {
            FileInfo actual = new FileInfo(filePath);
            AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies that two files have the same length and contents as each other.
        /// </summary>
        /// <param name="expected"><see cref="FileInfo"/> object representing the expected file and contents</param>
        /// <param name="actual"><see cref="FileInfo"/> object representing the actual file and contents</param>
        /// <exception cref="AssertionException">Thrown if either <see cref="FileInfo"/> object is null</exception>
        public static void AreEqual(FileInfo expected, FileInfo actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            Assert.AreEqual(expected.Length, actual.Length, "File length is not the same");
            using (Stream expectedStream = expected.OpenRead())
            {
                using (Stream actualStream = actual.OpenRead())
                {
                    AreStreamContentEqual(expectedStream, actualStream);
                }
            }
        }


        /// <summary>
        /// Verifies that two <see cref="Stream"/>s are readable and have the same contents as each other.
        /// </summary>
        /// <param name="expected"><see cref="Stream"/> object representing the expected results</param>
        /// <param name="actual"><see cref="Stream"/> object representing the actual results so far in the test</param>
        /// <exception cref="AssertionException">Thrown if either <see cref="Stream"/> object is null</exception>
        public static void AreStreamContentEqual(Stream expected, Stream actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            Assert.IsTrue(expected.CanRead);
            Assert.IsTrue(actual.CanRead);

            using (StreamReader esr = new StreamReader(expected))
            {
                using (StreamReader asr = new StreamReader(actual))
                {
                    string eline;
                    string aline;
                    do
                    {
                        eline = esr.ReadLine();
                        aline = asr.ReadLine();
                        Assert.AreEqual(eline, aline);
                    } while (eline != null);
                }
            }
        }

        /// <summary>
        /// Verifies that the file specified by <paramref name="fileName"/> exists
        /// </summary>
        /// <param name="fileName">The path of the file.</param>
        /// <exception cref="AssertionException">Thrown if <paramref name="fileName"/> is null</exception>
        public static void Exists(string fileName)
        {
            Assert.IsNotNull(fileName, "FileName is null");
            Assert.IsTrue(File.Exists(fileName), "file {0} does not exist", fileName);
        }

        /// <summary>
        /// Verifies that the file specified by <paramref name="fileName"/> does not exist
        /// </summary>
        /// <param name="fileName">The path of the file.</param>
        /// <exception cref="AssertionException">Thrown if <paramref name="fileName"/> is null</exception>
        public static void NotExists(string fileName)
        {
            Assert.IsNotNull(fileName, "FileName is null");
            Assert.IsFalse(File.Exists(fileName), "file {0} exists", fileName);
        }
    }
}
