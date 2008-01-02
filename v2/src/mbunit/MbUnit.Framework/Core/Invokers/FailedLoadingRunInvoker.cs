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
using MbUnit.Core.Exceptions;
using MbUnit.Core.Invokers;
using MbUnit.Core.Runs;

namespace MbUnit.Core.Invokers
{
    public class FailedLoadingRunInvoker : RunInvoker
    {
        private Exception exception;
        public FailedLoadingRunInvoker(IRun generator, Exception exception)
            :base(generator)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");
            this.exception = exception;
        }

        public Exception Exception
        {
            get
            {
                return this.exception;
            }
        }

        public override String Name
        {
            get 
            {
				Exception ex = this.exception;
				while(ex.InnerException!=null)
					ex=ex.InnerException;
                return String.Format("FailedLoading({0})",ex.Message);
            }
        }

        public override Object Execute(Object o, System.Collections.IList args)
        {
            throw new FixtureFailedLoadingException(this.exception);
        }

        public override bool ContainsMemberInfo(System.Reflection.MemberInfo memberInfo)
        {
            return true;
        }
    }
}
