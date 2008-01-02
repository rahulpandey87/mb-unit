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
using System.Reflection;

namespace MbUnit.Core.Invokers
{
    public class MethodFailedLoadingRunInvoker : FailedLoadingRunInvoker
    {
        private MethodInfo method;
        public MethodFailedLoadingRunInvoker(IRun generator, Exception exception, MethodInfo method)
            :base(generator,exception)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            this.method = method;
        }

        public override String Name
        {
            get
            {
                return String.Format("{0}.{1}",
                    this.method.Name, base.ToString()
                    );
            }
        }

        public override Object Execute(Object o, System.Collections.IList args)
        {
            throw new FixtureFailedLoadingException(this.Exception);
        }

        public override bool ContainsMemberInfo(MemberInfo memberInfo)
        {
            return this.method == memberInfo;
        }
    }
}
