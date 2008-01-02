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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace MbUnit.Core.Remoting
{
	public sealed class ConfigUtils
	{
		private ConfigUtils() {}

		private const string ASM_NAMESPACE = "urn:schemas-microsoft-com:asm.v1";

		/// <summary>
		/// Merge a 'dependentAssembly' directive into a given config document.
		/// If any entries exist for the same assembly they will be deleted
		/// before the new entry is merged.
		/// </summary>
		/// <param name="doc">The config document to merge</param>
		/// <param name="assemblyName">The Assembly that should be used</param>
		/// <param name="versionRange">The range of compatable versions (eg. "1.0.0.0-3.0.0.0")</param>
        /// <param name="codeBase">The codebase to use.</param>
		public static void MergeDependentAssembly(XmlDocument doc, AssemblyName assemblyName, string versionRange, string codeBase)
		{
			deleteDependentAssembly(doc, assemblyName);
			XmlElement dependentAssembly = createDependentAssembly(doc, assemblyName, versionRange, codeBase);
			insertDependentAssembly(doc, dependentAssembly);
		}

		internal static void deleteDependentAssembly(XmlDocument doc, AssemblyName assemblyName)
		{
			string publicKeyToken = getPublicKeyToken(assemblyName);
			string name = assemblyName.Name;

			XPathNavigator navigator = doc.CreateNavigator();

			XmlNamespaceManager nsmgr = new XmlNamespaceManager(navigator.NameTable);
			nsmgr.AddNamespace("asm", ASM_NAMESPACE);

			XPathExpression expr = navigator.Compile("configuration/runtime/asm:assemblyBinding/asm:dependentAssembly[asm:assemblyIdentity/@name='" + name + "' and asm:assemblyIdentity/@publicKeyToken='" + publicKeyToken + "']");
			expr.SetContext(nsmgr);

			XPathNodeIterator nodes = navigator.Select(expr);
			while(nodes.MoveNext())
			{
				XmlNode node = ((IHasXmlNode)nodes.Current).GetNode();
				Trace.WriteLine("node about to be removed: " + node.Name);
				node.ParentNode.RemoveChild(node);
			}
		}

		internal static void insertDependentAssembly(XmlDocument doc, XmlElement dependentAssembly)
		{
			XmlElement configuration = doc["configuration"];
			if(configuration == null)
				doc.AppendChild(configuration = doc.CreateElement("configuration"));

			XmlElement runtime = configuration["runtime"];
			if(runtime == null)
				configuration.AppendChild(runtime = doc.CreateElement("runtime"));

			XmlElement assemblyBinding = runtime["assemblyBinding", ASM_NAMESPACE];
			if(assemblyBinding == null)
				runtime.AppendChild(assemblyBinding = doc.CreateElement("assemblyBinding", ASM_NAMESPACE));

			assemblyBinding.AppendChild(dependentAssembly);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="assemblyName"></param>
		/// <param name="oldVersion"></param>
		/// <param name="assemblyCodeBase">specify a URL to define a codeBase otherwise null</param>
		/// <returns></returns>
		internal static XmlElement createDependentAssembly(XmlDocument doc, AssemblyName assemblyName, string oldVersion, string assemblyCodeBase)
		{
			XmlElement dependentAssembly;

			string publicKeyToken = getPublicKeyToken(assemblyName);
			string name = assemblyName.Name;
			string culture = getCulture(assemblyName);
			string newVersion = assemblyName.Version.ToString();

			XmlElement assemblyIdentity;
			XmlElement bindingRedirect;
			XmlElement codeBase;

			dependentAssembly = doc.CreateElement("dependentAssembly", ASM_NAMESPACE);

			dependentAssembly.AppendChild(assemblyIdentity = doc.CreateElement("assemblyIdentity", ASM_NAMESPACE));
			assemblyIdentity.SetAttribute("name", name);
			assemblyIdentity.SetAttribute("publicKeyToken", publicKeyToken);
			assemblyIdentity.SetAttribute("culture",  culture);

			dependentAssembly.AppendChild(bindingRedirect = doc.CreateElement("bindingRedirect", ASM_NAMESPACE));
			bindingRedirect.SetAttribute("oldVersion", oldVersion);
			bindingRedirect.SetAttribute("newVersion", newVersion);

			if(assemblyCodeBase != null)
			{
				dependentAssembly.AppendChild(codeBase = doc.CreateElement("codeBase", ASM_NAMESPACE));
				codeBase.SetAttribute("version", newVersion);
				codeBase.SetAttribute("href", assemblyCodeBase);
			}

			return dependentAssembly;
		}

		// HACK: Is there a better way for getting the culture as used here?
		// NOTE: 'assembly.GetName().CultureInfo.Name' will return ""
		public static string getCulture(AssemblyName assemblyName)
		{
			try
			{
				string culture;
				string cultureEquals = "Culture=";
				string fullName = assemblyName.FullName;
				int fromIndex = fullName.IndexOf(cultureEquals) + cultureEquals.Length;
				int toIndex = fullName.IndexOf(',', fromIndex);
				culture = fullName.Substring(fromIndex, toIndex - fromIndex);
				return culture;
			}
			catch(Exception e)
			{
				Trace.WriteLine(e);
				return "unknown";
			}
		}

		public static string getPublicKeyToken(AssemblyName assemblyName)
		{
			StringBuilder builder = new StringBuilder();
			byte[] myByteArray = assemblyName.GetPublicKeyToken();
			if(myByteArray == null) { throw new ApplicationException("Assembly '" + assemblyName.FullName + "' doesn't have a strong name."); }
			foreach(byte myByte in myByteArray)
			{
				string hex = myByte.ToString("x");
				if(hex.Length < 2) { hex = "0" + hex; }
				builder.Append(hex);
			}
			return builder.ToString();
		}

	}
}
