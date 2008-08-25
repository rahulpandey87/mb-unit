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
namespace MbUnit.Framework {

    /// <summary>
    /// 	<para>
    /// When you need to test a console application, this task usually involves calling the application using
    /// Process.Start and monitoring that process. This class simplifies this.</para>
    /// 	<list type="bullet">
    /// 		<item>it redirects output and error streams so that they are logged in the report</item>
    /// 		<item>it checks the exit code</item>
    /// 		<item>a time out can be specified</item>
    /// 	</list>
    /// </summary>
    /// <example>
    /// 	<para>This example launches the MbUnit console application without any parameter or with various combinations:</para>
    /// 	<code>
    /// using System;
    /// using MbUnit.Core.Framework;
    /// using MbUnit.Framework;
    /// namespace MyTests
    /// {
    /// [TestFixture]
    /// public class ConsoleTest
    /// {
    /// private ConsoleTester tester;
    /// [Test]
    /// public void NoArguments()
    /// {
    /// this.tester = new ConsoleTester("MbUnit.Cons.exe");
    /// this.tester.ExpectedExitCode = 0;
    /// this.tester.Run();
    /// }
    /// [Test]
    /// public void Help()
    /// {
    /// this.tester = new ConsoleTester("MbUnit.Cons.exe","--help");
    /// this.tester.ExpectedExitCode = 0;
    /// this.tester.Run();
    /// }
    /// }
    /// }
    /// </code>
    /// </example>
    public class ConsoleTester {
        private string fileName;
        private string[] args;
        private int expectedExitCode = 0;
        private int timeOut = 60000;
        private Process process;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleTester" /> class. 
        /// </summary>
        /// <param name="fileName">The name of the console application to be run</param>
        /// <param name="args">An array of arguments to send to the console application</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fileName"/> is null</exception>
        public ConsoleTester(
            string fileName,
            params string[] args) {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            this.fileName = fileName;
            this.args = args;
        }

        /// <summary>
        /// Gets or set the expected exit code from the application once it has completed its run
        /// </summary>
        /// <value>The expected exit code (an integer).</value>
        public int ExpectedExitCode {
            get { return this.expectedExitCode; }
            set { this.expectedExitCode = value; }
        }

        /// <summary>
        /// Gets or sets the time out.
        /// </summary>
        /// <value>The time out in seconds</value>
        public int TimeOut {
            get { return this.timeOut; }
            set { this.timeOut = value; }
        }

        /// <summary>
        /// Gets the arguments for the console application
        /// </summary>
        /// <returns>The arguments for the console application as a space-delimited string</returns>
        protected string GetArguments() {
            StringWriter sw = new StringWriter();
            foreach (string arg in this.args)
                sw.Write(" {0} ", arg);
            return sw.ToString();
        }


        /// <summary>
        /// Runs the console app with the given arguments
        /// </summary>
        public void Run() {
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
            Assert.IsTrue(timedOut, "Process timed out");

            // check return value
            Assert.AreEqual(this.ExpectedExitCode, process.ExitCode,
                "Process ExitCode ({0}) does not match expected ExitCode ({1})",
                this.process.ExitCode, this.ExpectedExitCode);
        }

        /// <summary>
        /// Writes the standard output and error streams from the app to the console
        /// </summary>
        protected void DumpConsoles() {
            if (this.process == null)
                return;

            Console.Out.WriteLine(process.StandardOutput.ReadToEnd());
            Console.Error.WriteLine(process.StandardError.ReadToEnd());
        }
    }
}
