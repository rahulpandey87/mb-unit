using System;
using System.CodeDom;

namespace Refly.Demo
{
	using Refly.CodeDom;
	using Refly.CodeDom.Expressions;
	using Refly.CodeDom.Statements;

	public class User
	{
		private string name;

		public User(string name)
		{
			this.name = name;
		}

		public String Name
		{
			get
			{
				return this.name;
			}
		}
	}


	public class UserGenerator
	{
		public static void CodeDom()
		{
			// creating the Refly.Demo namespace
			CodeNamespace demo = new CodeNamespace("Refly.Demo");

			// create the User class
			CodeTypeDeclaration user = new CodeTypeDeclaration("User");
			user.IsClass = true;
			demo.Types.Add(user);

			// add name field
			CodeMemberField name = new CodeMemberField(typeof(string),"name");
			user.Members.Add(name);
			
			// add constructor
			CodeConstructor cstr = new CodeConstructor();
			user.Members.Add(cstr);
			CodeParameterDeclarationExpression pname = 
				new CodeParameterDeclarationExpression(typeof(string),"name");
			cstr.Parameters.Add(pname);

			// this.name = name;
			CodeFieldReferenceExpression thisName = new CodeFieldReferenceExpression(
				new CodeThisReferenceExpression(),
				"name")
				;
			CodeAssignStatement assign = new CodeAssignStatement(
				thisName,
				pname
				);
			cstr.Statements.Add(assign);

			// add property
			CodeMemberProperty p = new CodeMemberProperty();
			p.Type=name.Type;
			p.Name = "Name";
			p.HasGet = true;
			p.GetStatements.Add(
				new CodeMethodReturnStatement(thisName)
				);
		}

		public static void Refly()
		{
			// creating the Refly.Demo namespace
			NamespaceDeclaration demo = new NamespaceDeclaration("Refly.Demo");

			// create the user class
			ClassDeclaration user = demo.AddClass("User");

			// add name field
			FieldDeclaration name = user.AddField(typeof(string), "name");

			// add constructor
			ConstructorDeclaration cstr = user.AddConstructor();
			// add name parameter
			ParameterDeclaration pname = cstr.Signature.Parameters.Add(typeof(string), "name",true);

			// this.name = name;
			cstr.Body.AddAssign( 
				Expr.This.Field(name),
				Expr.Arg(pname)
				); 

			// add property
			user.AddProperty(name, true, false, false);			
		}
	}
}
