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


namespace MbUnit.Core.Invokers
{
	using System;
	using System.Collections;
    using System.Reflection;
	using MbUnit.Core.Runs;
	
	
	/// <summary>
	/// Decorator invorkers are used to modify the way a fixute method is executed.
	/// Popular examples of such is the <see cref="ExpectedExceptionRunInvoker"/>
	/// or the <see cref="RepeatRunInvoker"/>.
	/// </summary>
	public abstract class DecoratorRunInvoker : IRunInvoker 
	{
		private IRunInvoker invoker;
        private string description;

		protected DecoratorRunInvoker(IRunInvoker invoker)
            :this(invoker,null)
        { }

		protected DecoratorRunInvoker(IRunInvoker invoker, string description) 
		{
			if (invoker == null)
				throw new ArgumentNullException("invoker");
			
			this.invoker = invoker;
            this.description = description;
        }
		
		public IRun Generator 
		{
			get
			{
				return this.invoker.Generator;
			}
		}

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public virtual String Name
		{
			get
			{
              return this.Invoker.Name;
 /*               return String.Format("{0}({1})",
				                     this.GetType().Name,
				                     this.invoker.Name);
*/
			}
		}
		
		public IRunInvoker Invoker
		{
			get
			{
				return this.invoker;
			}
			set
			{
				this.invoker = value;
			}
		}
		
		public abstract Object Execute(object o, IList args);

        public virtual bool ContainsMemberInfo(MemberInfo memberInfo)
        {
            return this.Invoker.ContainsMemberInfo(memberInfo);
        }
    }
}
