namespace MbUnits.Tests
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Diagnostics;
	using System.Configuration;
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[TestFixture]
	public class TestAppDomainAttribute
	{
		[Test]
		public void TestFriendlyName()
		{
			MyAppDomain domain = new MyAppDomain();
			Assert.AreEqual("MyFriendlyName", domain.GetFriendlyName());
			Assert.AreEqual("MyFriendlyName", domain.FriendlyName);
		}

		[Test]
		public void TestHashCode()
		{
			MyAppDomain domain1 = new MyAppDomain();
			int hash1 = domain1.GetHashCode();
			MyAppDomain domain2 = new MyAppDomain();
			int hash2 = domain2.GetHashCode();
			Assert.IsTrue(hash1 != hash2, "check we have two different domains");
		}

		[Test, Ignore("Not yet disposable")]
		public void TestDisposable()
		{
			MyAppDomain domain = new MyAppDomain();
			AppDomain appDomain = domain.AppDomain;
			Assert.AreEqual("MyFriendlyName", appDomain.FriendlyName, "check for expected friendly name");
			IDisposable disposable = (IDisposable)domain;
			disposable.Dispose();
		}

		[AppDomain("MyFriendlyName")]
			public class MyAppDomain : ContextBoundObject
		{
			public string GetFriendlyName()
			{
				return AppDomain.CurrentDomain.FriendlyName;
			}

			public string FriendlyName
			{
				get { return AppDomain.CurrentDomain.FriendlyName; }
			}

			public AppDomain AppDomain
			{
				get { return AppDomain.CurrentDomain; }
			}
		}
	}

	[TestFixture, AppDomain("FriendlyTest", PrivateBinPath="bin", ShadowCopyFiles=true,
					  ConfigFile="Web.config", Config=
					  @"
		<configuration>
			<appSettings>
				<add key=""MyKey"" value=""MyValue"" />
			</appSettings>
		</configuration>
	")]
	public class TextContext : ContextBoundObject
	{
		[Test]
		public void TestMyFriendlyName()
		{
			Assert.AreEqual("FriendlyTest", AppDomain.CurrentDomain.FriendlyName);
		}

		[Test]
		public void TestAppSetting()
		{
			Assert.AreEqual("MyValue", ConfigurationSettings.AppSettings["MyKey"]);
		}

		[Test]
		public void TestConfigFile()
		{
			string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			Assert.IsTrue(File.Exists(configFile), "check config file exists");
			Assert.AreEqual("Web.config", Path.GetFileName(configFile), "check config file name");
		}

		[Test]
		public void TestNoShadow()
		{
			Assembly assembly = GetType().Assembly;
			string localPath = new Uri(assembly.CodeBase).LocalPath;
			string location = assembly.Location;
			Assert.IsTrue(localPath.ToLower() != location.ToLower(), "assembly at '" + location + "' is being shadow copied");
		}

		[Test]
		public void TestPrivateBinPath()
		{
			Assembly assembly = GetType().Assembly;
			string localPath = new Uri(assembly.CodeBase).LocalPath;
			string dir = Path.GetDirectoryName(localPath);
			string binDir = Path.GetFileName(dir);
			Assert.AreEqual("bin", binDir, "check assembly in PrivateBinPath");
		}
	}
}

