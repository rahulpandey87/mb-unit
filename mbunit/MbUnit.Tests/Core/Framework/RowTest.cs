using System;
using System.Text;

using MbUnit.Framework;
using System.Reflection;
using System.Data.SqlClient;

namespace MbUnit.Tests.Core.Framework
{
    [TestFixture]
    public class RowTest
    {
        [RowTest]
        [Row(1000, 10, 100.0000)]
        [Row(-1000, 10, -100.0000)]
        [Row(1000, 7, 142.85715)]
        [Row(1000, 0.00001, 100000000)]
        [Row(4195835, 3145729, 1.3338196)]
        public void DivTest(double numerator, double denominator, double result)
        {
            Assert.AreEqual(result, numerator / denominator, 0.00001);
        }

        [RowTest]
        [Row(1000, 10, 100.0000)]
        [Row(-1000, 10, -100.0000)]
        [Row(1000, 0.00001, 100000000)]
        public void DivDecimalTest(decimal numerator, decimal denominator, decimal result)
        {
            Assert.AreEqual(result, numerator / denominator);
        }

        [RowTest]
        [Row()]
        public void RowWithNoValues()
        {
            // Nothing.  It's enough to know that the method ran.
        }

        [RowTest]
        [Row("value")]
        public void RowWithOneValue(object value)
        {
            Assert.AreEqual("value", value);
        }

        [RowTest]
        [Row(null)]
        public void RowWithOneNullValue(object value)
        {
            Assert.IsNull(value);
        }

        [RowTest]
        [Row(1, true)]
        [Row(2, 'c')]
        [Row(3, 4.345)]
        [Row(4, 2.356f)]
        [Row(5, 753)]
        [Row(6, 1200000000L)]
        [Row(7, "MbUnit")]
        public void RighTypeIsPassed(int rowNumber, object value)
        {
            object[] types = new object[] 
            { 
                typeof(bool), typeof(char),
                typeof(double), typeof(float),
                typeof(int), typeof(long),
                typeof(string)
            };

            Assert.IsNotNull(value);
            Assert.IsTrue(value.GetType() == types[rowNumber - 1]);
        }

        /// <summary>
        /// Makes sure that values in a row are passed in the right order
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        [RowTest]
        [Row("value1", "value2", "value3")]
        public void RowWithTreeValue(object value1, object value2, object value3)
        {
            Assert.AreEqual("value1", value1);
            Assert.AreEqual("value2", value2);
            Assert.AreEqual("value3", value3);
        }

        #region Conversion tests

        /// <summary>
        /// Check the conversion of numbers under several cultures to make sure that it
        /// works in a culture-invariant way.
        /// </summary>
        [Test]
        [MultipleCulture("ar-SA,bg-BG,ca-ES,zh-TW,cs-CZ,da-DK,de-DE,el-GR,en-US,fi-FI,fr-FR,he-IL,hu-HU,is-IS,it-IT,ja-JP,ko-KR,nl-NL,nb-NO,pl-PL,pt-BR,ro-RO,ru-RU,hr-HR,sk-SK,sq-AL,sv-SE,th-TH,tr-TR,ur-PK,id-ID,uk-UA,be-BY,sl-SI,et-EE,lv-LV,lt-LT,fa-IR,vi-VN,hy-AM,az-AZ-Latn,eu-ES,mk-MK,af-ZA,ka-GE,fo-FO,hi-IN,ms-MY,kk-KZ,ky-KG,sw-KE,uz-UZ-Latn,tt-RU,pa-IN,gu-IN,ta-IN,te-IN,kn-IN,mr-IN,sa-IN,mn-MN,gl-ES,kok-IN,syr-SY,div-MV,ar-IQ,zh-CN,de-CH,en-GB,es-MX,fr-BE,it-CH,nl-BE,nn-NO,pt-PT,sr-SP-Latn,sv-FI,az-AZ-Cyrl,ms-BN,uz-UZ-Cyrl,ar-EG,zh-HK,de-AT,en-AU,es-ES,fr-CA,sr-SP-Cyrl,ar-LY,zh-SG,de-LU,en-CA,es-GT,fr-CH,ar-DZ,zh-MO,de-LI,en-NZ,es-CR,fr-LU,ar-MA,en-IE,es-PA,fr-MC,ar-TN,en-ZA,es-DO,ar-OM,en-JM,es-VE,ar-YE,en-CB,es-CO,ar-SY,en-BZ,es-PE,ar-JO,en-TT,es-AR,ar-LB,en-ZW,es-EC,ar-KW,en-PH,es-CL,ar-AE,es-UY,ar-BH,es-PY,ar-QA,es-BO,es-SV,es-HN,es-NI,es-PR,sma-NO,sr-BA-Cyrl,zu-ZA,xh-ZA,tn-ZA,se-SE,sma-SE,hr-BA,smn-FI,quz-PE,se-FI,sms-FI,cy-GB,bs-BA-Latn,smj-NO,mi-NZ,quz-EC,sr-BA-Latn,smj-SE,ns-ZA,quz-BO,se-NO,mt-MT")]
        public void RowAttributeNumberConversion()
        {
            RowAttribute ra = new RowAttribute("25.5", "2006.32323", "90.2323289329329832");
            object[] row = ra.GetRow(GetParametersForMethod("RowAttributeNumberConversionParameters"));
            Assert.AreEqual(row[0], 25.5f);
            Assert.AreEqual(row[1], 2006.32323d);
            Assert.AreEqual(row[2], 90.2323289329329832m);
        }


        public void RowAttributeDateTimeConversion(string dateString, int year, int month, int day,
                          int hour, int minute, int second)
        {
            RowAttribute ra = new RowAttribute(dateString);
            object[] row = ra.GetRow(GetParametersForMethod("RowAttributeDateTimeConversionParameters"));
            DateTime dt = (DateTime)row[0];
            AssertDateTimeValues(dt, year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Check the conversion of dates under several cultures to make sure that it
        /// works in a culture-invariant way.
        /// </summary>
        [Test]
        [MultipleCulture("ar-SA,bg-BG,ca-ES,zh-TW,cs-CZ,da-DK,de-DE,el-GR,en-US,fi-FI,fr-FR,he-IL,hu-HU,is-IS,it-IT,ja-JP,ko-KR,nl-NL,nb-NO,pl-PL,pt-BR,ro-RO,ru-RU,hr-HR,sk-SK,sq-AL,sv-SE,th-TH,tr-TR,ur-PK,id-ID,uk-UA,be-BY,sl-SI,et-EE,lv-LV,lt-LT,fa-IR,vi-VN,hy-AM,az-AZ-Latn,eu-ES,mk-MK,af-ZA,ka-GE,fo-FO,hi-IN,ms-MY,kk-KZ,ky-KG,sw-KE,uz-UZ-Latn,tt-RU,pa-IN,gu-IN,ta-IN,te-IN,kn-IN,mr-IN,sa-IN,mn-MN,gl-ES,kok-IN,syr-SY,div-MV,ar-IQ,zh-CN,de-CH,en-GB,es-MX,fr-BE,it-CH,nl-BE,nn-NO,pt-PT,sr-SP-Latn,sv-FI,az-AZ-Cyrl,ms-BN,uz-UZ-Cyrl,ar-EG,zh-HK,de-AT,en-AU,es-ES,fr-CA,sr-SP-Cyrl,ar-LY,zh-SG,de-LU,en-CA,es-GT,fr-CH,ar-DZ,zh-MO,de-LI,en-NZ,es-CR,fr-LU,ar-MA,en-IE,es-PA,fr-MC,ar-TN,en-ZA,es-DO,ar-OM,en-JM,es-VE,ar-YE,en-CB,es-CO,ar-SY,en-BZ,es-PE,ar-JO,en-TT,es-AR,ar-LB,en-ZW,es-EC,ar-KW,en-PH,es-CL,ar-AE,es-UY,ar-BH,es-PY,ar-QA,es-BO,es-SV,es-HN,es-NI,es-PR,sma-NO,sr-BA-Cyrl,zu-ZA,xh-ZA,tn-ZA,se-SE,sma-SE,hr-BA,smn-FI,quz-PE,se-FI,sms-FI,cy-GB,bs-BA-Latn,smj-NO,mi-NZ,quz-EC,sr-BA-Latn,smj-SE,ns-ZA,quz-BO,se-NO,mt-MT")]
        public void RowAttributeDateTimeConversion()
        {
            RowAttributeDateTimeConversion("2006-05-04 12:03:05", 2006, 5, 4, 12, 3, 5);
            RowAttributeDateTimeConversion("2007-12-04 23:15:59", 2007, 12, 4, 23, 15, 59);
            RowAttributeDateTimeConversion("2006-05-04 12:03:05", 2006, 5, 4, 12, 3, 5);
        }

        [RowTest]
        [Row("1")]
        public void PositiveIntegerTest(int number)
        {
            Assert.AreEqual(number, 1);
        }

        [RowTest]
        [Row("-1")]
        public void NegativeIntegerTest(int number)
        {
            Assert.AreEqual(number, -1);
        }

        [RowTest]
        [Row("13.53")]
        public void FloatNumber(float number)
        {
            Assert.AreEqual(number, 13.53f);
        }

        [RowTest]
        [Row("27.3456")]
        public void DoubleNumber(double number)
        {
            Assert.AreEqual(number, 27.3456);
        }

        [RowTest]
        [Row("142.85714285714285714285714286")]
        public void DecimalNumber(decimal number)
        {
            Assert.AreEqual(number, 142.85714285714285714285714286m);
        }

        [RowTest]
        [Row("2006-05-04 12:03:05", 2006, 5, 4, 12, 3, 5)]
        [Row("2007-12-04 23:15:59", 2007, 12, 4, 23, 15, 59)]
        [Row("2007-11-30 00:59:01", 2007, 11, 30, 0, 59, 1)]
        public void TestConversion(DateTime dt, int year, int month, int day,
            int hour, int minute, int second)
        {
            Assert.AreEqual(dt.Year, year);
            Assert.AreEqual(dt.Month, month);
            Assert.AreEqual(dt.Day, day);
            Assert.AreEqual(dt.Hour, hour);
            Assert.AreEqual(dt.Minute, minute);
            Assert.AreEqual(dt.Second, second);
        }

        #endregion

        #region Special cases

        [RowTest]
        [Row(SpecialValue.Null)]
        public void SpecialValue_Null(string x)
        {
            Assert.IsNull(x);
        }

        [RowTest]
        [Row(typeof(StringBuilder))]
        public void CreationOfInstances(StringBuilder sb)
        {
            Assert.IsNotNull(sb, "A new StringBuilder object should have been created");
        }

        /// <summary>
        /// Enums are not supported by the "create instance" feature, but it's not a problem
        /// since it's intended yo be used with reference types (enums are valid attribute
        /// type paremeters, non-primitive classes aren't).
        /// </summary>
        /// <param name="e"></param>
        [RowTest]
        [ExpectedException(typeof(ArgumentException))]
        [Row(typeof(TestEnumeration))] // No instance will be created
        [Row(typeof(string))]
        public void ArgumentException(TestEnumeration e)
        {
        }

        [RowTest]
        [ExpectedException(typeof(InvalidCastException))]
        [Row("MbUnit")]
        public void InvalidCast(TestEnumeration e)
        {
        }

        [RowTest]
        [ExpectedException(typeof(FormatException))]
        [Row("MbUnit")]
        public void FormatException(int x)
        {
        }

        /// <summary>
        /// When a Type object is expected no object should be instantiated
        /// </summary>
        /// <param name="t"></param>
        [RowTest]
        [Row(typeof(StringBuilder))]
        public void ReferenceTypeParameter(Type t)
        {
            Assert.IsNotNull(t);
            Assert.AreSame(t, typeof(StringBuilder));
            Assert.AreNotSame(t, typeof(Encoder));
        }

        [RowTest]
        [Row(typeof(TestEnumeration))]
        public void EnumTypeParameter(Type t)
        {
            Assert.IsNotNull(t);
            Assert.AreSame(t, typeof(TestEnumeration));
        }

        #endregion // Special cases

        #region Private Methods and Helpers

        public ParameterInfo[] GetParametersForMethod(string methodName)
        {
            Type thisType = this.GetType();
            MethodInfo mi = thisType.GetMethod(methodName);
            return mi.GetParameters();
        }

        // This is a dummy method, we only needed it to get a ParameterInfo array
        public void RowAttributeNumberConversionParameters(float floatNumber, double doubleNumber, decimal decimalNumber)
        {
        }

        // This is a dummy method, we only needed it to get a ParameterInfo array
        public void RowAttributeDateTimeConversionParameters(DateTime dt)
        {
        }

        public void AssertDateTimeValues(DateTime dt, int year, int month, int day,
                    int hour, int minute, int second)
        {
            Assert.AreEqual(dt.Year, year);
            Assert.AreEqual(dt.Month, month);
            Assert.AreEqual(dt.Day, day);
            Assert.AreEqual(dt.Hour, hour);
            Assert.AreEqual(dt.Minute, minute);
            Assert.AreEqual(dt.Second, second);
        }

        #endregion
    }

    public enum TestEnumeration
    {
        Value1 = 1,
        Value2 = 2
    }

}
