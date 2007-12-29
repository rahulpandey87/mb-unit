namespace MbUnit.Tests.Core.Framework
{
	using System;
	
	using MbUnit.Core.Framework;
	using MbUnit.Framework;
	using MbUnit.Core.Exceptions;
	
	/// <summary>
	/// Test fixture of the <see cref="StringAssert"/>.
	/// </summary>
	/// <remark>
	/// </remarks>
	[TestFixture]
	public class StringAssertTest
    {
        #region AreEqual
        [Test]
		public void AreEqualIgnoreCase()
		{
			StringAssert.AreEqualIgnoreCase("hello","HELLO");
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualIgnoreCaseFail()
		{
			StringAssert.AreEqualIgnoreCase("hello","HELL");
        }
        #endregion

        #region Contains
        [Test]
		public void DoesNotContain()
		{
			StringAssert.DoesNotContain("hello",'k');
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void DoesNotContainFail()
		{
			StringAssert.DoesNotContain("hello",'l');
        }
        #endregion

        #region Emptiness
        [Test]
		public void IsEmpty()
		{
			StringAssert.IsEmpty("");
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsEmptyFail()
		{
			StringAssert.IsEmpty(" ");
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsEmptyNullFail()
		{
			StringAssert.IsEmpty(null);
		}

		[Test]
		public void IsNotEmpty()
		{
			StringAssert.IsNonEmpty(" ");
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsNotEmptyFail()
		{
			StringAssert.IsNonEmpty("");
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IsNotEmptyNullFail()
		{
			StringAssert.IsNonEmpty(null);
        }
        #endregion
    }
}

