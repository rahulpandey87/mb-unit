using System;
using System.Reflection;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Cons;
using MbUnit.Core.Cons.CommandLine;

namespace MbUnit.Tests.Core.Cons
{
	[TestFixture]
	public class MainArgumentsTest
	{
		[Test]
		public void Parse()
		{
			MainArguments parsedArgs = new MainArguments();
			string[] args = new string[]{"Hello.exe"};

			CommandLineUtility.ParseCommandLineArguments(args, parsedArgs);
		}

		[Test]
		public void ParseReportFolderWithShortName()
		{
			this.parseValue("/rf:Folder","Folder","ReportFolder");
		}

		[Test]
		public void ParseReportFolderWithLongName()
		{
			this.parseValue("/report-folder:Folder","Folder","ReportFolder");
		}

		[Test]
		public void ParseReportTypeWithShortName()
		{
			MainArguments parsedArgs = new MainArguments();
			string argument = "/rt:Xml";
			string[] args = new string[]{"Hello.exe","mydll.dll",argument};

			CommandLineUtility.ParseCommandLineArguments(args, parsedArgs);

			Assert.IsNotNull(parsedArgs.ReportTypes);
			Assert.AreEqual(1, parsedArgs.ReportTypes.Length);
			Assert.AreEqual(ReportType.Xml, parsedArgs.ReportTypes[0]);
		}

		[Test]
		public void ParseReportTypeWithLongName()
		{
			MainArguments parsedArgs = new MainArguments();
			string argument = "/report-type:Xml";
			string[] args = new string[]{"Hello.exe","mydll.dll",argument};

			CommandLineUtility.ParseCommandLineArguments(args, parsedArgs);

			Assert.IsNotNull(parsedArgs.ReportTypes);
			Assert.AreEqual(1, parsedArgs.ReportTypes.Length);
			Assert.AreEqual(ReportType.Xml, parsedArgs.ReportTypes[0]);
		}

		[Test]
		public void ParseTwoReportTypeWithShortName()
		{
			MainArguments parsedArgs = new MainArguments();
			string argument = "/rt:Xml";
			string argument2 = "/rt:Html";
			string[] args = new string[]{"Hello.exe","mydll.dll",argument, argument2};

			CommandLineUtility.ParseCommandLineArguments(args, parsedArgs);

			Assert.IsNotNull(parsedArgs.ReportTypes);
			Assert.AreEqual(2, parsedArgs.ReportTypes.Length);
			Assert.AreEqual(ReportType.Xml, parsedArgs.ReportTypes[0]);
			Assert.AreEqual(ReportType.Html, parsedArgs.ReportTypes[1]);
		}

		[Test]
		public void ParseFilterCategoryWithShortName()
		{
			this.parseValue("/fc:Filter","Filter","FilterCategory");
		}
		[Test]
		public void ParseFilterCategoryWithLongName()
		{
			this.parseValue("/filter-category:Filter","Filter","FilterCategory");
		}

		[Test]
		public void ParseFilterNamespaceWithShortName()
		{
			this.parseValue("/fn:Filter","Filter","FilterNamespace");
		}
		[Test]
		public void ParseFilterNamespaceWithLongName()
		{
			this.parseValue("/filter-namespace:Filter","Filter","FilterNamespace");
		}

		[Test]
		public void ParseFilterAuthorWithShortName()
		{
			this.parseValue("/fa:Filter","Filter","FilterAuthor");
		}
		[Test]
		public void ParseFilterAuthorWithLongName()
		{
			this.parseValue("/filter-author:Filter","Filter","FilterAuthor");
		}

		[Test]
		public void ParseFilterTypeWithShortName()
		{
			this.parseValue("/ft:Filter","Filter","FilterType");
		}
		[Test]
		public void ParseFilterTypeWithLongName()
		{
			this.parseValue("/filter-type:Filter","Filter","FilterType");
		}

		private void parseValue(string argument, string expectedOutput, string fieldName)
		{
			FieldInfo fi = typeof(MainArguments).GetField(fieldName);
			Assert.IsNotNull(fi);

			MainArguments parsedArgs = new MainArguments();
			string[] args = new string[]{"Hello.exe","mydll.dll",argument};

			CommandLineUtility.ParseCommandLineArguments(args, parsedArgs);

			Console.WriteLine(parsedArgs.ToString());
			Assert.AreEqual(expectedOutput, fi.GetValue(parsedArgs));
			Assert.IsNotNull(parsedArgs.GetFilter());
		}
	}
}
