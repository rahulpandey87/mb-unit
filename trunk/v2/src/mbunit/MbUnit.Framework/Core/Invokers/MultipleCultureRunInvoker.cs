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
using System.Reflection;
using System.Collections;
using System.Threading;
using System.Globalization;

namespace MbUnit.Core.Invokers 
{	
	using MbUnit.Framework;
	using MbUnit.Core.Exceptions;

	public class MultipleCultureRunInvoker : DecoratorRunInvoker
	{
		private MultipleCultureAttribute attribute;
		
		public MultipleCultureRunInvoker(IRunInvoker invoker, 
		                                 MultipleCultureAttribute attribute)
		:base(invoker)
		{
			if (attribute==null)
				throw new ArgumentNullException("attribute");
			this.attribute = attribute;
		}
		
		public MultipleCultureAttribute Attribute
		{
			get
			{
				return this.attribute;
			}
		}
		
		public override Object Execute(Object o, IList args)
		{
			// store current culture
			CultureInfo current = Thread.CurrentThread.CurrentCulture;
			
			try
			{
				// iterate over cultuers
				foreach(string culture in this.Attribute.Cultures)
				{
					// set up next culture
					CultureInfo ci = new CultureInfo(culture);
					Thread.CurrentThread.CurrentCulture = ci;
					// invokie
					this.Invoker.Execute(o,args);
				}			
			}
			catch(Exception ex)
			{
				throw new MultipleCultureException(Thread.CurrentThread.CurrentCulture,"",ex);
			}
			finally
			{
				// clean up
				Thread.CurrentThread.CurrentCulture = current;				
			}
			return null;
		}
	}
}
