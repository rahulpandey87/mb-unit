/// Refly License
/// 
/// Copyright (c) 2004 Jonathan de Halleux, http://www.dotnetwiki.org
///
/// This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
/// 
/// Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
///
/// 1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
/// 
/// 2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
///
///3. This notice may not be removed or altered from any source distribution.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Reflection;
using System.CodeDom;
using System.Xml.Serialization;
using System.IO;

namespace Refly.Xsd
{
	using Refly.CodeDom;
	using Refly.CodeDom.Collections;
	using Refly.CodeDom.Expressions;
	using Refly.CodeDom.Statements;

	/// <summary>
	/// Transforms the XSD wrapper classes outputed by the Xsd.exe utility
	/// to nicer classes using Reflection.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class XsdWrapperGenerator
	{
		private NamespaceDeclaration ns;
		private NameConformer conformer;
		private TypeTypeDeclarationDictionary types =  new TypeTypeDeclarationDictionary();
		private XsdWrapperConfig config = new XsdWrapperConfig();

		/// <summary>
		/// Creates a new wrapper generator.
		/// </summary>
		/// <param name="name"></param>
		public XsdWrapperGenerator(string name)
		{
			if (name==null)
				throw new ArgumentNullException("name");

			this.conformer = new NameConformer();
			this.ns = new NamespaceDeclaration(name,this.conformer);
			this.ns.Imports.Add("System.Xml");
			this.ns.Imports.Add("System.Xml.Serialization");
			this.ns.Imports.Add("System.IO");
		}

		public NameConformer Conformer
		{
			get
			{
				return this.conformer;
			}
		}

		public NamespaceDeclaration Ns
		{
			get
			{
				return this.ns;
			}
		}

		public TypeTypeDeclarationDictionary Types
		{
			get
			{
				return this.types;
			}
		}

		public XsdWrapperConfig Config
		{
			get
			{
				return this.config;
			}
			set
			{
				this.config = value;
			}
		}

		/// <summary>
		/// Adds a class to the wrapped class list
		/// </summary>
		/// <param name="t"></param>
		public void Add(Type t)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (t.IsEnum)
				CreateEnum(t);
			else if (t.IsClass)
				CreateClass(t);
		}

		public void Generate()
		{
			foreach(Type t in this.types.Keys)
			{
				if (t.IsEnum)
					GenerateEnumType(t);
				else if (t.IsClass)
					GenerateClassType(t);
			}
		}

		private void CreateClass(Type t)
		{
			ClassDeclaration c = this.ns.AddClass(this.conformer.ToCapitalized(t.Name));

			// check if XmlType present
			if(TypeHelper.HasCustomAttribute(t,typeof(XmlTypeAttribute)))
			{
				XmlTypeAttribute typeAttr = 
					(XmlTypeAttribute)TypeHelper.GetFirstCustomAttribute(t,typeof(XmlTypeAttribute));
				AttributeDeclaration type =c.CustomAttributes.Add(typeof(XmlTypeAttribute));
				type.Arguments.Add("IncludeInSchema",Expr.Prim(typeAttr.IncludeInSchema));
				if (this.Config.KeepNamespaces)
				{
					type.Arguments.Add("Namespace",Expr.Prim(typeAttr.Namespace));
				}
				type.Arguments.Add("TypeName",Expr.Prim(typeAttr.TypeName));
			}

			// check if XmlRoot present
			if(TypeHelper.HasCustomAttribute(t,typeof(XmlRootAttribute)) )
			{
				XmlRootAttribute rootAttr = 
					(XmlRootAttribute)TypeHelper.GetFirstCustomAttribute(t,typeof(XmlRootAttribute));
				AttributeDeclaration root = c.CustomAttributes.Add(typeof(XmlRootAttribute));

				root.Arguments.Add("ElementName",Expr.Prim(rootAttr.ElementName));
				root.Arguments.Add("IsNullable",Expr.Prim(rootAttr.IsNullable));
				if (this.Config.KeepNamespaces)
				{
					root.Arguments.Add("Namespace",Expr.Prim(rootAttr.Namespace));
				}
				root.Arguments.Add("DataType",Expr.Prim(rootAttr.DataType));
			}
			else
			{
				AttributeDeclaration root =c.CustomAttributes.Add(typeof(XmlRootAttribute));
				root.Arguments.Add("ElementName",Expr.Prim(t.Name));
			}
			this.types.Add(t,c);
		}

		private void CreateEnum(Type t)
		{
			bool flags = TypeHelper.HasCustomAttribute(t,typeof(FlagsAttribute));
			EnumDeclaration e = this.ns.AddEnum(this.conformer.ToCapitalized(t.Name),flags);
			this.types.Add(t,e);
		}

		private void GenerateClassType(Type t)
		{
			ClassDeclaration c = this.ns.Classes[this.conformer.ToCapitalized(t.Name)];
			if (c==null)
				throw new Exception(this.conformer.ToCapitalized(t.Name) + " not found.");
			c.CustomAttributes.Add(typeof(SerializableAttribute));

			// generate fields and properties	
			foreach(FieldInfo f in t.GetFields())
			{
				if (f.FieldType.IsArray)
				{
					AddArrayField(c,f);
				}
				else
				{
					AddField(c,f);
				}
			}

			// add constructors
			GenerateDefaultConstructor(c);
		}

		private void GenerateEnumType(Type t)
		{
			EnumDeclaration e = this.ns.Enums[this.conformer.ToCapitalized(t.Name)];
			e.CustomAttributes.Add(typeof(SerializableAttribute));

			if (e==null)
				throw new Exception();
			// generate fields and properties	
			foreach(FieldInfo f in t.GetFields())
			{
				if (f.Name == "Value__")
					continue;

				FieldDeclaration field = e.AddField(f.Name);
				// add XmlEnum attribute
				AttributeDeclaration xmlEnum = field.CustomAttributes.Add(typeof(XmlEnumAttribute));
				AttributeArgument arg = xmlEnum.Arguments.Add(
					"Name",
					Expr.Prim(f.Name)
					);
			}
		}

		private void AddArrayField(ClassDeclaration c, FieldInfo f)
		{
			// create a collection
			ClassDeclaration col = c.AddClass(conformer.ToSingular(f.Name)+"Collection");
			col.Parent = new TypeTypeDeclaration(typeof(System.Collections.CollectionBase));

			// add serializable attribute
			col.CustomAttributes.Add(typeof(SerializableAttribute));

			// default constructor
			col.AddConstructor();
			// default indexer
			IndexerDeclaration index = col.AddIndexer(
				typeof(Object)
				);
			ParameterDeclaration pindex = index.Signature.Parameters.Add(typeof(int),"index",false);
			// getter
			index.Get.Return( 
				Expr.This.Prop("List").Item( Expr.Arg(pindex) )
				);
			index.Set.AddAssign( 
				Expr.This.Prop("List").Item( Expr.Arg(pindex) ),
				Expr.Value
				);

			// add object method
			MethodDeclaration addObject = col.AddMethod("Add");
			ParameterDeclaration paraObject = addObject.Signature.Parameters.Add(new TypeTypeDeclaration(typeof(Object)),"o",true);
			addObject.Body.Add(
				Expr.This.Prop("List").Method("Add").Invoke(paraObject)
				);

			// if typed array add methods for type
			if (f.FieldType.GetElementType()!=typeof(Object))
			{
				AddCollectionMethods(
					col,
					MapType(f.FieldType.GetElementType()),
					this.conformer.ToCapitalized(f.FieldType.GetElementType().Name),
					"o"
					);
			}

			foreach(XmlElementAttribute ea in f.GetCustomAttributes(typeof(XmlElementAttribute),true))
			{
				string name = this.conformer.ToCapitalized(ea.ElementName);
				string pname= this.conformer.ToCamel(name);

				ITypeDeclaration mappedType = null;
				if (ea.Type!=null)
					mappedType = MapType(ea.Type);

				if (mappedType==null || mappedType == f.FieldType.GetElementType())
					continue;

				AddCollectionMethods(col,mappedType,name,pname);
			}

			// add field
			FieldDeclaration fd = c.AddField(col, f.Name);
			fd.InitExpression = Expr.New( col );
			PropertyDeclaration p = c.AddProperty(fd,f.Name,true,true,false);

			// setting attributes
			// attach xml text
			if (TypeHelper.HasCustomAttribute(f,typeof(XmlTextAttribute)))
			{
				AttributeDeclaration attr = p.CustomAttributes.Add(typeof(XmlTextAttribute));
				
				attr.Arguments.Add("Type",Expr.TypeOf(typeof(string)));

				// adding to string to collection
				MethodDeclaration tostring = col.AddMethod("ToString");
				tostring.Signature.ReturnType = new TypeTypeDeclaration(typeof(String));
				tostring.Attributes = MemberAttributes.Public | MemberAttributes.Override;

				VariableDeclarationStatement sw = Stm.Var(typeof(StringWriter),"sw");
				sw.InitExpression = Expr.New(typeof(StringWriter));
				tostring.Body.Add(sw);
				ForEachStatement fe = Stm.ForEach(
					typeof(string),"s",Expr.This.Prop("List"),false);
				
				fe.Body.Add(
					Expr.Var(sw).Method("Write").Invoke(fe.Local)
					);
		
				tostring.Body.Add(fe);
				tostring.Body.Return(Expr.Var(sw).Method("ToString").Invoke());
			}
			else if (TypeHelper.HasCustomAttribute(f,typeof(XmlArrayItemAttribute)))
			{
				// add xml array attribute
				AttributeDeclaration attr = p.CustomAttributes.Add(typeof(XmlArrayAttribute));
				attr.Arguments.Add("ElementName",Expr.Prim(f.Name));

				// add array item attribute
				XmlArrayItemAttribute arrayItem = 
					(XmlArrayItemAttribute)TypeHelper.GetFirstCustomAttribute(f,typeof(XmlArrayItemAttribute));

				attr = p.CustomAttributes.Add(typeof(XmlArrayItemAttribute));
				attr.Arguments.Add("ElementName",Expr.Prim(arrayItem.ElementName));
				//MMI:attr.Arguments.Add("Type",Expr.Prim(MapType(f.FieldType.GetElementType()).Name));
				attr.Arguments.Add("Type",Expr.TypeOf(MapType(f.FieldType.GetElementType())));

				if (arrayItem.Type!=null)
				{
					attr.Arguments.Add("DataType",Expr.Prim(arrayItem.DataType));
				}
				attr.Arguments.Add("IsNullable",Expr.Prim(arrayItem.IsNullable));
				if (this.Config.KeepNamespaces)
				{
					attr.Arguments.Add("Namespace",Expr.Prim(arrayItem.Namespace));
				}
			}
			else
			{
				AttachXmlElementAttributes(p,f);
			}
		}

		private void AddCollectionMethods(ClassDeclaration col, ITypeDeclaration mappedType, string name, string pname)
		{
			// add method
			MethodDeclaration add = col.AddMethod("Add"+name);
			ParameterDeclaration para = add.Signature.Parameters.Add(mappedType,pname,true);
			add.Body.Add(
				Expr.This.Prop("List").Method("Add").Invoke(para)
				);

			// add method
			MethodDeclaration contains = col.AddMethod("Contains"+name);
			contains.Signature.ReturnType = new TypeTypeDeclaration(typeof(bool));
			para = contains.Signature.Parameters.Add(mappedType,pname,true);
			contains.Body.Return(
				Expr.This.Prop("List").Method("Contains").Invoke(para)
				);

			// add method
			MethodDeclaration remove = col.AddMethod("Remove"+name);
			para = remove.Signature.Parameters.Add(mappedType,pname,true);
			remove.Body.Add(
				Expr.This.Prop("List").Method("Remove").Invoke(para)
				);
		}

		private void AddField(ClassDeclaration c, FieldInfo f)
		{
			if(c==null)
				throw new ArgumentNullException("c");
			if (f==null)
				throw new ArgumentNullException("f");

			FieldDeclaration fd = c.AddField(MapType(f.FieldType),f.Name);
			PropertyDeclaration p = c.AddProperty(fd,f.Name,true,true,false);
			// adding attributes
			if (TypeHelper.HasCustomAttribute(f,typeof(XmlAttributeAttribute)))
			{
				XmlAttributeAttribute att = (XmlAttributeAttribute)TypeHelper.GetFirstCustomAttribute(f,typeof(XmlAttributeAttribute));
				AttributeDeclaration attr = p.CustomAttributes.Add(typeof(XmlAttributeAttribute));
				string attrName = att.AttributeName;
				if (att.AttributeName.Length==0)
					attrName = f.Name;
				AttributeArgument arg = attr.Arguments.Add(
					"AttributeName",
					Expr.Prim(attrName)
					);
			}
			else
			{
				if (TypeHelper.HasCustomAttribute(f,typeof(XmlElementAttribute)))
				{
					AttachXmlElementAttributes(p,f);
				}
				else
				{
					AttributeDeclaration attr = p.CustomAttributes.Add(typeof(XmlElementAttribute));
					attr.Arguments.Add("ElementName",Expr.Prim(f.Name));
				}
			}
		}

		private void AttachXmlElementAttributes(PropertyDeclaration p, FieldInfo f)
		{
			string customName = null;
			// if array is type add element
			if (f.FieldType.IsArray && f.FieldType.GetElementType()!=typeof(Object))
			{
				AttributeDeclaration attr = p.CustomAttributes.Add(typeof(XmlElementAttribute));
				attr.Arguments.Add("ElementName",Expr.Prim(f.Name));
				//MMI:attr.Arguments.Add("Type",Expr.Prim(MapType(f.FieldType.GetElementType()).Name));
				attr.Arguments.Add("Type",Expr.TypeOf(MapType(f.FieldType.GetElementType())));						
				customName = f.Name;
			}

			// attach xml elements
			foreach(XmlElementAttribute el in f.GetCustomAttributes(typeof(XmlElementAttribute),true))
			{
				if (customName == el.ElementName)
					continue;
				AttributeDeclaration attr = p.CustomAttributes.Add(typeof(XmlElementAttribute));
				attr.Arguments.Add("ElementName",Expr.Prim(el.ElementName));
				if (el.Type!=null)
				{
					//MMI:attr.Arguments.Add("Type",Expr.Prim(MapType(el.Type).Name));						
					attr.Arguments.Add("Type",Expr.TypeOf(MapType(el.Type)));						
				}
			}
		}

		private void GenerateDefaultConstructor(ClassDeclaration c)
		{
			ConstructorDeclaration cd = new ConstructorDeclaration(c);
		}

		private ITypeDeclaration MapType(string t)
		{
			if (t==null)
				throw new ArgumentNullException("t");

			Type type = Type.GetType(t,true);
			return MapType(type);
		}

		private ITypeDeclaration MapType(Type t)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (this.types.Contains(t))
				return this.types[t];
			else					
				return new TypeTypeDeclaration(t);
		}
	}
}
