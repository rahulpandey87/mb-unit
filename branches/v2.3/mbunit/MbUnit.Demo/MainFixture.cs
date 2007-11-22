using System;

namespace MbUnit.Demo
{
    public class MainFixture1
    {
        static int Main(string[] args)
        {
            return 100;
        }
    }
    public class MainFixture2
    {
        static void Main()
        {
            Environment.ExitCode = 100;
        }
    }
}
