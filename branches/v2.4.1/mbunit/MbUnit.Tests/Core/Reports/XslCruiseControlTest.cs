using System;
using System.Xml;
using System.Xml.Xsl;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Core.Reports
{
    [TestFixture]
    public class XslCruiseControlTest
    {
        [Test]
        public void LoadText()
        {
            XslTransform transform = ResourceHelper.ReportTextTransform;
        }
        [Test]
        public void LoadHtml()
        {
            XslTransform transform = ResourceHelper.ReportHtmlTransform;
        }
        [Test]
        public void CreateImages()
        {
            ResourceHelper.CreateImages("Images");
        }
        [Test]
        public void LoadCruiseControl()
        {
            XslTransform transform = ResourceHelper.ReportCruiseControlTransform;
        }
    }
}
