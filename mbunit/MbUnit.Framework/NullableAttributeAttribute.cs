using System;
using System.Reflection;

namespace MbUnit.Framework
{
	using MbUnit.Core;

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class NullableAttributeAttribute : Attribute
    {
		private Type nullableType;
		public NullableAttributeAttribute(Type nullableType)
		{
			if (nullableType==null)
				throw new ArgumentNullException("nullableType");
			this.nullableType=nullableType;
		}

		public Type NullableType
		{
			get
			{
				return this.nullableType;
			}
			set
			{
				this.nullableType=value;				
			}
		}

		public static NullableAttributeAttribute GetAttribute(Type t)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			Assembly a = t.Assembly;
			NullableAttributeAttribute attribute = 
				(NullableAttributeAttribute)TypeHelper.TryGetFirstCustomAttribute(a,typeof(NullableAttributeAttribute));
			return attribute;
		}
	}
}
