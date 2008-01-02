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

namespace MbUnit.Core.Invokers
{
	using MbUnit.Core.Runs;
	using MbUnit.Framework;

	/// <summary>
	/// Summary description for PropertyGetRunInvoker.
	/// </summary>
	public class PropertyGetRunInvoker : RunInvoker
	{
		private PropertyInfo property;
		private bool feedArguments = true;
		private Object instance;

		public PropertyGetRunInvoker(IRun generator, PropertyInfo property)
			: base(generator)
		{
			if (property==null)
				throw new ArgumentNullException("property");
			if (!property.CanRead)
				throw new ArgumentException("property is write only");

			this.property = property;

			ConstructorInfo ci = TypeHelper.GetConstructor(this.property.DeclaringType,Type.EmptyTypes);
			this.instance = ci.Invoke(null);
		}

		public bool FeedArguments
		{
			get
			{
				return this.feedArguments;
			}
			set
			{
				this.feedArguments = value;
			}
		}

		public PropertyInfo Property
		{
			get
			{
				return this.property;
			}
		}

		public override String Name
		{
			get
			{
				return String.Format("{0}.{1}",
					this.property.DeclaringType.Name,
					this.property.Name
					);
			}
		}

		public override Object Execute(Object o, IList args)
		{
			Object[] oargs = new object[args.Count];
			args.CopyTo(oargs,0);

			Object result = this.property.GetValue(
				this.instance,
				oargs
				);
			if (this.feedArguments)
				args.Add(result);
			return result;
		}

        public override bool ContainsMemberInfo(MemberInfo memberInfo)
        {
            return this.Property == memberInfo;
        }

    }
}
