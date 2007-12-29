using System;

namespace MbUnit.Tests.Core.Cons.CommandLine
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;
	using MbUnit.Core.Cons.CommandLine;

	internal class TwoDefaultArgument
	{
		[DefaultCommandLineArgument(
			 CommandLineArgumentType.MultipleUnique
			 )]
		public string[] firstDefault = null;

		[DefaultCommandLineArgument(
			 CommandLineArgumentType.MultipleUnique
			 )]
		public string[] secondDefault = null;
	}

	internal class OccurenceArgument
	{
		[DefaultCommandLineArgument(
			 CommandLineArgumentType.MultipleUnique
			 )]
		public string[] defaultMultipleUnique = null;

		[CommandLineArgument(
			 CommandLineArgumentType.AtMostOnce,
			 ShortName = "amo",
			 LongName = "at-most-one"
			 )]
		public String atMostOne = null;

		[CommandLineArgument(
			 CommandLineArgumentType.MultipleUnique,
			 ShortName = "mu",
			 LongName = "multiple-unique"
			 )]
		public string[] multipleUnique = null;

		[CommandLineArgument(
			 CommandLineArgumentType.LastOccurenceWins,
			 ShortName = "low",
			 LongName = "last-occurence-wins"
			 )]
		public String lastOccurenceWins = null;
	}

	internal class DuplicateLongName
	{
		[CommandLineArgument(
			 CommandLineArgumentType.MultipleUnique,
			 LongName = "arg"
			 )]
		public string[] first = null;

		[CommandLineArgument(
			 CommandLineArgumentType.LastOccurenceWins,
			 LongName = "arg"
			 )]
		public String second = null;
	}

	[TestFixture]
	public class CommandLineUtilityTest
	{
		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TwoDefaultParameter()
		{
			CommandLineArgumentParser parser = 
				new CommandLineArgumentParser(typeof(TwoDefaultArgument),new ErrorReporter(this.error));
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void DuplicateLongNameParameter()
		{
			CommandLineArgumentParser parser = 
				new CommandLineArgumentParser(typeof(DuplicateLongName),new ErrorReporter(this.error));
		}

		[Test]
		public void OccurenceParameters()
		{
			CommandLineArgumentParser parser = 
				new CommandLineArgumentParser(typeof(OccurenceArgument),new ErrorReporter(this.error));
		}

		private void error(string message)
		{
			Assert.Fail(message);
		}
	}
}
