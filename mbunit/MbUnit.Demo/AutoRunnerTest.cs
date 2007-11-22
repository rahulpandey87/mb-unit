using System;
using System.IO;

namespace MbUnit.Tests
{
    using MbUnit.Core;
    using MbUnit.Core.Filters;

    public class AutoRunTest
    {
        public static void Main(string[] args)
        {
            using (AutoRunner auto = new AutoRunner())
            {
                //string reportName = String.Format("{0}.xml", auto.GetReportName());
              //  reportName = Path.GetFullPath(reportName);

            //    FailureRunPipeFilter filter = new FailureRunPipeFilter(reportName);
            //    auto.Domain.RunPipeFilter = filter;
                auto.Domain.Filter = FixtureFilters.Current;
                auto.Run();
                auto.ReportToHtml();
                auto.ReportToXml();
            }
            Console.ReadLine();
        }
    }
}
