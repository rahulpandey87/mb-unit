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

namespace MbUnit.Core.Framework
{
	using System;
	using System.IO;
	using System.Xml;
	using System.Threading;
	using System.Reflection;
	using System.Collections;
	using System.Runtime.Remoting.Contexts;
	using System.Runtime.Remoting.Messaging;
	using System.Runtime.Remoting.Activation;
	using System.Diagnostics;

	using MbUnit.Core.Remoting;

	[AttributeUsage(AttributeTargets.Class)]
	public class AppDomainAttribute : ContextAttribute, IContributeObjectSink
	{
		public AppDomainAttribute(string friendlyName) : base("AppDomain")
		{
			_friendlyName = friendlyName;
		}
		private string _friendlyName;

		public string FriendlyName
		{
			get { return _friendlyName; }
		}
		
		public string PrivateBinPath
		{
			get { return _privateBinPath; }
			set { _privateBinPath = value; }
		}
		private string _privateBinPath;
		
		public bool ShadowCopyFiles
		{
			get { return _shadowCopyFiles; }
			set { _shadowCopyFiles = value; }
		}
		private bool _shadowCopyFiles;

		public string Config
		{
			set { _config = value; }
			get { return _config; }
		}
		private string _config;

		public string ConfigFile
		{
			set { _configFile = value; }
			get { return _configFile; }
		}
		private string _configFile;

		public override bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			return (AppDomain.CurrentDomain.FriendlyName == _friendlyName);
		}

		public override void GetPropertiesForNewContext(IConstructionCallMessage msg)
		{
			_activationType = msg.ActivationType;
			base.GetPropertiesForNewContext(msg);
		}
		private Type _activationType;

		public Type ActivationType
		{
			get { return _activationType; }
		}

		public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
		{
			return new AppDomainMessageSink(this, obj, nextSink);
		}
		private static IDictionary _services = new Hashtable();
	}

	internal class AppDomainMessageSink : IMessageSink
	{
		public AppDomainMessageSink(AppDomainAttribute appDomainAttribute, MarshalByRefObject obj, IMessageSink nextSink)
		{
			_appDomainAttribute = appDomainAttribute;
			_obj = obj;
			_nextSink = nextSink;

			string baseDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(baseDir);
			string friendlyName = appDomainAttribute.FriendlyName;
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = baseDir;
			string configFile = appDomainAttribute.ConfigFile;
			string config = appDomainAttribute.Config;
			if(config != null)
			{
				if(configFile == null) { configFile = friendlyName + ".config"; }
				configFile = Path.Combine(baseDir, configFile);
				setup.ConfigurationFile = configFile;
				using(StreamWriter writer = new StreamWriter(configFile))
				{
					writer.Write(appDomainAttribute.Config);
				}
			}

			Type activationType = appDomainAttribute.ActivationType;

			XmlDocument doc = new XmlDocument();
			if(configFile != null) { doc.Load(configFile); }
			mergeDependentAssemblies(doc, activationType);
			if(doc.DocumentElement != null)
			{
				if(configFile == null) { configFile = Path.GetTempFileName(); }
				doc.Save(configFile);
			}

			string binDir = baseDir;
			if(appDomainAttribute.PrivateBinPath != null)
			{
				string privateBinPath = appDomainAttribute.PrivateBinPath;
				setup.PrivateBinPath = privateBinPath;
				privateBinPath = privateBinPath.Split(new char[] {';'})[0];
				binDir = Path.Combine(binDir, privateBinPath);
				Directory.CreateDirectory(binDir);
			}

			// Copy activated type assembly.
			string activationFile = new Uri(activationType.Assembly.CodeBase).LocalPath;
			string destFile = Path.Combine(binDir, Path.GetFileName(activationFile));
			if(activationFile != destFile) { File.Copy(activationFile, destFile, true); }

			// Copy context attribute assembly.
			string contextsFile = new Uri(GetType().Assembly.CodeBase).LocalPath;
			string destContextsFile = Path.Combine(binDir, Path.GetFileName(contextsFile));
			if(destContextsFile != destFile) { File.Copy(contextsFile, destContextsFile, true); }

			copyFiles(activationType, baseDir);

			if(appDomainAttribute.ShadowCopyFiles) { setup.ShadowCopyFiles = "true"; }

			_domain = AppDomain.CreateDomain(friendlyName, null, setup);
			_remoteObject = (MarshalByRefObject)_domain.CreateInstanceAndUnwrap(
				activationType.Assembly.FullName, activationType.FullName);
		}
		private AppDomainAttribute _appDomainAttribute;
		private MarshalByRefObject _obj;
		private IMessageSink _nextSink;
		private AppDomain _domain;
		private MarshalByRefObject _remoteObject;

		private static void mergeDependentAssemblies(XmlDocument doc, Type type)
		{
			object[] dependentAssemblies = type.GetCustomAttributes(typeof(DependentAssemblyAttribute), true);
			if(dependentAssemblies != null)
			{
				foreach(DependentAssemblyAttribute dependentAssembly in dependentAssemblies)
				{
					AssemblyName assemblyName = dependentAssembly.AssemblyName;

					string versionRange = dependentAssembly.OldVersion;
					if(versionRange == null) { versionRange = assemblyName.Version.ToString(); }

					string codeBase = null;
					if(dependentAssembly.NewVersion == null) { codeBase = assemblyName.CodeBase; }
					else { assemblyName.Version = new Version(dependentAssembly.NewVersion); }

					ConfigUtils.MergeDependentAssembly(doc, assemblyName, versionRange, codeBase);
				}
			}
		}

		private void copyFiles(Type type, string destination)
		{
			object[] copies = type.GetCustomAttributes(typeof(CopyAttribute), true);
			if(copies != null)
			{
				foreach(CopyAttribute copy in copies)
				{
					foreach(string file in copy.Files)
					{
						string fileName = Path.GetFileName(file);
						string destDir = destination;
						if(copy.Destination != null)
						{
							destDir = Path.Combine(destDir, copy.Destination);
							Directory.CreateDirectory(destDir);
						}
						string destFile = Path.Combine(destDir, fileName);
						File.Copy(file, destFile, true);
					}
				}
			}
		}

		private static string getAssemblyFileByName(string name, string dir)
		{
			string assemblyFile = Path.Combine(dir, name + ".dll");
			if(File.Exists(assemblyFile)) { return assemblyFile; }
			assemblyFile = Path.Combine(dir, name + ".exe");
			if(File.Exists(assemblyFile)) { return assemblyFile; }
			throw new ApplicationException("Couldn't find assembly with name '" + name + "' in directory '" + dir + "'");
		}
		
		#region IMessageSink Members

		public IMessage SyncProcessMessage(IMessage msg)
		{
			try
			{
				if(msg is IMethodCallMessage)
				{
					IMethodCallMessage methodCall = (IMethodCallMessage)msg;
					object ret = methodCall.MethodBase.Invoke(_remoteObject, methodCall.Args);

					ArrayList outArgs = new ArrayList();
					foreach(ParameterInfo parameter in methodCall.MethodBase.GetParameters())
					{
						if(parameter.IsOut)
						{
							object arg = methodCall.Args[parameter.Position];
							outArgs.Add(arg);
						}
					}
					return new ReturnMessage(ret, outArgs.ToArray(), outArgs.Count, methodCall.LogicalCallContext, methodCall);
				}
				return _nextSink.SyncProcessMessage(msg);
			}
			catch(Exception e)
			{
				return new ReturnMessage(e, (IMethodCallMessage)msg);
			}
		}

		public IMessageSink NextSink
		{
			get { return _nextSink; }
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

}

