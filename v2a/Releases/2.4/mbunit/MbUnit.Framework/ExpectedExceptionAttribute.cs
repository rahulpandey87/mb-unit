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
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux
using System;

using MbUnit.Framework;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
	/// <summary>
	/// Tags method that should throw an exception.
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='ExpectedExceptionAttribute']"/>	
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ExpectedExceptionAttribute : DecoratorPatternAttribute
    {
		Type exceptionType = null;
        string expectedMessage = null;
        Type innerExceptionType = null;
		
		public ExpectedExceptionAttribute(Type exceptionType) 
            : this(exceptionType, null, null)
		{
		}

        public ExpectedExceptionAttribute(Type exceptionType, Type innerExceptionType)
            : this(exceptionType, null, innerExceptionType)
        {
        }

        public ExpectedExceptionAttribute(Type exceptionType, string expectedMessage)
            : this(exceptionType, expectedMessage, null)
        {
        }

        public ExpectedExceptionAttribute(Type exceptionType, string expectedMessage, Type innerExceptionType)
        {
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");
            this.exceptionType = exceptionType;
            this.expectedMessage = expectedMessage;
            this.innerExceptionType = innerExceptionType;
        }
		
        /// <summary>
        /// The expected exception.
        /// </summary>
		public Type ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
		}

        /// <summary>
        /// The expected message text.
        /// </summary>
        public string ExpectedMessage
        {
            get
            {
                return this.expectedMessage;
            }
        }

        /// <summary>
        /// The expected inner exception.
        /// </summary>
        public Type InnerExceptionType
        {
            get
            {
                return innerExceptionType;
            }
        }
		
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new ExpectedExceptionRunInvoker(invoker, ExceptionType, ExpectedMessage, InnerExceptionType, Description);
		}
	}
}
