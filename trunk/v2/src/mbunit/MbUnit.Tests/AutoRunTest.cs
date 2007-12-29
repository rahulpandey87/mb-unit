using System;

namespace MbUnit.Tests
{
    using MbUnit.Core;
    using MbUnit.Core.Filters;

    public class AutoRunTest
    {    
        public static int Main(string[] args)
        {
            try
            {
                using (AutoRunner auto = new AutoRunner())
                {
                   // auto.Domain.Filter = FixtureFilters.Current;
                    auto.Run();
                    auto.ReportToHtml();

                    return auto.ExitCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
    }
}
