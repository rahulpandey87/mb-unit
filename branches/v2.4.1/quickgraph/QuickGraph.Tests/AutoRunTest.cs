using System;

namespace QuickGraph.UnitTests
{
    using MbUnit.Core;
    using MbUnit.Core.Filters;

    public class AutoRunTest
    {
        public static void Main(string[] args)
        {
            try
            {
                using (AutoRunner auto = new AutoRunner())
                {
                    auto.Domain.Filter = FixtureFilters.Current;
                    auto.Run();
                    auto.ReportToHtml();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
