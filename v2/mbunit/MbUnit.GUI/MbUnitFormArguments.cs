using MbUnit.Core.Cons;
using MbUnit.Core.Cons.CommandLine;

namespace MbUnit.GUI
{
    public sealed class MbUnitFormArguments : MainArguments
    {
        [CommandLineArgument(
            CommandLineArgumentType.AtMostOnce,
            ShortName = "h",
            LongName = "help",
            Description = "Display help in message box"
            )]
        public bool Help = false;

        [CommandLineArgument(
            CommandLineArgumentType.AtMostOnce,
            ShortName = "r",
            LongName = "run",
            Description = "Run tests on loaded assemblies"
            )]
        public bool Run = false;

        [CommandLineArgument(
            CommandLineArgumentType.AtMostOnce,
            ShortName = "c",
            LongName = "close",
            Description = "Close when tests finished"
            )]
        public bool Close = false;
    }
}