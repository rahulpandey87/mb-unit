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
//		MbUnit HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux


namespace MbUnit.Core.Reflection
{
	using System;
	using System.Collections;
	using System.Reflection;
	
	/// <summary>
	/// Class that computes the <em>transition variation</em> between two instance
	/// of a type.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class TypePublicProxy
	{
		private Type proxyType;
		private Type attirbuteType;

		private PropertyInfo[] properties = null;
		private FieldInfo[] fields = null;

		public TypePublicProxy(Type proxyType,Type attributeType)
		{
			if (proxyType==null)
				throw new ArgumentNullException("proxyType");

			this.proxyType = proxyType;
			this.attirbuteType = attributeType;
			
			ReflectType();
		}
		
		/// <summary>
		/// Gets the proxy type.
		/// </summary>
		public Type ProxyType
		{
			get
			{
				return this.proxyType;
			}
		}

		public PropertyInfo[] GetProperties()
		{
			return this.properties;
		}

		public FieldInfo[] GetFields()
		{
			return this.fields;
		}	

		private void ReflectType()
		{
			// count public properties
			ArrayList props = new ArrayList();
			foreach(PropertyInfo pi in this.proxyType.GetProperties())
			{
				if (!pi.CanRead)
					continue;
				if (pi.GetIndexParameters().Length!=0)
					continue;
				if (this.attirbuteType!=null && !TypeHelper.HasCustomAttribute(pi,this.attirbuteType))
					continue;
				
				props.Add(pi);
			}
			// allocated and copy
			this.properties = new PropertyInfo[ props.Count ];
			props.CopyTo(this.properties,0);
			props = null;
			
			// get fields
			ArrayList fs = new ArrayList();
			foreach(FieldInfo fi in this.proxyType.GetFields())
			{				
				if (this.attirbuteType!=null && !TypeHelper.HasCustomAttribute(fi,this.attirbuteType))
					continue;
				fs.Add(fi);
			}
			this.fields = new FieldInfo[ fs.Count ];
			fs.CopyTo(this.fields,0);
			fs = null;
		}
	}
}
