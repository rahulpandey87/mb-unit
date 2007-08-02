using System;
using System.Collections;
using MbUnit.Framework;
using MbUnit.Core.Exceptions;


namespace MbUnit.Framework.Tests.Asserts
{
    [TestFixture]
    public class Assert_Test
    {

        const string TEST_FORMAT_STRING = "Failed: Param1={0} Param2={1}";
        const string TEST_FORMAT_STRING_PARAM1 = "param1";
        const int TEST_FORMAT_STRING_PARAM2 = 2;
        const string EXPECTED_FORMATTED_MESSAGE = "Failed: Param1=param1 Param2=2";
        const string EXPECTED_FAIL_MESSAGE = "Test Failed";


    #region Messages

        [Test]
        public void FailWithCustomMessage()
        {
            string message = "CustomMessage";
            try
            {
                Assert.Fail("CustomMessage");
            }
            catch (AssertionException ex)
            {
                Assert.AreEqual(message, ex.Message);
            }
        }     

        #endregion

        #region Fail, Ignore, IsNull, IsNotNull, IsTrue, IsFalse

        [Test]
        public void Fail()
        {
            bool bolPass = true;
            try
            {
                Assert.Fail();
            }
            catch (AssertionException)
            {
                bolPass = false;
            }

            if (bolPass == true)
            {
                Assert.IsFalse(true, "Assert fail has failed");
            }

        }

        #region Ignore

        [Test, ExpectedException(typeof(IgnoreRunException))]
        public void Ignore()
        {
            Assert.Ignore("This test will be ignored.");
        }

        [Test, ExpectedArgumentNullException]
        public void IgnoreWithNullMessage()
        {
            Assert.Ignore(null);
        }

        [Test]
        public void IgnoreWithFormattedMessage()
        {
            bool asserted = false;
            
            try
            {
                Assert.Ignore(TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (IgnoreRunException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert Ignore(message, args) has failed");
        }

        #endregion

        #region IsNull

        [Test]
        public void IsNull()
        {
            Assert.IsNull(null);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsNullFail()
        {
            Assert.IsNull("non-null string");
        }

        [Test]
        public void IsNullWithMessage()
        {
            Assert.IsNull(null, "IsNotNull has failed.");
        }

        [Test]
        public void IsNullWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsNull(new object(), EXPECTED_FAIL_MESSAGE);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsNull(message) has failed");
        }

        [Test]
        public void IsNullWithFormattedMessage()
        {
            Assert.IsNull(null, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void IsNullWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsNull(new object(), TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsNull(message, args) has failed");
        }
        #endregion

        #region IsNotNull

        [Test]
        public void IsNotNull()
        {
            Assert.IsNotNull(new object());
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsNotNullFail()
        {
            Assert.IsNotNull(null);
        }

        [Test]
        public void IsNotNullWithMessage()
        {
            Assert.IsNotNull(new object(), "IsNotNull has failed.");
        }

        [Test]
        public void IsNotNullWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsNotNull(null, EXPECTED_FAIL_MESSAGE);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsNotNull(message) has failed");
        }

        [Test]
        public void IsNotNullWithFormattedMessage()
        {
            Assert.IsNotNull(new object(), TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }
        
        [Test]
        public void IsNotNullWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsNotNull(null, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsNotNull(message, args) has failed");
        }
        #endregion


        #region IsTrue

        [Test]
        public void IsTrue()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsTrueFail()
        {
            Assert.IsTrue(false);
        }

        [Test]
        public void IsTrueWithMessage()
        {
            Assert.IsTrue(true, "IsTrue Failed");
        }

        [Test]
        public void IsTrueWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsTrue(false, EXPECTED_FAIL_MESSAGE);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsTrue(message) has failed");
        }

        [Test]
        public void IsTrueWithFormattedMessage()
        {
            Assert.IsTrue(true, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void IsTrueWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsTrue(false, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsTrue(message, args) has failed");
        }


        #endregion

        #region IsFalse

        [Test]
        public void IsFalse()
        {
            Assert.IsFalse(false);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsFalseFail()
        {
            Assert.IsFalse(true);
        }

        [Test]
        public void IsFalseWithMessage()
        {
            Assert.IsFalse(false, "IsFalse has failed.");
        }

        [Test]
        public void IsFalseWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsFalse(true, EXPECTED_FAIL_MESSAGE);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsFalse(message) has failed");
        }

        [Test]
        public void IsFalseWithFormattedMessage()
        {
            Assert.IsFalse(false, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void IsFalseWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.IsFalse(true, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert IsFalse(message, args) has failed");
        }
        #endregion


        #endregion

        #region Assert Count

        [Test]
        public void ResetAssertCount()
        {
            Assert.ResetAssertCount();
            Assert.IsTrue(Assert.AssertCount == 0);
        }

        [Test]
        public void IncrementAssertCount()
        {
            int assertCountBefore = Assert.AssertCount;
            Assert.IncrementAssertCount();

            Assert.IsTrue(Assert.AssertCount == assertCountBefore + 1);
        }

        #endregion

        #region AreEqual

        [Test]
        public void AreEqual_WithFormattedMessage()
        {
            Assert.AreEqual("abcd", "abcd", TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void AreEqualWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreEqual("abcd", "dbca", TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreEqual(message, args) has failed");
        }

        [Test]
        public void AreEqual_NullArguments()
        {
            Assert.AreEqual(null, null);
        }

        #region AreEqual (String)

        [Test]
        public void AreEqualString()
        {
            Assert.AreEqual("hello", "hello");
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualStringActualNullFail()
        {
            Assert.AreEqual("hello", null);
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualStringExpectedNullFail()
        {
            Assert.AreEqual(null, "hello");
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualStringOrdinalCompare()
        {
            Assert.AreEqual("hello", "HELLO");
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualStringFail()
        {
            Assert.AreEqual("hello", "world");
        }

        [Test]
        public void AreEqualEmptyString()
        {
            Assert.AreEqual("", "");
        }

        #endregion

        #region AreEqual (Double)
        // TODO: It'd be great to refactor these tests using Generics (when under 2.0)

        [Test]
        public void AreEqualDoubleZero()
        {
            Assert.AreEqual((double)0.0, (double)0.0);
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualDoubleFail()
        {
            Assert.AreEqual((double)0.0, (double)1.0);
        }

        [Test]
        public void AreEqualDoublePositive()
        {
            Assert.AreEqual((double)1.0, (double)1.0);
        }

        [Test]
        public void AreEqualDoubleNegative()
        {
            Assert.AreEqual((double)-1.0, (double)-1.0);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualDoubleNegativeFail()
        {
            Assert.AreEqual((double)-1.0, (double)1.0);
        }

        [Test]
        public void AreEqualDoubleDeltaNegativeInfinity()
        {
            Assert.AreEqual(double.NegativeInfinity, double.NegativeInfinity, (double) 0.0);
        }

        [Test]
        public void AreEqualDoubleDeltaPositiveInfinity()
        {
            Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity, (double)0.0);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualDoubleDeltaExpectedInfinityFail()
        {
            Assert.AreEqual(double.PositiveInfinity, (double)1.0, (double)0.0);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualDoubleDeltaActualInfinityFail()
        {
            Assert.AreEqual((double)1.0, double.PositiveInfinity, (double)0.0);
        }

        [Test]
        public void AreEqualDoubleDelta()
        {
            Assert.AreEqual((double)0.0, (double)1.0, (double)1.0);
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualDoubleDeltaFail()
        {
            Assert.AreEqual((double)0.0, (double)2.0, (double)1.0);
        }

        [Test]
        [ExpectedArgumentException()]
        public void AreEqualDoubleDeltaNegative()
        {
            Assert.AreEqual((double)0.0, (double)0.0, (double)-1.0);
        }

        
        #endregion

        #region AreEqual (Float)

        [Test]
        public void AreEqualFloatZero()
        {
            Assert.AreEqual((float)0.0, (float) 0.0);
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualFloatFail()
        {
            Assert.AreEqual((float)0.0, (float)1.0);
        }

        [Test]
        public void AreEqualFloatPositive()
        {
            Assert.AreEqual((float)1.0, (float)1.0);
        }

        [Test]
        public void AreEqualFloatNegative()
        {
            Assert.AreEqual((float)-1.0, (float)-1.0);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualFloatNegativeFail()
        {
            Assert.AreEqual((float)-1.0, (float)1.0);
        }

        [Test]
        public void AreEqualFloatDeltaNegativeInfinity()
        {
            Assert.AreEqual(float.NegativeInfinity, float.NegativeInfinity, (float)1.0);
        }

        [Test]
        public void AreEqualFloatDeltaPositiveInfinity()
        {
            Assert.AreEqual(float.PositiveInfinity, float.PositiveInfinity, (float)1.0);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualFloatDeltaExpectedInfinityFail()
        {
            Assert.AreEqual(float.PositiveInfinity, (float)1.0, (float)1.0);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualFloatDeltaActualInfinityFail()
        {
            Assert.AreEqual((float)1.0, float.PositiveInfinity, (float)1.0);
        }

        [Test]
        public void AreEqualFloatDelta()
        {
            Assert.AreEqual((float)0.0, (float)1.0, (float)1.0);
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualFloatDeltaFail()
        {
            Assert.AreEqual((float)0.0, (float)2.0, (float)1.0);
        }

        [Test]
        [ExpectedArgumentException()]
        public void AreEqualFloatDeltaNegative()
        {
            Assert.AreEqual((float)0.0, (float)0.0, (float)-1.0);
        }

        [Test]
        public void AreEqualFloatDeltaWithMessage()
        {
            Assert.AreEqual((float)1.0, (float)1.0, (float)0.0, "Assert AreEqual(message) has failed");
        }

        [Test]
        public void AreEqualFloatDeltaWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreEqual((float)0.0, (float)1.0, (float)0.0, EXPECTED_FAIL_MESSAGE);
            }
            catch (NotEqualAssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreEqual(message) has failed");
        }

        [Test]
        public void AreEqualFloatDeltaWithFormattedMessage()
        {
            float l = 1.0f;
            float r = 1.0f;
            float d = 1.0f;
            Assert.AreEqual(l, r, d, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void AreEqualFloatDeltaWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreEqual((float)0.0, (float)1.0, (float)0.0, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (NotEqualAssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreEqual(message, args) has failed");
        }
        #endregion

        #region AreEqual (Arrays)

        [Test]
        public void AreEqual_EqualIntArrays()
        {
            Assert.AreEqual(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 });
        }

        [Test]
        public void AreEqual_NotEqualForArrayAndNonArray()
        {
            Assert.AreNotEqual(new int[] { 1, 2, 3 }, 3);
        }

        [Test]
        public void AreEqual_ArrayOfNullValues()
        {
            object[] a = new object[3];
            object[] b = new object[3];
            Assert.AreEqual(a, b);
        }

        [Test]
        public void AreEqual_EqualArrayWithNullElements()
        {
            object[] a = { 1, 2, null };
            object[] b = { 1, 2, null };
            Assert.AreEqual(a, b);
        }

        [Test]
        public void AreEqual_EqualStringArrays()
        {
            Assert.AreEqual(new string[] { "1", "2", "3" }, new string[] { "1", "2", "3" });
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreEqual_UnEqualIntArrays()
        {
            Assert.AreEqual(new int[] { 1, 2, 3 }, new int[] { 1, 2, 4 });
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreEqual_UnEqualSizeIntArrays()
        {
            Assert.AreEqual(new int[0], new int[] { 1, 2 });
        }

        #endregion

        #region AreEqual (Decimal)

        private const Decimal TEST_DECIMAL = (Decimal) 1.034;
        private const Decimal TEST_OTHER_DECIMAL = (Decimal) 2.034;
 
        [Test]
        public void AreEqualDecimalZero()
        {
            Assert.AreEqual((decimal)0.0, (decimal)0.0);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualDecimalFail()
        {
            Assert.AreEqual(TEST_DECIMAL, TEST_OTHER_DECIMAL);
        }

        [Test]
        public void AreEqualDecimalPositive()
        {
            Assert.AreEqual(TEST_DECIMAL, TEST_DECIMAL);
        }

        [Test]
        public void AreEqualDecimalNegative()
        {
            Assert.AreEqual(-TEST_DECIMAL, -TEST_DECIMAL);
        }

        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualDecimalNegativeFail()
        {
            Assert.AreEqual(-TEST_DECIMAL, TEST_DECIMAL);
        }

        [Test]
        public void AreEqualDecimalWithMessage()
        {
            Decimal l = TEST_DECIMAL;
            Decimal r = TEST_DECIMAL;
            Assert.AreEqual(l, r, "Assert AreEqual(message) has failed");
        }

        [Test]
        public void AreEqualDecimalWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreEqual(TEST_DECIMAL, TEST_OTHER_DECIMAL, EXPECTED_FAIL_MESSAGE);
            }
            catch (NotEqualAssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreEqual(message) has failed");
        }

        [Test]
        public void AreEqualDecimalWithFormattedMessage()
        {
            Assert.AreEqual(TEST_DECIMAL, TEST_DECIMAL, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void AreEqualDecimalWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreEqual(TEST_DECIMAL, TEST_OTHER_DECIMAL, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (NotEqualAssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreEqual(message, args) has failed");
        }
        #endregion

        #region AreEqual (Integer)

        [Test]
        public void AreEqualIntZero()
        {
            Assert.AreEqual(0, 0);
        }
        
        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualIntFail()
        {
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void AreEqualIntPositive()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void AreEqualIntNegative()
        {
            Assert.AreEqual(-1, -1);
        }
        
        [Test]
        [ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualIntNegativeFail()
        {
            Assert.AreEqual(-1, 1);
        }

        [Test]
        public void AreEqualIntDelta()
        {
            Assert.AreEqual(0, 1, 1);
        }

        [Test, ExpectedException(typeof(NotEqualAssertionException))]
        public void AreEqualIntDeltaFail()
        {
            Assert.AreEqual(0, 2, 1);
        }

        [Test]
        [ExpectedArgumentException()]
        public void AreEqualIntDeltaNegative()
        {
            Assert.AreEqual(0, 0, -1);
        }

        [Test]
        public void AreEqualIntWithMessage()
        {
            Assert.AreEqual(1, 1, "Assert AreEqual(message) has failed");
        }

        [Test]
        public void AreEqualIntWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreEqual(0, 1, EXPECTED_FAIL_MESSAGE);
            }
            catch (NotEqualAssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreEqual(message) has failed");
        }

        [Test]
        public void AreEqualIntWithFormattedMessage()
        {
            Assert.AreEqual(1,1, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void AreEqualIntWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreEqual(0,1, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (NotEqualAssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreEqual(message, args) has failed");
        }

        #endregion


        [Test]
        public void AreSame()
        {
            ArrayList list = new ArrayList();
            Assert.AreSame(list, list);
        }
        #endregion

        #region AreNotEqual

        [Test]
        public void NotEqual()
        {
            Assert.AreNotEqual(5, 3);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotEqualFails()
        {
            Assert.AreNotEqual(5, 5);
        }

        [Test]
        public void NullNotEqualToNonNull()
        {
            Assert.AreNotEqual(null, 3);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NullEqualsNull()
        {
            Assert.AreNotEqual(null, null);
        }

        [Test]
        public void ArraysNotEqual()
        {
            Assert.AreNotEqual(new object[] { 1, 2, 3 }, new int[] { 1, 3, 2 });
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void ArraysNotEqualFails()
        {
            Assert.AreNotEqual(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 });
        }

        [Test]
        public void AreNotEqualUInt()
        {
            uint u1 = 5;
            uint u2 = 8;
            Assert.AreNotEqual(u1, u2);
        }

        [Test]
        public void AreNotEqualTwoArraysContainingNull()
        {
            Assert.AreNotEqual(new object[] { 1, 2, null }, new object[] { 1, null, 3 });
        }

        [Test]
        public void AreNotEqual_WithFormattedMessage()
        {
            Assert.AreNotEqual("abcd", "dcba", TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void AreNotEqualWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreNotEqual("abcd", "abcd", TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreNotEqual(message, args) has failed");
        }


        #region AreNotEqual (String)

        [Test]
        public void AreNotEqualString()
        {
            Assert.AreNotEqual("hello", "world");
        }

        [Test]
        public void AreNotEqualStringActualNullFail()
        {
            Assert.AreNotEqual("hello", null);
        }

        [Test]
        public void AreNotEqualStringExpectedNullFail()
        {
            Assert.AreNotEqual(null, "hello");
        }

        [Test]
        public void AreNotEqualStringOrdinalCompare()
        {
            Assert.AreNotEqual("hello", "HELLO");
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AreNotEqualStringFail()
        {
            Assert.AreNotEqual("hello", "hello");
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AreNotEqualEmptyString()
        {
            Assert.AreNotEqual("", "");
        }

        #endregion

        #region AreNotEqual (Double)

        [Test, ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDoubleZero()
        {
            Assert.AreNotEqual((double)0.0, (double)0.0);
        }

        [Test]
        public void AreNotEqualDouble()
        {
            Assert.AreNotEqual((double)0.0, (double)1.0);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDoublePositiveFail()
        {
            Assert.AreNotEqual((double)1.0, (double)1.0);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDoubleNegativeFail()
        {
            Assert.AreNotEqual((double)-1.0, (double)-1.0);
        }

        [Test]
        public void AreNotEqualDoubleNegativeAndPositive()
        {
            Assert.AreNotEqual((double)-1.0, (double)1.0);
        }

        [Test]
        public void AreNotEqualDoubleWithMessage()
        {
            Assert.AreNotEqual((double)1.0, (double)2.0, EXPECTED_FAIL_MESSAGE);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDoubleWithMessageFail()
        {
            Assert.AreNotEqual((double) 1.0, (double) 1.0, EXPECTED_FAIL_MESSAGE);
        }

        [Test]
        public void AreNotEqualDoubleWithFormattedMessage()
        {
            Assert.AreNotEqual((double)1.0, (double)2.0, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDoubleWithFormattedMessageFail()
        {
            Assert.AreNotEqual((double)1.0, (double)1.0, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        #endregion

        #region AreNotEqual (Float)

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualFloatZero()
        {
            Assert.AreNotEqual((float)0.0, (float)0.0);
        }

        [Test]
        public void AreNotEqualFloat()
        {
            Assert.AreNotEqual((float)0.0, (float)1.0);
        }

        [Test]
        public void AreNotEqualFloatPositive()
        {
            Assert.AreNotEqual((float)1.0, (float)2.0);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualFloatPositiveFail()
        {
            Assert.AreNotEqual((float)1.0, (float)1.0);
        }

        [Test]
        public void AreNotEqualFloatNegative()
        {
            Assert.AreNotEqual((float)-1.0, (float)-2.0);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualFloatNegativeFail()
        {
            Assert.AreNotEqual((float)-1.0, (float)-1.0);
        }

        [Test]
        public void AreNotEqualFloatNegativeAndPositive()
        {
            Assert.AreNotEqual((float)-1.0, (float)1.0);
        }


        [Test]
        public void AreNotEqualFloatWithMessage()
        {
            Assert.AreNotEqual((float)1.0, (float)3.0, "Assert AreNotEqual(message) has failed");
        }

        [Test]
        public void AreNotEqualFloatWithMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreNotEqual((float)1.0, (float)1.0, EXPECTED_FAIL_MESSAGE);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FAIL_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreNotEqual(message) has failed");
        }

        [Test]
        public void AreNotEqualFloatWithFormattedMessage()
        {
            float l = 1.0f;
            float r = 3.0f;
            Assert.AreNotEqual(l, r, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        public void AreNotEqualFloatWithFormattedMessageFail()
        {
            bool asserted = false;

            try
            {
                Assert.AreNotEqual((float)1.0, (float)1.0, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf(EXPECTED_FORMATTED_MESSAGE) >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert AreNotEqual(message, args) has failed");
        }
        #endregion

        #region AreEqual (Decimal)

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDecimalZero()
        {
            Assert.AreNotEqual((decimal)0.0, (decimal)0.0);
        }

        [Test]
        public void AreNotEqualDecimal()
        {
            Assert.AreNotEqual(TEST_DECIMAL, TEST_OTHER_DECIMAL);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDecimalPositiveFail()
        {
            Assert.AreNotEqual(TEST_DECIMAL, TEST_DECIMAL);
        }

        [Test]
        public void AreNotEqualDecimalNegative()
        {
            Assert.AreNotEqual(-TEST_DECIMAL, -TEST_OTHER_DECIMAL);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDecimalNegativeFail()
        {
            Assert.AreNotEqual(-TEST_DECIMAL, -TEST_DECIMAL);
        }

        [Test]
        public void AreNotEqualDecimalNegativePositive()
        {
            Assert.AreNotEqual(-TEST_DECIMAL, TEST_DECIMAL);
        }

        [Test]
        public void AreNotEqualDecimalWithMessage()
        {
            Assert.AreNotEqual(TEST_DECIMAL, TEST_OTHER_DECIMAL, EXPECTED_FAIL_MESSAGE);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDecimalWithMessageFail()
        {
            Assert.AreNotEqual(TEST_DECIMAL, TEST_DECIMAL, EXPECTED_FAIL_MESSAGE);
        }

        [Test]
        public void AreNotEqualDecimalWithFormattedMessage()
        {
            Assert.AreNotEqual(TEST_DECIMAL, TEST_OTHER_DECIMAL, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualDecimalWithFormattedMessageFail()
        {
            Assert.AreNotEqual(TEST_DECIMAL, TEST_DECIMAL, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        #region AreNotEqual (Integer)

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualIntZero()
        {
            Assert.AreNotEqual(0, 0);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualIntFail()
        {
            Assert.AreNotEqual(1, 1);
        }

        [Test]
        public void AreNotEqualIntPositive()
        {
            Assert.AreNotEqual(1, 2);
        }

        [Test]
        public void AreNotEqualIntNegative()
        {
            Assert.AreNotEqual(-1, -2);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualIntNegativeFail()
        {
            Assert.AreNotEqual(-1, -1);
        }

        [Test]
        public void AreNotEqualIntWithMessage()
        {
            Assert.AreNotEqual(1, 2, "Assert AreEqual(message) has failed");
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualIntWithMessageFail()
        {
            Assert.AreNotEqual(1, 1, EXPECTED_FAIL_MESSAGE);
        }

        [Test]
        public void AreNotEqualIntWithFormattedMessage()
        {
            Assert.AreNotEqual(1, 2, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreNotEqualIntWithFormattedMessageFail()
        {
            Assert.AreNotEqual(1, 1, TEST_FORMAT_STRING, TEST_FORMAT_STRING_PARAM1, TEST_FORMAT_STRING_PARAM2);
        }

        #endregion

        #endregion

        #endregion

        #region Between

        #region int
        [Test]
        public void BetweenInt()
        {
            Assert.Between(1, 0, 2);
        }
        [Test]
        public void BetweenLowerEqualInt()
        {
            Assert.Between(1, 1, 2);
        }
        [Test]
        public void BetweenUpperEqualInt()
        {
            Assert.Between(2, 0, 2);
        }
        #endregion

        #region short
        [Test]
        public void BetweenShort()
        {
            Assert.Between((short)1, (short)0, (short)2);
        }
        [Test]
        public void BetweenLowerEqualShort()
        {
            Assert.Between((short)1, (short)1, (short)2);
        }
        [Test]
        public void BetweenUpperEqualShort()
        {
            Assert.Between((short)2, (short)0, (short)2);
        }
        #endregion

        #region byte
        [Test]
        public void BetweenByte()
        {
            Assert.Between((byte)1, (byte)0, (byte)2);
        }
        [Test]
        public void BetweenLowerEqualByte()
        {
            Assert.Between((byte)1, (byte)1, (byte)2);
        }
        [Test]
        public void BetweenUpperEqualByte()
        {
            Assert.Between((byte)2, (byte)0, (byte)2);
        }
        #endregion

        #region long
        [Test]
        public void BetweenLong()
        {
            Assert.Between((long)1, (long)0, (long)2);
        }
        [Test]
        public void BetweenLowerEqualLong()
        {
            Assert.Between((long)1, (long)1, (long)2);
        }
        [Test]
        public void BetweenUpperEqualLong()
        {
            Assert.Between((long)2, (long)0, (long)2);
        }
        #endregion

        #region double
        [Test]
        public void BetweenDouble()
        {
            Assert.Between((double)1, (double)0, (double)2);
        }
        [Test]
        public void BetweenLowerEqualDouble()
        {
            Assert.Between((double)1, (double)1, (double)2);
        }
        [Test]
        public void BetweenUpperEqualDouble()
        {
            Assert.Between((double)2, (double)0, (double)2);
        }
        #endregion

        #region float
        [Test]
        public void BetweenFloat()
        {
            Assert.Between((float)1, (float)0, (float)2);
        }
        [Test]
        public void BetweenLowerEqualFloat()
        {
            Assert.Between((float)1, (float)1, (float)2);
        }
        [Test]
        public void BetweenUpperEqualFloat()
        {
            Assert.Between((float)2, (float)0, (float)2);
        }
        #endregion

        #endregion

        #region NotBetween

        #region int
        [Test]
        public void NotBetweenIntInside()
        {
            Assert.NotBetween(4, 2, 3);
        }

        [Test]
        public void NotBetweenLowerInt()
        {
            Assert.NotBetween(0, 2, 3);
        }

        [Test]
        public void NotBetweenUpperInt()
        {
            Assert.NotBetween(4, 2, 3);
        }
        #endregion

        #region short
        [Test]
        public void NotBetweenShortInside()
        {
            Assert.NotBetween((short)4, (short)2, (short)3);
        }
        [Test]
        public void NotBetweenLowerShort()
        {
            Assert.NotBetween((short)0, (short)2, (short)3);
        }

        [Test]
        public void NotBetweenUpperShort()
        {
            Assert.NotBetween((short)4, (short)2, (short)3);
        }
        #endregion

        #region byte

        [Test]
        public void NotBetweenByteInside()
        {
            Assert.NotBetween((byte)4, (byte)2, (byte)3);
        }
        [Test]
        public void NotBetweenLowerByte()
        {
            Assert.NotBetween((byte)0, (byte)2, (byte)3);
        }

        [Test]
        public void NotBetweenUpperByte()
        {
            Assert.NotBetween((byte)4, (byte)2, (byte)3);
        }
        #endregion

        #region long
        [Test]
        public void NotBetweenLongInside()
        {
            Assert.NotBetween((long)4, (long)2, (long)3);
        }
        [Test]
        public void NotBetweenLowerLong()
        {
            Assert.NotBetween((long)0, (long)2, (long)3);
        }

        [Test]
        public void NotBetweenUpperLong()
        {
            Assert.NotBetween((long)4, (long)2, (long)3);
        }
        #endregion

        #region double
        [Test]
        public void NotBetweenDoubleInside()
        {
            Assert.NotBetween((double)4, (double)2, (double)3);
        }
        [Test]
        public void NotBetweenLowerDouble()
        {
            Assert.NotBetween((double)0, (double)2, (double)3);
        }

        [Test]
        public void NotBetweenUpperDouble()
        {
            Assert.NotBetween((double)4, (double)2, (double)3);
        }
        #endregion

        #region float
        [Test] 
        public void NotBetweenFloatInside()
        {
            Assert.NotBetween((float)4, (float)2, (float)3);
        }

        [Test]
        public void NotBetweenLowerFloat()
        {
            Assert.NotBetween((float)0, (float)2, (float)3);
        }

        [Test]
        public void NotBetweenUpperFloat()
        {
            Assert.NotBetween((float)4, (float)2, (float)3);
        }
        #endregion

        #endregion

        #region <, <=, >, >=
        #region LowerThan
        [Test]
        public void LowerThanInt()
        {
            Assert.LowerThan(0, 1);
        }
        [Test]
        public void LowerThanShort()
        {
            Assert.LowerThan((short)0, (short)1);
        }

        [Test]
        public void LowerThanByte()
        {
            Assert.LowerThan((byte)0, (byte)1);
        }
        [Test]
        public void LowerThanLong()
        {
            Assert.LowerThan((long)0, (long)1);
        }

        [Test]
        public void LowerThanDouble()
        {
            Assert.LowerThan((double)0, (double)1);
        }

        [Test]
        public void LowerThanFloat()
        {
            Assert.LowerThan((float)0, (float)1);
        }
        //[Test]
        //public void LowerThanFailEqualFloat()
        //{
        //    Assert.LowerThan((float)0, (float)0);
        //}
        //[Test]
        //public void LowerThanFailLessFloat()
        //{
        //    Assert.LowerThan((float)0, (float)-1);
        //}
        #endregion

        #region LowerEqualThan
        [Test]
        public void LowerEqualThanInt()
        {
            Assert.LowerEqualThan(0, 1);
        }
        [Test]
        public void LowerEqualThanEqualInt()
        {
            Assert.LowerEqualThan(0, 0);
        }

        [Test]
        public void LowerEqualThanShort()
        {
            Assert.LowerEqualThan((short)0, (short)1);
        }
        [Test]
        public void LowerEqualThanEqualShort()
        {
            Assert.LowerEqualThan((short)0, (short)0);
        }

        [Test]
        public void LowerEqualThanByte()
        {
            Assert.LowerEqualThan((byte)0, (byte)1);
        }
        [Test]
        public void LowerEqualThanEqualByte()
        {
            Assert.LowerEqualThan((byte)0, (byte)0);
        }

        [Test]
        public void LowerEqualThanLong()
        {
            Assert.LowerEqualThan((long)0, (long)1);
        }
        [Test]
        public void LowerEqualThanEqualLong()
        {
            Assert.LowerEqualThan((long)0, (long)0);
        }

        [Test]
        public void LowerEqualThanDouble()
        {
            Assert.LowerEqualThan((double)0, (double)1);
        }
        [Test]
        public void LowerEqualThanEqualDouble()
        {
            Assert.LowerEqualThan((double)0, (double)0);
        }

        [Test]
        public void LowerEqualThanFloat()
        {
            Assert.LowerEqualThan((float)0, (float)1);
        }
        [Test]
        public void LowerEqualThanEqualFloat()
        {
            Assert.LowerEqualThan((float)0, (float)0);
        }

        #endregion

        #region Less
        [Test]
        public void LessInt()
        {
            Assert.Less(0, 1);
        }
        [Test]
        public void LessShort()
        {
            Assert.Less((short)0, (short)1);
        }

        [Test]
        public void LessByte()
        {
            Assert.Less((byte)0, (byte)1);
        }
        [Test]
        public void LessLong()
        {
            Assert.Less((long)0, (long)1);
        }

        [Test]
        public void LessDouble()
        {
            Assert.Less((double)0, (double)1);
        }

        [Test]
        public void LessFloat()
        {
            Assert.Less((float)0, (float)1);
        }
        #endregion

        #region GreaterThan
        [Test]
        public void GreaterThanInt()
        {
            Assert.GreaterThan(1, 0);
        }

        [Test]
        public void GreaterThanShort()
        {
            Assert.GreaterThan((short)1, (short)0);
        }

        [Test]
        public void GreaterThanByte()
        {
            Assert.GreaterThan((byte)1, (byte)0);
        }

        [Test]
        public void GreaterThanLong()
        {
            Assert.GreaterThan((long)1, (long)0);
        }

        [Test]
        public void GreaterThanDouble()
        {
            Assert.GreaterThan((double)1, (double)0);
        }

        [Test]
        public void GreaterThanFloat()
        {
            Assert.GreaterThan((float)1, (float)0);
        }

        #endregion

        #region Greater

        [Test]
        public void GreaterInt()
        {
            Assert.Greater(1, 0);
        }

        [Test]
        public void GreaterIntWithMessage()
        {
            Assert.Greater(1, 0, "Int is not greater");
        }

        [Test]
        public void GreaterIntWithMessageAndArgs()
        {
            Assert.Greater(1, 0, "{0} is not greater than {1}", 1, 0);
        }

        [Test]
        public void GreaterUint()
        {
            Assert.Greater((uint)1, (uint)0);
        }

        [Test]
        public void GreaterUintWithMessage()
        {
            Assert.Greater((uint)1, (uint)0, "Int is not greater");
        }

        [Test]
        public void GreaterUintWithMessageAndArgs()
        {
            Assert.Greater((uint)1, (uint)0, "{0} is not greater than {1}", 1, 0);
        }

        [Test]
        public void GreaterShort()
        {
            Assert.Greater((short)1, (short)0);
        }

        [Test]
        public void GreaterShortWithMessage()
        {
            Assert.Greater((short)1, (short)0, "Short is not greater");
        }

        [Test]
        public void GreaterShortWithMessageAndArgs()
        {
            Assert.Greater((short)1, (short)0, "{0} is not greater than {1}", (short)1, (short)0);
        }
        
        [Test]
        public void GreaterByte()
        {
            Assert.Greater((byte)1, (byte)0);
        }

        [Test]
        public void GreaterByteWithMessage()
        {
            Assert.Greater((byte)1, (byte)0, "Byte is not greater");
        }

        [Test]
        public void GreaterByteWithMessageAndArgs()
        {
            Assert.Greater((byte)1, (byte)0, "{0} is not greater than {1}", (byte)0, (byte)1);
        }

        [Test]
        public void GreaterDecimal()
        {
            Assert.Greater((decimal)1, (decimal)0);
        }
        
        [Test]
        public void GreaterDecimalWithMessage()
        {
            Assert.Greater((decimal)1, (decimal)0, "Decimal is not greater");
        }

        [Test]
        public void GreaterDecimalWithMessageAndArgs()
        {
            Assert.Greater((decimal)1, (decimal)0, "{0} is not greater than {1}", (decimal)1, (decimal)0);
        }

        [Test]
        public void GreaterLong()
        {
            Assert.Greater((long)1, (long)0);
        }

        [Test]
        public void GreaterLongWithMessage()
        {
            Assert.Greater((long)1, (long)0, "Long is not greater");
        }

        [Test]
        public void GreaterLongWithMessageAndArgs()
        {
            Assert.Greater((long)1, (long)0, "{0} is not greater than {1}", (long)1, (long)0);
        }

        [Test]
        public void GreaterDouble()
        {
            Assert.Greater((double)1, (double)0);
        }

        [Test]
        public void GreaterDoubleWithMessage()
        {
            Assert.Greater((double)1, (double)0, "Double is not greater");
        }

        [Test]
        public void GreaterDoubleWithMessageAndArgs()
        {
            Assert.Greater((double)1, (double)0, "{0} is not greater than {1}", (double)1, (double)0);
        }

        [Test]
        public void GreaterFloat()
        {
            Assert.Greater((float)1, (float)0);
        }

        [Test]
        public void GreaterFloatWithMessage()
        {
            Assert.Greater((float)1, (float)0, "Float is not greater");
        }

        [Test]
        public void GreaterFloatWithMessageAndArgs()
        {
            Assert.Greater((float)1, (float)0, "{0} is not greater than {1}", (float)1, (float)0);
        }

        [Test]
        public void GreaterIComparable()
        {
            Assert.Greater(DateTime.Now, new DateTime(2000, 1, 1));
        }

        [Test]
        public void GreaterIComparableWithMessage()
        {
            Assert.Greater(DateTime.Now, new DateTime(2000, 1, 1), "DateTime is not greater");
        }

        [Test]
        public void GreaterIComparableWithMessageAndArgs()
        {
            DateTime actual = DateTime.Now;
            DateTime expected = new DateTime(2000, 1, 1);
            Assert.Greater(actual, expected, "{0} is not greater than {1}", actual, expected);
        }

        #endregion

        #region GreaterEqualThan
        [Test]
        public void GreaterEqualThanInt()
        {
            Assert.GreaterEqualThan(1, 0);
        }

        [Test]
        public void GreaterEqualThanShort()
        {
            Assert.GreaterEqualThan((short)1, (short)0);
        }
        [Test]
        public void GreaterEqualThanEqualShort()
        {
            Assert.GreaterEqualThan((short)0, (short)0);
        }

        [Test]
        public void GreaterEqualThanByte()
        {
            Assert.GreaterEqualThan((byte)1, (byte)0);
        }

        [Test]
        public void GreaterEqualThanEqualByte()
        {
            Assert.GreaterEqualThan((byte)0, (byte)0);
        }

        [Test]
        public void GreaterEqualThanLong()
        {
            Assert.GreaterEqualThan((long)1, (long)0);
        }

        [Test]
        public void GreaterEqualThanEqualLong()
        {
            Assert.GreaterEqualThan((long)0, (long)0);
        }

        [Test]
        public void GreaterEqualThanDouble()
        {
            Assert.GreaterEqualThan((double)1, (double)0);
        }

        [Test]
        public void GreaterEqualThanEqualDouble()
        {
            Assert.GreaterEqualThan((double)0, (double)0);
        }

        [Test]
        public void GreaterEqualThanFloat()
        {
            Assert.GreaterEqualThan((float)1, (float)0);
        }

        [Test]
        public void GreaterEqualThanEqualFloat()
        {
            Assert.GreaterEqualThan((float)0, (float)0);
        }

        #endregion
        #endregion

        #region In, NotIn

        #region In

        #region IDictionary
        [Test]
        public void InDictionary()
        {
            Hashtable dic = new Hashtable();
            string test = "test";
            dic.Add(test, null);
            Assert.In(test, dic);
        }

        #endregion

        #region IList

        [Test]
        public void InListTestNull()
        {
            ArrayList list = new ArrayList();
            list.Add(null);
            Assert.In(null, list);
        }
        [Test]
        public void InList()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.In(test, list);
        }

        #endregion

        #region IEnumerable
     
        [Test]
        public void InEnumerableTestNull()
        {
            ArrayList list = new ArrayList();
            list.Add(null);
            Assert.In(null, (IEnumerable)list);
        }

        [Test]
        public void InEnumerable()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.In(test, (IEnumerable)list);
        }

        [Test]
        public void In_ValueEquality()
        {
            string[] stringArray = {"item1", "item2"};
            IEnumerable enumerableStringArray = stringArray;
            string item1 = string.Format("item{0}", 1);
            Assert.In(item1, enumerableStringArray);
        }

        [Test]
        public void In_NullItem()
        {
            string[] stringArray = { "item1", null, "item2" };
            IEnumerable enumerableStringArray = stringArray;
            Assert.In(null, enumerableStringArray);
        }

        [Test]
        public void InEnumerableWithMessage()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);

            Assert.In(test, (IEnumerable)list, "InEnumerable Failed");
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void InEnumerableFail()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);

            Assert.In("someOtherObject", (IEnumerable)list);
        }

        [Test]
        public void InEnumerableWithMessageFail()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);

            bool asserted = false;
            try
            {
                Assert.In("someOtherObject", (IEnumerable)list, "InEnumerable Failed");
            }
            catch (AssertionException ex)
            {
                Assert.IsTrue(ex.Message.IndexOf("InEnumerable Failed") >= 0);
                asserted = true;
            }

            if (!asserted)
                Assert.Fail("Assert InEnumerable(message) has failed");
        }
        #endregion

        #endregion

        #region NotIn

        #region IDictionary
     
        [Test]
        public void NotInDictionary()
        {
            Hashtable dic = new Hashtable();
            string test = "test";
            dic.Add(test, null);
            Assert.NotIn(test + "modified", dic);
        }

        #endregion

        #region IList

        [Test]
        public void NotInListTestNull()
        {
            Assert.NotIn(null, new ArrayList());
        }
        [Test]
        public void NotInList()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.NotIn(test + "modified", list);
        }

        #endregion

        #region IEnumerable

        [Test]
        public void NotInEnumerableTestNull()
        {
            Assert.NotIn(null, (IEnumerable)new ArrayList());
        }

        [Test]
        public void NotInEnumerable()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.NotIn(test + "modified", (IEnumerable)list);
        }

        #endregion

        #endregion

        #endregion

        #region IsEmpty
        //NUnit Code
        [Test]
        public void IsEmpty()
        {
            Assert.IsEmpty("", "Failed on empty String");
            Assert.IsEmpty(new int[0], "Failed on empty Array");
            Assert.IsEmpty(new ArrayList(), "Failed on empty ArrayList");
            Assert.IsEmpty(new Hashtable(), "Failed on empty Hashtable");
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsEmptyFailsOnString()
        {
            Assert.IsEmpty("Hi!");
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsEmptyFailsOnNullString()
        {
            Assert.IsEmpty((string)null);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsEmptyFailsOnNonEmptyArray()
        {
            Assert.IsEmpty(new int[] { 1, 2, 3 });
        }


        #endregion

        #region IsNotEmpty
        [Test]
        public void IsNotEmpty()
        {
            ArrayList arr = new ArrayList();
            arr.Add("Testing");

            Hashtable hash = new Hashtable();
            hash.Add("MbUnit", "Testing");

            Assert.IsNotEmpty("MbUnit", "Failed on non empty String");
            Assert.IsNotEmpty(new int[1] { 1 }, "Failed on non empty Array");
            Assert.IsNotEmpty(arr, "Failed on non empty ArrayList");
            Assert.IsNotEmpty(hash, "Failed on empty Hashtable");
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsNotEmptyFailsOnString()
        {
            Assert.IsNotEmpty(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsNotEmptyFailsOnNonEmptyArray()
        {
            Assert.IsNotEmpty(new int[0] { });
        }


        #endregion

        #region IsNan
        //Nunit Code
        [Test]
        public void IsNaN()
        {
            Assert.IsNaN(double.NaN);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsNaNFails()
        {
            Assert.IsNaN(10.0);
        }

        #endregion

        #region Contains

        [Test]
        public void Contains()
        {
            string s = "MbUnit";
            string contain = "Unit";
            Assert.Contains(s, contain);
        }

        #endregion

        #region Equals

        [Test, ExpectedException(typeof(AssertionException))]
        public void Equals()
        {
            Assert.Equals(null, null);
        }

        #endregion



    }
}
