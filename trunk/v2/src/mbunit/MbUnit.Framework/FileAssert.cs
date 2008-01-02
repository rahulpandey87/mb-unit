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

namespace MbUnit.Framework
{
    public sealed class FileAssert
    {
        private FileAssert()
        {}

        public static void AreEqual(string expectedPath, string actualPath)
        {
            FileInfo expected = new FileInfo(expectedPath);
            AreEqual(expected, actualPath);
       }

        public static void AreEqual(FileInfo expected, string filePath)
        {
            FileInfo actual = new FileInfo(filePath);
            AreEqual(expected, actual);
        }

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

        public static void Exists(string fileName)
        {
            Assert.IsNotNull(fileName, "FileName is null");
            Assert.IsTrue(File.Exists(fileName), "file {0} does not exist", fileName);
        }

        public static void NotExists(string fileName)
        {
            Assert.IsNotNull(fileName, "FileName is null");
            Assert.IsFalse(File.Exists(fileName), "file {0} exists", fileName);
        }
    }
}
