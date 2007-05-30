
namespace MbUnit.Tests.Core.Framework 
{
	using System;
	using System.Collections;
	using MbUnit.Core.Framework;
	using MbUnit.Framework;
	using MbUnit.Core;
	using MbUnit.Core.Exceptions;
	
	[TestFixture("Assertion test")]
    [FixtureCategory("Framework.Assertions")]
	public class AssertTest
    {
		#region Messages
		[Test]
		public void FailWithCustomMessage()
		{
			string message = "CustomMessage";
			try
			{
				Assert.Fail("CustomMessage");
			}
			catch(AssertionException ex)
			{
				Assert.AreEqual(message,ex.Message);
			}
		}
		[Test]
		public void FailWithFormattedCustomMessage()
		{
			string format = "CustomMessage {0}";
			string arg = "hello";
			string message = String.Format(format,arg);
			try
			{
				Assert.Fail(format,arg);
			}
			catch(AssertionException ex)
			{
				Assert.AreEqual(message,ex.Message);
			}
		}
		[Test]
		public void AssertEqualWithFormattedCustomMessage()
		{
			string format = "CustomMessage {0}";
			string arg = "hello";
			string message = String.Format(format,arg);
			try
			{
				Assert.AreEqual(0,1,format,arg);
			}
			catch(AssertionException ex)
			{
                StringAssert.StartsWith(ex.Message, message);
			}
		}

		#endregion

        #region Fail, Ignore, IsNull, IsNotNull, IsTrue, IsFalse
        [Test]
        [ExpectedException(typeof(IgnoreRunException))]
        public void Ignore()
        {
            Assert.Ignore("Because I want to");
        }

        [Test]
		[ExpectedException(typeof(AssertionException))]
		public void Fail()
		{
			Assert.Fail();
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void FailWithMessage()
		{
			string message = "hello";
			Assert.Fail("this is a {0}",message);
		}
		
		[Test]
		public void IsNull()
		{
			Assert.IsNull(null);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsNullFail()
		{
			Assert.IsNull("hello");
		}		
		
		[Test]
		public void IsNotNull()
		{
			Assert.IsNotNull("hello");
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsNotNullFail()
		{
			Assert.IsNotNull(null);
		}		
		
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
        #endregion

        #region AreEqual
        [Test]
		public void AreEqualInt()
		{
			Assert.AreEqual(0,0);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualIntFail()
		{
			Assert.AreEqual(0,1);
		}

		[Test]
		public void AreEqualIntDelta()
		{
			Assert.AreEqual(0,1,1);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualIntDeltaFail()
		{
			Assert.AreEqual(0,2,1);
		}
		
		[Test]
		public void AreEqualString()
		{
			Assert.AreEqual("hello","hello");
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualStringFail()
		{
			Assert.AreEqual("hello","world");
		}

		[Test]
		public void AreEqualDecimal()
		{
			Decimal l = 0;
			Decimal r = 0;
			Assert.AreEqual(l,r);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualDecimalFail()
		{
			Decimal l = 0;
			Decimal r = 1;
			Assert.AreEqual(l,r);
		}
		
		[Test]
		public void AreEqualDoubleDelta()
		{
			Assert.AreEqual(0.0,1.0,1.0);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualDoubleDeltaFail()
		{
			Assert.AreEqual(0.0,1.0,0.5);
		}

		[Test]
		public void AreEqualFloatDelta()
		{
			float l = 0;
			float r = 1;
			Assert.AreEqual(l,r,r);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualFloatDeltaFail()
		{
			float l = 0;
			float r = 1;
			float d = 0.5F;
			Assert.AreEqual(l,r,d);
		}
		
		[Test]
		public void AreSame()
		{
			ArrayList list = new ArrayList();
			Assert.AreSame(list,list);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreSameFail()
		{
			ArrayList list = new ArrayList();
			ArrayList list2 = new ArrayList();
			Assert.AreSame(list,list2);
        }

        [Test]
        public void AreEqual_EqualIntArrays()
        {
            Assert.AreEqual(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 });
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
            Assert.AreEqual(new int[] { 1, 2, 3 }, new int[] { 1, 2, 4});
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreEqual_UnEqualSizeIntArrays()
        {
            Assert.AreEqual(new int[0], new int[] { 1, 2});
        }

        #endregion

        #region Between

        #region int
        [Test]
		public void BetweenInt()
		{
			Assert.Between(1,0,2);
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
        [Test]
		[ExpectedException(typeof(AssertionException))]
		public void BetweenLowerFailInt()
		{
			Assert.Between(-1,0,1);			
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void BetweenUpperFailInt()
		{
			Assert.Between(2,0,1);
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenLowerFailShort()
        {
            Assert.Between((short)-1, (short)0, (short)1);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenUpperFailShort()
        {
            Assert.Between((short)2, (short)0, (short)1);
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenLowerFailByte()
        {
            Assert.Between((byte)0, (byte)1, (byte)2);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenUpperFailByte()
        {
            Assert.Between((byte)2, (byte)0, (byte)1);
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenLowerFailLong()
        {
            Assert.Between((long)-1, (long)0, (long)1);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenUpperFailLong()
        {
            Assert.Between((long)2, (long)0, (long)1);
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenLowerFailDouble()
        {
            Assert.Between((double)-1, (double)0, (double)1);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenUpperFailDouble()
        {
            Assert.Between((double)2, (double)0, (double)1);
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenLowerFailFloat()
        {
            Assert.Between((float)-1, (float)0, (float)1);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void BetweenUpperFailFloat()
        {
            Assert.Between((float)2, (float)0, (float)1);
        }
        #endregion

        #endregion

        #region NotBetween

        #region int
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenIntInside()
        {
            Assert.NotBetween(2, 1, 3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenLowerEqualFailInt()
        {
            Assert.NotBetween(2, 2, 3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenUpperEqualFailInt()
        {
            Assert.NotBetween(3, 2, 3);
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
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenShortInside()
        {
            Assert.NotBetween((short)2, (short)1, (short)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenLowerEqualFailShort()
        {
            Assert.NotBetween((short)2, (short)2, (short)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenUpperEqualFailShort()
        {
            Assert.NotBetween((short)3, (short)2, (short)3);
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
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenByteInside()
        {
            Assert.NotBetween((byte)2, (byte)1, (byte)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenLowerEqualFailByte()
        {
            Assert.NotBetween((byte)2, (byte)2, (byte)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenUpperEqualFailByte()
        {
            Assert.NotBetween((byte)3, (byte)2, (byte)3);
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
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenLongInside()
        {
            Assert.NotBetween((long)2, (long)1, (long)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenLowerEqualFailLong()
        {
            Assert.NotBetween((long)2, (long)2, (long)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenUpperEqualFailLong()
        {
            Assert.NotBetween((long)3, (long)2, (long)3);
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
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenDoubleInside()
        {
            Assert.NotBetween((double)2, (double)1, (double)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenLowerEqualFailDouble()
        {
            Assert.NotBetween((double)2, (double)2, (double)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenUpperEqualFailDouble()
        {
            Assert.NotBetween((double)3, (double)2, (double)3);
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
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenFloatInside()
        {
            Assert.NotBetween((float)2, (float)1, (float)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenLowerEqualFailFloat()
        {
            Assert.NotBetween((float)2, (float)2, (float)3);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotBetweenUpperEqualFailFloat()
        {
            Assert.NotBetween((float)3, (float)2, (float)3);
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
			Assert.LowerThan(0,1);
		}
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void LowerThanFailEqualInt()
		{
			Assert.LowerThan(0,0);
		}
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void LowerThanFailLessInt()
		{
			Assert.LowerThan(0,-1);
		}

        [Test]
        public void LowerThanShort()
        {
            Assert.LowerThan((short)0, (short)1);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailEqualShort()
        {
            Assert.LowerThan((short)0, (short)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailLessShort()
        {
            Assert.LowerThan((short)0, (short)-1);
        }


        [Test]
        public void LowerThanByte()
        {
            Assert.LowerThan((byte)0, (byte)1);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailEqualByte()
        {
            Assert.LowerThan((byte)0, (byte)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailLessByte()
        {
            Assert.LowerThan((byte)1, (byte)0);
        }

        [Test]
        public void LowerThanLong()
        {
            Assert.LowerThan((long)0, (long)1);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailEqualLong()
        {
            Assert.LowerThan((long)0, (long)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailLessLong()
        {
            Assert.LowerThan((long)0, (long)-1);
        }

        [Test]
        public void LowerThanDouble()
        {
            Assert.LowerThan((double)0, (double)1);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailEqualDouble()
        {
            Assert.LowerThan((double)0, (double)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailLessDouble()
        {
            Assert.LowerThan((double)0, (double)-1);
        }

        [Test]
        public void LowerThanFloat()
        {
            Assert.LowerThan((float)0, (float)1);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailEqualFloat()
        {
            Assert.LowerThan((float)0, (float)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerThanFailLessFloat()
        {
            Assert.LowerThan((float)0, (float)-1);
        }
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
        [ExpectedException(typeof(AssertionException))]
        public void LowerEqualThanFailLessInt()
        {
            Assert.LowerEqualThan(0, -1);
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
        [ExpectedException(typeof(AssertionException))]
        public void LowerEqualThanFailLessShort()
        {
            Assert.LowerEqualThan((short)0, (short)-1);
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
        [ExpectedException(typeof(AssertionException))]
        public void LowerEqualThanFailLessByte()
        {
            Assert.LowerEqualThan((byte)1, (byte)0);
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
        [ExpectedException(typeof(AssertionException))]
        public void LowerEqualThanFailLessLong()
        {
            Assert.LowerEqualThan((long)0, (long)-1);
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
        [ExpectedException(typeof(AssertionException))]
        public void LowerEqualThanFailLessDouble()
        {
            Assert.LowerEqualThan((double)0, (double)-1);
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void LowerEqualThanFailLessFloat()
        {
            Assert.LowerEqualThan((float)0, (float)-1);
        }
        #endregion

        #region GreaterThan
        [Test]
        public void GreaterThanInt()
        {
            Assert.GreaterThan(1, 0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailEqualInt()
        {
            Assert.GreaterThan(0, 0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailLessInt()
        {
            Assert.GreaterThan(0, 1);
        }

        [Test]
        public void GreaterThanShort()
        {
            Assert.GreaterThan((short)1, (short)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailEqualShort()
        {
            Assert.GreaterThan((short)0, (short)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailLessShort()
        {
            Assert.GreaterThan((short)0, (short)1);
        }


        [Test]
        public void GreaterThanByte()
        {
            Assert.GreaterThan((byte)1, (byte)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailEqualByte()
        {
            Assert.GreaterThan((byte)0, (byte)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailLessByte()
        {
            Assert.GreaterThan((byte)0, (byte)1);
        }

        [Test]
        public void GreaterThanLong()
        {
            Assert.GreaterThan((long)1, (long)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailEqualLong()
        {
            Assert.GreaterThan((long)0, (long)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailLessLong()
        {
            Assert.GreaterThan((long)0, (long)1);
        }

        [Test]
        public void GreaterThanDouble()
        {
            Assert.GreaterThan((double)1, (double)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailEqualDouble()
        {
            Assert.GreaterThan((double)0, (double)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailLessDouble()
        {
            Assert.GreaterThan((double)0, (double)1);
        }

        [Test]
        public void GreaterThanFloat()
        {
            Assert.GreaterThan((float)1, (float)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailEqualFloat()
        {
            Assert.GreaterThan((float)0, (float)0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterThanFailLessFloat()
        {
            Assert.GreaterThan((float)0, (float)1);
        }
        #endregion

        #region GreaterEqualThan
        [Test]
        public void GreaterEqualThanInt()
        {
            Assert.GreaterEqualThan(1, 0);
        }
        [Test]
        public void GreaterEqualThanEqualInt()
        {
            Assert.GreaterEqualThan(0, 0);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterEqualThanFailLessInt()
        {
            Assert.GreaterEqualThan(0, 1);
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
        [ExpectedException(typeof(AssertionException))]
        public void GreaterEqualThanFailLessShort()
        {
            Assert.GreaterEqualThan((short)0, (short)1);
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
        [ExpectedException(typeof(AssertionException))]
        public void GreaterEqualThanFailLessByte()
        {
            Assert.GreaterEqualThan((byte)0, (byte)1);
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
        [ExpectedException(typeof(AssertionException))]
        public void GreaterEqualThanFailLessLong()
        {
            Assert.GreaterEqualThan((long)0, (long)1);
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
        [ExpectedException(typeof(AssertionException))]
        public void GreaterEqualThanFailLessDouble()
        {
            Assert.GreaterEqualThan((double)0, (double)1);
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void GreaterEqualThanFailLessFloat()
        {
            Assert.GreaterEqualThan((float)0, (float)1);
        }
        #endregion
        #endregion

        #region In, NotIn

        #region In

        #region IDictionary
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void InDictionaryDictionaryNull()
        {
            Hashtable table = null;
            Assert.In("test", table);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void InDictionaryTestNull()
        {
            Assert.In(null, new Hashtable());
        }
        [Test]
        public void InDictionary()
        {
            Hashtable dic = new Hashtable();
            string test = "test";
            dic.Add(test, null);
            Assert.In(test, dic);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void InDictionaryFail()
        {
            Hashtable dic = new Hashtable();
            string test = "test";
            dic.Add(test, null);
            Assert.In(test+"modified", dic);
        }
        #endregion

        #region IList
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void InListListNull()
        {
            IList table = null;
            Assert.In("test", table);
        }
        [Test]
        public void InListTestNull()
        {
            ArrayList list= new ArrayList();
            list.Add(null);
            Assert.In(null,list );
        }
        [Test]
        public void InList()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.In(test, list);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void InListFail()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.In(test+"modified", list);
        }
        #endregion 

        #region IEnumerable
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void InEnumerableListNull()
        {
            IEnumerable table = null;
            Assert.In("test", table);
        }
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
        [ExpectedException(typeof(AssertionException))]
        public void InEnumerableFail()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.In(test + "modified", (IEnumerable)list);
        }
        #endregion 

        #endregion

        #region NotIn

        #region IDictionary
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotInDictionaryDictionaryNull()
        {
            Hashtable table = null;
            Assert.NotIn("test", table);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotInDictionaryTestNull()
        {
            Assert.NotIn(null, new Hashtable());
        }
        [Test]
        public void NotInDictionary()
        {
            Hashtable dic = new Hashtable();
            string test = "test";
            dic.Add(test, null);
            Assert.NotIn(test + "modified", dic);
        }
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotInDictionaryFail()
        {
            Hashtable dic = new Hashtable();
            string test = "test";
            dic.Add(test, null);
            Assert.NotIn(test, dic);
        }
        #endregion

        #region IList
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotInListListNull()
        {
            IList table = null;
            Assert.NotIn("test", table);
        }
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotInListFail()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.NotIn(test, list);
        }
        #endregion

        #region IEnumerable
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotInEnumerableEnumerableNull()
        {
            IEnumerable table = null;
            Assert.NotIn("test", table);
        }
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
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotInEnumerableFail()
        {
            ArrayList list = new ArrayList();
            string test = "test";
            list.Add(test);
            Assert.NotIn(test, (IEnumerable)list);
        }
        #endregion

        #endregion
        #endregion
    }
}
