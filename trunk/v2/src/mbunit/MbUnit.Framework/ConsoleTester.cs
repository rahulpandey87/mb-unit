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
using System.Diagnostics;
using System.IO;
namespace MbUnit.Framework
{
    public class ConsoleTester
    {
        private string fileName;
        private string[] args;
        private int expectedExitCode = 0;
        private int timeOut = 60000;
        private Process process;

        public ConsoleTester(
            string fileName,
            params string[] args)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            this.fileName = fileName;
            this.args = args;
        }

        public int ExpectedExitCode
        {
            get { return this.expectedExitCode; }
            set { this.expectedExitCode = value; }
        }

        public int TimeOut
        {
            get { return this.timeOut; }
            set { this.timeOut = value; }
        }

        protected string GetArguments()
        {
            StringWriter sw = new StringWriter();
            foreach(string arg in this.args)
                sw.Write(" {0} ",arg);
            return sw.ToString();
        }

        public void Run()
        {
            ProcessStartInfo info = new ProcessStartInfo(
                this.fileName,
                this.GetArguments()
                );
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;

            Console.WriteLine("Process command line:");
            Console.WriteLine("\"{0}\" {1}", info.FileName, info.Arguments);

            this.process = Process.Start(info);
            bool timedOut = process.WaitForExit(this.TimeOut);

            this.DumpConsoles();

            // check time out
            Assert.IsTrue(timedOut,"Process timed out");

            // check return value
            Assert.AreEqual(this.ExpectedExitCode, process.ExitCode,
                "Process ExitCode ({0}) does not match expected ExitCode ({1})",
                this.process.ExitCode, this.ExpectedExitCode);
        }

        protected void DumpConsoles()
        {
            if (this.process == null)
                return;

            Console.Out.WriteLine(process.StandardOutput.ReadToEnd());
            Console.Error.WriteLine(process.StandardError.ReadToEnd());
        }
    }
}
