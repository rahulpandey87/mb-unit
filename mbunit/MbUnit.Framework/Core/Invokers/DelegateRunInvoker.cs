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
using System.Text;

namespace MbUnit.Core.Invokers
{
    using MbUnit.Core.Runs;
    public class DelegateRunInvoker : RunInvoker
    {
        private Delegate test;
        private Object[] parameters;

        public DelegateRunInvoker(IRun generator, Delegate test, Object[] parameters)
            :base(generator)
        {
            if (test == null)
                throw new ArgumentNullException("test");
            this.test = test;
            this.parameters = parameters;
        }

        public override Object Execute(Object o, System.Collections.IList args)
        {
            if (o.GetType() == this.test.Target.GetType())
                return this.test.Method.Invoke(o, this.parameters);
            else
                return this.test.DynamicInvoke(parameters);
        }

        public override String Name
        {
            get 
            {
                return this.test.Method.Name;
            }
        }

        public override bool ContainsMemberInfo(System.Reflection.MemberInfo memberInfo)
        {
            return this.test.Method == memberInfo;
        }
    }
}
