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
using System.Runtime.Serialization;
using System.IO;
using System.CodeDom.Compiler;

namespace MbUnit.Core.Exceptions
{

   /// <summary>
   /// Exception thrown when not finding a vertex.
   /// </summary>
	[Serializable]
	public class CompilationException : AssertionException
	{		
		private string message;

      /// <summary>
      /// Initializes a <see cref="CompilationException"/> including details of test compilation failure
      /// </summary>
      /// <param name="compiler">The <see cref="System.CodeDom.Compiler.ICodeCompiler"/> being used in the compilation</param>
      /// <param name="parameters">The <see cref="System.CodeDom.Compiler.CompilerParameters"/> that were sent to the <paramref name="compiler"/></param>
      /// <param name="results">The <see cref="System.CodeDom.Compiler.CompilerResults"/> sent back from the compiler</param>
      /// <param name="sources">An array of source (code) being compiled</param>
		public CompilationException(
			ICodeCompiler compiler,
			CompilerParameters parameters,
            CompilerResults results,
			params String[] sources
			)
		{
			StringWriter sw = new StringWriter();
			sw.WriteLine("Compilation:  {0} errors",results.Errors.Count);
			sw.WriteLine("Compiler: {0}",compiler.GetType().Name);
			sw.WriteLine("CompilerParameters: {0}",parameters.ToString());
			foreach(CompilerError error in results.Errors)
			{
				sw.WriteLine(error.ToString());
			}
			sw.WriteLine("Sources:");
            foreach(string source in sources)
    			sw.WriteLine(source);

			this.message =sw.ToString();
		}

      /// <summary>
      /// The exception message
      /// </summary>
		public override string Message
		{
			get
			{
				return this.message;
			}
		}

	}
}
