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
using System.Reflection;
using System.Collections;

using MbUnit.Core.Exceptions;
using MbUnit.Core.Runs;

namespace MbUnit.Core.Invokers
{	
	using MbUnit.Core.Exceptions;
	
	public class ExpectedExceptionRunInvoker : DecoratorRunInvoker
	{
		private Type exceptionType;

        public ExpectedExceptionRunInvoker(
		        IRunInvoker invoker,
		        Type exceptionType,
                string description
                )
		:base(invoker,description)
		{
			if (exceptionType==null)
				throw new ArgumentNullException("exceptionType");
			this.exceptionType = exceptionType;
        }
		
		public Type ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
		}
		
        public override object Execute(Object o, IList args)
        {
            try
            {
                Object result = this.Invoker.Execute(o, args);
            }
            catch (Exception ex)
            {
                Exception catchedException = ex;
                if (catchedException is TargetInvocationException)
                    catchedException = ex.InnerException;

                Exception current = catchedException;
                // check the execption is not ignoreexecption
                if (this.ExceptionType != typeof(IgnoreRunException))
                {
                    Exception cu = catchedException;
                    while (cu!=null)
                    {
                        // ignore exection, propagate
                        if (typeof(IgnoreRunException).IsInstanceOfType(cu))
                            throw ex;
                        cu = cu.InnerException;
                    }
                }

                while (!this.ExceptionType.IsInstanceOfType(current))
                {
                    current = current.InnerException;
                    if (current == null)
                        throw new ExceptionTypeMistmachException(
                            this.ExceptionType,
                            this.Description,
                            catchedException
                            );
                }

                return null;
            }
            // if we are here it did not throw
            throw new ExceptionNotThrownException(ExceptionType,this.Description);
        }
    }
}
