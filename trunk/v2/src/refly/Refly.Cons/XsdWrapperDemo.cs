using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;

namespace Refly.Cons
{
	using Refly.CodeDom;
	using Refly.Xsd;	
	
	/// <summary>
	/// Summary description for XsdWrapperDemo.
	/// </summary>
	public class XsdWrapperDemo
	{
		public static void GraphML()
		{
			// XSD wrapper demo
			XsdWrapperGenerator xsg = new XsdWrapperGenerator("QuickGraph.Serialization.GraphML");
			foreach(Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
			{
				if (t.Namespace!= "QuickGraph.Serialization.GraphML")
					continue;
				xsg.Add(t);
			}

			xsg.Generate();

			// getting generator
			CodeGenerator gen = new CodeGenerator();
			gen.GenerateCode("..\\..",xsg.Ns);
		}

		public static void NCover()
		{
			// XSD wrapper demo
			XsdWrapperGenerator xsg = new XsdWrapperGenerator("NCover");
			foreach(Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
			{
				if (t.Namespace!= "NCover")
					continue;
				xsg.Add(t);
			}
//			xsg.Ns.Conformer.AddWord("seqpnt");
			xsg.Generate();

			// getting generator
			CodeGenerator gen = new CodeGenerator();
			gen.GenerateCode("..\\..",xsg.Ns);
		}

		public static void GUnit()
		{
			// XSD wrapper demo
			XsdWrapperGenerator xsg = new XsdWrapperGenerator("GUnit.Core.Reports.Serialization");
			foreach(Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
			{
				if (t.Namespace!= "GUnit.Core.Reports.Xsd")
					continue;
				xsg.Add(t);
			}
			xsg.Generate();

			// getting generator
			CodeGenerator gen = new CodeGenerator();
			gen.GenerateCode("..\\..",xsg.Ns);
		}

		public static void NUnit()
		{
			// XSD wrapper demo
			XsdWrapperGenerator xsg = new XsdWrapperGenerator("GUnit.Core.Reports.NUnit");
			foreach(Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
			{
				if (t.Namespace!= "GUnit.Core.Reports.NUnit")
					continue;
				xsg.Add(t);
			}

			xsg.Generate();

			// getting generator
			CodeGenerator gen = new CodeGenerator();
			gen.GenerateCode("..\\..",xsg.Ns);
		}

		public static void DocBook()
		{
			// XSD wrapper demo
			XsdWrapperGenerator xsg = new XsdWrapperGenerator("GUnit.Core.Reports.DocBook");
			foreach(Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
			{
				if (t.Namespace!= "NDocBook.Xsd")
					continue;
				xsg.Add(t);
			}

			xsg.Generate();

			// getting generator
			CodeGenerator gen = new CodeGenerator();
			gen.GenerateCode("..\\..",xsg.Ns);
		}
	}
}
