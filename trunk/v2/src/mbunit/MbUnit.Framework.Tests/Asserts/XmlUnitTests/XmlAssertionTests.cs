namespace MbUnit.Framework.Tests.Asserts.XmlUnit {
    using MbUnit.Framework;
    using MbUnit.Framework.Xml;
    using MbUnit.Core.Exceptions;
	using System.IO;
    
    [TestFixture]
    public class XmlAssertionTests {
        private string _xmlTrueTest;
        private string _xmlFalseTest;

        [TestFixtureSetUp]
        public void StartTest()
        {
            _xmlTrueTest = "<assert>true</assert>";
            _xmlFalseTest = "<assert>false</assert>";
        }

        #region XmlEquals
        [Test]
        public void XmlEqualsWithTextReader()
        {
            XmlAssert.XmlEquals(new StringReader(_xmlTrueTest), new StringReader(_xmlTrueTest));
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void XmlEqualsWithTextReaderFail()
        {
            XmlAssert.XmlEquals(new StringReader(_xmlTrueTest), new StringReader(_xmlFalseTest));
        }

        [Test]
        public void XmlEqualsWithString()
        {
            XmlAssert.XmlEquals(_xmlTrueTest, _xmlTrueTest);
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void XmlEqualsWithStringFail()
        {
            XmlAssert.XmlEquals(_xmlTrueTest, _xmlFalseTest);
        }

        [Test]
        public void XmlEqualsWithXmlInput()
        {
            XmlAssert.XmlEquals(new XmlInput(_xmlTrueTest), new XmlInput(_xmlTrueTest));
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void XmlEqualsWithXmlInputFail()
        {
            XmlAssert.XmlEquals(new XmlInput(_xmlTrueTest), new XmlInput(_xmlFalseTest));
        }

        [Test]
        public void XmlEqualsWithXmlDiff()
        {
            XmlAssert.XmlEquals(new XmlDiff(_xmlTrueTest, _xmlTrueTest));
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void XmlEqualsWithXmlDiffFail()
        {
            XmlAssert.XmlEquals(new XmlDiff(new XmlInput(_xmlTrueTest), new XmlInput(_xmlFalseTest)));
        }

        [RowTest]
        [Row("Optional Description", "Optional Description")]
        [Row("", "Xml does not match")]
        [Row("XmlDiff", "Xml does not match")]
        public void XmlEqualsWithXmlDiffFail_WithDiffConfiguration(string optionalDesciption, string expectedMessage)
        {
            try
            {
                XmlAssert.XmlEquals(new XmlDiff(new XmlInput(_xmlTrueTest), new XmlInput(_xmlFalseTest), new DiffConfiguration(optionalDesciption)));
            }
            catch (AssertionException e)
            {
                Assert.AreEqual(true, e.Message.StartsWith(expectedMessage));
            }
        }

        [Test]
        public void XmlEqualsWithXmlDiffFail_WithNullOptionalDescription()
        {
            try
            {
                XmlAssert.XmlEquals(new XmlDiff(new XmlInput(_xmlTrueTest), new XmlInput(_xmlFalseTest), new DiffConfiguration(null)));
            }
            catch (AssertionException e)
            {
                Assert.AreEqual(true, e.Message.StartsWith("Xml does not match"));
            }
        }
        #endregion

        [Test] 
		public void AssertStringEqualAndIdenticalToSelf() 
		{
            string control = _xmlTrueTest;
            string test = _xmlTrueTest;
            XmlAssert.XmlIdentical(control, test);
            XmlAssert.XmlEquals(control, test);
        }

        [Test]
		public void AssertDifferentStringsNotEqualNorIdentical() {
            string control = "<assert>true</assert>";
            string test = "<assert>false</assert>";
            XmlDiff xmlDiff = new XmlDiff(control, test);
            XmlAssert.XmlNotIdentical(xmlDiff);
            XmlAssert.XmlNotEquals(xmlDiff);
        }        
        
        [Test] 
		public void AssertXmlIdenticalUsesOptionalDescription() 
		{
            string description = "An Optional Description";
            try {
                XmlDiff diff = new XmlDiff(new XmlInput("<a/>"), new XmlInput("<b/>"), 
                                           new DiffConfiguration(description));
                XmlAssert.XmlIdentical(diff);
            } catch (AssertionException e) {
                Assert.AreEqual(true, e.Message.StartsWith(description));
            }
        }
        
        [Test]      
        public void AssertXmlEqualsUsesOptionalDescription() {
            string description = "Another Optional Description";
            try {
                XmlDiff diff = new XmlDiff(new XmlInput("<a/>"), new XmlInput("<b/>"), 
                                           new DiffConfiguration(description));
                XmlAssert.XmlEquals(diff);
            } catch (AssertionException e) {
                Assert.AreEqual(true, e.Message.StartsWith(description));
            }
        }
        
        [Test] 
        public void AssertXmlValidTrueForValidFile() {
            StreamReader reader = new StreamReader(ValidatorTests.ValidFile);
            try {
                XmlAssert.XmlValid(reader);
            } finally {
                reader.Close();
            }
        }
        
        [Test] 
		public void AssertXmlValidFalseForInvalidFile() {
            StreamReader reader = new StreamReader(ValidatorTests.InvalidFile);
            try {
                XmlAssert.XmlValid(reader);
                Assert.Fail("Expected assertion failure");
            } catch(AssertionException e) {
                AvoidUnusedVariableCompilerWarning(e);
            } finally {
                reader.Close();
            }
        }
        
        private static readonly string MY_SOLAR_SYSTEM = "<solar-system><planet name='Earth' position='3' supportsLife='yes'/><planet name='Venus' position='4'/></solar-system>";
        
        [Test] public void AssertXPathExistsWorksForExistentXPath() {
            XmlAssert.XPathExists("//planet[@name='Earth']", 
                                           MY_SOLAR_SYSTEM);
        }
        
        [Test] public void AssertXPathExistsFailsForNonExistentXPath() {
            try {
                XmlAssert.XPathExists("//star[@name='alpha centauri']", 
                                               MY_SOLAR_SYSTEM);
                Assert.Fail("Expected assertion failure");
            } catch (AssertionException e) {
                AvoidUnusedVariableCompilerWarning(e);
            }
        }
        
        [Test] public void AssertXPathEvaluatesToWorksForMatchingExpression() {
            XmlAssert.XPathEvaluatesTo("//planet[@position='3']/@supportsLife", 
                                                MY_SOLAR_SYSTEM,
                                                "yes");
        }
        
        [Test] public void AssertXPathEvaluatesToWorksForNonMatchingExpression() {
            XmlAssert.XPathEvaluatesTo("//planet[@position='4']/@supportsLife", 
                                                MY_SOLAR_SYSTEM,
                                                "");
        }
        
        [Test] public void AssertXPathEvaluatesToWorksConstantExpression() {
            XmlAssert.XPathEvaluatesTo("true()", 
                                                MY_SOLAR_SYSTEM,
                                                "True");
            XmlAssert.XPathEvaluatesTo("false()", 
                                                MY_SOLAR_SYSTEM,
                                                "False");
        }
        
        [Test] 
        public void AssertXslTransformResultsWorksWithStrings() {
        	string xslt = XsltTests.IDENTITY_TRANSFORM;
        	string someXml = "<a><b>c</b><b/></a>";
        	XmlAssert.XslTransformResults(xslt, someXml, someXml);
        }
        
        [Test] 
        public void AssertXslTransformResultsWorksWithXmlInput() {
        	StreamReader xsl = ValidatorTests.GetTestReader("animal.xsl");
        	XmlInput xslt = new XmlInput(xsl);
        	StreamReader xml = ValidatorTests.GetTestReader("testAnimal.xml");
        	XmlInput xmlToTransform = new XmlInput(xml);
        	XmlInput expectedXml = new XmlInput("<dog/>");
        	XmlAssert.XslTransformResults(xslt, xmlToTransform, expectedXml);
        }
        
        [Test] 
        public void AssertXslTransformResultsCatchesFalsePositive() {
        	StreamReader xsl = ValidatorTests.GetTestReader("animal.xsl");
        	XmlInput xslt = new XmlInput(xsl);
        	StreamReader xml = ValidatorTests.GetTestReader("testAnimal.xml");
        	XmlInput xmlToTransform = new XmlInput(xml);
        	XmlInput expectedXml = new XmlInput("<cat/>");
        	bool exceptionExpected = true;
        	try {
        		XmlAssert.XslTransformResults(xslt, xmlToTransform, expectedXml);
        		exceptionExpected = false;
        		Assert.Fail("Expected dog not cat!");
        	} catch (AssertionException e) {
        		AvoidUnusedVariableCompilerWarning(e);
        		if (!exceptionExpected) {
        			throw e;
        		}
        	}
        }

        private void AvoidUnusedVariableCompilerWarning(AssertionException e) {
            string msg = e.Message;
        }
    }
}
